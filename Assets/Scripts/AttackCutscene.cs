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

    // Start is called before the first frame update
    void Start()
    {
        
    }

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

    internal IEnumerator PlayEnemyAttack()
    {
        attackerUnit.Attack(defenderUnit);
        yield return StartCoroutine(WaitForEnemyDelay());
    }


    internal void PlayPlayerAttack()
    {
        attackerUnit.Attack(defenderUnit);
        StartCoroutine(WaitForPlayerDelay());
    }

    void HideAttackCutscene()
    {
        attackerUnit.Reset();
        gameObject.SetActive(false);
    }

    IEnumerator WaitForEnemyDelay()
    {
        yield return new WaitForSeconds(5);
        HideAttackCutscene();
        uiManager.GetComponent<UiManager>().OnEnemyCutsceneFinished();
    }

    IEnumerator WaitForPlayerDelay()
    {
        yield return new WaitForSeconds(5);
        HideAttackCutscene();
        uiManager.GetComponent<UiManager>().OnPlayerCutsceneFinished();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
