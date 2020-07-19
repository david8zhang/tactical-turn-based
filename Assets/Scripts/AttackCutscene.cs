using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCutscene : MonoBehaviour
{

    bool isAttacking = false;
    GameObject cutsceneAttacker;
    GameObject cutsceneDefender;

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
        cutsceneAttacker.GetComponent<Animator>().SetBool("isAttacking", true);
        isAttacking = true;
        StartCoroutine(PlayCutscene());
    }

    internal void PlayAttackAnimation()
    {
        // Move Attacker forward
        countDown -= Time.deltaTime;
        float speed = 5f;
        if (countDown > 0)
        {
            cutsceneAttacker.transform.position += new Vector3(1, 0, 0) * Time.deltaTime * speed;
        }
        else if (countDown > -2.0f)
        {
            cutsceneAttacker.transform.position += new Vector3(-1, 0, 0) * Time.deltaTime * speed;
        }

        //Vector3 targetPosition = cutsceneDefender.transform.position;
        //Vector3 oldPosition = new Vector3(cutsceneAttacker.transform.position.x, cutsceneAttacker.transform.position.y, cutsceneAttacker.transform.position.z);
        //float step = 10 * Time.deltaTime;
        //cutsceneAttacker.transform.position = Vector3.Lerp(cutsceneAttacker.transform.position, targetPosition, step);
        //cutsceneAttacker.transform.position = Vector3.Lerp(cutsceneAttacker.transform.position, oldPosition, step);

    }

    IEnumerator PlayCutscene()
    {
        yield return new WaitForSeconds(5);
        gameObject.SetActive(false);
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
