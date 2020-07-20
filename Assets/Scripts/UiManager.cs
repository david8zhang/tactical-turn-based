using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField]
    GameObject attackMenu;

    [SerializeField]
    internal GameObject gameMap;

    [SerializeField]
    GameObject attackCutscene;

    [SerializeField]
    Canvas canvas;

    Unit attacker;
    Unit defender;

    // Start is called before the first frame update
    void Start()
    {
        attackMenu.SetActive(false);
        attackCutscene.SetActive(false);
    }

    internal void ShowAttackMenu(Unit attacker, Unit defender)
    {
        Sprite attackerSprite = attacker.gameObject.GetComponent<SpriteRenderer>().sprite;
        Sprite defenderSprite = defender.gameObject.GetComponent<SpriteRenderer>().sprite;

        Image attackerImage = attackMenu.transform.Find("Attacker").GetComponent<Image>();
        Image defenderImage = attackMenu.transform.Find("Defender").GetComponent<Image>();

        attackerImage.sprite = attackerSprite;
        defenderImage.sprite = defenderSprite;

        this.attacker = attacker;
        this.defender = defender;
        attackMenu.SetActive(true);
    }

    public void HideAttackMenu()
    {
        attackMenu.SetActive(false);
    }

    public void PlayAttackCutscene()
    {
        HideAttackMenu();
        Camera mainCamera = gameMap.GetComponent<GameMap>().mainCamera;
        attackCutscene.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, 1);
        attackCutscene.GetComponent<AttackCutscene>().Play(attacker, defender);
    }

    internal void OnCutsceneFinished()
    {
        gameMap.GetComponent<GameMap>().OnCutsceneFinished();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
