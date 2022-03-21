using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stat Ability", menuName = "Animatronic Ability/Stat Ability")]
public class StatAbility : AnimatronicAbility
{
    [Header ("Stat Ability")]
    [Range(1, 4)]
    public int targets;
    public bool randomTargets;
    public AnimatronicStats[] statsToAffect;
    
    List<int> unusedIndexOrder = new List<int>();
 
    public override void ApplyEffect(bool entity, bool flip = false, bool offset = false)
    {
        GameManager manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        foreach (var t in statsToAffect)
        {
            if (t.abilityToAffect != AnimatronicStats.AbilityToAffect.MimicBall)
            {
                if (entity)
                {
                    if (t.abilityToAffect != AnimatronicStats.AbilityToAffect.NeonWall &&
                        t.abilityToAffect != AnimatronicStats.AbilityToAffect.BubbleBreath &&
                        t.abilityToAffect != AnimatronicStats.AbilityToAffect.Munchies &&
                        t.abilityToAffect != AnimatronicStats.AbilityToAffect.Haunt)
                    {
                        for (int j = 0; j < manager.animatronicParty.Length; j++)
                        {
                            switch (t.abilityToAffect)
                            {
                                case AnimatronicStats.AbilityToAffect.Attack:
                                    if (t.affectValue == AnimatronicStats.AffectValue.PercentageBased)
                                    {
                                        int increaseValue = manager.GetValueFromPercentage(manager.animatronicParty[j].animatronicItem.currentAttack, t.percentageValue);
                                        manager.StartCoroutine(manager.animatronicParty[j].animatronicItem.IncreaseStatTimer("ATK", increaseValue, t.increaseStatTime, t.affectMode == AnimatronicStats.AffectMode.Additive, timeBeforeAbility));
                                    }

                                    else if (t.affectValue == AnimatronicStats.AffectValue.PointBased)
                                        manager.StartCoroutine(manager.animatronicParty[j].animatronicItem.IncreaseStatTimer("ATK", t.pointValue, t.increaseStatTime, t.affectMode == AnimatronicStats.AffectMode.Additive, timeBeforeAbility));
                                    break;

                                case AnimatronicStats.AbilityToAffect.Defense:
                                    if (t.affectValue == AnimatronicStats.AffectValue.PercentageBased)
                                    {
                                        int increaseValue = manager.GetValueFromPercentage(manager.animatronicParty[j].animatronicItem.currentDefense, t.percentageValue);
                                        manager.StartCoroutine(manager.animatronicParty[j].animatronicItem.IncreaseStatTimer("DEF", increaseValue, t.increaseStatTime, t.affectMode == AnimatronicStats.AffectMode.Additive, timeBeforeAbility));
                                    }

                                    else if (t.affectValue == AnimatronicStats.AffectValue.PointBased)
                                        manager.StartCoroutine(manager.animatronicParty[j].animatronicItem.IncreaseStatTimer("DEF", t.pointValue, t.increaseStatTime, t.affectMode == AnimatronicStats.AffectMode.Additive, timeBeforeAbility));
                                    break;

                                case AnimatronicStats.AbilityToAffect.CriticalChance:
                                    if (t.affectValue == AnimatronicStats.AffectValue.PercentageBased)
                                    {
                                        int increaseValue = manager.GetValueFromPercentage(manager.animatronicParty[j].animatronicItem.currentDefense, t.percentageValue);
                                        manager.StartCoroutine(manager.animatronicParty[j].animatronicItem.IncreaseStatTimer("CRT", increaseValue, t.increaseStatTime, t.affectMode == AnimatronicStats.AffectMode.Additive, timeBeforeAbility));
                                    }

                                    else if (t.affectValue == AnimatronicStats.AffectValue.PointBased)
                                        manager.StartCoroutine(manager.animatronicParty[j].animatronicItem.IncreaseStatTimer("CRT", t.pointValue, t.increaseStatTime, t.affectMode == AnimatronicStats.AffectMode.Additive, timeBeforeAbility));
                                    break;

                                case AnimatronicStats.AbilityToAffect.Stun:
                                    manager.animatronicParty[j].animatronicItem.TriggerStunEffect(t.stunTime);
                                    break;

                                case AnimatronicStats.AbilityToAffect.InstaKill:
                                    switch (t.instaKillMode)
                                    {
                                        case AnimatronicStats.InstaKillMode.ChanceBased:
                                            if (manager.GetRandomBoolChance(t.chanceToInstaKill))
                                                if (manager.animatronicParty[j].alive)
                                                    manager.animatronicParty[j].animatronicItem.MakeDamage(manager.animatronicParty[j].animatronicItem.currentHealth);
                                            break;

                                        case AnimatronicStats.InstaKillMode.HealthBased:
                                            int minHealthValue = manager.GetValueFromPercentage(manager.animatronicParty[j].animatronicItem.currentHealth, t.minHealthPercentageToKill);

                                            if (minHealthValue >= manager.animatronicParty[j].animatronicItem.currentHealth)
                                                if (manager.animatronicParty[j].alive)
                                                    manager.animatronicParty[j].animatronicItem.MakeDamage(manager.animatronicParty[j].animatronicItem.currentHealth);
                                            break;

                                        case AnimatronicStats.InstaKillMode.TrueMode:
                                            if (manager.animatronicParty[j].alive)
                                                manager.animatronicParty[j].animatronicItem.MakeDamage(manager.animatronicParty[j].animatronicItem.currentHealth);
                                            break;
                                    }
                                    break;

                                case AnimatronicStats.AbilityToAffect.Gifts:
                                    manager.animatronicParty[j].animatronicItem.TriggerGift();
                                    break;

                                case AnimatronicStats.AbilityToAffect.OtherSpecific:
                                    foreach (AnimatronicAbility aa in t.specificAbilities)
                                        aa.ApplyEffect(entity, flip, offset);
                                    break;
                            }
                        }
                    }

                    else if (t.abilityToAffect == AnimatronicStats.AbilityToAffect.NeonWall)
                        manager.NeonWallActivation("animatronic", t.neonWallType, t.neonWallDuration);
                    
                    else if (t.abilityToAffect == AnimatronicStats.AbilityToAffect.BubbleBreath)
                        manager.BubbleBreathActivation("animatronic", t.bubbleBreathDuration);
                    
                    else if (t.abilityToAffect == AnimatronicStats.AbilityToAffect.Munchies)
                    {
                        var affectedByMunchie = manager.GetEntityList("animatronic", targets, randomTargets);
                        
                        for (int j = 0; j < affectedByMunchie.Count; j++)
                            manager.animatronicParty[affectedByMunchie[j]].animatronicItem.TriggerMunchies(t.munchiesDuration, t.munchiesDamage, t.timeBtwMunchiesAttack);
                    }
                }       

                else
                {
                    if (manager.battleMusicContext == GameManager.BattleMusicContext.Fight)
                    {
                        if (t.abilityToAffect != AnimatronicStats.AbilityToAffect.Haunt &&
                            t.abilityToAffect != AnimatronicStats.AbilityToAffect.InstaKill &&
                            t.abilityToAffect != AnimatronicStats.AbilityToAffect.Munchies &&
                            t.abilityToAffect != AnimatronicStats.AbilityToAffect.OtherSpecific)
                        {
                            for (int j = 0; j < manager.enemyParty.Length; j++)
                            {
                                switch (t.abilityToAffect)
                                {
                                    case AnimatronicStats.AbilityToAffect.Attack:
                                        if (t.affectValue == AnimatronicStats.AffectValue.PercentageBased)
                                        {
                                            int increaseValue = manager.GetValueFromPercentage(manager.enemyParty[j].enemyItem.currentAttack, t.percentageValue);
                                            manager.StartCoroutine(manager.enemyParty[j].enemyItem.IncreaseStatTimer("ATK", increaseValue, t.increaseStatTime, t.affectMode == AnimatronicStats.AffectMode.Additive, timeBeforeAbility));
                                        }

                                        else if (t.affectValue == AnimatronicStats.AffectValue.PointBased)
                                            manager.StartCoroutine(manager.enemyParty[j].enemyItem.IncreaseStatTimer("ATK", t.pointValue, t.increaseStatTime, t.affectMode == AnimatronicStats.AffectMode.Additive, timeBeforeAbility));
                                        break;

                                    case AnimatronicStats.AbilityToAffect.Defense:
                                        if (t.affectValue == AnimatronicStats.AffectValue.PercentageBased)
                                        {
                                            Debug.Log("huh?");
                                            int increaseValue = manager.GetValueFromPercentage(manager.enemyParty[j].enemyItem.currentAttack, t.percentageValue);
                                            manager.StartCoroutine(manager.enemyParty[j].enemyItem.IncreaseStatTimer("DEF", increaseValue, t.increaseStatTime, t.affectMode == AnimatronicStats.AffectMode.Additive, timeBeforeAbility));
                                        }

                                        else if (t.affectValue == AnimatronicStats.AffectValue.PointBased)
                                            manager.StartCoroutine(manager.enemyParty[j].enemyItem.IncreaseStatTimer("DEF", t.pointValue, t.increaseStatTime, t.affectMode == AnimatronicStats.AffectMode.Additive, timeBeforeAbility));
                                        break;

                                    case AnimatronicStats.AbilityToAffect.Stun:
                                        manager.enemyParty[j].enemyItem.TriggerStunEffect(t.stunTime);
                                        break;
                                }
                            }
                        }

                        else if (t.abilityToAffect == AnimatronicStats.AbilityToAffect.Haunt)
                        {
                            List<int> entityIdxHaunt = manager.GetEntityList("enemy", targets, randomTargets);

                            for (int j = 0; j < entityIdxHaunt.Count; j++)
                                manager.enemyParty[entityIdxHaunt[j]].enemyItem.TriggerHauntingEffect(t.hauntDuration, timeBeforeAbility);
                        }

                        else if (t.abilityToAffect == AnimatronicStats.AbilityToAffect.OtherSpecific)
                        {
                            manager.StartCoroutine(ActivateAbilityAfterDelay(t.specificAbilities, t.otherSpecificExecution, entity, flip, offset));
                        }

                        else if (t.abilityToAffect == AnimatronicStats.AbilityToAffect.InstaKill)
                        {
                            List<int> entityIdxInsta = manager.GetEntityList("enemy", targets, randomTargets);

                            switch (t.instaKillMode)
                            {
                                case AnimatronicStats.InstaKillMode.ChanceBased:
                                    Debug.Log(entityIdxInsta[0] + " " + t.chanceToInstaKill);

                                    for (int j = 0; j < entityIdxInsta.Count; j++)
                                    {   
                                        if (manager.GetRandomBoolChance(t.chanceToInstaKill))
                                            manager.StartCoroutine(manager.enemyParty[entityIdxInsta[j]].enemyItem.DelayInstaKill(timeBeforeAbility));
                                        
                                        else
                                            manager.StartCoroutine(manager.enemyParty[entityIdxInsta[j]].enemyItem.FailedInstaKill(timeBeforeAbility));
                                    }
                                    break;

                                case AnimatronicStats.InstaKillMode.HealthBased:
                                    for (int j = 0; j < entityIdxInsta.Count; j++)
                                    {
                                        int healthValuePercentage = manager.GetValueFromPercentage(manager.enemyParty[entityIdxInsta[j]].enemyItem.GetComponent<Enemy>().enemyData.maxHealth, t.minHealthPercentageToKill);

                                        if (manager.enemyParty[entityIdxInsta[j]].enemyItem.currentHealth <= healthValuePercentage)
                                            manager.StartCoroutine(manager.enemyParty[entityIdxInsta[j]].enemyItem.DelayInstaKill(timeBeforeAbility));
                                    }
                                    break;

                                case AnimatronicStats.InstaKillMode.TrueMode:
                                    for (int j = 0; j < entityIdxInsta.Count; j++)
                                        manager.StartCoroutine(manager.enemyParty[entityIdxInsta[j]].enemyItem.DelayInstaKill(timeBeforeAbility));
                                    break;
                            }
                        }

                        else if (t.abilityToAffect == AnimatronicStats.AbilityToAffect.Munchies)
                        {
                            List<int> affectedByMunchie = manager.GetEntityList("enemy", targets, randomTargets);

                            for (int j = 0; j < affectedByMunchie.Count; j++)
                                manager.enemyParty[affectedByMunchie[j]].enemyItem.TriggerMunchies(t.munchiesDuration, t.munchiesDamage, t.timeBtwMunchiesAttack);
                        }
                    }

                    else
                    {
                        Debug.Log("instaboss");
                        if (t.abilityToAffect != AnimatronicStats.AbilityToAffect.Haunt &&
                            t.abilityToAffect != AnimatronicStats.AbilityToAffect.InstaKill)
                        {
                            Debug.Log("instao");
                            switch (t.abilityToAffect)
                            {
                                case AnimatronicStats.AbilityToAffect.Defense:
                                    int increaseValue = manager.GetValueFromPercentage(manager.bossParty.enemyItem.currentDefense, t.percentageValue);
                                    manager.StartCoroutine(manager.bossParty.enemyItem.IncreaseStatTimer("DEF", increaseValue, t.increaseStatTime, t.affectMode == AnimatronicStats.AffectMode.Additive, timeBeforeAbility));
                                    break;
                                
                                case AnimatronicStats.AbilityToAffect.Stun:
                                    manager.bossParty.enemyItem.TriggerStunEffect(t.stunTime);
                                    break;

                                case AnimatronicStats.AbilityToAffect.Haunt:
                                    manager.bossParty.enemyItem.TriggerHauntingEffect(t.hauntDuration, timeBeforeAbility);
                                    break;

                                case AnimatronicStats.AbilityToAffect.OtherSpecific:
                                    manager.StartCoroutine(ActivateAbilityAfterDelay(t.specificAbilities, t.otherSpecificExecution, entity, flip, offset));
                                    break;
                            }
                        }

                        else if (t.abilityToAffect == AnimatronicStats.AbilityToAffect.Haunt)
                        {
                            Debug.Log("instah");
                            unusedIndexOrder.Clear();
                            manager.bossParty.enemyItem.TriggerHauntingEffect(t.hauntDuration, timeBeforeAbility);
                        }

                        else if (t.abilityToAffect == AnimatronicStats.AbilityToAffect.InstaKill)
                        {
                            Debug.Log("insta");
                            switch (t.instaKillMode)
                            {
                                case AnimatronicStats.InstaKillMode.ChanceBased:
                                    if (manager.GetRandomBoolChance(t.chanceToInstaKill))
                                        manager.bossParty.enemyItem.DelayInstaKill(timeBeforeAbility);
                                    
                                    else
                                        Debug.Log("INSTA FAILED");
                                    break;

                                case AnimatronicStats.InstaKillMode.HealthBased:
                                    int trueHealth = manager.GetValueFromPercentage(manager.bossParty.enemyItem.GetComponent<Enemy>().enemyData.maxHealth, t.minHealthPercentageToKill);

                                    if (manager.bossParty.enemyItem.currentHealth <= trueHealth)
                                        manager.bossParty.enemyItem.DelayInstaKill(timeBeforeAbility);
                                    break;

                                case AnimatronicStats.InstaKillMode.TrueMode:
                                    manager.bossParty.enemyItem.DelayInstaKill(timeBeforeAbility);
                                    break;
                            }
                        }
                    }
                }
            }

            else
            if (!manager.hasMimicBall)
                manager.TriggerMimicBall();
        }

        manager.StartCoroutine(AbilitySoundEffect());

        base.ApplyEffect(entity, flip, offset);
    }

