using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Animatronic Data", menuName = "Animatronic Data")]
public class AnimatronicData : ScriptableObject
{
    public string animatronicName;
    public enum AnimatronicType { Attack, Defense, Healer, Sponger, HeavyWizard }
    public AnimatronicType animatronicType;

    public enum RealityGroup 
    {   
        Freddy_Fazbear_Pizza, 
        Freddy_Fazbear_Pizza_JR, 
        Fazbear_Fright, 
        WA_Robotics, 
        Circus_Baby_Pizza_World, 
        Chica_Party_World, 
        Freddy_Fazbear_Pizza_Place, 
        Freddy_Fazbear_Mega_Pixaplex 
    }

    public RealityGroup realityGroup;
    public GameObject animatronicPrefab;
    public SkinInfo[] skins;
    public int defaultSkin;
    public AnimatronicAbility[] animatronicAbilities;
    public Sprite animatronicPortrait;
    [Header ("Start Stats")]
    [Range(75, 150000)]
    public int maxHealth;
    [Range (1, 100)]
    public int maxDefense;
    [Range (1, 200)]
    public int maxAttack;
    [Range(0, 100)]
    public int maxCritChance;
}

[System.Serializable]
public class SkinInfo
{
    public string skinName;
    public int skinIndex;
    public enum SkinQuality { Normal, Rare, Epic, Legendary, Ultra }
    public SkinQuality currentSkinQuality;
}
