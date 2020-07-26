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

    public delegate void OnFinishedDelegate();
    public OnFinishedDelegate onFinishedDelegate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    internal void Play(Unit attacker, Unit defender)
    {
        gameObject.SetActive(true);

        // Get all game object references
        GameObject attackerObj = attacker.gameObject;
        GameObject defenderObj = defender.gameObject;

        // Set all game object references to cutscene unita
        attackerUnit.SetUnitObjRef(attackerObj);
        defenderUnit.SetUnitObjRef(defenderObj);

        // Have attacker unit attack defender
        attackerUnit.Attack(defenderUnit);
        StartCoroutine(PlayCutscene());
    }

    IEnumerator PlayCutscene()
    {
        yield return new WaitForSeconds(5);
        OnCutsceneFinished();
    }

    void OnCutsceneFinished()
    {
        attackerUnit.Reset();
        gameObject.SetActive(false);
        uiManager.GetComponent<UiManager>().OnCutsceneFinished();
        onFinishedDelegate?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