    IEnumerator AbilitySoundEffect()
    {   
        GameManager manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        switch (sFXPlayMode)
        {   
            case SFXPlayMode.Continuous:
                foreach (AbilitySoundEffect ase in abilitySoundEffect)
                {
                    yield return new WaitForSeconds(ase.delayToPlaySFX);

                    if (ase.SFX != null)
                    {
                        for (int i = 0; i < ase.amountToPlayAudio; i++)
                        {
                            yield return new WaitForSeconds(Random.Range(ase.intervalsBtwPlays.x, ase.intervalsBtwPlays.y));
                            manager.PlaySoundEffect(ase.SFX);
                        }
                    }
                }
            break;

            case SFXPlayMode.SelectOneRandom:
                int randomSound = Random.Range(0, abilitySoundEffect.Length);

                yield return new WaitForSeconds(abilitySoundEffect[randomSound].delayToPlaySFX);

                if (abilitySoundEffect[randomSound].SFX != null)
                {
                    for (int i = 0; i < abilitySoundEffect[randomSound].amountToPlayAudio; i++)
                    {
                        yield return new WaitForSeconds(Random.Range(abilitySoundEffect[randomSound].intervalsBtwPlays.x, abilitySoundEffect[randomSound].intervalsBtwPlays.y));
                        manager.PlaySoundEffect(abilitySoundEffect[randomSound].SFX);
                    }
                }
            break;

            case SFXPlayMode.SelectMultipleRandom:
                for (int i = 0; i < timeOfRandomIterations; i++)
                {
                    int randomSFX = Random.Range(0, abilitySoundEffect.Length);
                    AbilitySoundEffect _ase = abilitySoundEffect[randomSFX];
                    
                    yield return new WaitForSeconds(_ase.delayToPlaySFX);
                    manager.PlaySoundEffect(_ase.SFX);

                    yield return new WaitForSeconds(globalIntervalsBtwSFX);
                }
            break;
        }
    }

