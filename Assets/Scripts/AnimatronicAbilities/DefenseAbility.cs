using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Defense Ability", menuName = "Animatronic Ability/Defense Ability")]
public class DefenseAbility : AnimatronicAbility
{
    [Header ("Defense Ability")]
    [Range(10f, 100f)]
    public float defensePercentageIncrement;
}
