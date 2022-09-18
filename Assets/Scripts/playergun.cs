using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playergun : MonoBehaviour
{
    //gunType, damage, bST, reload, ammo?, shotspeed, element, effect, numshots, burstfire?, auto?,
    //enum, int, float, float, int, float, enum, enum, int, bool?, bool?


    public gun theGun;
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

    public Sprite pistol, shotgun, sniper, smg;
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



        theGun = new gun(); //replaces below when done


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
        theGun.betweenShotTimer -= Time.deltaTime;
       // lookDirection = playerMove.pms.lookDir;


        if (Input.GetButtonDown("Fire1")) //semi-auto
        {
            if (theGun.betweenShotTimer <= 0)
            {
                if (theGun.gunType == gunEnumScript.gunType.Pistol || theGun.gunType == gunEnumScript.gunType.Sniper)
                {
                    shoot();
                }
                else if (theGun.gunType == gunEnumScript.gunType.Shotgun)
                {
                    shoot(theGun.gunType);
                }

            }
        }
        if (Input.GetMouseButton(0)) //full auto
        {
            if (theGun.betweenShotTimer <= 0)
            {
                if (theGun.gunType == gunEnumScript.gunType.SMG)
                {
                    shoot();
                }
            }
                
        }
        spriteUpdate();
    }
    public void resetGun()
    {
        theGun = new gun();

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
    public void playShootSound()
    {
        mySource.clip = shootSound;
        mySource.Play();
    }
    public void shoot()
    {
        mySource.PlayOneShot(shootSound); //sumner stinky
        theGun.betweenShotTimer = theGun.bSTog;
        for (int i = 0; i <= theGun.numShots; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            bullet.transform.Rotate(bullet.transform.forward, scatterAngle * i);

            bullet.GetComponent<shot>().effect = theGun.effect;
            Destroy(bullet, 5f);
        }
    }
    public void shoot(gunEnumScript.gunType shotgun)
    {
        mySource.PlayOneShot(shootSound); //sumner stinky
        theGun.betweenShotTimer = theGun.bSTog;
        for (int i = -theGun.numShots; i <= theGun.numShots; i++)
        {
            //int i2 = i - 1;
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            //GameManager.GM.addSpawnedObject(bullet);

            bullet.transform.Rotate(bullet.transform.forward, scatterAngle * i);


            //bullet.GetComponent<shot>().overrideDirection = new Vector3(lookDirection.x + Mathf.Cos(scatterAngle)*i, lookDirection.y + Mathf.Sin(scatterAngle)*i, lookDirection.z);
            bullet.GetComponent<shot>().shotgun = true;
            bullet.GetComponent<shot>().effect = theGun.effect;
            Destroy(bullet, 5f);
        }
    }
    public void shotgunShoot()
    {
        //numshots
        //scatterAngle
        playShootSound();
        theGun.betweenShotTimer = theGun.bSTog;
        for (int i = -theGun.numShots; i <= theGun.numShots; i++)
        {
            //int i2 = i - 1;
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            //GameManager.GM.addSpawnedObject(bullet);

            bullet.transform.Rotate(bullet.transform.forward, scatterAngle*i);


            //bullet.GetComponent<shot>().overrideDirection = new Vector3(lookDirection.x + Mathf.Cos(scatterAngle)*i, lookDirection.y + Mathf.Sin(scatterAngle)*i, lookDirection.z);
            bullet.GetComponent<shot>().shotgun = true;
            bullet.GetComponent<shot>().effect = theGun.effect;
            Destroy(bullet, 5f);
        }
    }

}
