using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stat Ability", menuName = "Animatronic Ability/Stat Ability")]
public class StatAbility : AnimatronicAbility
{
    public enum SupposedToAffect { SameEntity, OppositeEntity }
    [Header ("Stat Ability")]
    public SupposedToAffect supposedToAffect;
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
            {   //Animatronics
                if (entity)
                {
                    if (t.abilityToAffect == AnimatronicStats.AbilityToAffect.Attack)
                    {
                        if (supposedToAffect == SupposedToAffect.SameEntity)
                        {
                            List<int> aliveIdx = manager.GetEntityList("animatronic", targets, randomTargets);

                            if (t.affectValue == AnimatronicStats.AffectValue.PercentageBased)
                            {
                                for (int i = 0; i < aliveIdx.Count; i++)
                                {
                                    int increaseValue = manager.GetValueFromPercentage(
                                    manager.animatronicParty[aliveIdx[i]].animatronicItem.currentAttack, t.percentageValue);
                                    manager.animatronicParty[aliveIdx[i]].animatronicItem.AddStatEffectToList(
                                        abilityName, increaseValue, t.affectMode == AnimatronicStats.AffectMode.Additive, t.increaseStatTime, AllStatsMods.StatType.ATK
                                    );
                                }
                            }

                            else
                            {
                                for (int i = 0; i < aliveIdx.Count; i++)
                                {
                                    manager.animatronicParty[aliveIdx[i]].animatronicItem.AddStatEffectToList(
                                        abilityName, t.pointValue, t.affectMode == AnimatronicStats.AffectMode.Additive, t.increaseStatTime, AllStatsMods.StatType.ATK
                                    );
                                }
                            }
                        }

                        else
                        {
                            List<int> aliveIdx = manager.GetEntityList("enemy", targets, randomTargets);

                            if (t.affectValue == AnimatronicStats.AffectValue.PercentageBased)
                            {
                                for (int i = 0; i < aliveIdx.Count; i++)
                                {
                                    int increaseValue = manager.GetValueFromPercentage(
                                    manager.enemyParty[aliveIdx[i]].enemyItem.currentAttack, t.percentageValue);
                                    manager.enemyParty[aliveIdx[i]].enemyItem.AddStatEffectToList(
                                        abilityName, increaseValue, t.affectMode == AnimatronicStats.AffectMode.Additive, t.increaseStatTime, AllStatsMods.StatType.ATK
                                    );
                                }
                            }

                            else
                            {
                                for (int i = 0; i < aliveIdx.Count; i++)
                                {
                                    manager.enemyParty[aliveIdx[i]].enemyItem.AddStatEffectToList(
                                        abilityName, t.pointValue, t.affectMode == AnimatronicStats.AffectMode.Additive, t.increaseStatTime, AllStatsMods.StatType.ATK
                                    );
                                }
                            }
                        }
                    }

                    else if (t.abilityToAffect == AnimatronicStats.AbilityToAffect.Defense)
                    {
                        if (supposedToAffect == SupposedToAffect.SameEntity)
                        {
                            List<int> aliveIdx = manager.GetEntityList("animatronic", targets, randomTargets);

                            if (t.affectValue == AnimatronicStats.AffectValue.PercentageBased)
                            {
                                for (int i = 0; i < aliveIdx.Count; i++)
                                {
                                    int increaseValue = manager.GetValueFromPercentage(
                                    manager.animatronicParty[aliveIdx[i]].animatronicItem.currentAttack, t.percentageValue);
                                    manager.animatronicParty[aliveIdx[i]].animatronicItem.AddStatEffectToList(
                                        abilityName, increaseValue, t.affectMode == AnimatronicStats.AffectMode.Additive, t.increaseStatTime, AllStatsMods.StatType.DEF
                                    );
                                }
                            }

                            else
                            {
                                for (int i = 0; i < aliveIdx.Count; i++)
                                {
                                    manager.animatronicParty[aliveIdx[i]].animatronicItem.AddStatEffectToList(
                                        abilityName, t.pointValue, t.affectMode == AnimatronicStats.AffectMode.Additive, t.increaseStatTime, AllStatsMods.StatType.DEF
                                    );
                                }
                            }
                        }

                        else
                        {
                            List<int> aliveIdx = manager.GetEntityList("enemy", targets, randomTargets);

                            if (t.affectValue == AnimatronicStats.AffectValue.PercentageBased)
                            {
                                for (int i = 0; i < aliveIdx.Count; i++)
                                {
                                    Debug.Log("xd");
                                    int increaseValue = manager.GetValueFromPercentage(
                                    manager.enemyParty[aliveIdx[i]].enemyItem.currentAttack, t.percentageValue);
                                    manager.enemyParty[aliveIdx[i]].enemyItem.AddStatEffectToList(
                                        abilityName, increaseValue, t.affectMode == AnimatronicStats.AffectMode.Additive, t.increaseStatTime, AllStatsMods.StatType.DEF
                                    );
                                }
                            }

                            else
                            {
                                for (int i = 0; i < aliveIdx.Count; i++)
                                {
                                    manager.enemyParty[aliveIdx[i]].enemyItem.AddStatEffectToList(
                                        abilityName, t.pointValue, t.affectMode == AnimatronicStats.AffectMode.Additive, t.increaseStatTime, AllStatsMods.StatType.DEF
                                    );
                                }
                            }
                        }
                    }

                    else if (t.abilityToAffect == AnimatronicStats.AbilityToAffect.CriticalChance)
                    {
                        if (supposedToAffect == SupposedToAffect.SameEntity)
                        {
                            List<int> aliveIdx = manager.GetEntityList("animatronic", targets, randomTargets);

                            if (t.affectValue == AnimatronicStats.AffectValue.PercentageBased)
                            {
                                for (int i = 0; i < aliveIdx.Count; i++)
                                {
                                    int increaseValue = manager.GetValueFromPercentage(
                                    manager.animatronicParty[aliveIdx[i]].animatronicItem.currentAttack, t.percentageValue);
                                    manager.animatronicParty[aliveIdx[i]].animatronicItem.AddStatEffectToList(
                                        abilityName, increaseValue, t.affectMode == AnimatronicStats.AffectMode.Additive, t.increaseStatTime, AllStatsMods.StatType.CRT
                                    );
                                }
                            }

                            else
                            {
                                for (int i = 0; i < aliveIdx.Count; i++)
                                {
                                    manager.animatronicParty[aliveIdx[i]].animatronicItem.AddStatEffectToList(
                                        abilityName, t.pointValue, t.affectMode == AnimatronicStats.AffectMode.Additive, t.increaseStatTime, AllStatsMods.StatType.CRT
                                    );
                                }
                            }
                        }

                        else
                        {
                            List<int> aliveIdx = manager.GetEntityList("enemy", targets, randomTargets);

                            if (t.affectValue == AnimatronicStats.AffectValue.PercentageBased)
                            {
                                for (int i = 0; i < aliveIdx.Count; i++)
                                {
                                    int increaseValue = manager.GetValueFromPercentage(
                                    manager.enemyParty[aliveIdx[i]].enemyItem.currentAttack, t.percentageValue);
                                    manager.enemyParty[aliveIdx[i]].enemyItem.AddStatEffectToList(
                                        abilityName, increaseValue, t.affectMode == AnimatronicStats.AffectMode.Additive, t.increaseStatTime, AllStatsMods.StatType.DEF
                                    );
                                }
                            }

                            else
                            {
                                for (int i = 0; i < aliveIdx.Count; i++)
                                {
                                    manager.enemyParty[aliveIdx[i]].enemyItem.AddStatEffectToList(
                                        abilityName, t.pointValue, t.affectMode == AnimatronicStats.AffectMode.Additive, t.increaseStatTime, AllStatsMods.StatType.DEF
                                    );
                                }
                            }
                        }
                    }

                    else if (t.abilityToAffect == AnimatronicStats.AbilityToAffect.Gifts)
                    {
                        List<int> aliveIdx = manager.GetEntityList("animatronic", targets, randomTargets);

                        for (int i = 0; i < aliveIdx.Count; i++)
                            if (!manager.animatronicParty[aliveIdx[i]].animatronicItem.hasGift)
                                manager.animatronicParty[aliveIdx[i]].animatronicItem.TriggerGift();
                    }

                    else if (t.abilityToAffect == AnimatronicStats.AbilityToAffect.Stun)
                    {
                        List<int> aliveIdx = manager.GetEntityList("enemy", targets, randomTargets);

                        for (int i = 0; i < aliveIdx.Count; i++)
                            manager.enemyParty[aliveIdx[i]].enemyItem.TriggerStunEffect(t.stunTime);
                    }

                    else if (t.abilityToAffect == AnimatronicStats.AbilityToAffect.OtherSpecific)
                    {
                        foreach (AnimatronicAbility aa in t.specificAbilities)
                            aa.ApplyEffect(entity, flip, offset);
                    }

                    else if (t.abilityToAffect == AnimatronicStats.AbilityToAffect.InstaKill)
                    {
                        List<int> aliveIdx = manager.GetEntityList("enemy", targets, randomTargets);

                        switch (t.instaKillMode)
                        {
                            case AnimatronicStats.InstaKillMode.ChanceBased:
                                for (int i = 0; i < aliveIdx.Count; i++)
                                    if (manager.GetRandomBoolChance(t.chanceToInstaKill))
                                        manager.enemyParty[aliveIdx[i]].enemyItem.InstaKill();
                            break;

                            case AnimatronicStats.InstaKillMode.HealthBased:
                                for (int i = 0; i < aliveIdx.Count; i++)
                                {   
                                    int minHealthValue = manager.GetValueFromPercentage(manager.animatronicParty[aliveIdx[i]].animatronicItem.GetComponent<Animatronic>().animatronicData.maxHealth, t.minHealthPercentageToKill);

                                    if (manager.enemyParty[aliveIdx[i]].enemyItem.currentHealth <= minHealthValue)
                                        manager.enemyParty[aliveIdx[i]].enemyItem.InstaKill();
                                }
                            break;

                            case AnimatronicStats.InstaKillMode.TrueMode:
                                for (int i = 0; i < aliveIdx.Count; i++)
                                    manager.enemyParty[aliveIdx[i]].enemyItem.InstaKill();
                            break;
                        }
                    }

                    else if (t.abilityToAffect == AnimatronicStats.AbilityToAffect.NeonWall)
                        manager.NeonWallActivation("animatronic", t.neonWallType, t.neonWallDuration);
                    
                    else if (t.abilityToAffect == AnimatronicStats.AbilityToAffect.BubbleBreath)
                        manager.BubbleBreathActivation("animatronic", t.bubbleBreathDuration);
                    
                    else if (t.abilityToAffect == AnimatronicStats.AbilityToAffect.Munchies)
                    {
                        var affectedByMunchie = manager.GetEntityList("enemy", targets, randomTargets);
                        
                        for (int j = 0; j < affectedByMunchie.Count; j++)
                            manager.enemyParty[affectedByMunchie[j]].enemyItem.TriggerMunchies(t.munchiesDuration, t.munchiesDamage, t.timeBtwMunchiesAttack);
                    }

                    else if (t.abilityToAffect == AnimatronicStats.AbilityToAffect.Haunt)
                    {
                        List<int> entityIdxHaunt = manager.GetEntityList("enemy", targets, randomTargets);

                        for (int j = 0; j < entityIdxHaunt.Count; j++)
                            manager.enemyParty[entityIdxHaunt[j]].enemyItem.TriggerHauntingEffect(t.hauntDuration, timeBeforeAbility);
                    }
                }       
                //Enemies
                else
                {   //Regular Battle
                    if (manager.battleMusicContext == GameManager.BattleMusicContext.Fight)
                    {
                        if (t.abilityToAffect == AnimatronicStats.AbilityToAffect.Attack)
                        {
                            if (supposedToAffect == SupposedToAffect.SameEntity)
                            {
                                List<int> aliveIdx = manager.GetEntityList("enemy", targets, randomTargets);

                                if (t.affectValue == AnimatronicStats.AffectValue.PercentageBased)
                                {
                                    for (int i = 0; i < aliveIdx.Count; i++)
                                    {
                                        int increaseValue = manager.GetValueFromPercentage(
                                        manager.enemyParty[aliveIdx[i]].enemyItem.currentAttack, t.percentageValue);
                                        manager.enemyParty[aliveIdx[i]].enemyItem.AddStatEffectToList(
                                            abilityName, increaseValue, t.affectMode == AnimatronicStats.AffectMode.Additive, t.increaseStatTime, AllStatsMods.StatType.ATK
                                        );
                                    }
                                }

                                else
                                {
                                    for (int i = 0; i < aliveIdx.Count; i++)
                                    {
                                        manager.enemyParty[aliveIdx[i]].enemyItem.AddStatEffectToList(
                                            abilityName, t.pointValue, t.affectMode == AnimatronicStats.AffectMode.Additive, t.increaseStatTime, AllStatsMods.StatType.ATK
                                        );
                                    }
                                }   
                            }

                            else
                            {
                                List<int> aliveIdx = manager.GetEntityList("animatronic", targets, randomTargets);

                                if (t.affectValue == AnimatronicStats.AffectValue.PercentageBased)
                                {
                                    for (int i = 0; i < aliveIdx.Count; i++)
                                    {
                                        int increaseValue = manager.GetValueFromPercentage(
                                        manager.animatronicParty[aliveIdx[i]].animatronicItem.currentAttack, t.percentageValue);
                                        manager.animatronicParty[aliveIdx[i]].animatronicItem.AddStatEffectToList(
                                            abilityName, increaseValue, t.affectMode == AnimatronicStats.AffectMode.Additive, t.increaseStatTime, AllStatsMods.StatType.ATK
                                        );
                                    }
                                }

                                else
                                {
                                    for (int i = 0; i < aliveIdx.Count; i++)
                                    {
                                        manager.animatronicParty[aliveIdx[i]].animatronicItem.AddStatEffectToList(
                                            abilityName, t.pointValue, t.affectMode == AnimatronicStats.AffectMode.Additive, t.increaseStatTime, AllStatsMods.StatType.ATK
                                        );
                                    }
                                }
                            }
                        }

                        else if (t.abilityToAffect == AnimatronicStats.AbilityToAffect.Defense)
                        {
                            if (supposedToAffect == SupposedToAffect.SameEntity)
                            {
                                List<int> aliveIdx = manager.GetEntityList("enemy", targets, randomTargets);

                                if (t.affectValue == AnimatronicStats.AffectValue.PercentageBased)
                                {
                                    for (int i = 0; i < aliveIdx.Count; i++)
                                    {
                                        int increaseValue = manager.GetValueFromPercentage(
                                        manager.enemyParty[aliveIdx[i]].enemyItem.currentAttack, t.percentageValue);
                                        manager.enemyParty[aliveIdx[i]].enemyItem.AddStatEffectToList(
                                            abilityName, increaseValue, t.affectMode == AnimatronicStats.AffectMode.Additive, t.increaseStatTime, AllStatsMods.StatType.DEF
                                        );
                                    }
                                }

                                else
                                {
                                    for (int i = 0; i < aliveIdx.Count; i++)
                                    {
                                        manager.enemyParty[aliveIdx[i]].enemyItem.AddStatEffectToList(
                                            abilityName, t.pointValue, t.affectMode == AnimatronicStats.AffectMode.Additive, t.increaseStatTime, AllStatsMods.StatType.DEF
                                        );
                                    }
                                }   
                            }

                            else
                            {
                                List<int> aliveIdx = manager.GetEntityList("animatronic", targets, randomTargets);

                                if (t.affectValue == AnimatronicStats.AffectValue.PercentageBased)
                                {
                                    for (int i = 0; i < aliveIdx.Count; i++)
                                    {
                                        int increaseValue = manager.GetValueFromPercentage(
                                        manager.animatronicParty[aliveIdx[i]].animatronicItem.currentAttack, t.percentageValue);
                                        manager.animatronicParty[aliveIdx[i]].animatronicItem.AddStatEffectToList(
                                            abilityName, increaseValue, t.affectMode == AnimatronicStats.AffectMode.Additive, t.increaseStatTime, AllStatsMods.StatType.DEF
                                        );
                                    }
                                }

                                else
                                {
                                    for (int i = 0; i < aliveIdx.Count; i++)
                                    {
                                        manager.animatronicParty[aliveIdx[i]].animatronicItem.AddStatEffectToList(
                                            abilityName, t.pointValue, t.affectMode == AnimatronicStats.AffectMode.Additive, t.increaseStatTime, AllStatsMods.StatType.DEF
                                        );
                                    }
                                }
                            }
                        }

                        else if (t.abilityToAffect == AnimatronicStats.AbilityToAffect.CriticalChance)
                        {
                            if (supposedToAffect == SupposedToAffect.SameEntity)
                            {
                                List<int> aliveIdx = manager.GetEntityList("enemy", targets, randomTargets);

                                if (t.affectValue == AnimatronicStats.AffectValue.PercentageBased)
                                {
                                    for (int i = 0; i < aliveIdx.Count; i++)
                                    {
                                        int increaseValue = manager.GetValueFromPercentage(
                                        manager.enemyParty[aliveIdx[i]].enemyItem.currentAttack, t.percentageValue);
                                        manager.enemyParty[aliveIdx[i]].enemyItem.AddStatEffectToList(
                                            abilityName, increaseValue, t.affectMode == AnimatronicStats.AffectMode.Additive, t.increaseStatTime, AllStatsMods.StatType.CRT
                                        );
                                    }
                                }

                                else
                                {
                                    for (int i = 0; i < aliveIdx.Count; i++)
                                    {
                                        manager.enemyParty[aliveIdx[i]].enemyItem.AddStatEffectToList(
                                            abilityName, t.pointValue, t.affectMode == AnimatronicStats.AffectMode.Additive, t.increaseStatTime, AllStatsMods.StatType.CRT
                                        );
                                    }
                                }   
                            }

                            else
                            {
                                List<int> aliveIdx = manager.GetEntityList("animatronic", targets, randomTargets);

                                if (t.affectValue == AnimatronicStats.AffectValue.PercentageBased)
                                {
                                    for (int i = 0; i < aliveIdx.Count; i++)
                                    {
                                        int increaseValue = manager.GetValueFromPercentage(
                                        manager.animatronicParty[aliveIdx[i]].animatronicItem.currentAttack, t.percentageValue);
                                        manager.animatronicParty[aliveIdx[i]].animatronicItem.AddStatEffectToList(
                                            abilityName, increaseValue, t.affectMode == AnimatronicStats.AffectMode.Additive, t.increaseStatTime, AllStatsMods.StatType.CRT
                                        );
                                    }
                                }

                                else
                                {
                                    for (int i = 0; i < aliveIdx.Count; i++)
                                    {
                                        manager.animatronicParty[aliveIdx[i]].animatronicItem.AddStatEffectToList(
                                            abilityName, t.pointValue, t.affectMode == AnimatronicStats.AffectMode.Additive, t.increaseStatTime, AllStatsMods.StatType.CRT
                                        );
                                    }
                                }
                            }
                        }

                        else if (t.abilityToAffect == AnimatronicStats.AbilityToAffect.Gifts)
                        {
                            List<int> aliveIdx = manager.GetEntityList("enemy", targets, randomTargets);

                            for (int i = 0; i < aliveIdx.Count; i++)
                                if (!manager.enemyParty[aliveIdx[i]].enemyItem.hasGift)
                                    manager.enemyParty[aliveIdx[i]].enemyItem.TriggerGift();
                        }

                        else if (t.abilityToAffect == AnimatronicStats.AbilityToAffect.Stun)
                        {
                            List<int> aliveIdx = manager.GetEntityList("animatronic", targets, randomTargets);

                            for (int i = 0; i < aliveIdx.Count; i++)
                                manager.animatronicParty[aliveIdx[i]].animatronicItem.TriggerStunEffect(t.stunTime);
                        }

                        else if (t.abilityToAffect == AnimatronicStats.AbilityToAffect.OtherSpecific)
                        {
                            foreach (AnimatronicAbility aa in t.specificAbilities)
                                aa.ApplyEffect(entity, flip, offset);
                        }

                        else if (t.abilityToAffect == AnimatronicStats.AbilityToAffect.InstaKill)
                        {
                            List<int> aliveIdx = manager.GetEntityList("animatronic", targets, randomTargets);

                            switch (t.instaKillMode)
                            {
                                case AnimatronicStats.InstaKillMode.ChanceBased:
                                    for (int i = 0; i < aliveIdx.Count; i++)
                                        if (manager.GetRandomBoolChance(t.chanceToInstaKill))
                                            manager.animatronicParty[aliveIdx[i]].animatronicItem.InstaKill();
                                break;

                                case AnimatronicStats.InstaKillMode.HealthBased:
                                    for (int i = 0; i < aliveIdx.Count; i++)
                                    {   
                                        int minHealthValue = manager.GetValueFromPercentage(manager.animatronicParty[aliveIdx[i]].animatronicItem.GetComponent<Animatronic>().animatronicData.maxHealth, t.minHealthPercentageToKill);

                                        if (manager.animatronicParty[aliveIdx[i]].animatronicItem.currentHealth <= minHealthValue)
                                            manager.animatronicParty[aliveIdx[i]].animatronicItem.InstaKill();
                                    }
                                break;

                                case AnimatronicStats.InstaKillMode.TrueMode:
                                    for (int i = 0; i < aliveIdx.Count; i++)
                                        manager.animatronicParty[aliveIdx[i]].animatronicItem.InstaKill();
                                break;
                            }
                        }

                        else if (t.abilityToAffect == AnimatronicStats.AbilityToAffect.NeonWall)
                            manager.NeonWallActivation("enemy", t.neonWallType, t.neonWallDuration);
                        
                        else if (t.abilityToAffect == AnimatronicStats.AbilityToAffect.BubbleBreath)
                            manager.BubbleBreathActivation("enemy", t.bubbleBreathDuration);
                        
                        else if (t.abilityToAffect == AnimatronicStats.AbilityToAffect.Munchies)
                        {
                            var affectedByMunchie = manager.GetEntityList("animatronic", targets, randomTargets);
                            
                            for (int j = 0; j < affectedByMunchie.Count; j++)
                                manager.enemyParty[affectedByMunchie[j]].enemyItem.TriggerMunchies(t.munchiesDuration, t.munchiesDamage, t.timeBtwMunchiesAttack);
                        }

                        else if (t.abilityToAffect == AnimatronicStats.AbilityToAffect.Haunt)
                        {
                            List<int> entityIdxHaunt = manager.GetEntityList("animatronic", targets, randomTargets);

                            for (int j = 0; j < entityIdxHaunt.Count; j++)
                                manager.enemyParty[entityIdxHaunt[j]].enemyItem.TriggerHauntingEffect(t.hauntDuration, timeBeforeAbility);
                        }
                    }
                    //Boss Battle
                    else
                    {
                        if (t.abilityToAffect == AnimatronicStats.AbilityToAffect.Attack)
                        {
                            if (supposedToAffect == SupposedToAffect.SameEntity)
                            {
                                if (t.affectValue == AnimatronicStats.AffectValue.PercentageBased)
                                {
                                    int increaseValue = manager.GetValueFromPercentage(
                                    manager.bossParty.enemyItem.currentAttack, t.percentageValue);
                                    manager.bossParty.enemyItem.AddStatEffectToList(
                                        abilityName, increaseValue, t.affectMode == AnimatronicStats.AffectMode.Additive, t.increaseStatTime, AllStatsMods.StatType.ATK
                                    );
                                }

                                else
                                {
                                    int increaseValue = manager.GetValueFromPercentage(
                                    manager.bossParty.enemyItem.currentAttack, t.percentageValue);
                                    manager.bossParty.enemyItem.AddStatEffectToList(
                                        abilityName, t.pointValue, t.affectMode == AnimatronicStats.AffectMode.Additive, t.increaseStatTime, AllStatsMods.StatType.ATK
                                    );
                                }   
                            }

                            else
                            {
                                List<int> aliveIdx = manager.GetEntityList("animatronic", targets, randomTargets);

                                if (t.affectValue == AnimatronicStats.AffectValue.PercentageBased)
                                {
                                    for (int i = 0; i < aliveIdx.Count; i++)
                                    {
                                        int increaseValue = manager.GetValueFromPercentage(
                                        manager.animatronicParty[aliveIdx[i]].animatronicItem.currentAttack, t.percentageValue);
                                        manager.animatronicParty[aliveIdx[i]].animatronicItem.AddStatEffectToList(
                                            abilityName, increaseValue, t.affectMode == AnimatronicStats.AffectMode.Additive, t.increaseStatTime, AllStatsMods.StatType.ATK
                                        );
                                    }
                                }

                                else
                                {
                                    for (int i = 0; i < aliveIdx.Count; i++)
                                    {
                                        manager.animatronicParty[aliveIdx[i]].animatronicItem.AddStatEffectToList(
                                            abilityName, t.pointValue, t.affectMode == AnimatronicStats.AffectMode.Additive, t.increaseStatTime, AllStatsMods.StatType.ATK
                                        );
                                    }
                                }
                            }
                        }

                        else if (t.abilityToAffect == AnimatronicStats.AbilityToAffect.Defense)
                        {
                            if (supposedToAffect == SupposedToAffect.SameEntity)
                            {
                                if (t.affectValue == AnimatronicStats.AffectValue.PercentageBased)
                                {
                                    int increaseValue = manager.GetValueFromPercentage(
                                    manager.bossParty.enemyItem.currentAttack, t.percentageValue);
                                    manager.bossParty.enemyItem.AddStatEffectToList(
                                        abilityName, increaseValue, t.affectMode == AnimatronicStats.AffectMode.Additive, t.increaseStatTime, AllStatsMods.StatType.DEF
                                    );
                                }

                                else
                                {
                                    int increaseValue = manager.GetValueFromPercentage(
                                    manager.bossParty.enemyItem.currentAttack, t.percentageValue);
                                    manager.bossParty.enemyItem.AddStatEffectToList(
                                        abilityName, t.pointValue, t.affectMode == AnimatronicStats.AffectMode.Additive, t.increaseStatTime, AllStatsMods.StatType.DEF
                                    );
                                }
                            }

                            else
                            {
                                List<int> aliveIdx = manager.GetEntityList("animatronic", targets, randomTargets);

                                if (t.affectValue == AnimatronicStats.AffectValue.PercentageBased)
                                {
                                    for (int i = 0; i < aliveIdx.Count; i++)
                                    {
                                        int increaseValue = manager.GetValueFromPercentage(
                                        manager.animatronicParty[aliveIdx[i]].animatronicItem.currentAttack, t.percentageValue);
                                        manager.animatronicParty[aliveIdx[i]].animatronicItem.AddStatEffectToList(
                                            abilityName, increaseValue, t.affectMode == AnimatronicStats.AffectMode.Additive, t.increaseStatTime, AllStatsMods.StatType.DEF
                                        );
                                    }
                                }

                                else
                                {
                                    for (int i = 0; i < aliveIdx.Count; i++)
                                    {
                                        manager.animatronicParty[aliveIdx[i]].animatronicItem.AddStatEffectToList(
                                            abilityName, t.pointValue, t.affectMode == AnimatronicStats.AffectMode.Additive, t.increaseStatTime, AllStatsMods.StatType.DEF
                                        );
                                    }
                                }
                            }
                        }

                        else if (t.abilityToAffect == AnimatronicStats.AbilityToAffect.CriticalChance)
                        {
                            if (supposedToAffect == SupposedToAffect.SameEntity)
                            {
                                if (t.affectValue == AnimatronicStats.AffectValue.PercentageBased)
                                {
                                    int increaseValue = manager.GetValueFromPercentage(
                                    manager.bossParty.enemyItem.currentAttack, t.percentageValue);
                                    manager.bossParty.enemyItem.AddStatEffectToList(
                                        abilityName, increaseValue, t.affectMode == AnimatronicStats.AffectMode.Additive, t.increaseStatTime, AllStatsMods.StatType.CRT
                                    );
                                }

                                else
                                {
                                    int increaseValue = manager.GetValueFromPercentage(
                                    manager.bossParty.enemyItem.currentAttack, t.percentageValue);
                                    manager.bossParty.enemyItem.AddStatEffectToList(
                                        abilityName, t.pointValue, t.affectMode == AnimatronicStats.AffectMode.Additive, t.increaseStatTime, AllStatsMods.StatType.CRT
                                    );
                                } 
                            }

                            else
                            {
                                List<int> aliveIdx = manager.GetEntityList("animatronic", targets, randomTargets);

                                if (t.affectValue == AnimatronicStats.AffectValue.PercentageBased)
                                {
                                    for (int i = 0; i < aliveIdx.Count; i++)
                                    {
                                        int increaseValue = manager.GetValueFromPercentage(
                                        manager.animatronicParty[aliveIdx[i]].animatronicItem.currentAttack, t.percentageValue);
                                        manager.animatronicParty[aliveIdx[i]].animatronicItem.AddStatEffectToList(
                                            abilityName, increaseValue, t.affectMode == AnimatronicStats.AffectMode.Additive, t.increaseStatTime, AllStatsMods.StatType.CRT
                                        );
                                    }
                                }

                                else
                                {
                                    for (int i = 0; i < aliveIdx.Count; i++)
                                    {
                                        manager.animatronicParty[aliveIdx[i]].animatronicItem.AddStatEffectToList(
                                            abilityName, t.pointValue, t.affectMode == AnimatronicStats.AffectMode.Additive, t.increaseStatTime, AllStatsMods.StatType.CRT
                                        );
                                    }
                                }
                            }
                        }

                        else if (t.abilityToAffect == AnimatronicStats.AbilityToAffect.Gifts)
                        {
                            if (!manager.bossParty.enemyItem.hasGift)
                                manager.bossParty.enemyItem.TriggerGift();
                        }

                        else if (t.abilityToAffect == AnimatronicStats.AbilityToAffect.Stun)
                        {
                            List<int> aliveIdx = manager.GetEntityList("animatronic", targets, randomTargets);

                            for (int i = 0; i < aliveIdx.Count; i++)
                                manager.animatronicParty[aliveIdx[i]].animatronicItem.TriggerStunEffect(t.stunTime);
                        }

                        else if (t.abilityToAffect == AnimatronicStats.AbilityToAffect.OtherSpecific)
                        {
                            foreach (AnimatronicAbility aa in t.specificAbilities)
                                aa.ApplyEffect(entity, flip, offset);
                        }

                        else if (t.abilityToAffect == AnimatronicStats.AbilityToAffect.InstaKill)
                        {
                            List<int> aliveIdx = manager.GetEntityList("animatronic", targets, randomTargets);

                            switch (t.instaKillMode)
                            {
                                case AnimatronicStats.InstaKillMode.ChanceBased:
                                    for (int i = 0; i < aliveIdx.Count; i++)
                                        if (manager.GetRandomBoolChance(t.chanceToInstaKill))
                                            manager.animatronicParty[aliveIdx[i]].animatronicItem.InstaKill();
                                break;

                                case AnimatronicStats.InstaKillMode.HealthBased:
                                    for (int i = 0; i < aliveIdx.Count; i++)
                                    {   
                                        int minHealthValue = manager.GetValueFromPercentage(manager.animatronicParty[aliveIdx[i]].animatronicItem.GetComponent<Animatronic>().animatronicData.maxHealth, t.minHealthPercentageToKill);

                                        if (manager.animatronicParty[aliveIdx[i]].animatronicItem.currentHealth <= minHealthValue)
                                            manager.animatronicParty[aliveIdx[i]].animatronicItem.InstaKill();
                                    }
                                break;

                                case AnimatronicStats.InstaKillMode.TrueMode:
                                    for (int i = 0; i < aliveIdx.Count; i++)
                                        manager.animatronicParty[aliveIdx[i]].animatronicItem.InstaKill();
                                break;
                            }
                        }

                        else if (t.abilityToAffect == AnimatronicStats.AbilityToAffect.NeonWall)
                            manager.NeonWallActivation("enemy", t.neonWallType, t.neonWallDuration);
                        
                        else if (t.abilityToAffect == AnimatronicStats.AbilityToAffect.BubbleBreath)
                            manager.BubbleBreathActivation("boss", t.bubbleBreathDuration);
                        
                        else if (t.abilityToAffect == AnimatronicStats.AbilityToAffect.Munchies)
                        {
                            var affectedByMunchie = manager.GetEntityList("animatronic", targets, randomTargets);
                            
                            for (int j = 0; j < affectedByMunchie.Count; j++)
                                manager.enemyParty[affectedByMunchie[j]].enemyItem.TriggerMunchies(t.munchiesDuration, t.munchiesDamage, t.timeBtwMunchiesAttack);
                        }

                        else if (t.abilityToAffect == AnimatronicStats.AbilityToAffect.Haunt)
                        {
                            List<int> entityIdxHaunt = manager.GetEntityList("animatronic", targets, randomTargets);

                            for (int j = 0; j < entityIdxHaunt.Count; j++)
                                manager.enemyParty[entityIdxHaunt[j]].enemyItem.TriggerHauntingEffect(t.hauntDuration, timeBeforeAbility);
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
