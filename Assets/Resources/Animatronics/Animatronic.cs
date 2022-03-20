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

    void Update()
    {
        if (!manager.animatronicParty[idxInManager].alive && entityMesh.activeSelf)
            entityMesh.SetActive(false);
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
        {
            StartCoroutine(DamageFeedback());
        }

        else
            entityMesh.SetActive(false);
    }

    public override void Heal(int healthValue, LivingEntity healedEntity)
    {   
        if (hasGift)
        {
            currentGiftHealth += healthValue;

            if (currentGiftHealth >= animatronicData.maxHealth)
                currentGiftHealth = animatronicData.maxHealth;
        }

        else
        {
            currentHealth += healthValue;

            if (currentHealth >= animatronicData.maxHealth)
                currentHealth = animatronicData.maxHealth;
        }

        base.Heal(healthValue, healedEntity);
    }

    public override void ReviveEntity()
    {
        manager.animatronicParty[idxInManager].alive = true;
        manager.animatronicParty[idxInManager].animatronicTombstone.SetActive(false);
        currentHealth = animatronicData.maxHealth;
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

            currentGiftHealth = currentHealth = animatronicData.maxHealth;
            Heal(animatronicData.maxHealth, this);

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
                    manager.imageDefBuffUp.SetActive(true);
                }

                else
                {
                    currentDefense -= _value;
                    manager.imageDefBuffDown.SetActive(true);
                }

                manager.textDefBuff.SetActive(true);
            break;

            case "ATK":
                if (_operation)
                {
                    currentAttack += _value;
                    manager.imageAtkBuffUp.SetActive(true);
                }

                else
                {
                    currentAttack -= _value;
                    manager.imageAtkBuffDown.SetActive(true);
                }

                manager.textAtkBuff.SetActive(true);
            break;

            case "CRT":
                if (_operation)
                {
                    currentCritChance += _value;
                    manager.imageCrtBuffUp.SetActive(true);
                }

                else
                {
                    currentCritChance -= _value;
                    manager.imageCrtBuffDown.SetActive(true);
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
                        manager.imageDefBuffUp.SetActive(false);
                }

                else
                {
                    currentDefense += _value;

                    if (instancesOfNerf <= 1)
                        manager.imageDefBuffDown.SetActive(false);
                }

                if ((!manager.imageDefBuffDown.activeSelf || !manager.imageDefBuffUp.activeSelf) && instancesOfNerf <= 1)
                    manager.textDefBuff.SetActive(false);
            break;

            case "ATK":
                if (_operation)
                {
                    currentAttack -= _value;

                    if (instancesOfNerf <= 1)
                        manager.imageAtkBuffUp.SetActive(false);
                }

                else
                {
                    currentAttack += _value;

                    if (instancesOfNerf <= 1)
                        manager.imageAtkBuffDown.SetActive(false);
                }

                if ((!manager.imageAtkBuffDown.activeSelf || !manager.imageAtkBuffUp.activeSelf) && instancesOfNerf <= 1)
                    manager.textAtkBuff.SetActive(false);
            break;

            case "CRT":
                if (_operation)
                {
                    currentCritChance -= _value;

                    if (instancesOfNerf <= 1)
                        manager.imageCrtBuffUp.SetActive(false);
                }

                else
                {
                    currentCritChance += _value;

                    if (instancesOfNerf <= 1)
                        manager.imageCrtBuffDown.SetActive(false);
                }

                if ((!manager.imageCrtBuffDown.activeSelf || !manager.imageCrtBuffUp.activeSelf) && instancesOfNerf <= 1)
                    manager.textCrtBuff.SetActive(false);
            break;
        }

        instancesOfNerf--;
        yield return base.IncreaseStatTimer(_stat, _value, _duration, _operation, _deltaBefore);
    }

    public override IEnumerator DamageFeedback()
    {
        if (!playingFeedback)
        {
            for (int i = 0; i < 3; i++)
            {
                if (manager.animatronicParty[idxInManager].alive)
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

    public override void TriggerEntityDeath()
    {
        if (manager.currentAnimatronicTurn == this)
            manager.DisableAttackButtons();

        if (manager.animatronicParty[idxInManager].alive)
        {
            manager.CheckForGameOver();
            source.PlayOneShot(deathSound);
        }

        StopCoroutine(DamageFeedback());

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
