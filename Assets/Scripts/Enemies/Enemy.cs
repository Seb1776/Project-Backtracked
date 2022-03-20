using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : LivingEntity
{
    public EnemyData enemyData;
    public AnimatronicAbility[] normalAbilities;
    public EnemySpecialAbility[] specialAbilities;
    public GameObject abilityText;
    [Range(15f, 85f)]
    public float chanceToUseSpecialAbility;
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

    void Update()
    {
        if (!manager.enemyParty[idxInManager].alive && entityMesh.activeSelf)
            entityMesh.SetActive(false);
    }

    public override void TriggerGift()
    {
        currentGiftHealth = currentHealth = enemyData.maxHealth;

        if (!manager.enemyParty[idxInManager].enemyGift.GetComponent<Animator>().GetBool("appear"))
            manager.enemyParty[idxInManager].enemyGift.GetComponent<Animator>().SetBool("appear", true);

        base.TriggerGift();
    }

    public override void UnTriggerGift()
    {
        manager.enemyParty[idxInManager].enemyGift.GetComponent<Animator>().SetBool("appear", false);
        base.UnTriggerGift();
    }

    public override void TriggerHauntingEffect(float hauntTime, float timeBeforeAbility)
    {
        if (hauntingCoroutine == null)
            hauntingCoroutine = StartCoroutine(StartHaunt(hauntTime, timeBeforeAbility));
        
        else
        {
            StopCoroutine(hauntingCoroutine);
            hauntingCoroutine = StartCoroutine(StartHaunt(hauntTime, timeBeforeAbility));
        }

        base.TriggerHauntingEffect(hauntTime, timeBeforeAbility);
    }

    IEnumerator ReEnableAnimator(float reactivateIn)
    {
        yield return new WaitForSeconds(reactivateIn);
        entityMesh.GetComponent<Animator>().speed = 1f;
        hauntingCoroutine = _haunt = null;
    }

    IEnumerator StartHaunt(float _hauntTime, float delay)
    {
        yield return new WaitForSeconds(delay);
        TriggerStunEffect(_hauntTime);
        entityMesh.GetComponent<Animator>().speed = 0f;

        if (_haunt == null)
            _haunt = StartCoroutine(ReEnableAnimator(_hauntTime));
        
        else
        {
            StopCoroutine(_haunt);
            _haunt = StartCoroutine(ReEnableAnimator(_hauntTime));
        }
    }

    public override IEnumerator DamageFeedback()
    {
        if (!playingFeedback)
        {
            for (int i = 0; i < 3; i++)
            {
                if (manager.enemyParty[idxInManager].alive)
                {
                    entityMesh.SetActive(false);
                    yield return new WaitForSeconds(0.05f);
                    entityMesh.SetActive(true);
                    yield return new WaitForSeconds(0.05f);
                }
            }
        }

        yield return base.DamageFeedback();
    }

    public override IEnumerator IncreaseStatTimer(string _stat, int _value, float _duration, bool _operation, float _deltaBefore)
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
    }

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
            StartCoroutine(DamageFeedback());

        else
            entityMesh.SetActive(false);
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

        if (hasMunchies)
        {
            hasMunchies = false;

            if (munchiesCoroutine != null)
                StopCoroutine(munchiesCoroutine);
            
            if (hauntingCoroutine != null)
                StopCoroutine(hauntingCoroutine);
            
            if (_haunt != null)
                StopCoroutine(_haunt);

            if (manager.battleMusicContext == GameManager.BattleMusicContext.Fight)
                manager.enemyParty[idxInManager].munchieObject.GetComponent<Animator>().SetBool("appear_munchie", false);

            else
                manager.bossParty.munchieObject.GetComponent<Animator>().SetBool("appear_munchie", false);
        }

        StopCoroutine(DamageFeedback());

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
            
            hasMunchies = true;
        }

        else
            StopCoroutine(munchiesCoroutine);

        munchiesCoroutine = StartCoroutine(MunchiesBehaviour(_munchieDuration, _munchieDamage, _timeBtwMunchieDamage));

        base.TriggerMunchies(_munchieDuration, _munchieDamage, _timeBtwMunchieDamage);
    }

    IEnumerator MunchiesBehaviour(float _munchieDuration, int _munchieDamage, Vector2 _timeBtwMunchieDamage)
    {
        StartCoroutine(UnTriggerMunchies(_munchieDuration));

        while (hasMunchies)
        {
            float realTime = Random.Range(_timeBtwMunchieDamage.x, _timeBtwMunchieDamage.y);
            yield return new WaitForSeconds(realTime);

            if (manager.battleMusicContext == GameManager.BattleMusicContext.Fight)
                manager.enemyParty[idxInManager].munchieObject.GetComponent<Animator>().SetTrigger("attack_munchie");

            else
                manager.bossParty.munchieObject.GetComponent<Animator>().SetTrigger("attack_munchie");

            yield return new WaitForSeconds(.1f);
            MakeDamage(_munchieDamage, "normal");
        }
    }

    public override IEnumerator UnTriggerMunchies(float duration)
    {
        yield return new WaitForSeconds(duration);

        StopCoroutine(munchiesCoroutine);
        hasMunchies = false;
        
        if (manager.battleMusicContext == GameManager.BattleMusicContext.Fight)
            manager.enemyParty[idxInManager].munchieObject.GetComponent<Animator>().SetBool("appear_munchie", false);

        else
            manager.bossParty.munchieObject.GetComponent<Animator>().SetBool("appear_munchie", false);

        yield return base.UnTriggerMunchies(duration);
    }
}

[System.Serializable]
public class EnemySpecialAbility
{
    public AnimatronicAbility _ability;
    [Range(5f, 100f)]
    public float chanceOfUsing;
}
