using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatronicAbility : ScriptableObject
{
    public enum AbilityType { Damage, Defense, Heal, StatAffect }
    public AbilityType ability;
    public enum ColorGroup { Red, Yellow, Pink, Purple, White, Orange, Green, Black, BlazeOrange }
    public ColorGroup colorGroup;
    public string abilityName;
    public float timeBeforeAbility;
    public float timeToDestroyEffect;
    [Header ("Ability Effect Properties")]
    public GameObject attackEffect;
    public bool effectIsNotCentered;
    public Vector2 offsetIfFlipped;
    public enum SFXPlayMode { Continuous, SelectOneRandom, SelectMultipleRandom }
    public SFXPlayMode sFXPlayMode;
    [Header("Random SFX Select Properties")]
    public int timeOfRandomIterations;
    public float globalIntervalsBtwSFX;
    public AbilitySoundEffect[] abilitySoundEffect;
    [Header("Overlay Properties")]
    public bool abilityHasOverlay;

    public enum OverlaySelection 
    { 
        BrightGreen, 
        BrightOrange, 
        BrightRed, BrightPink, 
        SuperStars, 
        Bubbles, 
        BrightBlue, 
        PoisonBubbles, 
        PurpleBubbles, 
        BrightLight, 
        PartyBubbles 
    }

    public OverlaySelection abilityOverlay;
    public float timeBeforeOverlay;
    public Vector2 fadeInOffDurations;
    public float overlayStayDuration;
    [Header ("Camera Shake Properties")]
    public bool abilityHasCameraShake;
    public enum AbilityCameraShake { SmallShake, MediumShake, HeavyShake }
    public AbilityCameraShake acs;
    public float customShakeDuration = -1f;
    public float timeBeforeCameraShake;

    GameObject _attackEffect;
    int currentInstanceOfEffect;

    public virtual void ApplyEffect(bool entity, bool flip = false, bool offset = false)
    {
        if (attackEffect != null)
        {
            _attackEffect = Instantiate(attackEffect, attackEffect.transform.position, Quaternion.identity);

            if (flip)
            {
                var localScale = _attackEffect.transform.localScale;
                localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
                _attackEffect.transform.localScale = localScale;

                if (offset)
                {
                    _attackEffect.transform.position = new Vector3(offsetIfFlipped.x, offsetIfFlipped.y, _attackEffect.transform.position.z);
                }
            }

            Destroy(_attackEffect, timeToDestroyEffect);
        }

        GameManager manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        if (abilityHasOverlay)
            manager.StartCoroutine(StartOverlay());
        
        if (abilityHasCameraShake)
            manager.StartCoroutine(StartCameraShake());
    }

    IEnumerator StartOverlay()
    {
        yield return new WaitForSeconds(timeBeforeOverlay);
        GameManager manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        manager.CreateOverlay((int)abilityOverlay, fadeInOffDurations, overlayStayDuration);
    }

    IEnumerator StartCameraShake()
    {
        yield return new WaitForSeconds(timeBeforeCameraShake);
        CameraShakeManager camManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().cameraShake;
        camManager.CreateCameraShake(acs.ToString(), customShakeDuration, true);
    }

    public Color GetColorFromColorGroup()
    {
        switch (colorGroup)
        {
            case ColorGroup.Red:
                return new Color(255f/255f, 37f/255f, 35f/255f);

            case ColorGroup.Yellow:
                return new Color(255f/255f, 219f/255f, 35f/255f);
            
            case ColorGroup.Pink:
                return new Color(249f/255f, 0f/255f, 255f/255f);
            
            case ColorGroup.Purple:
                return new Color(74f/255f, 0f/255f, 255f/255f);
            
            case ColorGroup.White:
                return new Color(255f/255f, 255f/255f, 255f/255f);
            
            case ColorGroup.Orange:
                return new Color(255f/255f, 158f/255f, 35f/255f);
            
            case ColorGroup.Green:
                return new Color(17f/255f, 154f/255f, 0f/255f);
            
            case ColorGroup.BlazeOrange:
                return new Color(255f/255f, 103f/255f, 0f/255f);
        }

        return new Color(0f, 0f, 0f);
    }
}

[System.Serializable]
public class AbilitySoundEffect
{
    public AudioClip SFX;
    public int amountToPlayAudio;
    public Vector2 intervalsBtwPlays;
    public float delayToPlaySFX;
}
