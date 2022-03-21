using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    public PartyLoader[] partyToLoad;
    public AnimatronicParty[] animatronicParty;
    public EnemyParty[] enemyParty;
    public EnemyParty bossParty;
    public AttackButton[] attackButtons;
    public CameraShakeManager cameraShake;
    public enum BattleMusicContext { Fight, BossFight, SecretFight, FinalFight }
    public BattleMusicContext battleMusicContext;
    public float timeBtwTurns;
    public bool battleEnded;
    public bool hasMimicBall;
    public MimicBall mimicBall;
    public AudioMixerGroup sfxMixer;
    public Animator[] neonWallAnimatronics;
    public Animator[] neonWallEnemies;
    [Header ("UI")]
    public Transform overlaysParent;
    public GameObject overlayObj;
    public Sprite[] overlays;
    public GameObject imageDefBuffDown;
    public GameObject imageDefBuffUp;
    public GameObject textDefBuff;
    public GameObject imageAtkBuffDown;
    public GameObject imageAtkBuffUp;
    public GameObject textAtkBuff;
    public GameObject imageCrtBuffDown;
    public GameObject imageCrtBuffUp;
    public GameObject textCrtBuff;
    public GameObject e_imageDefBuffDown;
    public GameObject e_imageDefBuffUp;
    public GameObject e_textDefBuff;
    public GameObject e_imageAtkBuffDown;
    public GameObject e_imageAtkBuffUp;
    public GameObject e_textAtkBuff;
    public GameObject e_imageCrtBuffDown;
    public GameObject e_imageCrtBuffUp;
    public GameObject e_textCrtBuff;
    public Animator battleWonPanel;
    public Slider bossHealth;
    public Text bossHealthText;
    [Header ("Audio")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public MusicSystem[] battleThemes;
    public AudioClip battleVictory;
    public LivingEntity currentAnimatronicTurn;
    public LivingEntity currentEnemyTurn;
    public bool waitingForTurn;

    Coroutine neonWallCoroutine;
    Coroutine bubbleBreathCoroutine;
    private static readonly int Mimic = Animator.StringToHash("mimic");
    private static readonly int MimicAppear = Animator.StringToHash("mimic_appear");
    private static readonly int Move = Animator.StringToHash("move");

    void Start()
    {
        LoadAnimatronics();
        GetInitialTurns();
        PlayMusicContext();

        foreach (Animator a in neonWallAnimatronics)
            if (!a.gameObject.activeSelf)
                a.gameObject.SetActive(true);

        foreach (AnimatronicParty ap in animatronicParty)
        {
            ap.SetAnimatronicData();
            ap.SetUI();
        }

        for (int i = 0; i < neonWallEnemies.Length; i++)
        {
            if (!neonWallEnemies[i].gameObject.activeSelf)
                neonWallEnemies[i].gameObject.SetActive(true);
            
            if (!neonWallAnimatronics[i].gameObject.activeSelf)
                neonWallAnimatronics[i].gameObject.SetActive(true);
        }

        if (battleMusicContext == BattleMusicContext.Fight)
        {
            foreach (EnemyParty ep in enemyParty)
                if (ep.enemyItem != null)
                    ep.SetEnemyData();
        }
        
        else if (battleMusicContext == BattleMusicContext.BossFight ||
                 battleMusicContext == BattleMusicContext.SecretFight ||
                 battleMusicContext == BattleMusicContext.FinalFight)
        {
            if (bossParty.enemyItem != null)
            {
                bossParty.SetEnemyData();
            }
        }
    }

    void Update()
    {   
        if (!battleEnded)
        {
            BattleBehaviour();
            HandleUpdateUI();
        }

        //Debug.Log(sfxMixer.volume);
    }

    void BattleBehaviour()
    {
        foreach (AnimatronicParty ap in animatronicParty)
        {   
            if ((!waitingForTurn && !ap.animatronicItem.stunned) && ap.alive)
            {
                if (ap.remainingTimeToPlay < timeBtwTurns)
                {
                    ap.remainingTimeToPlay += Time.deltaTime * ap.turnMultiplier;
                    ap.turnSlider.value = ap.remainingTimeToPlay;
                }
                    
                if (ap.remainingTimeToPlay >= timeBtwTurns)
                {
                    waitingForTurn = true;
                    ap.turnModel.SetBool("turn", true);
                    currentAnimatronicTurn = ap.animatronicItem;
                    TriggerAttackButtons(ap.animatronicItem.GetComponent<Animatronic>());
                    break;
                }
            }
        }

        if (battleMusicContext == BattleMusicContext.Fight)
        {
            foreach (EnemyParty ep in enemyParty)
            {
                if (!ep.enemyItem.stunned && ep.alive)
                {
                    if (ep.remainingTimeToPlay < timeBtwTurns)
                    {
                        ep.remainingTimeToPlay += Time.deltaTime * ep.turnMultiplier;
                    }

                    if (ep.remainingTimeToPlay >= timeBtwTurns)
                    {
                        ep.positionAnimatior.SetTrigger("move");
                        ep.enemyItem.GetComponent<Enemy>().SelectAttack();
                        currentEnemyTurn = ep.enemyItem;
                        ep.remainingTimeToPlay = 0f;
                        ep.turnMultiplier = ep.GetNewTurnTimeMultiplier();
                    }
                }
            }
        }

        else
        {
            if (!bossParty.enemyItem.stunned && bossParty.alive)
            {
                if (bossParty.remainingTimeToPlay < timeBtwTurns)
                {
                    bossParty.remainingTimeToPlay += Time.deltaTime * bossParty.turnMultiplier;
                }

                if (bossParty.remainingTimeToPlay >= timeBtwTurns)
                {
                    bossParty.positionAnimatior.SetTrigger(Move);
                    bossParty.enemyItem.GetComponent<Enemy>().SelectAttack();
                    currentEnemyTurn = bossParty.enemyItem;
                    bossParty.remainingTimeToPlay = 0f;
                    bossParty.turnMultiplier = bossParty.GetNewTurnTimeMultiplier();
                }
            }
        }
    }

    public void TriggerMimicBall()
    {
        mimicBall.mimicBallAnim.SetTrigger(MimicAppear);
        hasMimicBall = true;
    }

    public void TriggerMimicAbility()
    {
        mimicBall.mimicBallAnim.SetTrigger(Mimic);
    }

    public void NeonWallActivation(string createFor, int neonWallType, float duration)
    {
        if (animatronicParty[0].animatronicItem.hasNeonWall)
            StopCoroutine(neonWallCoroutine);

        neonWallCoroutine = StartCoroutine(TriggerNeonWall(createFor, neonWallType, duration));
    }

    public IEnumerator TriggerNeonWall(string createFor, int neonWallType, float duration)
    {
        if (createFor == "animatronic")
        {
            neonWallAnimatronics[neonWallType].SetBool("neonwall", true);
            
            foreach (AnimatronicParty ap in animatronicParty)
            {
                ap.animatronicItem.hasNeonWall = true;
                ap.animatronicItem.neonWallType = neonWallType;
            }
        }

        yield return new WaitForSeconds(duration);

        if (createFor == "animatronic")
        {
            neonWallAnimatronics[neonWallType].SetBool("neonwall", false);
            
            foreach (AnimatronicParty ap in animatronicParty)
            {
                ap.animatronicItem.hasNeonWall = false;
                ap.animatronicItem.neonWallType = -1;
            }
        }
    }

    public void BubbleBreathActivation(string invokeFor, float duration)
    {
        Debug.Log(animatronicParty[0].animatronicItem.hasBubbleBreath);
        if (animatronicParty[0].animatronicItem.hasBubbleBreath)
        {
            StopCoroutine(bubbleBreathCoroutine);
        }

        bubbleBreathCoroutine = StartCoroutine(TriggerBubbleBreath(invokeFor, duration));
    }

    public IEnumerator TriggerBubbleBreath(string invokeFor, float duration)
    {
        if (invokeFor == "animatronic")
        {
            foreach (AnimatronicParty ap in animatronicParty)
            {
                ap.animatronicItem.hasBubbleBreath = true;
                ap.bubbleBreathBubble.SetBool("bubble", true);
            }
        }

        yield return new WaitForSeconds(duration);

        if (invokeFor == "animatronic")
        {
            foreach (AnimatronicParty ap in animatronicParty)
            {
                ap.animatronicItem.hasBubbleBreath = false;
                ap.bubbleBreathBubble.SetBool("bubble", false);
            }
        }
    }

    public void CreateOverlay(int overlayIndex, Vector2 fadeInOffDuration, float stayDuration)
    {
        GameObject _overlayObj = Instantiate(overlayObj, overlayObj.transform.position, Quaternion.identity, overlaysParent);
        _overlayObj.GetComponent<Image>().sprite = overlays[overlayIndex];
        _overlayObj.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, 0f, 0f);
        StartCoroutine(TriggerFadeIn(_overlayObj.GetComponent<Animator>(), fadeInOffDuration, stayDuration));
        Destroy(_overlayObj, (fadeInOffDuration.x + stayDuration + fadeInOffDuration.y));
    }

    IEnumerator TriggerFadeIn(Animator ovrl, Vector2 fadeInOff, float stayDuration)
    {
        ovrl.speed = 1f / fadeInOff.x;
        yield return new WaitForSeconds(fadeInOff.x);
        ovrl.speed = 1f;
        StartCoroutine(TriggerStay(ovrl, fadeInOff, stayDuration));
    }

    IEnumerator TriggerStay(Animator ovrl, Vector2 fadeInOff, float duration)
    {
        yield return new WaitForSeconds(duration);
        StartCoroutine(TriggerFadeOut(ovrl, fadeInOff));
    }

    IEnumerator TriggerFadeOut(Animator ovrl, Vector2 fadeInOff)
    {
        ovrl.SetTrigger("fadeoff");
        ovrl.speed = 1f / fadeInOff.y;
        yield return new WaitForSeconds(fadeInOff.y);
    }

    void LoadAnimatronics()
    {
        for (int i = 0; i < partyToLoad.Length; i++)
        {
            AnimatronicData animData = Resources.Load<AnimatronicData>("Animatronics/" + partyToLoad[i].animatronicGame.ToString() + "/" + partyToLoad[i].animatronicName);
            GameObject animPrefab = Instantiate(animData.animatronicPrefab, animatronicParty[partyToLoad[i].position].posSpawn.position, animatronicParty[partyToLoad[i].position].posSpawn.rotation, animatronicParty[partyToLoad[i].position].posSpawn);
            animatronicParty[partyToLoad[i].position].animatronicItem = animPrefab.GetComponent<Animatronic>();
            animPrefab.GetComponent<Animatronic>().currentSkinIndex = animData.defaultSkin;
        }
    }

    void GetInitialTurns()
    {
        foreach (AnimatronicParty ap in animatronicParty)
        {
            ap.remainingTimeToPlay = timeBtwTurns / 2f;
            ap.turnSlider.value = ap.remainingTimeToPlay;
            ap.turnMultiplier = ap.GetNewTurnTimeMultiplier();
        }

        int randomTurn = Random.Range(0, 3);
        animatronicParty[randomTurn].remainingTimeToPlay = timeBtwTurns;
        animatronicParty[randomTurn].turnSlider.value = 5f;

        if (battleMusicContext == BattleMusicContext.Fight)
        {
            foreach (EnemyParty ep in enemyParty)
                if (ep.enemyItem != null)
                    ep.turnMultiplier = ep.GetNewTurnTimeMultiplier();
        }
        
        else if (battleMusicContext == BattleMusicContext.BossFight)
        {
            bossParty.turnMultiplier = bossParty.GetNewTurnTimeMultiplier();
        }
    }

    public void PlayedTurn()
    {
        for (int i = 0; i < animatronicParty.Length; i++)
        {
            if (animatronicParty[i].animatronicItem == currentAnimatronicTurn)
            {
                animatronicParty[i].remainingTimeToPlay = 0f;
                animatronicParty[i].turnModel.SetBool("turn", false);
                animatronicParty[i].turnMultiplier = animatronicParty[i].GetNewTurnTimeMultiplier();
                waitingForTurn = false;
            }
        }
    }

    public void CheckForBattleEnd()
    {
        bool[] deadList = new bool[4];
        int trueCount = 0;

        for (int i = 0; i < enemyParty.Length; i++)
            deadList[i] = enemyParty[i].alive;
        
        for (int i = 0; i < deadList.Length; i++)
            if (!deadList[i]) 
                trueCount++;
        
        if (trueCount >= 3)
            TriggerBattleEnd();
    }

    public void CheckForGameOver()
    {
        bool[] deadList = new bool[4];
        int trueCount = 0;

        for (int i = 0; i < animatronicParty.Length; i++)
            deadList[i] = animatronicParty[i].alive;
        
        for (int i = 0; i < deadList.Length; i++)
            if (!deadList[i])
                trueCount++;
        
        if (trueCount >= 3)
            TriggerGameOver();
    }

    void HandleUpdateUI()
    {
        foreach (AnimatronicParty ap in animatronicParty)
        {
            ap.healthSlider.value = ap.animatronicItem.currentHealth;
            ap.animatronicHealth.text = ap.animatronicItem.currentHealth.ToString();

            if (ap.animatronicItem.hasGift)
            {
                ap.giftBox.value = ap.animatronicItem.currentGiftHealth;
            }
        }

        if (battleMusicContext != BattleMusicContext.Fight)
        {
            bossHealth.maxValue = bossParty.enemyItem.GetComponent<Enemy>().enemyData.maxHealth;
            bossHealth.value = bossParty.enemyItem.currentHealth;
            bossHealthText.text = bossParty.enemyItem.currentHealth.ToString();
        }
    }

    float GetRelativeValueFrom(int relative, int value)
    {
        return value / relative;
    }

    void TriggerBattleEnd()
    {
        battleEnded = true;

        foreach (AnimatronicParty ap in animatronicParty)
        {
            ap.remainingTimeToPlay = 0f;
            ap.turnModel.SetBool("turn", false);
        }
        
        foreach (AttackButton ab in attackButtons)
            ab.GetComponent<Animator>().SetBool("appear", false);
        
        battleWonPanel.SetTrigger("battlewon");
        musicSource.clip = battleVictory;
        musicSource.Play();
    }

    void TriggerGameOver()
    {
        battleEnded = true;

        musicSource.Stop();
    }

    void PlayMusicContext()
    {
        for (int i = 0; i < battleThemes.Length; i++)
        {
            if (battleThemes[i].musicContext == battleMusicContext.ToString())
            {
                if (battleThemes[i].startAudio != null)
                {
                    musicSource.loop = false;
                    musicSource.clip = battleThemes[i].startAudio;
                    musicSource.Play();
                    StartCoroutine(PlayMusicAfterStart(battleThemes[i].startAudio.length, battleThemes[i].audioMusic));
                }

                else
                    StartCoroutine(PlayMusicAfterStart(0f, battleThemes[i].audioMusic));
            }
        }
    }

    IEnumerator PlayMusicAfterStart(float delay, AudioClip _music)
    {
        yield return new WaitForSeconds(delay);
        musicSource.clip = _music;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void DisableAttackButtons()
    {
        foreach (AttackButton ab in attackButtons)
            ab.GetComponent<Animator>().SetBool("appear", false);
        
        PlayedTurn();
    }

    List<int> GetAliveEntitiesList(string entity)
    {
        List<int> retList = new List<int>() { 0, 1, 2, 3 };

        if (entity == "enemy")
        {
            for (int i = 0; i < enemyParty.Length; i++)
                if (!enemyParty[i].alive)
                    retList.Remove(i);
        }

        else
        {
            for (int i = 0; i < animatronicParty.Length; i++)
                if (!animatronicParty[i].alive)
                    retList.Remove(i);
        }

        return retList;
    }

    public List<int> GetEntityList (string entity, int amount, bool randomOrder)
    {
        List<int> retList = new List<int>();

        if (randomOrder)
        {
            if (amount > 1)
            {
                List<int> alives = GetAliveEntitiesList(entity);
                int limitor = 0;

                List<int> unusedLimitorIndex = alives;
                List<int> usedLimitorIndex = new List<int>();

                if (amount > alives.Count) limitor = alives.Count;
                else if (amount <= alives.Count) limitor = amount;

                for (int i = 0; i < limitor; i++)
                {
                    int jackpot = unusedLimitorIndex[Random.Range(0, unusedLimitorIndex.Count)];
                    usedLimitorIndex.Add(jackpot);
                    unusedLimitorIndex.Remove(jackpot);
                }

                retList = usedLimitorIndex;
            }

            else
            {
                List<int> alives = GetAliveEntitiesList(entity);
                retList.Add(alives[Random.Range(0, alives.Count)]);
            }
        }
        
        else
        {
            if (amount > 1)
            {
                int limitor = 0;
                List<int> alives = GetAliveEntitiesList(entity);

                if (amount > alives.Count) limitor = alives.Count;
                else if (amount <= alives.Count) limitor = amount;

                for (int i = 0; i < limitor; i++)
                    retList.Add(alives[i]);
            }

            else
            {
                List<int> alives = GetAliveEntitiesList(entity);
                retList.Add(alives[0]);
            }
        }

        return retList;
    }

    public void PlaySoundEffect(AudioClip _clip)
    {
        AudioSource src = gameObject.AddComponent<AudioSource>();
        //src.outputAudioMixerGroup = sfxMixer;
        src.PlayOneShot(_clip);
        Destroy(src, _clip.length);
    }

    IEnumerator DestroyAudioSourceAfterTime(AudioSource sourceTo, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(sourceTo);
    }

    void TriggerAttackButtons(Animatronic currentTurn)
    {
        for (int i = 0; i < attackButtons.Length; i++)
            attackButtons[i].AppearButton(currentTurn.animatronicData.animatronicAbilities[i]);
    }

    public int GetValueFromPercentage(float fullValue, float percentageToTake)
    {
        return (int)((fullValue * percentageToTake) / 100f);
    }

    public bool GetRandomBoolChance(float percentageChance)
    {   
        if (percentageChance > 0)
        {
            float randChance = Random.value;

            if (percentageChance > 1)
                percentageChance /= 100f;

            return randChance <= percentageChance;
        }

        return false;
    }
}

