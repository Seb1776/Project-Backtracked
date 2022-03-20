using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AttackButton : MonoBehaviour
{
    public Text attackName;
    public Image attackType;
    public AnimatronicAbility setAbility;

    GameManager manager;

    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    public void AppearButton(AnimatronicAbility _setAbility)
    {
        setAbility = _setAbility;
        attackType.color = setAbility.GetColorFromColorGroup();
        attackName.text = setAbility.abilityName;
        GetComponent<Animator>().SetBool("appear", true);
    }

    public void SelectAttack()
    {   
        if (manager.hasMimicBall && !manager.battleEnded)
            StartCoroutine(MimicBallRepeat(setAbility, true));

        setAbility.ApplyEffect(true);
        manager.currentAnimatronicTurn.GetComponent<Animatronic>().skins[manager.currentAnimatronicTurn.GetComponent<Animatronic>().currentSkinIndex].skinAnimator.SetTrigger("ability");
        manager.DisableAttackButtons();
    }

    IEnumerator MimicBallRepeat(AnimatronicAbility _copy, bool entity)
    {
        yield return new WaitForSeconds(manager.mimicBall.delayBeforeRepeat);

        if (!manager.battleEnded)
        {
            _copy.ApplyEffect(entity);
            manager.TriggerMimicAbility();
        }
    }
}
