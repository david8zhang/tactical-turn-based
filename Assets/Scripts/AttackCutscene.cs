using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCutscene : MonoBehaviour
{

    bool isAttacking = false;

    [SerializeField]
    GameObject uiManager;

    GameObject cutsceneAttacker;
    GameObject cutsceneDefender;

    Vector3 oldAttackerPosition;
    Vector3 windupPosition;

    float countDown = 2.0f;

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
        cutsceneAttacker = gameObject.transform.Find("Attacker").gameObject;
        cutsceneDefender = gameObject.transform.Find("Defender").gameObject;

        // Dynamically attach animator controller
        cutsceneAttacker.GetComponent<Animator>().runtimeAnimatorController = attackerObj.GetComponent<Animator>().runtimeAnimatorController;
        cutsceneDefender.GetComponent<Animator>().runtimeAnimatorController = defenderObj.GetComponent<Animator>().runtimeAnimatorController;

        // initialization for attack animation
        cutsceneAttacker.GetComponent<Animator>().SetBool("isAttacking", true);
        oldAttackerPosition = cutsceneAttacker.transform.position;
        windupPosition = cutsceneAttacker.transform.position + new Vector3(-1, 0, 0);


        isAttacking = true;
        StartCoroutine(PlayCutscene());
    }

    internal void PlayAttackAnimation()
    {
        // Move Attacker forward
        countDown -= Time.deltaTime;
        Vector3 targetPosition = cutsceneDefender.transform.position;
        float forwardStep = 35 * Time.deltaTime;
        float backwardStep = 5 * Time.deltaTime;

        Vector3 currentPosition = cutsceneAttacker.transform.position;

        // Wind up animation
        if (countDown > 1.0f && countDown <= 2.0f)
        {
            cutsceneAttacker.transform.position = Vector3.MoveTowards(currentPosition, windupPosition, backwardStep);
        }

        // Overshoot and pullback
        if (countDown > 0.8f && countDown <= 1.0f)
        {
            cutsceneAttacker.transform.position = Vector3.MoveTowards(cutsceneAttacker.transform.position, targetPosition, forwardStep);
        }
        else if (countDown > 0.5f && countDown <= 0.7f)
        {
            cutsceneAttacker.transform.position = Vector3.MoveTowards(cutsceneAttacker.transform.position, oldAttackerPosition, forwardStep);
        }
    }

    IEnumerator PlayCutscene()
    {
        yield return new WaitForSeconds(5);
        OnCutsceneFinished();
    }

    void OnCutsceneFinished()
    {
        gameObject.SetActive(false);
        oldAttackerPosition = new Vector3();
        windupPosition = new Vector3();
        isAttacking = false;
        countDown = 2.0f;
        uiManager.GetComponent<UiManager>().OnCutsceneFinished();
    }



    // Update is called once per frame
    void Update()
    {
        if (isAttacking)
        {
            PlayAttackAnimation();
        }
    }
}
