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

        for (int o = 0; o < statsToAffect.Length; o++/*var statsToAffect[o] in statsToAffect*/)
        {
            if (statsToAffect[o].abilityToAffect != AnimatronicStats.AbilityToAffect.MimicBall)
            {   //Animatronics
                if (entity)
                {
                    if (statsToAffect[o].abilityToAffect == AnimatronicStats.AbilityToAffect.Attack)
                    {
                        if (supposedToAffect == SupposedToAffect.SameEntity)
                        {
                            List<int> aliveIdx = manager.GetEntityList("animatronic", targets, randomTargets);

                            if (statsToAffect[o].affectValue == AnimatronicStats.AffectValue.PercentageBased)
                            {
                                for (int i = 0; i < aliveIdx.Count; i++)
                                {
                                    int increaseValue = manager.GetValueFromPercentage(
                                    manager.animatronicParty[aliveIdx[i]].animatronicItem.currentAttack, statsToAffect[o].percentageValue);
                                    manager.animatronicParty[aliveIdx[i]].animatronicItem.AddStatEffectToList(
                                        abilityName, increaseValue, statsToAffect[o].affectMode == AnimatronicStats.AffectMode.Additive, statsToAffect[o].increaseStatTime, AllStatsMods.StatType.ATK
                                    );
                                }
                            }

                            else
                            {
                                for (int i = 0; i < aliveIdx.Count; i++)
                                {
                                    manager.animatronicParty[aliveIdx[i]].animatronicItem.AddStatEffectToList(
                                        abilityName, statsToAffect[o].pointValue, statsToAffect[o].affectMode == AnimatronicStats.AffectMode.Additive, statsToAffect[o].increaseStatTime, AllStatsMods.StatType.ATK
                                    );
                                }
                            }

                            for (int i = 0; i < manager.animatronicStats.Length; i++)
                            {
                                if (manager.animatronicStats[i].statUIMod == AllStatsMods.StatType.ATK)
                                {
                                    if (statsToAffect[o].affectMode == AnimatronicStats.AffectMode.Additive) manager.animatronicStats[i].Buff(true, abilityName);
                                    else manager.animatronicStats[i].Nerf(true, abilityName);
                                    break;
                                }
                            }
                        }

                        else
                        {
                            List<int> aliveIdx = manager.GetEntityList("enemy", targets, randomTargets);

                            if (statsToAffect[o].affectValue == AnimatronicStats.AffectValue.PercentageBased)
                            {
                                for (int i = 0; i < aliveIdx.Count; i++)
                                {
                                    int increaseValue = manager.GetValueFromPercentage(
                                    manager.enemyParty[aliveIdx[i]].enemyItem.currentAttack, statsToAffect[o].percentageValue);
                                    manager.enemyParty[aliveIdx[i]].enemyItem.AddStatEffectToList(
                                        abilityName, increaseValue, statsToAffect[o].affectMode == AnimatronicStats.AffectMode.Additive, statsToAffect[o].increaseStatTime, AllStatsMods.StatType.ATK
                                    );
                                }
                            }

                            else
                            {
                                for (int i = 0; i < aliveIdx.Count; i++)
                                {
                                    manager.enemyParty[aliveIdx[i]].enemyItem.AddStatEffectToList(
                                        abilityName, statsToAffect[o].pointValue, statsToAffect[o].affectMode == AnimatronicStats.AffectMode.Additive, statsToAffect[o].increaseStatTime, AllStatsMods.StatType.ATK
                                    );
                                }
                            }

                            for (int i = 0; i < manager.enemyStats.Length; i++)
                            {
                                if (manager.enemyStats[i].statUIMod == AllStatsMods.StatType.ATK)
                                {
                                    if (statsToAffect[o].affectMode == AnimatronicStats.AffectMode.Additive) manager.enemyStats[i].Buff(true, abilityName);
                                    else manager.enemyStats[i].Nerf(true, abilityName);
                                    break;
                                }
                            }
                        }
                    }

                    if (statsToAffect[o].abilityToAffect == AnimatronicStats.AbilityToAffect.Defense)
                    {
                        if (supposedToAffect == SupposedToAffect.SameEntity)
                        {
                            List<int> aliveIdx = manager.GetEntityList("animatronic", targets, randomTargets);
                            
                            if (statsToAffect[o].affectValue == AnimatronicStats.AffectValue.PercentageBased)
                            {
                                for (int i = 0; i < aliveIdx.Count; i++)
                                {
                                    int increaseValue = manager.GetValueFromPercentage(
                                    manager.animatronicParty[aliveIdx[i]].animatronicItem.currentAttack, statsToAffect[o].percentageValue);
                                    manager.animatronicParty[aliveIdx[i]].animatronicItem.AddStatEffectToList(
                                        abilityName, increaseValue, statsToAffect[o].affectMode == AnimatronicStats.AffectMode.Additive, statsToAffect[o].increaseStatTime, AllStatsMods.StatType.DEF
                                    );
                                }
                            }

                            else
                            {
                                for (int i = 0; i < aliveIdx.Count; i++)
                                {
                                    manager.animatronicParty[aliveIdx[i]].animatronicItem.AddStatEffectToList(
                                        abilityName, statsToAffect[o].pointValue, statsToAffect[o].affectMode == AnimatronicStats.AffectMode.Additive, statsToAffect[o].increaseStatTime, AllStatsMods.StatType.DEF
                                    );
                                }
                            }

                            for (int i = 0; i < manager.animatronicStats.Length; i++)
                            {
                                if (manager.animatronicStats[i].statUIMod == AllStatsMods.StatType.DEF)
                                {
                                    if (statsToAffect[o].affectMode == AnimatronicStats.AffectMode.Additive) manager.animatronicStats[i].Buff(true, abilityName);
                                    else manager.animatronicStats[i].Nerf(true, abilityName);
                                    break;
                                }
                            }
                        }

                        else
                        {
                            List<int> aliveIdx = manager.GetEntityList("enemy", targets, randomTargets);

                            if (statsToAffect[o].affectValue == AnimatronicStats.AffectValue.PercentageBased)
                            {
                                for (int i = 0; i < aliveIdx.Count; i++)
                                {
                                    Debug.Log("xd");
                                    int increaseValue = manager.GetValueFromPercentage(
                                    manager.enemyParty[aliveIdx[i]].enemyItem.currentAttack, statsToAffect[o].percentageValue);
                                    manager.enemyParty[aliveIdx[i]].enemyItem.AddStatEffectToList(
                                        abilityName, increaseValue, statsToAffect[o].affectMode == AnimatronicStats.AffectMode.Additive, statsToAffect[o].increaseStatTime, AllStatsMods.StatType.DEF
                                    );
                                }
                            }

                            else
                            {
                                for (int i = 0; i < aliveIdx.Count; i++)
                                {
                                    manager.enemyParty[aliveIdx[i]].enemyItem.AddStatEffectToList(
                                        abilityName, statsToAffect[o].pointValue, statsToAffect[o].affectMode == AnimatronicStats.AffectMode.Additive, statsToAffect[o].increaseStatTime, AllStatsMods.StatType.DEF
                                    );
                                }
                            }

                            for (int i = 0; i < manager.enemyStats.Length; i++)
                            {
                                if (manager.enemyStats[i].statUIMod == AllStatsMods.StatType.DEF)
                                {
                                    if (statsToAffect[o].affectMode == AnimatronicStats.AffectMode.Additive) manager.enemyStats[i].Buff(true, abilityName);
                                    else manager.enemyStats[i].Nerf(true, abilityName);
                                    break;
                                }
                            }
                        }
                    }

                    if (statsToAffect[o].abilityToAffect == AnimatronicStats.AbilityToAffect.CriticalChance)
                    {
                        if (supposedToAffect == SupposedToAffect.SameEntity)
                        {
                            List<int> aliveIdx = manager.GetEntityList("animatronic", targets, randomTargets);

                            if (statsToAffect[o].affectValue == AnimatronicStats.AffectValue.PercentageBased)
                            {
                                for (int i = 0; i < aliveIdx.Count; i++)
                                {
                                    int increaseValue = manager.GetValueFromPercentage(
                                    manager.animatronicParty[aliveIdx[i]].animatronicItem.currentAttack, statsToAffect[o].percentageValue);
                                    manager.animatronicParty[aliveIdx[i]].animatronicItem.AddStatEffectToList(
                                        abilityName, increaseValue, statsToAffect[o].affectMode == AnimatronicStats.AffectMode.Additive, statsToAffect[o].increaseStatTime, AllStatsMods.StatType.CRT
                                    );
                                }
                            }

                            else
                            {
                                for (int i = 0; i < aliveIdx.Count; i++)
                                {
                                    manager.animatronicParty[aliveIdx[i]].animatronicItem.AddStatEffectToList(
                                        abilityName, statsToAffect[o].pointValue, statsToAffect[o].affectMode == AnimatronicStats.AffectMode.Additive, statsToAffect[o].increaseStatTime, AllStatsMods.StatType.CRT
                                    );
                                }
                            }

                            for (int i = 0; i < manager.animatronicStats.Length; i++)
                            {
                                if (manager.animatronicStats[i].statUIMod == AllStatsMods.StatType.CRT)
                                {
                                    if (statsToAffect[o].affectMode == AnimatronicStats.AffectMode.Additive) manager.animatronicStats[i].Buff(true, abilityName);
                                    else manager.animatronicStats[i].Nerf(true, abilityName);
                                    break;
                                }
                            }
                        }

                        else
                        {
                            List<int> aliveIdx = manager.GetEntityList("enemy", targets, randomTargets);

                            if (statsToAffect[o].affectValue == AnimatronicStats.AffectValue.PercentageBased)
                            {
                                for (int i = 0; i < aliveIdx.Count; i++)
                                {
                                    int increaseValue = manager.GetValueFromPercentage(
                                    manager.enemyParty[aliveIdx[i]].enemyItem.currentAttack, statsToAffect[o].percentageValue);
                                    manager.enemyParty[aliveIdx[i]].enemyItem.AddStatEffectToList(
                                        abilityName, increaseValue, statsToAffect[o].affectMode == AnimatronicStats.AffectMode.Additive, statsToAffect[o].increaseStatTime, AllStatsMods.StatType.CRT
                                    );
                                }
                            }

                            else
                            {
                                for (int i = 0; i < aliveIdx.Count; i++)
                                {
                                    manager.enemyParty[aliveIdx[i]].enemyItem.AddStatEffectToList(
                                        abilityName, statsToAffect[o].pointValue, statsToAffect[o].affectMode == AnimatronicStats.AffectMode.Additive, statsToAffect[o].increaseStatTime, AllStatsMods.StatType.CRT
                                    );
                                }
                            }

                            for (int i = 0; i < manager.enemyStats.Length; i++)
                            {
                                if (manager.enemyStats[i].statUIMod == AllStatsMods.StatType.CRT)
                                {
                                    if (statsToAffect[o].affectMode == AnimatronicStats.AffectMode.Additive) manager.enemyStats[i].Buff(true, abilityName);
                                    else manager.enemyStats[i].Nerf(true, abilityName);
                                    break;
                                }
                            }
                        }
                    }

                    if (statsToAffect[o].abilityToAffect == AnimatronicStats.AbilityToAffect.Gifts)
                    {
                        for (int i = 0; i < manager.animatronicParty.Length; i++)
                            if (!manager.animatronicParty[i].animatronicItem.hasGift)
                                manager.animatronicParty[i].animatronicItem.TriggerGift();
                    }

                    if (statsToAffect[o].abilityToAffect == AnimatronicStats.AbilityToAffect.Stun)
                    {
                        List<int> aliveIdx = manager.GetEntityList("enemy", targets, randomTargets);

                        for (int i = 0; i < aliveIdx.Count; i++)
                            manager.enemyParty[aliveIdx[i]].enemyItem.TriggerStunEffect(statsToAffect[o].stunTime);
                    }

                    if (statsToAffect[o].abilityToAffect == AnimatronicStats.AbilityToAffect.OtherSpecific)
                    {
                        foreach (AnimatronicAbility aa in statsToAffect[o].specificAbilities)
                            aa.ApplyEffect(entity, flip, offset);
                    }

                    if (statsToAffect[o].abilityToAffect == AnimatronicStats.AbilityToAffect.InstaKill)
                    {
                        List<int> aliveIdx = manager.GetEntityList("enemy", targets, randomTargets);

                        switch (statsToAffect[o].instaKillMode)
                        {
                            case AnimatronicStats.InstaKillMode.ChanceBased:
                                for (int i = 0; i < aliveIdx.Count; i++)
                                    if (manager.GetRandomBoolChance(statsToAffect[o].chanceToInstaKill))
                                        manager.enemyParty[aliveIdx[i]].enemyItem.InstaKill();
                                    
                                    else
                                        manager.StartCoroutine(manager.enemyParty[aliveIdx[i]].enemyItem.FailedInstaKill(2.5f));
                            break;

                            case AnimatronicStats.InstaKillMode.HealthBased:
                                for (int i = 0; i < aliveIdx.Count; i++)
                                {   
                                    int minHealthValue = manager.GetValueFromPercentage(manager.animatronicParty[aliveIdx[i]].animatronicItem.GetComponent<Animatronic>().animatronicData.maxHealth, statsToAffect[o].minHealthPercentageToKill);

                                    if (manager.enemyParty[aliveIdx[i]].enemyItem.currentHealth <= minHealthValue)
                                        manager.enemyParty[aliveIdx[i]].enemyItem.InstaKill();
                                    
                                    else
                                        manager.StartCoroutine(manager.enemyParty[aliveIdx[i]].enemyItem.FailedInstaKill(2.5f));
                                }
                            break;

                            case AnimatronicStats.InstaKillMode.TrueMode:
                                for (int i = 0; i < aliveIdx.Count; i++)
                                    manager.enemyParty[aliveIdx[i]].enemyItem.InstaKill();
                            break;
                        }
                    }

                    if (statsToAffect[o].abilityToAffect == AnimatronicStats.AbilityToAffect.NeonWall)
                        manager.NeonWallActivation("animatronic", statsToAffect[o].neonWallType, statsToAffect[o].neonWallDuration);
                    
                    if (statsToAffect[o].abilityToAffect == AnimatronicStats.AbilityToAffect.BubbleBreath)
                        manager.BubbleBreathActivation("animatronic", statsToAffect[o].bubbleBreathDuration);
                    
                    if (statsToAffect[o].abilityToAffect == AnimatronicStats.AbilityToAffect.Munchies)
                    {
                        var affectedByMunchie = manager.GetEntityList("enemy", targets, randomTargets);
                        
                        for (int j = 0; j < affectedByMunchie.Count; j++)
                            manager.enemyParty[affectedByMunchie[j]].enemyItem.TriggerMunchies(statsToAffect[o].munchiesDuration, statsToAffect[o].munchiesDamage, statsToAffect[o].timeBtwMunchiesAttack);
                    }

                    if (statsToAffect[o].abilityToAffect == AnimatronicStats.AbilityToAffect.Haunt)
                    {
                        List<int> entityIdxHaunt = manager.GetEntityList("enemy", targets, randomTargets);

                        for (int j = 0; j < entityIdxHaunt.Count; j++)
                            manager.enemyParty[entityIdxHaunt[j]].enemyItem.TriggerHauntingEffect(statsToAffect[o].hauntDuration, timeBeforeAbility);
                    }
                }       
                //Enemies
                else
                {   //Regular Battle
                    if (manager.battleMusicContext == GameManager.BattleMusicContext.Fight)
                    {
                        if (statsToAffect[o].abilityToAffect == AnimatronicStats.AbilityToAffect.Attack)
                        {
                            if (supposedToAffect == SupposedToAffect.SameEntity)
                            {
                                List<int> aliveIdx = manager.GetEntityList("enemy", targets, randomTargets);

                                if (statsToAffect[o].affectValue == AnimatronicStats.AffectValue.PercentageBased)
                                {
                                    for (int i = 0; i < aliveIdx.Count; i++)
                                    {
                                        int increaseValue = manager.GetValueFromPercentage(
                                        manager.enemyParty[aliveIdx[i]].enemyItem.currentAttack, statsToAffect[o].percentageValue);
                                        manager.enemyParty[aliveIdx[i]].enemyItem.AddStatEffectToList(
                                            abilityName, increaseValue, statsToAffect[o].affectMode == AnimatronicStats.AffectMode.Additive, statsToAffect[o].increaseStatTime, AllStatsMods.StatType.ATK
                                        );
                                    }
                                }

                                else
                                {
                                    for (int i = 0; i < aliveIdx.Count; i++)
                                    {
                                        manager.enemyParty[aliveIdx[i]].enemyItem.AddStatEffectToList(
                                            abilityName, statsToAffect[o].pointValue, statsToAffect[o].affectMode == AnimatronicStats.AffectMode.Additive, statsToAffect[o].increaseStatTime, AllStatsMods.StatType.ATK
                                        );
                                    }
                                }

                                for (int i = 0; i < manager.enemyStats.Length; i++)
                                {
                                    if (manager.enemyStats[i].statUIMod == AllStatsMods.StatType.ATK)
                                    {
                                        if (statsToAffect[o].affectMode == AnimatronicStats.AffectMode.Additive) manager.enemyStats[i].Buff(true, abilityName);
                                        else manager.enemyStats[i].Nerf(true, abilityName);
                                        break;
                                    }
                                }
                            }

                            else
                            {
                                List<int> aliveIdx = manager.GetEntityList("animatronic", targets, randomTargets);

                                if (statsToAffect[o].affectValue == AnimatronicStats.AffectValue.PercentageBased)
                                {
                                    for (int i = 0; i < aliveIdx.Count; i++)
                                    {
                                        int increaseValue = manager.GetValueFromPercentage(
                                        manager.animatronicParty[aliveIdx[i]].animatronicItem.currentAttack, statsToAffect[o].percentageValue);
                                        manager.animatronicParty[aliveIdx[i]].animatronicItem.AddStatEffectToList(
                                            abilityName, increaseValue, statsToAffect[o].affectMode == AnimatronicStats.AffectMode.Additive, statsToAffect[o].increaseStatTime, AllStatsMods.StatType.ATK
                                        );
                                    }
                                }

                                else
                                {
                                    for (int i = 0; i < aliveIdx.Count; i++)
                                    {
                                        manager.animatronicParty[aliveIdx[i]].animatronicItem.AddStatEffectToList(
                                            abilityName, statsToAffect[o].pointValue, statsToAffect[o].affectMode == AnimatronicStats.AffectMode.Additive, statsToAffect[o].increaseStatTime, AllStatsMods.StatType.ATK
                                        );
                                    }
                                }

                                for (int i = 0; i < manager.animatronicStats.Length; i++)
                                {
                                    if (manager.animatronicStats[i].statUIMod == AllStatsMods.StatType.ATK)
                                    {
                                        if (statsToAffect[o].affectMode == AnimatronicStats.AffectMode.Additive) manager.animatronicStats[i].Buff(true, abilityName);
                                        else manager.animatronicStats[i].Nerf(true, abilityName);
                                        break;
                                    }
                                }
                            }
                        }

                        else if (statsToAffect[o].abilityToAffect == AnimatronicStats.AbilityToAffect.Defense)
                        {
                            if (supposedToAffect == SupposedToAffect.SameEntity)
                            {
                                List<int> aliveIdx = manager.GetEntityList("enemy", targets, randomTargets);

                                if (statsToAffect[o].affectValue == AnimatronicStats.AffectValue.PercentageBased)
                                {
                                    for (int i = 0; i < aliveIdx.Count; i++)
                                    {
                                        int increaseValue = manager.GetValueFromPercentage(
                                        manager.enemyParty[aliveIdx[i]].enemyItem.currentAttack, statsToAffect[o].percentageValue);
                                        manager.enemyParty[aliveIdx[i]].enemyItem.AddStatEffectToList(
                                            abilityName, increaseValue, statsToAffect[o].affectMode == AnimatronicStats.AffectMode.Additive, statsToAffect[o].increaseStatTime, AllStatsMods.StatType.DEF
                                        );
                                    }
                                }

                                else
                                {
                                    for (int i = 0; i < aliveIdx.Count; i++)
                                    {
                                        manager.enemyParty[aliveIdx[i]].enemyItem.AddStatEffectToList(
                                            abilityName, statsToAffect[o].pointValue, statsToAffect[o].affectMode == AnimatronicStats.AffectMode.Additive, statsToAffect[o].increaseStatTime, AllStatsMods.StatType.DEF
                                        );
                                    }
                                }

                                for (int i = 0; i < manager.enemyStats.Length; i++)
                                {
                                    if (manager.enemyStats[i].statUIMod == AllStatsMods.StatType.DEF)
                                    {
                                        if (statsToAffect[o].affectMode == AnimatronicStats.AffectMode.Additive) manager.enemyStats[i].Buff(true, abilityName);
                                        else manager.enemyStats[i].Nerf(true, abilityName);
                                        break;
                                    }
                                }  
                            }

                            else
                            {
                                List<int> aliveIdx = manager.GetEntityList("animatronic", targets, randomTargets);

                                if (statsToAffect[o].affectValue == AnimatronicStats.AffectValue.PercentageBased)
                                {
                                    for (int i = 0; i < aliveIdx.Count; i++)
                                    {
                                        int increaseValue = manager.GetValueFromPercentage(
                                        manager.animatronicParty[aliveIdx[i]].animatronicItem.currentAttack, statsToAffect[o].percentageValue);
                                        manager.animatronicParty[aliveIdx[i]].animatronicItem.AddStatEffectToList(
                                            abilityName, increaseValue, statsToAffect[o].affectMode == AnimatronicStats.AffectMode.Additive, statsToAffect[o].increaseStatTime, AllStatsMods.StatType.DEF
                                        );
                                    }
                                }

                                else
                                {
                                    for (int i = 0; i < aliveIdx.Count; i++)
                                    {
                                        manager.animatronicParty[aliveIdx[i]].animatronicItem.AddStatEffectToList(
                                            abilityName, statsToAffect[o].pointValue, statsToAffect[o].affectMode == AnimatronicStats.AffectMode.Additive, statsToAffect[o].increaseStatTime, AllStatsMods.StatType.DEF
                                        );
                                    }
                                }

                                for (int i = 0; i < manager.animatronicStats.Length; i++)
                                {
                                    if (manager.animatronicStats[i].statUIMod == AllStatsMods.StatType.DEF)
                                    {
                                        if (statsToAffect[o].affectMode == AnimatronicStats.AffectMode.Additive) manager.animatronicStats[i].Buff(true, abilityName);
                                        else manager.animatronicStats[i].Nerf(true, abilityName);
                                        break;
                                    }
                                }
                            }
                        }

                        else if (statsToAffect[o].abilityToAffect == AnimatronicStats.AbilityToAffect.CriticalChance)
                        {
                            if (supposedToAffect == SupposedToAffect.SameEntity)
                            {
                                List<int> aliveIdx = manager.GetEntityList("enemy", targets, randomTargets);

                                if (statsToAffect[o].affectValue == AnimatronicStats.AffectValue.PercentageBased)
                                {
                                    for (int i = 0; i < aliveIdx.Count; i++)
                                    {
                                        int increaseValue = manager.GetValueFromPercentage(
                                        manager.enemyParty[aliveIdx[i]].enemyItem.currentAttack, statsToAffect[o].percentageValue);
                                        manager.enemyParty[aliveIdx[i]].enemyItem.AddStatEffectToList(
                                            abilityName, increaseValue, statsToAffect[o].affectMode == AnimatronicStats.AffectMode.Additive, statsToAffect[o].increaseStatTime, AllStatsMods.StatType.CRT
                                        );
                                    }
                                }

                                else
                                {
                                    for (int i = 0; i < aliveIdx.Count; i++)
                                    {
                                        manager.enemyParty[aliveIdx[i]].enemyItem.AddStatEffectToList(
                                            abilityName, statsToAffect[o].pointValue, statsToAffect[o].affectMode == AnimatronicStats.AffectMode.Additive, statsToAffect[o].increaseStatTime, AllStatsMods.StatType.CRT
                                        );
                                    }
                                }

                                for (int i = 0; i < manager.enemyStats.Length; i++)
                                {
                                    if (manager.enemyStats[i].statUIMod == AllStatsMods.StatType.CRT)
                                    {
                                        if (statsToAffect[o].affectMode == AnimatronicStats.AffectMode.Additive) manager.enemyStats[i].Buff(true, abilityName);
                                        else manager.enemyStats[i].Nerf(true, abilityName);
                                        break;
                                    }
                                }  
                            }

                            else
                            {
                                List<int> aliveIdx = manager.GetEntityList("animatronic", targets, randomTargets);

                                if (statsToAffect[o].affectValue == AnimatronicStats.AffectValue.PercentageBased)
                                {
                                    for (int i = 0; i < aliveIdx.Count; i++)
                                    {
                                        int increaseValue = manager.GetValueFromPercentage(
                                        manager.animatronicParty[aliveIdx[i]].animatronicItem.currentAttack, statsToAffect[o].percentageValue);
                                        manager.animatronicParty[aliveIdx[i]].animatronicItem.AddStatEffectToList(
                                            abilityName, increaseValue, statsToAffect[o].affectMode == AnimatronicStats.AffectMode.Additive, statsToAffect[o].increaseStatTime, AllStatsMods.StatType.CRT
                                        );
                                    }
                                }

                                else
                                {
                                    for (int i = 0; i < aliveIdx.Count; i++)
                                    {
                                        manager.animatronicParty[aliveIdx[i]].animatronicItem.AddStatEffectToList(
                                            abilityName, statsToAffect[o].pointValue, statsToAffect[o].affectMode == AnimatronicStats.AffectMode.Additive, statsToAffect[o].increaseStatTime, AllStatsMods.StatType.CRT
                                        );
                                    }
                                }

                                for (int i = 0; i < manager.animatronicStats.Length; i++)
                                {
                                    if (manager.animatronicStats[i].statUIMod == AllStatsMods.StatType.CRT)
                                    {
                                        if (statsToAffect[o].affectMode == AnimatronicStats.AffectMode.Additive) manager.animatronicStats[i].Buff(true, abilityName);
                                        else manager.animatronicStats[i].Nerf(true, abilityName);
                                        break;
                                    }
                                }
                            }
                        }

                        else if (statsToAffect[o].abilityToAffect == AnimatronicStats.AbilityToAffect.Gifts)
                        {
                            List<int> aliveIdx = manager.GetEntityList("enemy", targets, randomTargets);

                            for (int i = 0; i < aliveIdx.Count; i++)
                                if (!manager.enemyParty[aliveIdx[i]].enemyItem.hasGift)
                                    manager.enemyParty[aliveIdx[i]].enemyItem.TriggerGift();
                        }

                        else if (statsToAffect[o].abilityToAffect == AnimatronicStats.AbilityToAffect.Stun)
                        {
                            List<int> aliveIdx = manager.GetEntityList("animatronic", targets, randomTargets);

                            for (int i = 0; i < aliveIdx.Count; i++)
                                manager.animatronicParty[aliveIdx[i]].animatronicItem.TriggerStunEffect(statsToAffect[o].stunTime);
                        }

                        else if (statsToAffect[o].abilityToAffect == AnimatronicStats.AbilityToAffect.OtherSpecific)
                        {
                            foreach (AnimatronicAbility aa in statsToAffect[o].specificAbilities)
                                aa.ApplyEffect(entity, flip, offset);
                        }

                        else if (statsToAffect[o].abilityToAffect == AnimatronicStats.AbilityToAffect.InstaKill)
                        {
                            List<int> aliveIdx = manager.GetEntityList("animatronic", targets, randomTargets);

                            switch (statsToAffect[o].instaKillMode)
                            {
                                case AnimatronicStats.InstaKillMode.ChanceBased:
                                    for (int i = 0; i < aliveIdx.Count; i++)
                                        if (manager.GetRandomBoolChance(statsToAffect[o].chanceToInstaKill))
                                            manager.animatronicParty[aliveIdx[i]].animatronicItem.InstaKill();
                                        
                                        else
                                            manager.StartCoroutine(manager.animatronicParty[aliveIdx[i]].animatronicItem.FailedInstaKill(2.5f));
                                break;

                                case AnimatronicStats.InstaKillMode.HealthBased:
                                    for (int i = 0; i < aliveIdx.Count; i++)
                                    {   
                                        int minHealthValue = manager.GetValueFromPercentage(manager.animatronicParty[aliveIdx[i]].animatronicItem.GetComponent<Animatronic>().animatronicData.maxHealth, statsToAffect[o].minHealthPercentageToKill);

                                        if (manager.animatronicParty[aliveIdx[i]].animatronicItem.currentHealth <= minHealthValue)
                                            manager.animatronicParty[aliveIdx[i]].animatronicItem.InstaKill();
                                        
                                        else
                                            manager.StartCoroutine(manager.animatronicParty[aliveIdx[i]].animatronicItem.FailedInstaKill(2.5f));
                                    }
                                break;

                                case AnimatronicStats.InstaKillMode.TrueMode:
                                    for (int i = 0; i < aliveIdx.Count; i++)
                                        manager.animatronicParty[aliveIdx[i]].animatronicItem.InstaKill();
                                break;
                            }
                        }

                        else if (statsToAffect[o].abilityToAffect == AnimatronicStats.AbilityToAffect.NeonWall)
                            manager.NeonWallActivation("enemy", statsToAffect[o].neonWallType, statsToAffect[o].neonWallDuration);
                        
                        else if (statsToAffect[o].abilityToAffect == AnimatronicStats.AbilityToAffect.BubbleBreath)
                            manager.BubbleBreathActivation("enemy", statsToAffect[o].bubbleBreathDuration);
                        
                        else if (statsToAffect[o].abilityToAffect == AnimatronicStats.AbilityToAffect.Munchies)
                        {
                            var affectedByMunchie = manager.GetEntityList("animatronic", targets, randomTargets);
                            
                            for (int j = 0; j < affectedByMunchie.Count; j++)
                                manager.enemyParty[affectedByMunchie[j]].enemyItem.TriggerMunchies(statsToAffect[o].munchiesDuration, statsToAffect[o].munchiesDamage, statsToAffect[o].timeBtwMunchiesAttack);
                        }

                        else if (statsToAffect[o].abilityToAffect == AnimatronicStats.AbilityToAffect.Haunt)
                        {
                            List<int> entityIdxHaunt = manager.GetEntityList("animatronic", targets, randomTargets);

                            for (int j = 0; j < entityIdxHaunt.Count; j++)
                                manager.enemyParty[entityIdxHaunt[j]].enemyItem.TriggerHauntingEffect(statsToAffect[o].hauntDuration, timeBeforeAbility);
                        }
                    }
                    //Boss Battle
                    else
                    {
                        if (statsToAffect[o].abilityToAffect == AnimatronicStats.AbilityToAffect.Attack)
                        {
                            if (supposedToAffect == SupposedToAffect.SameEntity)
                            {
                                if (statsToAffect[o].affectValue == AnimatronicStats.AffectValue.PercentageBased)
                                {
                                    int increaseValue = manager.GetValueFromPercentage(
                                    manager.bossParty.enemyItem.currentAttack, statsToAffect[o].percentageValue);
                                    manager.bossParty.enemyItem.AddStatEffectToList(
                                        abilityName, increaseValue, statsToAffect[o].affectMode == AnimatronicStats.AffectMode.Additive, statsToAffect[o].increaseStatTime, AllStatsMods.StatType.ATK
                                    );
                                }

                                else
                                {
                                    int increaseValue = manager.GetValueFromPercentage(
                                    manager.bossParty.enemyItem.currentAttack, statsToAffect[o].percentageValue);
                                    manager.bossParty.enemyItem.AddStatEffectToList(
                                        abilityName, statsToAffect[o].pointValue, statsToAffect[o].affectMode == AnimatronicStats.AffectMode.Additive, statsToAffect[o].increaseStatTime, AllStatsMods.StatType.ATK
                                    );
                                }   
                            }

                            else
                            {
                                List<int> aliveIdx = manager.GetEntityList("animatronic", targets, randomTargets);

                                if (statsToAffect[o].affectValue == AnimatronicStats.AffectValue.PercentageBased)
                                {
                                    for (int i = 0; i < aliveIdx.Count; i++)
                                    {
                                        int increaseValue = manager.GetValueFromPercentage(
                                        manager.animatronicParty[aliveIdx[i]].animatronicItem.currentAttack, statsToAffect[o].percentageValue);
                                        manager.animatronicParty[aliveIdx[i]].animatronicItem.AddStatEffectToList(
                                            abilityName, increaseValue, statsToAffect[o].affectMode == AnimatronicStats.AffectMode.Additive, statsToAffect[o].increaseStatTime, AllStatsMods.StatType.ATK
                                        );
                                    }
                                }

                                else
                                {
                                    for (int i = 0; i < aliveIdx.Count; i++)
                                    {
                                        manager.animatronicParty[aliveIdx[i]].animatronicItem.AddStatEffectToList(
                                            abilityName, statsToAffect[o].pointValue, statsToAffect[o].affectMode == AnimatronicStats.AffectMode.Additive, statsToAffect[o].increaseStatTime, AllStatsMods.StatType.ATK
                                        );
                                    }
                                }
                            }
                        }

                        else if (statsToAffect[o].abilityToAffect == AnimatronicStats.AbilityToAffect.Defense)
                        {
                            if (supposedToAffect == SupposedToAffect.SameEntity)
                            {
                                if (statsToAffect[o].affectValue == AnimatronicStats.AffectValue.PercentageBased)
                                {
                                    int increaseValue = manager.GetValueFromPercentage(
                                    manager.bossParty.enemyItem.currentAttack, statsToAffect[o].percentageValue);
                                    manager.bossParty.enemyItem.AddStatEffectToList(
                                        abilityName, increaseValue, statsToAffect[o].affectMode == AnimatronicStats.AffectMode.Additive, statsToAffect[o].increaseStatTime, AllStatsMods.StatType.DEF
                                    );
                                }

                                else
                                {
                                    int increaseValue = manager.GetValueFromPercentage(
                                    manager.bossParty.enemyItem.currentAttack, statsToAffect[o].percentageValue);
                                    manager.bossParty.enemyItem.AddStatEffectToList(
                                        abilityName, statsToAffect[o].pointValue, statsToAffect[o].affectMode == AnimatronicStats.AffectMode.Additive, statsToAffect[o].increaseStatTime, AllStatsMods.StatType.DEF
                                    );
                                }
                            }

                            else
                            {
                                List<int> aliveIdx = manager.GetEntityList("animatronic", targets, randomTargets);

                                if (statsToAffect[o].affectValue == AnimatronicStats.AffectValue.PercentageBased)
                                {
                                    for (int i = 0; i < aliveIdx.Count; i++)
                                    {
                                        int increaseValue = manager.GetValueFromPercentage(
                                        manager.animatronicParty[aliveIdx[i]].animatronicItem.currentAttack, statsToAffect[o].percentageValue);
                                        manager.animatronicParty[aliveIdx[i]].animatronicItem.AddStatEffectToList(
                                            abilityName, increaseValue, statsToAffect[o].affectMode == AnimatronicStats.AffectMode.Additive, statsToAffect[o].increaseStatTime, AllStatsMods.StatType.DEF
                                        );
                                    }
                                }

                                else
                                {
                                    for (int i = 0; i < aliveIdx.Count; i++)
                                    {
                                        manager.animatronicParty[aliveIdx[i]].animatronicItem.AddStatEffectToList(
                                            abilityName, statsToAffect[o].pointValue, statsToAffect[o].affectMode == AnimatronicStats.AffectMode.Additive, statsToAffect[o].increaseStatTime, AllStatsMods.StatType.DEF
                                        );
                                    }
                                }
                            }
                        }

                        else if (statsToAffect[o].abilityToAffect == AnimatronicStats.AbilityToAffect.CriticalChance)
                        {
                            if (supposedToAffect == SupposedToAffect.SameEntity)
                            {
                                if (statsToAffect[o].affectValue == AnimatronicStats.AffectValue.PercentageBased)
                                {
                                    int increaseValue = manager.GetValueFromPercentage(
                                    manager.bossParty.enemyItem.currentAttack, statsToAffect[o].percentageValue);
                                    manager.bossParty.enemyItem.AddStatEffectToList(
                                        abilityName, increaseValue, statsToAffect[o].affectMode == AnimatronicStats.AffectMode.Additive, statsToAffect[o].increaseStatTime, AllStatsMods.StatType.CRT
                                    );
                                }

                                else
                                {
                                    int increaseValue = manager.GetValueFromPercentage(
                                    manager.bossParty.enemyItem.currentAttack, statsToAffect[o].percentageValue);
                                    manager.bossParty.enemyItem.AddStatEffectToList(
                                        abilityName, statsToAffect[o].pointValue, statsToAffect[o].affectMode == AnimatronicStats.AffectMode.Additive, statsToAffect[o].increaseStatTime, AllStatsMods.StatType.CRT
                                    );
                                } 
                            }

                            else
                            {
                                List<int> aliveIdx = manager.GetEntityList("animatronic", targets, randomTargets);

                                if (statsToAffect[o].affectValue == AnimatronicStats.AffectValue.PercentageBased)
                                {
                                    for (int i = 0; i < aliveIdx.Count; i++)
                                    {
                                        int increaseValue = manager.GetValueFromPercentage(
                                        manager.animatronicParty[aliveIdx[i]].animatronicItem.currentAttack, statsToAffect[o].percentageValue);
                                        manager.animatronicParty[aliveIdx[i]].animatronicItem.AddStatEffectToList(
                                            abilityName, increaseValue, statsToAffect[o].affectMode == AnimatronicStats.AffectMode.Additive, statsToAffect[o].increaseStatTime, AllStatsMods.StatType.CRT
                                        );
                                    }
                                }

                                else
                                {
                                    for (int i = 0; i < aliveIdx.Count; i++)
                                    {
                                        manager.animatronicParty[aliveIdx[i]].animatronicItem.AddStatEffectToList(
                                            abilityName, statsToAffect[o].pointValue, statsToAffect[o].affectMode == AnimatronicStats.AffectMode.Additive, statsToAffect[o].increaseStatTime, AllStatsMods.StatType.CRT
                                        );
                                    }
                                }
                            }
                        }

                        else if (statsToAffect[o].abilityToAffect == AnimatronicStats.AbilityToAffect.Gifts)
                        {
                            if (!manager.bossParty.enemyItem.hasGift)
                                manager.bossParty.enemyItem.TriggerGift();
                        }

                        else if (statsToAffect[o].abilityToAffect == AnimatronicStats.AbilityToAffect.Stun)
                        {
                            List<int> aliveIdx = manager.GetEntityList("animatronic", targets, randomTargets);

                            for (int i = 0; i < aliveIdx.Count; i++)
                                manager.animatronicParty[aliveIdx[i]].animatronicItem.TriggerStunEffect(statsToAffect[o].stunTime);
                        }

                        else if (statsToAffect[o].abilityToAffect == AnimatronicStats.AbilityToAffect.OtherSpecific)
                        {
                            foreach (AnimatronicAbility aa in statsToAffect[o].specificAbilities)
                                aa.ApplyEffect(entity, flip, offset);
                        }

                        else if (statsToAffect[o].abilityToAffect == AnimatronicStats.AbilityToAffect.InstaKill)
                        {
                            List<int> aliveIdx = manager.GetEntityList("animatronic", targets, randomTargets);

                            switch (statsToAffect[o].instaKillMode)
                            {
                                case AnimatronicStats.InstaKillMode.ChanceBased:
                                    for (int i = 0; i < aliveIdx.Count; i++)
                                        if (manager.GetRandomBoolChance(statsToAffect[o].chanceToInstaKill))
                                            manager.animatronicParty[aliveIdx[i]].animatronicItem.InstaKill();
                                        
                                        else
                                            manager.StartCoroutine(manager.animatronicParty[aliveIdx[i]].animatronicItem.FailedInstaKill(2.5f));
                                break;

                                case AnimatronicStats.InstaKillMode.HealthBased:
                                    for (int i = 0; i < aliveIdx.Count; i++)
                                    {   
                                        int minHealthValue = manager.GetValueFromPercentage(manager.animatronicParty[aliveIdx[i]].animatronicItem.GetComponent<Animatronic>().animatronicData.maxHealth, statsToAffect[o].minHealthPercentageToKill);

                                        if (manager.animatronicParty[aliveIdx[i]].animatronicItem.currentHealth <= minHealthValue)
                                            manager.animatronicParty[aliveIdx[i]].animatronicItem.InstaKill();
                                        
                                        else
                                            manager.StartCoroutine(manager.animatronicParty[aliveIdx[i]].animatronicItem.FailedInstaKill(2.5f));
                                    }
                                break;

                                case AnimatronicStats.InstaKillMode.TrueMode:
                                    for (int i = 0; i < aliveIdx.Count; i++)
                                        manager.animatronicParty[aliveIdx[i]].animatronicItem.InstaKill();
                                break;
                            }
                        }

                        else if (statsToAffect[o].abilityToAffect == AnimatronicStats.AbilityToAffect.NeonWall)
                            manager.NeonWallActivation("enemy", statsToAffect[o].neonWallType, statsToAffect[o].neonWallDuration);
                        
                        else if (statsToAffect[o].abilityToAffect == AnimatronicStats.AbilityToAffect.BubbleBreath)
                            manager.BubbleBreathActivation("boss", statsToAffect[o].bubbleBreathDuration);
                        
                        else if (statsToAffect[o].abilityToAffect == AnimatronicStats.AbilityToAffect.Munchies)
                        {
                            var affectedByMunchie = manager.GetEntityList("animatronic", targets, randomTargets);
                            
                            for (int j = 0; j < affectedByMunchie.Count; j++)
                                manager.enemyParty[affectedByMunchie[j]].enemyItem.TriggerMunchies(statsToAffect[o].munchiesDuration, statsToAffect[o].munchiesDamage, statsToAffect[o].timeBtwMunchiesAttack);
                        }

                        else if (statsToAffect[o].abilityToAffect == AnimatronicStats.AbilityToAffect.Haunt)
                        {
                            List<int> entityIdxHaunt = manager.GetEntityList("animatronic", targets, randomTargets);

                            for (int j = 0; j < entityIdxHaunt.Count; j++)
                                manager.enemyParty[entityIdxHaunt[j]].enemyItem.TriggerHauntingEffect(statsToAffect[o].hauntDuration, timeBeforeAbility);
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