    IEnumerator ActivateAbilityAfterDelay(AnimatronicAbility[] abs, AnimatronicStats.OtherSpecificExecution ose, bool entity, bool flip, bool offset)
    {
        Debug.Log(ose);

        yield return new WaitForSeconds(timeBeforeAbility);

        if (ose == AnimatronicStats.OtherSpecificExecution.All)
            foreach (AnimatronicAbility aa in abs)
                aa.ApplyEffect(entity, flip, offset);
                                        
        else if (ose == AnimatronicStats.OtherSpecificExecution.SelectRandom)
        {
            int specificAbility = Random.Range(0, abs.Length);
            Debug.Log(abs[specificAbility].abilityName);
            abs[specificAbility].ApplyEffect(entity, flip, offset);
        }
    }
}

[System.Serializable]
public class AnimatronicStats
{
    public enum AbilityToAffect 
    { 
        Attack, 
        Defense, 
        CriticalChance, 
        Stun, 
        InstaKill, 
        OtherSpecific, 
        Gifts, 
        Haunt,
        Munchies,
        NeonWall,
        BubbleBreath,
        MimicBall
    }

    public AbilityToAffect abilityToAffect;
    public enum EntityToAffect { Animatronic, Enemy }
    public EntityToAffect entityToAffect;
    public enum AffectMode { Additive, Decreative }
    public AffectMode affectMode;
    public enum AffectValue { PointBased, PercentageBased }
    public AffectValue affectValue;
    public enum InstaKillMode { ChanceBased, HealthBased, TrueMode }
    public InstaKillMode instaKillMode;
    [Tooltip ("Leave empty if using points.")]
    [Range (0f, 100f)]
    public float percentageValue;
    [Tooltip ("Leave empty if using percentage.")]
    [Range(0, 100)]
    public int pointValue;
    public float increaseStatTime;
    [Header ("Stun Fields")]
    public float stunTime;
    [Header ("Insta Kill Fields")]
    [Range (15f, 100f)]
    public float chanceToInstaKill;
    [Range (10f, 100f)]
    public float minHealthPercentageToKill;
    [Header ("Haunt Ability Fields")]
    public float hauntDuration;
    [Header ("Other Specifics Fields")]
    public AnimatronicAbility[] specificAbilities;
    public enum OtherSpecificExecution { All, SelectRandom }
    public OtherSpecificExecution otherSpecificExecution;
    [Header ("Munchies Specific Fields")]
    public Vector2 timeBtwMunchiesAttack;
    public int munchiesDamage;
    public float munchiesDuration;
    [Header ("Neon Wall Specific Fields")]
    [Range(0, 1)]
    public int neonWallType;
    public float neonWallDuration;
    [Header ("Bubble Breath Specific Fields")]
    public float bubbleBreathDuration;
}
