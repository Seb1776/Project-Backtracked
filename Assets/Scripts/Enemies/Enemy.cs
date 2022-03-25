using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : LivingEntity
{
    public EnemyData enemyData;
    public AnimatronicAbility[] normalAbilities;
    public EnemySpecialAbility[] specialAbilities;
    public GameObject abilityText;
    public int idxInManager;

    Coroutine munchiesCoroutine;
    Coroutine hauntingCoroutine;
    Coroutine _haunt;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        for (int i = 0; i < manager.enemyParty.Length; i++)
        {
            if (manager.enemyParty[i].enemyItem == this)
            {
                idxInManager = i;
                break;
            }
        }

        base.Start();
    }

    public override void Update()
    {
        if (!manager.enemyParty[idxInManager].alive && entityMesh.activeSelf)
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

    void HandleStatEffects()
    {
        for (int i = 0; i < allStats.Count; i++)
        {
            if (allStats[i].allStatMods.Count > 0)
            {
                for (int j = 0; j < allStats[i].allStatMods.Count; j++)
                {
                    if (allStats[i].allStatMods[j].currentTimeToAffect < allStats[i].allStatMods[j].timeToAffect)
                    {
                        allStats[i].allStatMods[j].currentTimeToAffect += Time.deltaTime;

                        if (allStats[i].allStatMods[j].currentTimeToAffect >= allStats[i].allStatMods[j].timeToAffect)
                        {
                            if (!allStats[i].allStatMods[j].operation) allStats[i].allStatMods[j].AddStat(allStats[i].statType, this);
                            else allStats[i].allStatMods[j].SubtractStat(allStats[i].statType, this);

                            allStats[i].allStatMods.Remove(allStats[i].allStatMods[j]);
                        }
                    }
                }
            }
        }
    }

    public override void TriggerGift()
    {
        if (manager.battleMusicContext != GameManager.BattleMusicContext.Fight)
        {
            hasGift = true;

            currentGiftHealth = currentHealth = (int)(enemyData.maxHealth / 2f);

            if (!manager.enemyParty[idxInManager].enemyGift.GetComponent<Animator>().GetBool("appear"))
                manager.enemyParty[idxInManager].enemyGift.GetComponent<Animator>().SetBool("appear", true);
        }

        else
        {
            if (!manager.enemyParty[idxInManager].alive)
                ReviveEntity();
            
            else
            {
                hasGift = true;
                currentGiftHealth = (int)(enemyData.maxHealth / 2f);

                if (!manager.enemyParty[idxInManager].enemyGift.GetComponent<Animator>().GetBool("appear"))
                    manager.enemyParty[idxInManager].enemyGift.GetComponent<Animator>().SetBool("appear", true);
            }
        }

        base.TriggerGift();
    }

    public override void UnTriggerGift()
    {
        manager.enemyParty[idxInManager].enemyGift.GetComponent<Animator>().SetBool("appear", false);
        hasGift = false;
        base.UnTriggerGift();
    }

    public override void AddStatEffectToList(string _statID, int _amountToMod, bool _operation, float _timeToAffect, AllStatsMods.StatType _statType)
    {
        bool exists = false;

        for (int i = 0; i < allStats.Count; i++)
        {
            for (int j = 0; j < allStats[i].allStatMods.Count; j++)
            {
                if (allStats[i].allStatMods[j].statID == _statID)
                    exists = true;
            }
        }

        if (!exists)
        {
            StatModification _sm = new StatModification(_statID, _amountToMod, _operation, _timeToAffect);

            for (int i = 0; i < allStats.Count; i++)
                if (allStats[i].statType == _statType)
                    allStats[i].allStatMods.Add(_sm);
            
            if (_operation) _sm.AddStat(_statType, this);
            else _sm.SubtractStat(_statType, this);
        }

        base.AddStatEffectToList(_statID, _amountToMod, _operation, _timeToAffect, _statType);
    }

    public override void TriggerHauntingEffect(float _hauntTime, float _timeBeforeAbility)
    {
        if (!haunted)
        {
            haunted = true;
            hauntTime = _hauntTime;
            delay = _timeBeforeAbility;
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
            entityMesh.GetComponent<Animator>().speed = 0f;

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

    /*public override IEnumerator IncreaseStatTimer(string _stat, int _value, float _duration, bool _operation, float _deltaBefore)
    {
        instancesOfNerf++;
        yield return new WaitForSeconds(_deltaBefore);

        switch (_stat)
        {
            case "DEF":
                if (_operation)
                {
                    currentDefense += _value;

                    if (currentDefense > enemyData.maxDefense)
                        currentDefense = enemyData.maxDefense;

                    manager.e_imageDefBuffUp.SetActive(true);
                }

                else
                {
                    currentDefense -= _value;
                    
                    if (currentDefense < 0)
                        currentDefense = 0;

                    manager.e_imageDefBuffDown.SetActive(true);
                }

                manager.e_textDefBuff.SetActive(true);
            break;

            case "ATK":
                if (_operation)
                {
                    currentAttack += _value;
                    manager.e_imageAtkBuffUp.SetActive(true);
                }

                else
                {
                    currentAttack -= _value;
                    manager.e_imageAtkBuffDown.SetActive(true);
                }

                manager.e_textAtkBuff.SetActive(true);
            break;

            case "CRT":
                if (_operation)
                {
                    currentCritChance += _value;
                    manager.e_imageCrtBuffUp.SetActive(true);
                }

                else
                {
                    currentCritChance -= _value;
                    manager.e_imageCrtBuffDown.SetActive(true);
                } 
                break;
        }

        yield return new WaitForSeconds(_duration);

        switch (_stat)
        {
            case "DEF":
                if (_operation)
                {
                    currentDefense -= _value;

                    if (instancesOfNerf <= 1)
                        manager.e_imageDefBuffUp.SetActive(false);
                }

                else
                {
                    currentDefense += _value;

                    if (instancesOfNerf <= 1)
                        manager.e_imageDefBuffDown.SetActive(false);
                }

                if ((!manager.e_imageDefBuffDown.activeSelf || !manager.e_imageDefBuffUp.activeSelf) && instancesOfNerf <= 1)
                    manager.e_textDefBuff.SetActive(false);
            break;

            case "ATK":
                if (_operation)
                {
                    currentAttack -= _value;

                    if (instancesOfNerf <= 1)
                        manager.e_imageAtkBuffUp.SetActive(false);
                }

                else
                {
                    currentAttack += _value;

                    if (instancesOfNerf <= 1)
                        manager.e_imageAtkBuffDown.SetActive(false);
                }

                if ((!manager.e_imageAtkBuffDown.activeSelf || !manager.e_imageAtkBuffUp.activeSelf) && instancesOfNerf <= 1)
                    manager.e_textAtkBuff.SetActive(false);
            break;

            case "CRT":
                if (_operation)
                {
                    currentCritChance -= _value;

                    if (instancesOfNerf <= 1)
                        manager.e_imageCrtBuffUp.SetActive(false);
                }

                else
                {
                    currentCritChance += _value;

                    if (instancesOfNerf <= 1)
                        manager.e_imageCrtBuffDown.SetActive(false);
                }

                if ((!manager.e_imageCrtBuffDown.activeSelf || !manager.e_imageCrtBuffUp.activeSelf) && instancesOfNerf <= 1)
                    manager.e_textCrtBuff.SetActive(false);
            break;
        }

        instancesOfNerf--;
        yield return base.IncreaseStatTimer(_stat, _value, _duration, _operation, _deltaBefore);
    }*/

    public void SelectAttack()
    {
        List<int> usedIdxs = new List<int>();
        List<int> unusedIdx = new List<int>();
        bool usedSpecial = false;

        if (specialAbilities.Length > 0)
        {
            for (int i = 0; i < specialAbilities.Length; i++)
                unusedIdx.Add(i);
            
            for (int i = 0; i < specialAbilities.Length; i++)
            {
                int _n = unusedIdx[Random.Range(0, unusedIdx.Count)];

                if (usedIdxs.Contains(_n)) continue;
                usedIdxs.Add(_n);
                unusedIdx.Remove(_n);
            }

            for (int i = 0; i < specialAbilities.Length; i++)
            {
                if (!manager.GetRandomBoolChance(specialAbilities[usedIdxs[i]].chanceOfUsing)) continue;
                StartCoroutine(WaitForAbilityPanel(i, usedIdxs));
                AppearSpecialAttackPanel(specialAbilities[usedIdxs[i]]._ability.abilityName);
                usedSpecial = true;
                break;
            }
        }

        if (!usedSpecial)
        {
            int randomNormal = 0;
            randomNormal = Random.Range(0, normalAbilities.Length);
            normalAbilities[randomNormal].ApplyEffect(false);
        }
    }

    void AppearSpecialAttackPanel(string ab)
    {
        if (!manager.battleEnded)
        {
            if (manager.enemyParty[idxInManager].alive)
            {
                GameObject sap = Instantiate(abilityText, transform.position, Quaternion.identity, parentCanvas);
                sap.GetComponent<SpecialAbilityPanel>().AppearPanel(ab);
            }
        }
    }

    IEnumerator WaitForAbilityPanel(int abIndex, List<int> idxes)
    {
        yield return new WaitForSeconds(.750f + .5f);

        specialAbilities[idxes[abIndex]]._ability.ApplyEffect
        (
            false,
            specialAbilities[idxes[abIndex]]._ability.effectIsNotCentered,
            specialAbilities[idxes[abIndex]]._ability.effectIsNotCentered
        );
    }

    public override void MakeDamage(int damageValue, string context = "", LivingEntity attackingEntity = null, LivingEntity attackedEntity = null)
    {
        base.MakeDamage(damageValue, context, attackingEntity, attackedEntity);

        if (manager.enemyParty[idxInManager].alive)
        {
            playingFeedback = true;
            entityMesh.SetActive(false);
        }
    }

    public override void Heal(int healthValue, LivingEntity healedEntity)
    {
        currentHealth += healthValue;

        if (currentHealth >= enemyData.maxHealth)
            currentHealth = enemyData.maxHealth;

        base.Heal(healthValue, healedEntity);
    }

    public override void TriggerEntityDeath()
    {   
        if (manager.enemyParty[idxInManager].alive)
        {
            manager.CheckForBattleEnd();
            source.PlayOneShot(deathSound);
        }

        if (hauntingCoroutine != null)
            StopCoroutine(hauntingCoroutine);
            
        if (_haunt != null)
            StopCoroutine(_haunt);
        
        if (hasBubbleBreath)
            manager.enemyParty[idxInManager].bubbleBreathBubble.SetBool("bubble", false);

        if (hasMunchies)
        {
            hasMunchies = false;


            if (manager.battleMusicContext == GameManager.BattleMusicContext.Fight)
                manager.enemyParty[idxInManager].munchieObject.GetComponent<Animator>().SetBool("appear_munchie", false);

            else
                manager.bossParty.munchieObject.GetComponent<Animator>().SetBool("appear_munchie", false);
        }

        if (playingFeedback) playingFeedback = false;
        entityMesh.SetActive(false);

        manager.enemyParty[idxInManager].alive = false;
        manager.enemyParty[idxInManager].enemyDeathEffect.SetActive(true);
        manager.enemyParty[idxInManager].canvasParent.gameObject.SetActive(false);
        entityMesh.SetActive(false);

        base.TriggerEntityDeath();
    }

    public override void InstaKill()
    {
        MakeDamage(currentHealth);
        base.InstaKill();
    }

    public override IEnumerator FailedInstaKill(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject dn = Instantiate(manager.enemyParty[idxInManager].enemyItem.respectiveDamageNumber, manager.enemyParty[idxInManager].enemyItem.respectiveDamageNumber.transform.position, manager.enemyParty[idxInManager].enemyItem.respectiveDamageNumber.transform.rotation, manager.enemyParty[idxInManager].enemyItem.parentCanvas);
        string[] failedInstaMsgs = {"NOPE", "NAH M8", "GITGUD", "LMAO"};
        dn.GetComponent<DamageNumbers>().AppearText(0, "normal", failedInstaMsgs[Random.Range(0, failedInstaMsgs.Length)]);
        yield return base.FailedInstaKill(delay);
    }

    public override void TriggerMunchies(float _munchieDuration, int _munchieDamage, Vector2 _timeBtwMunchieDamage)
    {   
        if (!hasMunchies)
        {
            if (manager.battleMusicContext == GameManager.BattleMusicContext.Fight)
                manager.enemyParty[idxInManager].munchieObject.GetComponent<Animator>().SetBool("appear_munchie", true);

            else
                manager.bossParty.munchieObject.GetComponent<Animator>().SetBool("appear_munchie", true);
            
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
                    if (manager.battleMusicContext == GameManager.BattleMusicContext.Fight)
                        manager.enemyParty[idxInManager].munchieObject.GetComponent<Animator>().SetTrigger("attack_munchie");

                    else
                        manager.bossParty.munchieObject.GetComponent<Animator>().SetTrigger("attack_munchie");
                    
                    MakeDamage(munchieDamage, "normal");
                }
            }

            if (currentMunchieDuration >= munchieDuration)
            {
                hasMunchies = false;

                if (manager.battleMusicContext == GameManager.BattleMusicContext.Fight)
                    manager.enemyParty[idxInManager].munchieObject.GetComponent<Animator>().SetBool("appear_munchie", false);
                
                else
                    manager.bossParty.munchieObject.GetComponent<Animator>().SetBool("appear_munchie", false);
            }
        }
    }
}

[System.Serializable]
public class EnemySpecialAbility
{
    public AnimatronicAbility _ability;
    [Range(0f, 100f)]
    public float chanceOfUsing;
}
