using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageNumbers : MonoBehaviour
{
    public float animationDuration;
    public Color normalDamage, critDamage, poisonDamage, healDamage;

    public void AppearText(int value, string context, string instaMsg = "")
    {
        Destroy(this.gameObject, animationDuration);

        if (instaMsg == "")
            GetComponent<Text>().text = value.ToString();
        
        else
            GetComponent<Text>().text = instaMsg;

        switch (context)
        {
            case "normal":
                GetComponent<Text>().color = normalDamage;
            break;

            case "crit":
                GetComponent<Text>().color = critDamage;
            break;

            case "poison":
                GetComponent<Text>().color = poisonDamage;
            break;

            case "heal":
                GetComponent<Text>().color = healDamage;
            break;
        }
    }
}
