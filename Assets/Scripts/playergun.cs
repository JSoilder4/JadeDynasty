﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playergun : MonoBehaviour
{
    //gunType, damage, bST, reload, ammo?, shotspeed, element, effect, numshots, burstfire?, auto?,
    //enum, int, float, float, int, float, enum, enum, int, bool?, bool?


    public gun activeGun;
    public gun[] equippedGuns;
    public static playergun gunScript;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public playerMove pms;
    public Vector3 lookDirection;
    [Header("Gun")]
    public SpriteRenderer sprite;
    //public gunEnumScript.gunType gunType;//Pistol 0, Shotgun 1, Sniper 2, SMG 3
    [Header("Gun Modifiers")]
    //public int damage;
    //public float betweenShotTimer;
    //public float bSTog; //original of ^(betweenShotTimerOG) (CHANGE THIS WHEN SWITCHING WEAPONS)
    //public float reloadTimer;
    [Header("Shot")]
    //public float shotSpeed;
    //public gunEnumScript.element element;//nothing 0, fire 1, water 2, earth 3, air 4
    [Header("Shot Effect")]
    //public gunEnumScript.effect effect;//nothing 0, sciShot 1, split 2, explode 3, radiation 4, 


    public Sprite[] gunSprites; // pistol 0, shotgun 1, sniper 2, smg 3
    //public Sprite pistol, shotgun, sniper, smg;
    public Color clense;
    public Color fire = new Color(255, 0, 0);
    public Color water = new Color(0, 130, 255);
    public Color earth = new Color(0, 255, 0);
    public Color air = new Color(255, 255, 0);

    public float scatterAngle;
    //public int numShots;

    public AudioClip shootSound;
    public AudioSource mySource;
    // Start is called before the first frame update
    void Start()
    {
        mySource = GetComponent<AudioSource>();
        pms = GetComponentInParent<playerMove>();
        firePoint = GetComponentInParent<Transform>();
        sprite = GetComponent<SpriteRenderer>();
        gunScript = this;
        clense = sprite.color;



        activeGun = new gun(); //replaces below when done
        equippedGuns = new gun[4];

        //gunType = gunEnumScript.gunType.Pistol;
        //damage = 10;
        //bSTog = 0.25f;
        //betweenShotTimer = bSTog;
        //reloadTimer = 1f;
        //shotSpeed = 0.2f;
        //numShots = 0;
    }

    // Update is called once per frame
    void Update()
    {
        activeGun.betweenShotTimer -= Time.deltaTime;
       // lookDirection = playerMove.pms.lookDir;


        if (Input.GetButtonDown("Fire1")) //semi-auto
        {
            if (activeGun.betweenShotTimer <= 0)
            {
                if (activeGun.gunType == gunEnumScript.gunType.Pistol || activeGun.gunType == gunEnumScript.gunType.Sniper)
                {
                    shoot();
                }
                else if (activeGun.gunType == gunEnumScript.gunType.Shotgun)
                {
                    shoot(activeGun.gunType);
                }

            }
        }
        if (Input.GetMouseButton(0)) //full auto
        {
            if (activeGun.betweenShotTimer <= 0)
            {
                if (activeGun.gunType == gunEnumScript.gunType.SMG)
                {
                    shoot();
                }
            }
                
        }
        spriteUpdate();
    }
    public void resetGun()
    {
        activeGun = new gun();

        //gunType = gunEnumScript.gunType.Pistol;
        //damage = 10;
        //bSTog = 0.25f;
        //betweenShotTimer = bSTog;
        //reloadTimer = 1f;
        //shotSpeed = 0.2f;
        //numShots = 0;
        //element = gunEnumScript.element.Nothing;
        //effect = gunEnumScript.effect.Nothing;
    }
    public void spriteUpdate()
    {
        switch (activeGun.gunType)
        {
            case gunEnumScript.gunType.Pistol:
                sprite.sprite = gunSprites[(int)gunEnumScript.gunType.Pistol];
                break;
            case gunEnumScript.gunType.Shotgun:
                sprite.sprite = gunSprites[(int)gunEnumScript.gunType.Shotgun];
                break;
            case gunEnumScript.gunType.Sniper:
                sprite.sprite = gunSprites[(int)gunEnumScript.gunType.Sniper];
                break;
            case gunEnumScript.gunType.SMG:
                sprite.sprite = gunSprites[(int)gunEnumScript.gunType.SMG];
                break;
        }
        switch (activeGun.element)
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
    public void playShootSound()
    {
        mySource.clip = shootSound;
        mySource.Play();
    }
    public void shoot()
    {
        mySource.PlayOneShot(shootSound); //sumner stinky
        activeGun.betweenShotTimer = activeGun.bSTog;
        for (int i = 0; i <= activeGun.numShots; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            bullet.transform.Rotate(bullet.transform.forward, scatterAngle * i);

            bullet.GetComponent<shot>().effect = activeGun.effect;
            Destroy(bullet, 5f);
        }
    }
    public void shoot(gunEnumScript.gunType shotgun)
    {
        mySource.PlayOneShot(shootSound); //sumner stinky
        activeGun.betweenShotTimer = activeGun.bSTog;
        for (int i = -activeGun.numShots; i <= activeGun.numShots; i++)
        {
            //int i2 = i - 1;
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            //GameManager.GM.addSpawnedObject(bullet);

            bullet.transform.Rotate(bullet.transform.forward, scatterAngle * i);


            //bullet.GetComponent<shot>().overrideDirection = new Vector3(lookDirection.x + Mathf.Cos(scatterAngle)*i, lookDirection.y + Mathf.Sin(scatterAngle)*i, lookDirection.z);
            bullet.GetComponent<shot>().shotgun = true;
            bullet.GetComponent<shot>().effect = activeGun.effect;
            Destroy(bullet, 5f);
        }
    }
    public void shotgunShoot()
    {
        //numshots
        //scatterAngle
        playShootSound();
        activeGun.betweenShotTimer = activeGun.bSTog;
        for (int i = -activeGun.numShots; i <= activeGun.numShots; i++)
        {
            //int i2 = i - 1;
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            //GameManager.GM.addSpawnedObject(bullet);

            bullet.transform.Rotate(bullet.transform.forward, scatterAngle*i);


            //bullet.GetComponent<shot>().overrideDirection = new Vector3(lookDirection.x + Mathf.Cos(scatterAngle)*i, lookDirection.y + Mathf.Sin(scatterAngle)*i, lookDirection.z);
            bullet.GetComponent<shot>().shotgun = true;
            bullet.GetComponent<shot>().effect = activeGun.effect;
            Destroy(bullet, 5f);
        }
    }

}
