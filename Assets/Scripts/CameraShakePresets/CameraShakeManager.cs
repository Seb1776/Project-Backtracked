using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeManager : MonoBehaviour
{
    public ShakePreset[] presets;
    public List<IEnumerator> dedicatedCoroutines = new List<IEnumerator>();

    const float maxAngle = 10f;

    IEnumerator currentShakeCoroutine;

    public void TriggerShake(ShakeProperties _properties, bool dedicatedCoroutine = false)
    {
        if (!dedicatedCoroutine)
        {
            if (currentShakeCoroutine != null)
                StopCoroutine(currentShakeCoroutine);

            currentShakeCoroutine = Shake(_properties);
            StartCoroutine(currentShakeCoroutine);
        }

        else
            StartCoroutine(Shake(_properties));
    }

    public void CreateCameraShake(string _presetName, float customDuration = -1f, bool dedicatedCoroutine = false)
    {
        for (int i = 0; i < presets.Length; i++)
        {
            if (presets[i].presetName == _presetName)
            {
                TriggerShake(presets[i].GetShakeFromPreset(customDuration));
                break;
            }
        }
    }

    IEnumerator Shake(ShakeProperties properties)
    {
		float completionPercent = 0;
		float movePercent = 0;

		float angle_radians = properties.angle * Mathf.Deg2Rad - Mathf.PI;
		Vector3 previousWaypoint = transform.position;
		Vector3 currentWaypoint = transform.position;
		float moveDistance = 0;
		float speed = 0;

		Quaternion targetRotation = Quaternion.identity;
		Quaternion previousRotation = Quaternion.identity;

		do {
			if (movePercent >= 1 || completionPercent == 0) {
				float dampingFactor = DampingCurve (completionPercent, properties.dampingPercent);
				float noiseAngle = (Random.value - .5f) * Mathf.PI;
				angle_radians += Mathf.PI + noiseAngle * properties.noisePercent;
				currentWaypoint = new Vector3 (Mathf.Cos (angle_radians), Mathf.Sin (angle_radians)) * properties.strength * dampingFactor;
				previousWaypoint = transform.localPosition;
				moveDistance = Vector3.Distance (currentWaypoint, previousWaypoint);

				targetRotation = Quaternion.Euler (new Vector3 (currentWaypoint.y, currentWaypoint.x).normalized * properties.rotationPercent * dampingFactor * maxAngle);
				previousRotation = transform.localRotation;

				speed = Mathf.Lerp(properties.minSpeed,properties.maxSpeed,dampingFactor);

				movePercent = 0;
			}

			completionPercent += Time.deltaTime / properties.duration;
			movePercent += Time.deltaTime / moveDistance * speed;
			transform.localPosition = Vector3.Lerp (previousWaypoint, currentWaypoint, movePercent);
			transform.localRotation = Quaternion.Slerp (previousRotation, targetRotation, movePercent);
	

			yield return null;
		} while (moveDistance > 0);
    }

    float DampingCurve (float x, float dampingPercent)
    {
        x = Mathf.Clamp01(x);
        float a = Mathf.Lerp(2, .25f, dampingPercent);
        float b = 1 - Mathf.Pow(x, a);
        return Mathf.Pow(b, 3);
    }

    [System.Serializable]
    public class ShakeProperties
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

        public ShakeProperties(float angle, float strength, float speed, float duration, float noisePercent, float dampingPercent, float rotationPercent)
        {
            this.angle = angle;
            this.strength = strength;
            this.maxSpeed = speed;
            this.duration = duration;
            this.noisePercent = Mathf.Clamp01(noisePercent);
			this.dampingPercent = Mathf.Clamp01(dampingPercent);
			this.rotationPercent = Mathf.Clamp01(rotationPercent);
        }
    }

    [System.Serializable]
    public class ShakePreset
    {
        public string presetName;
        public ShakeCameraPreset preset;

        public ShakeProperties GetShakeFromPreset(float customDuration = -1f)
        {
            float newDuration = 0f;

            if (customDuration <= -1f) newDuration = preset.duration;
            else newDuration = customDuration;

            ShakeProperties sp = new ShakeProperties(
                preset.angle,
                preset.strength,
                preset.maxSpeed,
                newDuration,
                preset.noisePercent,
                preset.dampingPercent,
                preset.rotationPercent
            );

            return sp;
        }
    } 
}
