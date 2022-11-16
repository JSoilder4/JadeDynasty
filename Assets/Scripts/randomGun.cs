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

    public Sprite pistol, shotgun, sniper, smg;

    public Color clense;
    public Color fire = new Color(255, 0, 0);
    public Color water = new Color(0, 130, 255);
    public Color earth = new Color(0, 255, 0);
    public Color air = new Color(255, 255, 0);

    public AudioSource mySource;

    public Collider2D col;

    public TextMeshProUGUI tutText;

    // Start is called before the first frame update
    void Start()
    {
        mySource = GetComponent<AudioSource>();
        //elemental = new bool[5]; //nothing 0, fire 1, water 2, earth 3, air 4
        //effect = new bool[5]; //nothing 0, sciShot 1, split 2, explode 3, radiation 4
        sprite = GetComponent<SpriteRenderer>();
        clense = sprite.color;

        col = GetComponent<Collider2D>();
        tutText = GetComponentInChildren<TextMeshProUGUI>();

        theGun.roll();
        //rollGun();
    }

    public void rollGun()
    {
        //resetRoll();
        theGun.gunType = (gunEnumScript.gunType) Random.Range(0, System.Enum.GetNames(typeof(gunEnumScript.gunType)).Length);

        switch (theGun.gunType)
        {
            case gunEnumScript.gunType.Pistol:
                theGun.damage = 10;
                theGun.betweenShotTimer = Random.Range(0.15f, 0.35f);
                theGun.reloadTimer = Random.Range(0.5f, 1f);
                theGun.shotSpeed = Random.Range(0.1f, 0.4f);
                theGun.numShots = Random.Range(0, 3);
                break;
            case gunEnumScript.gunType.Shotgun:
                theGun.damage = 5;
                theGun.betweenShotTimer = Random.Range(0.35f, 0.55f);
                theGun.reloadTimer = Random.Range(0.5f, 1f);
                theGun.shotSpeed = Random.Range(0.1f, 0.3f);
                theGun.numShots = Random.Range(1, 4);
                break;
            case gunEnumScript.gunType.Sniper:
                theGun.damage = 30;
                theGun.betweenShotTimer = Random.Range(0.55f, 0.75f);
                theGun.reloadTimer = Random.Range(0.5f, 1f);
                theGun.shotSpeed = Random.Range(0.3f, 0.5f);
                theGun.numShots = 0;
                break;
            case gunEnumScript.gunType.SMG:
                theGun.damage = 2;
                theGun.betweenShotTimer = Random.Range(0.05f, 0.15f);
                theGun.reloadTimer = Random.Range(0.5f, 1f);
                theGun.shotSpeed = Random.Range(0.1f, 0.4f);
                theGun.numShots = Random.Range(0, 2);
                break;
        }

        theGun.element = (gunEnumScript.element)Random.Range(0, System.Enum.GetNames(typeof(gunEnumScript.element)).Length);
        theGun.effect = (gunEnumScript.effect)Random.Range(0, System.Enum.GetNames(typeof(gunEnumScript.effect)).Length);
    }
    private void Update()
    {
        switch (theGun.gunType)
        {
            case gunEnumScript.gunType.Pistol:
                sprite.sprite = pistol;
                break;
            case gunEnumScript.gunType.Shotgun:
                sprite.sprite = shotgun;
                break;
            case gunEnumScript.gunType.Sniper:
                sprite.sprite = sniper;
                break;
            case gunEnumScript.gunType.SMG:
                sprite.sprite = smg;
                break;
        }
    }
    private void LateUpdate()
    {
        switch (theGun.element)
        {
            case gunEnumScript.element.Nothing:
                sprite.color = clense;
                break;
            case gunEnumScript.element.Fire:
                sprite.color = fire;
                break;
            case gunEnumScript.element.Water:
                sprite.color = water;
                break;
            case gunEnumScript.element.Earth:
                sprite.color = earth;
                break;
            case gunEnumScript.element.Air:
                sprite.color = air;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            print("bitch");
            tutText.enabled = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player")){
            print("n-word pass");
            tutText.enabled = false;
        }
    }
}
