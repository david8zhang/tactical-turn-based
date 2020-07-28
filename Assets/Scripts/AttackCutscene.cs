using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCutscene : MonoBehaviour
{
    [SerializeField]
    GameObject uiManager;

    [SerializeField]
    CutsceneUnit attackerUnit;

    [SerializeField]
    CutsceneUnit defenderUnit;

    internal void SetReferences(Unit attacker, Unit defender)
    {
        gameObject.SetActive(true);

        // Get all game object references
        GameObject attackerObj = attacker.gameObject;
        GameObject defenderObj = defender.gameObject;

        // Set all game object references to cutscene unita
        attackerUnit.SetUnitObjRef(attackerObj);
        defenderUnit.SetUnitObjRef(defenderObj);
    }

    // Enemy attack returns ienumerator because enemy "moves" on its own,
    // IEnumerator notifies EnemyUnits Manager when to move the next enemy
    internal IEnumerator PlayEnemyAttack()
    {
        attackerUnit.Attack(defenderUnit);
        yield return new WaitForSeconds(2f);
        attackerUnit.ResetAttacker();

        defenderUnit.Attack(attackerUnit);
        yield return StartCoroutine(WaitForDelay());

    }

    internal void PlayPlayerAttack()
    {
        attackerUnit.Attack(defenderUnit);
        StartCoroutine(WaitForDelay());
    }

    void HideAttackCutscene()
    {
        defenderUnit.ResetAttacker();
        gameObject.SetActive(false);
    }

    IEnumerator WaitForDelay()
    {
        yield return new WaitForSeconds(5);
        HideAttackCutscene();
        uiManager.GetComponent<UiManager>().OnEnemyCutsceneFinished();
    }
}