[System.Serializable]
public class AnimatronicParty
{
    public LivingEntity animatronicItem;
    public float remainingTimeToPlay;
    public float turnMultiplier;
    public Animator turnModel;
    public GameObject animatronicDeathEffect;
    public GameObject animatronicTombstone;
    public GameObject animatronicGift;
    public GameObject munchieObject;
    public Transform posSpawn;
    public Animator bubbleBreathBubble;
    [Header ("UI")]
    public Transform canvasParent;
    public GameObject damageTextPrefab;
    public Text animatronicHealth;
    public GameObject giftBoxParent;
    public Slider giftBox;
    public Image animatronicPortrait;
    public Slider turnSlider;
    public Slider healthSlider;
    public bool alive;
    public bool gracePeriod;

    public void SetAnimatronicData()
    {
        if (!turnModel.gameObject.activeSelf)
            turnModel.gameObject.SetActive(true);
        
        if (!animatronicGift.activeSelf)
            animatronicGift.SetActive(true);
        
        if (!bubbleBreathBubble.gameObject.activeSelf)
            bubbleBreathBubble.gameObject.SetActive(true);

        animatronicItem.currentHealth = animatronicItem.GetComponent<Animatronic>().animatronicData.maxHealth;
        animatronicItem.currentAttack = animatronicItem.GetComponent<Animatronic>().animatronicData.maxAttack;
        animatronicItem.currentDefense = animatronicItem.GetComponent<Animatronic>().animatronicData.maxDefense;
        animatronicItem.currentCritChance = animatronicItem.GetComponent<Animatronic>().animatronicData.maxCritChance;
        animatronicItem.respectiveDamageNumber = damageTextPrefab;
        animatronicItem.parentCanvas = canvasParent;
        giftBox.maxValue = animatronicItem.GetComponent<Animatronic>().animatronicData.maxHealth;
        alive = true;
    }

