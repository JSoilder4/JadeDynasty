using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomGun : MonoBehaviour
{

    


    [Header("Gun")]
    public SpriteRenderer sprite;
    //public bool[] gunType = new bool[4];//Pistol 0, Shotgun 1, Sniper 2, SMG 3
    //public int gunTypeIndex; //swap
    gunEnumScript.gunType gunType;


    [Header("Gun Modifiers")]
    public int damage; //swap
    public float betweenShotTimer; //swap
    public float reloadTimer; //swap

    [Header("Shot")]
    public float shotSpeed; //swap
    //public bool[] elemental =  new bool[5]; //nothing 0, fire 1, water 2, earth 3, air 4
    //public int elementIndex; //swap
    gunEnumScript.element element;

    [Header("Shot Effect")]
    //public bool[] effect = new bool[5]; //nothing 0, sciShot 1, explode 2, comet 3, bigBullets 4,
    //public int effectIndex; //swap
    gunEnumScript.effect effect;

    public Sprite pistol, shotgun, sniper, smg;

    public Color clense;
    public Color fire = new Color(255, 0, 0);
    public Color water = new Color(0, 130, 255);
    public Color earth = new Color(0, 255, 0);
    public Color air = new Color(255, 255, 0);

    public AudioClip pickupSound;
    public AudioSource mySource;
    // Start is called before the first frame update
    void Start()
    {
        mySource = GetComponent<AudioSource>();
        //elemental = new bool[5]; //nothing 0, fire 1, water 2, earth 3, air 4
        //effect = new bool[5]; //nothing 0, sciShot 1, split 2, explode 3, radiation 4
        sprite = GetComponent<SpriteRenderer>();
        clense = sprite.color;
        rollGun();
    }
    public void playPickupSound()
    {
        mySource.clip = pickupSound;
        mySource.Play();
    }
    public void rollGun()
    {
        //resetRoll();
        gunType = (gunEnumScript.gunType) Random.Range(0, System.Enum.GetNames(typeof(gunEnumScript.gunType)).Length);

        switch (gunType)
        {
            case gunEnumScript.gunType.Pistol:
                damage = 10;
                betweenShotTimer = Random.Range(0.15f, 0.35f);
                reloadTimer = Random.Range(0.5f, 1f);
                shotSpeed = Random.Range(0.1f, 0.4f);
                break;
            case gunEnumScript.gunType.Shotgun:
                damage = 5;
                betweenShotTimer = Random.Range(0.35f, 0.55f);
                reloadTimer = Random.Range(0.5f, 1f);
                shotSpeed = Random.Range(0.1f, 0.3f);
                break;
            case gunEnumScript.gunType.Sniper:
                damage = 30;
                betweenShotTimer = Random.Range(0.55f, 0.75f);
                reloadTimer = Random.Range(0.5f, 1f);
                shotSpeed = Random.Range(0.3f, 0.5f);
                break;
            case gunEnumScript.gunType.SMG:
                damage = 2;
                betweenShotTimer = Random.Range(0.05f, 0.15f);
                reloadTimer = Random.Range(0.5f, 1f);
                shotSpeed = Random.Range(0.1f, 0.4f);
                break;

                break;
        }

        //DELETE LATER
        //if (gunType[0]) //Pistol
        //{
        //    damage = 10;
        //    betweenShotTimer = Random.Range(0.15f, 0.35f);
        //    reloadTimer = Random.Range(0.5f, 1f);
        //    shotSpeed = Random.Range(0.1f, 0.4f);
        //}
        //else if (gunType[1]) //Shotgun
        //{
        //    damage = 5;
        //    betweenShotTimer = Random.Range(0.35f, 0.55f);
        //    reloadTimer = Random.Range(0.5f, 1f);
        //    shotSpeed = Random.Range(0.1f, 0.3f);
        //}
        //else if (gunType[2]) //Sniper
        //{
        //    damage = 30;
        //    betweenShotTimer = Random.Range(0.55f, 0.75f);
        //    reloadTimer = Random.Range(0.5f, 1f);
        //    shotSpeed = Random.Range(0.3f, 0.5f);
        //}
        //else if (gunType[3]) //SMG
        //{
        //    damage = 2;
        //    betweenShotTimer = Random.Range(0.05f, 0.15f);
        //    reloadTimer = Random.Range(0.5f, 1f);
        //    shotSpeed = Random.Range(0.1f, 0.4f);
        //}
        
       // elemental[Random.Range(0, elemental.Length)] = true;
       // effect[Random.Range(0, effect.Length)] = true;

        elemental = (element)(gunEnumScript.element)Random.Range(0, System.Enum.GetNames(typeof(gunEnumScript.element)).Length);
        effect = (effect)(gunEnumScript.effect)Random.Range(0, System.Enum.GetNames(typeof(gunEnumScript.effect)).Length);



        //for (int i = 0; i < elemental.Length; i++)
        //{
        //    if (elemental[i])
        //    {
        //        elementIndex = i;
        //    }
        //}
        //for (int i = 0; i < effect.Length; i++)
        //{
        //    if (effect[i])
        //    {
        //        effectIndex = i;
        //    }
        //}
        //for (int i = 0; i < gunType.Length; i++)
        //{
        //    if (gunType[i])
        //    {
        //        gunTypeIndex = i;
        //    }
        //}
    }
    //public void resetRoll()
    //{
    //    for (int i = 0; i < gunType.Length; i++)
    //    {
    //        gunType[i] = false;
    //    }
    //    for (int i = 0; i < elemental.Length; i++)
    //    {
    //        elemental[i] = false;
    //    }
    //    for (int i = 0; i < effect.Length; i++)
    //    {
    //        effect[i] = false;
    //    }
    //}
    
    private void Update()
    {
        switch (gunType)
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

                break;
        }
    }
    private void LateUpdate()
    {
        switch (element)
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

                break;
        }

        //if (elementIndex == 0)
        //{
        //    sprite.color = clense;
        //}
        //else if (elementIndex == 1)
        //    sprite.color = fire;
        //else if (elementIndex == 2)
        //    sprite.color = water;
        //else if (elementIndex == 3)
        //    sprite.color = earth;
        //else if (elementIndex == 4)
        //    sprite.color = air;
    }
}
