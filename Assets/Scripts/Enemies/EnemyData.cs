using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "Enemy Data")]
public class EnemyData : ScriptableObject
{
    public enum EnemyType { Common, Boss }
    public EnemyType currentEnemyType;
    public string enemyName;
    [Header ("Start Stats")]
    [Range(75, 150000)]
    public int maxHealth;
    [Range (1, 100)]
    public int maxDefense;
    [Range (1, 200)]
    public int maxAttack;
}
