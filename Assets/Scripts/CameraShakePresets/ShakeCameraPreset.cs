using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shake Preset", menuName = "Create Shake Preset")]
public class ShakeCameraPreset : ScriptableObject
{
    public float angle;
    public float strength;
    public float maxSpeed;
    public float minSpeed;
    public float duration;
    [Range(0, 1)]
    public float noisePercent;
    [Range(0, 1)]
    public float dampingPercent;
    [Range(0, 1)]
    public float rotationPercent;
}
