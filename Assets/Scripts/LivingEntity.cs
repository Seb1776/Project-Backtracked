using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LivingEntity : MonoBehaviour
{
    [Header ("Entity Fields")]
    public int currentHealth;
    public int currentDefense;
    public int currentAttack;
    public int currentCritChance;
    public int currentGiftHealth;
    public GameObject entityMesh;
    public bool poisoned;
    public bool stunned;
    public bool playingFeedback;
    public bool hasGift;
    public bool hasMunchies;
    public bool hasNeonWall;
    public bool haunted;
    public int neonWallType;
    public bool hasBubbleBreath;
    public Transform parentCanvas;
    public GameObject respectiveDamageNumber;
    [Header ("Audio")]
    public AudioClip deathSound;
    public AudioClip damageSound;
    public GameManager manager;
    public AudioSource source;
    public int instancesOfNerf;
    [Header ("Stats")]
    public List<AllStatsMods> allStats = new List<AllStatsMods>();

    Coroutine stunCoroutine;
    Coroutine buffCoroutine;

    public virtual void Awake()
    {
        source = GetComponent<AudioSource>();
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    public virtual void Start() 
    {
        DefineAllStats();
    }

    public virtual void Update() 
    {
        if (stunned)
            StunBehaviour();
    }

    public void TriggerPoisonousEffect(int _poisonDamage, float _poisonDuration, float _timeBtwPoisonDamage)
    {
        poisoned = true;
        StartCoroutine(GeneralPoisonEffect(_poisonDuration));
        StartCoroutine(PoisonDamageEffect(_poisonDamage, _timeBtwPoisonDamage));
    }

    public void TriggerStunEffect(float _stunDuration)
    {
        if (!stunned)
        {
            stunned = true;
            stunDuration = _stunDuration;
        }

        else
            currentStunDuration = 0f;
    }

    void DefineAllStats()
    {
        AllStatsMods STRStat = new AllStatsMods(AllStatsMods.StatType.ATK);
        AllStatsMods DEFStat = new AllStatsMods(AllStatsMods.StatType.DEF);
        AllStatsMods CRTStat = new AllStatsMods(AllStatsMods.StatType.CRT);

        allStats.Add(STRStat);
        allStats.Add(DEFStat);
        allStats.Add(CRTStat);
    }

    float stunDuration, currentStunDuration;

    public void StunBehaviour()
    {
        if (currentStunDuration < stunDuration)
        {
            currentStunDuration += Time.deltaTime;

            if (currentStunDuration >= stunDuration)
            {
                currentStunDuration = 0f;
                stunned = false;
            }
        }
    }

    public virtual void TriggerHauntingEffect(float hauntTime, float timeBeforeFreeze) {}

    public virtual void ReviveEntity() {}

    public virtual void TriggerGift() {}

    public virtual void UnTriggerGift() {}

    public virtual void TriggerMunchies(float _munchieDuration, int _munchieDamage, Vector2 _timeBtwMunchieDamage) {}

    public virtual void TriggerInstaKill() {}

    public virtual void InstaKill() {}

    public virtual IEnumerator FailedInstaKill(float delay) { yield return null; }

    public IEnumerator DelayInstaKill(float delay)
    {
        yield return new WaitForSeconds(delay);
        InstaKill();
    }

    IEnumerator StunEffect(float _stunDuration)
    {
        yield return new WaitForSeconds(_stunDuration);
        stunned = false;
    }

    public virtual void ApplySTR() {}

    public virtual void ApplyDEF() {}

    public virtual void ApplyCRT() {}

    public virtual void IncreaseStatTimer(string _stat, int _value, float _duration, bool _operation, float _deltaBefore) {}

    public virtual void Heal(int healValue, LivingEntity healedEntity)
    {
        if (!manager.battleEnded)
        {   
            if (respectiveDamageNumber != null)
            {
                bool _aliveEntity = false;

                if (healedEntity.GetComponent<Animatronic>() != null)
                    _aliveEntity = manager.animatronicParty[GetComponent<Animatronic>().idxInManager].alive;
                    
                if (healedEntity.GetComponent<Enemy>() != null)
                    _aliveEntity = manager.enemyParty[GetComponent<Enemy>().idxInManager].alive;

                if (_aliveEntity)
                {
                    GameObject dn = Instantiate(respectiveDamageNumber, respectiveDamageNumber.transform.position, respectiveDamageNumber.transform.rotation, parentCanvas);
                    dn.GetComponent<DamageNumbers>().AppearText(healValue, "heal");
                }
            }
        }
    }

    public virtual void MakeDamage(int damageValue, string context = "", LivingEntity attackingEntity = null, LivingEntity attackedEntity = null)
    {   
        if (!manager.battleEnded)
        {
            int dmgWithAtk = 0;
            int dmgWithDef = 0;
            int idxInManager = 0;

            int totalDamage = 0;
            int finalDamage = 0;
            int attackingEntityAtk = 0;
            int attackedEntityDef = 0;

            bool gotCritical = false;

            manager.cameraShake.CreateCameraShake("SmallShake");

            if (attackingEntity != null && attackedEntity != null)
            {
                gotCritical = manager.GetRandomBoolChance(attackedEntity.currentCritChance);

                attackingEntityAtk = attackingEntity.currentAttack;
                attackedEntityDef = attackedEntity.currentDefense;

                dmgWithAtk = manager.GetValueFromPercentage(damageValue, attackingEntityAtk);
                totalDamage = dmgWithAtk + damageValue;
                dmgWithDef = manager.GetValueFromPercentage(totalDamage, attackedEntityDef);
                finalDamage = totalDamage - dmgWithDef;

                if (gotCritical)
                    finalDamage *= 2;

                if (finalDamage <= 0)
                    finalDamage = 0;
                
                if (hasNeonWall)
                    finalDamage = manager.GetValueFromPercentage(finalDamage, neonWallType == 0 ? 50f : 85f);

                if (attackedEntity.GetComponent<Animatronic>() != null)
                {
                    for (int i = 0; i < manager.animatronicParty.Length; i++)
                    {
                        if (manager.animatronicParty[i].animatronicItem != this) continue;
                        idxInManager = i;
                        break;
                    }
                }

                else if (attackedEntity.GetComponent<Enemy>() != null)
                {
                    for (int i = 0; i < manager.enemyParty.Length; i++)
                    {
                        if (manager.enemyParty[i].enemyItem != this) continue;
                        idxInManager = i;
                        break;
                    }
                }
                
                if (respectiveDamageNumber != null)
                {
                    bool _aliveEntity = false;

                    if (attackedEntity.GetComponent<Animatronic>() != null)
                        _aliveEntity = manager.animatronicParty[idxInManager].alive;
                    
                    if (attackedEntity.GetComponent<Enemy>() != null)
                        _aliveEntity = manager.enemyParty[idxInManager].alive;

                    if (_aliveEntity)
                    {
                        GameObject dn = Instantiate(respectiveDamageNumber, respectiveDamageNumber.transform.position, respectiveDamageNumber.transform.rotation, parentCanvas);

                        if (context == "")
                        {
                            dn.GetComponent<DamageNumbers>().AppearText(finalDamage, gotCritical ? "crit" : "normal");
                        }

                        else
                            dn.GetComponent<DamageNumbers>().AppearText(finalDamage, context);
                    }
                }
            }

            else
            {
                finalDamage = damageValue;

                if (respectiveDamageNumber != null)
                {
                    bool _aliveEntity = false;

                    if (GetComponent<Animatronic>() != null)
                        _aliveEntity = manager.animatronicParty[idxInManager].alive;
                    
                    if (GetComponent<Enemy>() != null)
                        _aliveEntity = manager.enemyParty[idxInManager].alive;

                    if (_aliveEntity)
                    {
                        GameObject dn = Instantiate(respectiveDamageNumber, respectiveDamageNumber.transform.position, respectiveDamageNumber.transform.rotation, parentCanvas);

                        if (context == "")
                        {
                            if (manager.GetRandomBoolChance(currentCritChance))
                                dn.GetComponent<DamageNumbers>().AppearText(finalDamage, "crit");
                                
                            else
                                dn.GetComponent<DamageNumbers>().AppearText(finalDamage, "normal");
                        }

                        else
                            dn.GetComponent<DamageNumbers>().AppearText(finalDamage, context);
                    }
                }
            }

            if (!hasGift)
            {
                currentHealth -= finalDamage;

                if (currentHealth <= 0)
                {
                    currentHealth = 0;
                    TriggerEntityDeath();
                }

                else
                    source.PlayOneShot(damageSound);
            }

            else
            {
                currentGiftHealth -= finalDamage;

                if (currentGiftHealth <= 0)
                    UnTriggerGift();
                
                source.PlayOneShot(damageSound);
            }
        }
    }

    public virtual void TriggerEntityDeath() {}

    IEnumerator GeneralPoisonEffect(float _poisonDuration)
    {
        yield return new WaitForSeconds(_poisonDuration);
        poisoned = false;
    }

    IEnumerator PoisonDamageEffect(int _poisonDamage, float _timeBtwPosionDamage)
    {
        while (poisoned)
        {
            yield return new WaitForSeconds(_timeBtwPosionDamage);
            MakeDamage(_poisonDamage, "poison");
        }
    }

    public virtual void DamageFeedback() {}
}

[System.Serializable]
public class AllStatsMods
{
    public enum StatType { ATK, DEF, CRT }
    public StatType statType;
    public List<StatModification> allStatMods = new List<StatModification>();

    public AllStatsMods(StatType statType)
    {
        this.statType = statType;
    }
}

[System.Serializable]
public class StatModification
{
    public string statID;
    public int amountToMod;
    public float timeToAffect, currentTimeToAffect;
    public bool operation;

    public StatModification(string statID, int amountToMod, bool operation, float timeToAffect)
    {
        this.statID = statID;
        this.amountToMod = amountToMod;
        this.operation = operation;
        this.timeToAffect = timeToAffect;
    }

    public void AddStat(AllStatsMods.StatType _stat, LivingEntity ent)
    {
        switch (_stat)
        {
            case AllStatsMods.StatType.ATK:
                ent.currentAttack += amountToMod;
            break;

            case AllStatsMods.StatType.DEF:
                ent.currentDefense += amountToMod;
            break;

            case AllStatsMods.StatType.CRT:
                ent.currentCritChance += amountToMod;
            break;
        }
    }

    public void SubtractStat(AllStatsMods.StatType _stat, LivingEntity ent)
    {
        switch (_stat)
        {
            case AllStatsMods.StatType.ATK:
                ent.currentAttack -= amountToMod;
            break;

            case AllStatsMods.StatType.DEF:
                ent.currentDefense -= amountToMod;
            break;

            case AllStatsMods.StatType.CRT:
                ent.currentCritChance -= amountToMod;
            break;
        }
    }
}