    public void SetUI()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = animatronicItem.GetComponent<Animatronic>().animatronicData.maxHealth;
            healthSlider.value = animatronicItem.currentHealth;
        }

        if (animatronicHealth != null)
            animatronicHealth.text = animatronicItem.currentHealth.ToString();
        
        if (animatronicPortrait != null && animatronicItem.GetComponent<Animatronic>().animatronicData.animatronicPortrait != null)
            animatronicPortrait.sprite = animatronicItem.GetComponent<Animatronic>().animatronicData.animatronicPortrait;
    }

    public float GetNewTurnTimeMultiplier()
    {
        return Random.Range(.5f, 1.5f);
    }
}

[System.Serializable]
public class EnemyParty
{
    public LivingEntity enemyItem;
    public float remainingTimeToPlay;
    public float turnMultiplier;
    public Animator positionAnimatior;
    public Transform canvasParent;
    public GameObject enemyDeathEffect;
    public GameObject damageTextPrefab;
    public GameObject abilityPanelPrefab;
    public GameObject munchieObject;
    public GameObject enemyGift;
    public Text enemyBossName;
    public bool alive;

    public void SetEnemyData()
    {
        enemyItem.currentHealth = enemyItem.GetComponent<Enemy>().enemyData.maxHealth;
        enemyItem.currentAttack = enemyItem.GetComponent<Enemy>().enemyData.maxAttack;
        enemyItem.currentDefense = enemyItem.GetComponent<Enemy>().enemyData.maxDefense;
        enemyItem.respectiveDamageNumber = damageTextPrefab;
        enemyItem.GetComponent<Enemy>().abilityText = abilityPanelPrefab;
        enemyItem.parentCanvas = canvasParent;

        if (enemyBossName != null)
            enemyBossName.text = enemyItem.GetComponent<Enemy>().enemyData.enemyName;

        alive = true;
    }

    public float GetNewTurnTimeMultiplier()
    {
        return Random.Range(.5f, 1.5f);
    }
}

[System.Serializable]
public class PartyLoader
{
    public string animatronicName;
    public enum AnimatronicGame { FNaF, FNaF2, FNaF3, FNaF4, FNaFSL, FFPS, FNaFSD, FNaFSB }
    public AnimatronicGame animatronicGame;
    [Range(0, 3)]
    public int position;
}

[System.Serializable]
public class MimicBall
{
    public Animator mimicBallAnim;
    public float delayBeforeRepeat;
}

[System.Serializable]
public class MusicSystem
{
    public string musicContext;
    public AudioClip startAudio;
    public AudioClip audioMusic;
}
