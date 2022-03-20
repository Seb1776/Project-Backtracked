using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialAbilityPanel : MonoBehaviour
{
    public float lifeTime;

    void Start()
    {
        GetComponent<RectTransform>().localPosition = new Vector3(7.03f, 1.88f, -7.37f);
    }

    public void AppearPanel(string ability)
    {
        if (ability == string.Empty) return;
        Destroy(this.gameObject, lifeTime);
        transform.GetChild(1).GetComponent<Text>().text = ability;
    }
}
