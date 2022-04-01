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
    public string abilityDescription;
    public float timeBeforeAbility;
    public float timeToDestroyEffect;
    [Header ("Ability Effect Properties")]
    public GameObject attackEffect;
    public bool effectIsNotCentered;
    public Vector2 offsetIfFlipped;
    public enum SFXPlayMode { Continuous, SelectOneRandom, SelectMultipleRandom }
    public SFXPlayMode sFXPlayMode;
    public bool overrideParticleDuration;
    public float particleDuration;
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

            if (overrideParticleDuration)
            {
                ParticleSystem effectPS = _attackEffect.transform.GetChild(0).GetComponent<ParticleSystem>();
                effectPS.Stop();
                var main = effectPS.main;
                main.loop = false;
                main.duration = particleDuration;
                effectPS.Play();
            }

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
        return colorGroup switch
        {
            ColorGroup.Red => new Color(255f / 255f, 37f / 255f, 35f / 255f),
            ColorGroup.Yellow => new Color(255f / 255f, 219f / 255f, 35f / 255f),
            ColorGroup.Pink => new Color(249f / 255f, 0f / 255f, 255f / 255f),
            ColorGroup.Purple => new Color(74f / 255f, 0f / 255f, 255f / 255f),
            ColorGroup.White => new Color(255f / 255f, 255f / 255f, 255f / 255f),
            ColorGroup.Orange => new Color(255f / 255f, 158f / 255f, 35f / 255f),
            ColorGroup.Green => new Color(17f / 255f, 154f / 255f, 0f / 255f),
            ColorGroup.BlazeOrange => new Color(255f / 255f, 103f / 255f, 0f / 255f),
            _ => new Color(0f, 0f, 0f)
        };
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
