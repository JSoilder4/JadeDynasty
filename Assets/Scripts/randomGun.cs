using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class randomGun : MonoBehaviour
{
    //gunType, damage, bST, reload, ammo?, shotspeed, element, effect, numshots, burstfire?, auto?,
    //enum, int, float, float, int, float, enum, enum, int, bool?, bool?

    public gun theGun = new gun();

    [Header("Gun")]
    public SpriteRenderer sprite;
    public SpriteRenderer highlightSprite;

    //public Sprite[] pistol, shotgun, sniper, smg;

    public Color clense;
    public Color fire = new Color(1, 0, 0);
    public Color water = new Color(0, 0, 1);
    public Color lightning = new Color(1, 1, 0);
    public Color stasis = new Color(1, 0, 1);

    public AudioSource mySource;

    public Collider2D col;

    public TextMeshProUGUI tutText;

    public GenerationManager genManage;

    // Start is called before the first frame update
    void Start()
    {
        mySource = GetComponent<AudioSource>();
        //elemental = new bool[5]; //nothing 0, fire 1, water 2, earth 3, air 4
        //effect = new bool[5]; //nothing 0, sciShot 1, split 2, explode 3, radiation 4
        sprite = GetComponent<SpriteRenderer>();
        highlightSprite = GetComponentsInChildren<SpriteRenderer>()[1];
        clense = sprite.color;

        col = GetComponent<Collider2D>();
        tutText = GetComponentInChildren<TextMeshProUGUI>();
        genManage = GameObject.FindWithTag("GameController").GetComponent<GenerationManager>();
        theGun.floor = genManage.floor;
        theGun.Roll("");
        //rollGun();
    }

    // public void rollGun()
    // {
    //     //resetRoll();
    //     theGun.gunType = (gunEnumScript.gunType) Random.Range(0, System.Enum.GetNames(typeof(gunEnumScript.gunType)).Length);

    //     switch (theGun.gunType)
    //     {
    //         case gunEnumScript.gunType.Pistol:
    //             theGun.damage = 10;
    //             theGun.betweenShotTimer = Random.Range(0.15f, 0.35f);
    //             theGun.reloadTimer = Random.Range(0.5f, 1f);
    //             theGun.shotSpeed = Random.Range(0.1f, 0.4f);
    //             theGun.numShots = Random.Range(0, 3);
    //             break;
    //         case gunEnumScript.gunType.Shotgun:
    //             theGun.damage = 5;
    //             theGun.betweenShotTimer = Random.Range(0.35f, 0.55f);
    //             theGun.reloadTimer = Random.Range(0.5f, 1f);
    //             theGun.shotSpeed = Random.Range(0.1f, 0.3f);
    //             theGun.numShots = Random.Range(1, 4);
    //             break;
    //         case gunEnumScript.gunType.Sniper:
    //             theGun.damage = 30;
    //             theGun.betweenShotTimer = Random.Range(0.55f, 0.75f);
    //             theGun.reloadTimer = Random.Range(0.5f, 1f);
    //             theGun.shotSpeed = Random.Range(0.3f, 0.5f);
    //             theGun.numShots = 0;
    //             break;
    //         case gunEnumScript.gunType.SMG:
    //             theGun.damage = 2;
    //             theGun.betweenShotTimer = Random.Range(0.05f, 0.15f);
    //             theGun.reloadTimer = Random.Range(0.5f, 1f);
    //             theGun.shotSpeed = Random.Range(0.1f, 0.4f);
    //             theGun.numShots = Random.Range(0, 2);
    //             break;
    //     }

    //     theGun.element = (gunEnumScript.element)Random.Range(0, System.Enum.GetNames(typeof(gunEnumScript.element)).Length);
    //     theGun.effect = (gunEnumScript.effect)Random.Range(0, System.Enum.GetNames(typeof(gunEnumScript.effect)).Length);
    // }
    private void Update()
    {
        switch (theGun.gunType)
        {
            case gunEnumScript.gunType.Pistol:
                sprite.sprite = GameManager.GM.pistol[theGun.spriteNum];
                highlightSprite.sprite = GameManager.GM.pistolH;
                break;
            case gunEnumScript.gunType.Shotgun:
                sprite.sprite = GameManager.GM.shotgun[theGun.spriteNum];
                highlightSprite.sprite = GameManager.GM.shotgunH;
                break;
            case gunEnumScript.gunType.Sniper:
                sprite.sprite = GameManager.GM.sniper[theGun.spriteNum];
                highlightSprite.sprite = GameManager.GM.sniperH;
                break;
            case gunEnumScript.gunType.SMG:
                sprite.sprite = GameManager.GM.smg[theGun.spriteNum];
                highlightSprite.sprite = GameManager.GM.smgH;
                break;
        }
    }
    private void LateUpdate()
    {
        switch (theGun.element)
        {
            case gunEnumScript.element.Nothing:
                highlightSprite.color = clense;
                break;
            case gunEnumScript.element.Fire:
                highlightSprite.color = fire;
                break;
            case gunEnumScript.element.Water:
                highlightSprite.color = water;
                break;
            case gunEnumScript.element.Lightning:
                highlightSprite.color = lightning;
                break;
            case gunEnumScript.element.Stasis:
                highlightSprite.color = stasis;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            tutText.enabled = true;
            GameManager.GM.gunSwapUI(theGun, false);
            //print("crying in the club rn: ");
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player")){
            tutText.enabled = false;
            GameManager.GM.gunSwapUI(theGun, true);
        }
    }
}
