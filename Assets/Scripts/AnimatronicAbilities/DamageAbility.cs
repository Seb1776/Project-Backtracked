using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Damage Ability", menuName = "Animatronic Ability/Damage Ability")]
public class DamageAbility : AnimatronicAbility
{
    public enum MakesDamageTo { Enemy, Animatronic }
    [Header ("Damage Ability")]
    public MakesDamageTo makesDamageTo;
    public int damageToDo;
    [Range(1, 4)]
    public int targets;
    [Range(1, 50)]
    public int instanceOfDamage;
    public float timeBtwInstancesOfDamage;
    public bool canPoison;
    public int poisonDamage;
    public float poisonDuration;
    public float timeBtwPoisonDamage;
    public bool randomTargetSelection;

    bool usedFirstDamage;

    public override void ApplyEffect(bool entity, bool flip = false, bool offset = false)
    {
        usedFirstDamage = false;
        GameManager manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        if (entity)
        {
            if (manager.battleMusicContext == GameManager.BattleMusicContext.Fight)
                manager.StartCoroutine(MakeDamageToEnemy(manager.battleMusicContext, manager, manager.GetEntityList("enemy", targets, randomTargetSelection)));
            
            else
                manager.StartCoroutine(MakeDamageToEnemy(manager.battleMusicContext, manager));
        }
        
        else
            manager.StartCoroutine(MakeDamageToAnimatronic(manager.GetEntityList("animatronic", targets, randomTargetSelection), manager));
        
        manager.StartCoroutine(AbilitySoundEffect());

        base.ApplyEffect(entity, flip, offset);
    }

    IEnumerator MakeDamageToEnemy(GameManager.BattleMusicContext _battleContext, GameManager _manager, List<int> idxes = null)
    {
        for (int j = 0; j < instanceOfDamage; j++)
        {   
            if (!usedFirstDamage)
            {
                yield return new WaitForSeconds(timeBeforeAbility);
                usedFirstDamage = true;
            }

            if (_battleContext == GameManager.BattleMusicContext.Fight)
            {
                for (int i = 0; i < idxes.Count; i++)
                {
                    if (_manager.enemyParty[idxes[i]].alive)
                    {
                        _manager.enemyParty[idxes[i]].enemyItem.MakeDamage(damageToDo, "", _manager.currentAnimatronicTurn, _manager.enemyParty[idxes[i]].enemyItem);

                        if (canPoison && !_manager.enemyParty[idxes[i]].enemyItem.hasBubbleBreath)
                            _manager.enemyParty[idxes[i]].enemyItem.TriggerPoisonousEffect(poisonDamage, poisonDuration, timeBtwPoisonDamage);
                    }
                }
            }

            else
            {
                if (_manager.bossParty.alive)
                {
                    _manager.bossParty.enemyItem.MakeDamage(damageToDo, "", _manager.currentAnimatronicTurn, _manager.bossParty.enemyItem);

                    if (canPoison && !_manager.bossParty.enemyItem.hasBubbleBreath)
                        _manager.bossParty.enemyItem.TriggerPoisonousEffect(poisonDamage, poisonDuration, timeBtwPoisonDamage);
                }
            }
            
            yield return new WaitForSeconds(timeBtwInstancesOfDamage);
        }
    }

    IEnumerator MakeDamageToAnimatronic(List<int> idxes, GameManager _manager)
    {
        for (int i = 0; i < instanceOfDamage; i++)
        {
            if (!usedFirstDamage)
            {
                yield return new WaitForSeconds(timeBeforeAbility);
                usedFirstDamage = true;
            }

            for (int j = 0; j < idxes.Count; j++)
            {
                if (_manager.animatronicParty[idxes[j]].alive)
                {
                    _manager.animatronicParty[idxes[j]].animatronicItem.MakeDamage(damageToDo, "", _manager.currentEnemyTurn, _manager.animatronicParty[idxes[j]].animatronicItem);

                    if (canPoison && !_manager.animatronicParty[idxes[j]].animatronicItem.hasBubbleBreath)
                        _manager.animatronicParty[idxes[j]].animatronicItem.TriggerPoisonousEffect(poisonDamage, poisonDuration, timeBtwPoisonDamage);
                }
            }
        }

        yield return new WaitForSeconds(timeBtwInstancesOfDamage);
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
}
