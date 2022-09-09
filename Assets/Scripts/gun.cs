using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun : MonoBehaviour
{
    //public enum gunType
    //{
    //    Pistol,
    //    Shotgun,
    //    Sniper,
    //    SMG
    //}
    //public gunType theGunType;
    

    public static gun gunScript;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public playerMove pms;
    public Vector3 lookDirection;
    [Header("Gun")]
    public SpriteRenderer sprite;
    public gunEnumScript.gunType gunType;//Pistol 0, Shotgun 1, Sniper 2, SMG 3
    [Header("Gun Modifiers")]
    public int damage;
    public float betweenShotTimer;
    public float bSTog; //original of ^(betweenShotTimerOG) (CHANGE THIS WHEN SWITCHING WEAPONS)
    public float reloadTimer;
    [Header("Shot")]
    public float shotSpeed;
    public gunEnumScript.element element;//nothing 0, fire 1, water 2, earth 3, air 4
    [Header("Shot Effect")]
    public gunEnumScript.effect effect;//nothing 0, sciShot 1, split 2, explode 3, radiation 4, 

    public Sprite pistol, shotgun, sniper, smg;
    public Color clense;
    public Color fire = new Color(255, 0, 0);
    public Color water = new Color(0, 130, 255);
    public Color earth = new Color(0, 255, 0);
    public Color air = new Color(255, 255, 0);

    public float scatterAngle;
    public int numShots;

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

        //theGunType = (gunType)3;
        //print(theGunType);

        

        gunType = (gunEnumScript.gunType) 0;
        damage = 10;
        bSTog = 0.25f;
        betweenShotTimer = bSTog;
        reloadTimer = 1f;
        shotSpeed = 0.2f;

    }

    // Update is called once per frame
    void Update()
    {
        betweenShotTimer -= Time.deltaTime;
       // lookDirection = playerMove.pms.lookDir;


        if (Input.GetButtonDown("Fire1")) //semi-auto
        {
            if (betweenShotTimer <= 0)
            {
                if (gunType == gunEnumScript.gunType.Pistol || gunType == gunEnumScript.gunType.Sniper)
                {
                    shoot();
                }
                else if (gunType == gunEnumScript.gunType.Shotgun)
                {
                    shotgunShoot();
                }

            }
        }
        if (Input.GetMouseButton(0)) //full auto
        {
            if (betweenShotTimer <= 0)
            {
                if (gunType == gunEnumScript.gunType.SMG)
                {
                    shoot();
                }
            }
                
        }
        spriteUpdate();
    }
    public void resetGun()
    {
        gunType = gunEnumScript.gunType.Pistol;
        damage = 10;
        bSTog = 0.25f;
        betweenShotTimer = bSTog;
        reloadTimer = 1f;
        shotSpeed = 0.2f;
        element = gunEnumScript.element.Nothing;
        effect = gunEnumScript.effect.Nothing;
    }
    public void spriteUpdate()
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

        //if (gunTypeIndex == 0)
        //{
        //    sprite.sprite = pistol;
        //  //  transform.localScale = new Vector3(10, 10, 0);//temp
        //}
        //else if (gunTypeIndex == 1)
        //{
        //    sprite.sprite = shotgun;
        //   // transform.localScale = new Vector3(10, 10, 0);//temp
        //}
        //else if (gunTypeIndex == 2)
        //{
        //    sprite.sprite = sniper;
        //   // transform.localScale = new Vector3(1, 1, 0);//temp
        //}
        //else if (gunTypeIndex == 3)
        //{
        //    sprite.sprite = smg;
        //   // transform.localScale = new Vector3(10, 10, 0);//temp
        //}
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
    public void playShootSound()
    {
        mySource.clip = shootSound;
        mySource.Play();
    }
    public void shoot()
    {
        playShootSound();
        betweenShotTimer = bSTog;
        GameObject shot = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
       // GameManager.GM.addSpawnedObject(shot);
        Destroy(shot, 5f);
    }
    public void shotgunShoot()
    {
        //numshots
        //scatterAngle
        playShootSound();
        betweenShotTimer = bSTog;
        for (int i = -numShots; i <= numShots; i++)
        {
            //int i2 = i - 1;
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            //GameManager.GM.addSpawnedObject(bullet);

            bullet.transform.Rotate(bullet.transform.forward, scatterAngle*i);


            //bullet.GetComponent<shot>().overrideDirection = new Vector3(lookDirection.x + Mathf.Cos(scatterAngle)*i, lookDirection.y + Mathf.Sin(scatterAngle)*i, lookDirection.z);
            bullet.GetComponent<shot>().shotgun = true;
            Destroy(bullet, 5f);
        }
    }

}
