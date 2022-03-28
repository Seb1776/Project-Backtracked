using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Animatronic : LivingEntity
{
    [Range(1, 75)]
    public int animatronicLevel;
    public AnimatronicData animatronicData;
    public SkinHolder[] skins;
    public enum AnimatronicState { Downed, Alive, Gift }
    public AnimatronicState currentAnimatronicState;
    public int currentSkinIndex;
    public int idxInManager;

    Animator animator;
    Coroutine munchiesCoroutine;

    public override void Awake()
    {
        base.Awake();
        SetSkin();
    }

    public override void Start()
    {
        for (int i = 0; i < manager.animatronicParty.Length; i++)
        {
            if (manager.animatronicParty[i].animatronicItem == this)
            {
                idxInManager = i;
                break;
            }
        }

        base.Start();
    }

    public override void Update()
    {
        if (!manager.animatronicParty[idxInManager].alive && entityMesh.activeSelf)
            entityMesh.SetActive(false);
        
        if (hasMunchies)
            MunchiesBehaviour();
        
        if (playingFeedback)
            DamageFeedback();
        
        if (haunted)
            StartHaunt();
        
        HandleStatEffects();
        
        base.Update();
    }

    void SetSkin()
    {
        foreach (SkinHolder sh in skins)
            sh.skinAnimator.gameObject.SetActive(false);

        skins[animatronicData.defaultSkin].skinAnimator.gameObject.SetActive(true);
        skins[animatronicData.defaultSkin].skinAnimator.enabled = true;
        entityMesh = skins[animatronicData.defaultSkin].skinMesh;
    }
    
    public override void MakeDamage(int damageValue, string context = "", LivingEntity attackingEntity = null, LivingEntity attackedEntity = null)
    {
        base.MakeDamage(damageValue, context, attackingEntity, attackedEntity);
        
        if (manager.animatronicParty[idxInManager].alive)
            playingFeedback = true;
    }

    public override void Heal(int healthValue, LivingEntity healedEntity)
    {
        if (!hasGift)
        {
            currentHealth += healthValue;

            if (currentHealth >= animatronicData.maxHealth)
                currentHealth = animatronicData.maxHealth;
        }

        base.Heal(healthValue, healedEntity);
    }

    public override void TriggerMunchies(float _munchieDuration, int _munchieDamage, Vector2 _timeBtwMunchieDamage)
    {
        if (!hasMunchies)
        {
            manager.animatronicParty[idxInManager].munchieObject.GetComponent<Animator>().SetBool("appear_munchie", true);
            munchieDamage = _munchieDamage;
            munchieDuration = _munchieDuration;
            timeBtwMunchieDamage = _timeBtwMunchieDamage;
            newTimeMunchieDamage = Random.Range(timeBtwMunchieDamage.x, timeBtwMunchieDamage.y);
            hasMunchies = true;
        }
        
        else
            currentMunchieDuration = 0f;

        base.TriggerMunchies(_munchieDuration, _munchieDamage, _timeBtwMunchieDamage);
    }

    public override void TriggerHauntingEffect(float _hauntTime, float _timeBeforeAbility)
    {
        if (!haunted)
        {
            haunted = true;
            hauntTime = _hauntTime;
            delay = _timeBeforeAbility;
            entityMesh.GetComponent<Animator>().speed = 0f;
        }
        
        else
            currentHauntTime = 0f;

        base.TriggerHauntingEffect(_hauntTime, _timeBeforeAbility);
    }

    float hauntTime, delay, currentHauntTime, currentDelay;

    void StartHaunt()
    {
        if (currentDelay < delay)
            currentDelay += Time.deltaTime;
        
        else if (currentDelay >= delay)
        {
            if (currentHauntTime < hauntTime)
            {
                currentHauntTime += Time.deltaTime;

                if (currentHauntTime >= hauntTime)
                {
                    currentDelay = 0f;
                    currentHauntTime = 0f;
                    haunted = false;
                    entityMesh.GetComponent<Animator>().speed = 1f;
                }
            }
        }
    }

    float currentMunchieDuration, munchieDuration, newTimeMunchieDamage, currentTimeBtwMunchieDamage;
    int munchieDamage;
    Vector2 timeBtwMunchieDamage;

    void MunchiesBehaviour()
    {
        if (currentMunchieDuration < munchieDuration)
        {
            currentMunchieDuration += Time.deltaTime;

            if (currentTimeBtwMunchieDamage < newTimeMunchieDamage)
            {
                currentTimeBtwMunchieDamage += Time.deltaTime;

                if (currentTimeBtwMunchieDamage >= newTimeMunchieDamage)
                {
                    manager.animatronicParty[idxInManager].munchieObject.GetComponent<Animator>().SetTrigger("attack_munchie");
                    MakeDamage(munchieDamage, "normal");
                }
            }

            if (currentMunchieDuration >= munchieDuration)
            {
                hasMunchies = false;
                manager.animatronicParty[idxInManager].munchieObject.GetComponent<Animator>().SetBool("appear_munchie", false);
            }
        }
    }

    public override void InstaKill()
    {
        if (hasGift)
            UnTriggerGift();
        
        MakeDamage(currentHealth, "normal");

        base.InstaKill();
    }

    public override void ReviveEntity()
    {
        manager.animatronicParty[idxInManager].alive = true;
        manager.animatronicParty[idxInManager].animatronicTombstone.SetActive(false);
        currentHealth = (int)(animatronicData.maxHealth / 2f);
        entityMesh.SetActive(true);

        base.ReviveEntity();
    }

    public override void TriggerGift()
    {
        if (!manager.animatronicParty[idxInManager].alive)
            ReviveEntity();
        
        else
        {
            hasGift = true;
            currentGiftHealth = (int)(animatronicData.maxHealth / 2f);

            if (!manager.animatronicParty[idxInManager].animatronicGift.GetComponent<Animator>().GetBool("appear"))
                manager.animatronicParty[idxInManager].animatronicGift.GetComponent<Animator>().SetBool("appear", true);
            
            if (!manager.animatronicParty[idxInManager].giftBoxParent.activeSelf)
                manager.animatronicParty[idxInManager].giftBoxParent.SetActive(true);
        }

        base.TriggerGift();
    }

    public override void UnTriggerGift()
    {
        hasGift = false;

        manager.animatronicParty[idxInManager].animatronicGift.GetComponent<Animator>().SetBool("appear", false);
        manager.animatronicParty[idxInManager].giftBoxParent.SetActive(false);
        base.UnTriggerGift();
    }

    public override IEnumerator FailedInstaKill(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject dn = Instantiate(manager.animatronicParty[idxInManager].animatronicItem.respectiveDamageNumber, manager.enemyParty[idxInManager].enemyItem.respectiveDamageNumber.transform.position, manager.enemyParty[idxInManager].enemyItem.respectiveDamageNumber.transform.rotation, manager.enemyParty[idxInManager].enemyItem.parentCanvas);
        string[] failedInstaMsgs = {"NOPE", "NAH M8", "GITGUD", "LMAO"};
        dn.GetComponent<DamageNumbers>().AppearText(0, "normal", failedInstaMsgs[Random.Range(0, failedInstaMsgs.Length)]);
        yield return base.FailedInstaKill(delay);
    }

    public override void AddStatEffectToList(string _statID, int _amountToMod, bool _operation, float _timeToAffect, AllStatsMods.StatType _statType)
    {
        bool exists = false;

        for (int i = 0; i < allStats.Count; i++)
        {
            for (int j = 0; j < allStats[i].allStatMods.Count; j++)
            {
                if (allStats[i].allStatMods[j].statID == _statID && allStats[i].statType == _statType)
                    exists = true;
            }
        }

        if (!exists)
        {
            StatModification _sm = new StatModification(_statID, _amountToMod, _operation, _timeToAffect);

            for (int i = 0; i < allStats.Count; i++)
                if (allStats[i].statType == _statType)
                    allStats[i].allStatMods.Add(_sm);
            
            if (_operation)
            {
                _sm.AddStat(_statType, this);
                
                for (int i = 0; i < manager.animatronicStats.Length; i++)
                {
                    if (manager.animatronicStats[i].statUIMod == _statType)
                    {
                        manager.animatronicStats[i].Buff(true, _statID);
                        break;
                    }
                }
            }

            else 
            {   
                _sm.SubtractStat(_statType, this);

                for (int i = 0; i < manager.animatronicStats.Length; i++)
                {
                    if (manager.animatronicStats[i].statUIMod == _statType)
                    {
                        manager.animatronicStats[i].Buff(false, _statID);
                        break;
                    }
                }
            }
        }

        base.AddStatEffectToList(_statID, _amountToMod, _operation, _timeToAffect, _statType);
    }

    public void HandleStatEffects()
    {
        foreach (AllStatsMods asm in allStats)
        {
            if (asm.allStatMods.Count > 0)
            {
                for (int j = 0; j < asm.allStatMods.Count; j++)
                {
                    if (asm.allStatMods[j].currentTimeToAffect < asm.allStatMods[j].timeToAffect)
                    {
                        asm.allStatMods[j].currentTimeToAffect += Time.deltaTime;

                        if (asm.allStatMods[j].currentTimeToAffect >= asm.allStatMods[j].timeToAffect)
                        {
                            if (!asm.allStatMods[j].operation) asm.allStatMods[j].AddStat(asm.statType, this);
                            else asm.allStatMods[j].SubtractStat(asm.statType, this);

                            manager.StatInstanceOver(asm.allStatMods[j].statID, asm.statType, this, asm.allStatMods[j].operation);

                            asm.allStatMods.Remove(asm.allStatMods[j]);
                        }
                    }
                }
            }
        }
    }

    float currentFeedbackTimer;
    int currentFeedbackIter;

    public override void DamageFeedback()
    {
        if (currentFeedbackIter >= 6)
        {
            playingFeedback = false;
            currentFeedbackIter = 0;
            currentFeedbackTimer = 0f;
            entityMesh.SetActive(true);
        }

        else if (currentFeedbackIter < 6)
        {
            if (entityMesh.activeSelf)
            {
                if (currentFeedbackTimer < 0.01f)
                    currentFeedbackTimer += Time.deltaTime;

                if (currentFeedbackTimer >= 0.01f)
                {
                    entityMesh.SetActive(false);
                    currentFeedbackTimer = 0f;
                    currentFeedbackIter++;
                }
            }

            else
            {
                if (currentFeedbackTimer < 0.01f)
                    currentFeedbackTimer += Time.deltaTime;
                
                if (currentFeedbackTimer >= 0.01f)
                {
                    entityMesh.SetActive(true);
                    currentFeedbackTimer = 0f;
                    currentFeedbackIter++;
                }
            }
        }

        base.DamageFeedback();
    }

    public override void TriggerEntityDeath()
    {
        if (manager.currentAnimatronicTurn == this)
            manager.DisableAttackButtons();

        if (manager.animatronicParty[idxInManager].alive)
        {
            manager.CheckForGameOver();
            source.PlayOneShot(deathSound);
        }

        if (playingFeedback) playingFeedback = false;
        entityMesh.SetActive(false);

        manager.animatronicParty[idxInManager].alive = false;
        manager.animatronicParty[idxInManager].animatronicDeathEffect.SetActive(true);
        manager.animatronicParty[idxInManager].canvasParent.gameObject.SetActive(false);
        manager.animatronicParty[idxInManager].animatronicTombstone.SetActive(true);
        manager.animatronicParty[idxInManager].remainingTimeToPlay = 0f;
        manager.animatronicParty[idxInManager].turnSlider.value = 0f;
        entityMesh.SetActive(false);

        base.TriggerEntityDeath();
    }
}

[System.Serializable]
public class SkinHolder
{
    public Animator skinAnimator;
    public GameObject skinMesh;
}

[System.Serializable]
public class AnimatronicTier
{
    public string tierLevel;
    public int defenseUpgrade;
    public int attackUpgrade;
    public int critChanceUpgrade;
    public int healthUpgrade;
}
