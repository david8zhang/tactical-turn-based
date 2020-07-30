using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField]
    AttackMenu attackMenu;

    [SerializeField]
    internal GameObject gameMap;

    [SerializeField]
    internal GameObject attackCutscene;

    [SerializeField]
    Canvas canvas;

    Unit attacker;
    Unit defender;

    // Start is called before the first frame update
    void Start()
    {
        attackMenu.Hide();
        attackCutscene.SetActive(false);
    }

    internal void ShowAttackMenu(Unit attacker, Unit defender)
    {
        this.attacker = attacker;
        this.defender = defender;
        attackMenu.Show(attacker, defender);
    }

    public void HideAttackMenu()
    {
        attackMenu.Hide();
    }

    public void OnAttackButtonClick()
    {
        PlayPlayerAttackCutscene();
    }

    public void InitCutscene()
    {
        HideAttackMenu();
        Camera mainCamera = gameMap.GetComponent<GameMap>().mainCamera;
        attackCutscene.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, 1);
    }

    public IEnumerator PlayEnemyAttackCutscene(Unit attacker, Unit defender) {
        AttackCutscene scene = attackCutscene.GetComponent<AttackCutscene>();
        this.attacker = attacker;
        this.defender = defender;
        InitCutscene();
        scene.SetReferences(attacker, defender);
        yield return StartCoroutine(scene.PlayEnemyAttack());
    }

    public void PlayPlayerAttackCutscene()
    {
        AttackCutscene scene = attackCutscene.GetComponent<AttackCutscene>();
        InitCutscene();
        scene.SetReferences(attacker, defender);
        scene.PlayPlayerAttack();
    }

    internal void OnPlayerCutsceneFinished()
    {
        GameMap gm = gameMap.GetComponent<GameMap>();
        gm.OnCutsceneFinished();
        gm.CheckTurnFinished();
    }

    internal void OnEnemyCutsceneFinished()
    {
        GameMap gm = gameMap.GetComponent<GameMap>();
        gm.OnCutsceneFinished();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
