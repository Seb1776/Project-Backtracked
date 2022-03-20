using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Heal Ability", menuName = "Animatronic Ability/Heal Ability")]
public class HealAbility : AnimatronicAbility
{
    public enum HealMode { LifePoints, Percentage }
    [Header ("Heal Ability")]
    public HealMode healMode;
    public enum Heals { Animatronic, Enemy }
    public Heals entityToHeal;
    [Tooltip("Leave on zero if using Percentage")]
    public int healAmount;
    [Tooltip("Leave on zero if using Life Points")]
    [Range(0f, 150f)]
    public float healPercentage;
    [Range(1, 4)]
    public int targets;
    public bool randomTargets;
    [Range(1, 10)]
    public int instancesOfHeal;
    public float timeBtwInstancesOfHeal;

    bool usedFirstHeal;

    public override void ApplyEffect(bool entity, bool flip = false, bool offset = false)
    {
        List<int> toHeal = new List<int>();
        usedFirstHeal = false;
        GameManager manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        if (!entity)
        {
            if (manager.battleMusicContext == GameManager.BattleMusicContext.Fight)
            {
                toHeal = manager.GetEntityList("enemy", targets, randomTargets);
                manager.StartCoroutine(HealEnemy(manager.battleMusicContext, manager, toHeal));
            }

            else
                manager.StartCoroutine(HealEnemy(manager.battleMusicContext, manager));
        }

        else
        {
            toHeal = manager.GetEntityList("animatronic", targets, randomTargets);
            manager.StartCoroutine(HealAnimatronic(toHeal, manager));
        }   

        manager.StartCoroutine(AbilitySoundEffect());
        base.ApplyEffect(flip, offset);
    }

    public IEnumerator HealEnemy(GameManager.BattleMusicContext _battleContext, GameManager _manager, List<int> idxes = null)
    {
        for (int i = 0; i < instancesOfHeal; i++)
        {
            if (!usedFirstHeal)
            {
                yield return new WaitForSeconds(timeBeforeAbility);
                usedFirstHeal = true;
            }

            if (_battleContext == GameManager.BattleMusicContext.Fight)
            {
                for (int j = 0; j < idxes.Count; j++)
                {
                    if (_manager.enemyParty[idxes[j]].alive)
                    {
                        int healValue = 0;

                        if (healPercentage > 0)
                            healValue = _manager.GetValueFromPercentage(_manager.enemyParty[idxes[j]].enemyItem.currentHealth, healPercentage);
                        
                        else if (healAmount > 0)
                            healValue = healAmount;

                        Debug.Log(healValue);
                        _manager.enemyParty[idxes[j]].enemyItem.Heal(healValue, _manager.currentEnemyTurn);
                    }
                }
            }

            else
            {
                if (_manager.bossParty.alive)
                {
                    int healValue = 0;

                    if (healPercentage > 0)
                        healValue = _manager.GetValueFromPercentage(_manager.bossParty.enemyItem.currentHealth, healPercentage);
                        
                    else if (healAmount > 0)
                        healValue = healAmount;
                    
                    _manager.bossParty.enemyItem.Heal(healValue, _manager.bossParty.enemyItem);
                }
            }

            yield return new WaitForSeconds(instancesOfHeal);
        }
    }

    public IEnumerator HealAnimatronic(List<int> idxes, GameManager _manager)
    {
        for (int i = 0; i < instancesOfHeal; i++)
        {
            if (!usedFirstHeal)
            {
                yield return new WaitForSeconds(timeBeforeAbility);
                usedFirstHeal = true;
            }
            
            for (int j = 0; j < idxes.Count; j++)
            {
                if (_manager.animatronicParty[idxes[j]].alive)
                {
                    int healValue = 0;

                    if (healPercentage > 0)
                        healValue = _manager.GetValueFromPercentage(_manager.animatronicParty[idxes[j]].animatronicItem.currentHealth, healPercentage);
                        
                    else if (healAmount > 0)
                        healValue = healAmount;

                    _manager.animatronicParty[idxes[j]].animatronicItem.Heal(healValue, _manager.currentAnimatronicTurn);
                }
            }

            yield return new WaitForSeconds(instancesOfHeal);
        }
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
