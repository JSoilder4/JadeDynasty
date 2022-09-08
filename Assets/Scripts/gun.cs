using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun : MonoBehaviour
{
    public enum gunType
    {
        Pistol,
        Shotgun,
        Sniper,
        SMG
    }
    public gunType theGunType;
    

    public static gun gunScript;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public playerMove pms;
    public Vector3 lookDirection;
    [Header("Gun")]
    public SpriteRenderer sprite;
    public int gunTypeIndex;//Pistol 0, Shotgun 1, Sniper 2, SMG 3
    [Header("Gun Modifiers")]
    public int damage;
    public float betweenShotTimer;
    public float bSTog; //original of ^ (CHANGE THIS WHEN SWITCHING WEAPONS)
    public float reloadTimer;
    [Header("Shot")]
    public float shotSpeed;
    public int elementIndex;//nothing 0, fire 1, water 2, earth 3, air 4
    [Header("Shot Effect")]
    public int effectIndex;//nothing 0, sciShot 1, split 2, explode 3, radiation 4, 

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

        theGunType = (gunType)3;
        print(theGunType);

        

        gunTypeIndex = 0;
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
                if (gunTypeIndex == 0 || gunTypeIndex == 2)
                {
                    shoot();
                }
                else if (gunTypeIndex == 1)
                {
                    shotgunShoot();
                }

            }
        }
        if (Input.GetMouseButton(0)) //full auto
        {
            if (betweenShotTimer <= 0)
            {
                if (gunTypeIndex == 3)
                {
                    shoot();
                }
            }
                
        }
        spriteUpdate();
    }
    public void resetGun()
    {
        gunTypeIndex = 0;
        damage = 10;
        bSTog = 0.25f;
        betweenShotTimer = bSTog;
        reloadTimer = 1f;
        shotSpeed = 0.2f;
        elementIndex = 0;
        effectIndex = 0;
    }
    public void spriteUpdate()
    {
        if (gunTypeIndex == 0)
        {
            sprite.sprite = pistol;
          //  transform.localScale = new Vector3(10, 10, 0);//temp
        }
        else if (gunTypeIndex == 1)
        {
            sprite.sprite = shotgun;
           // transform.localScale = new Vector3(10, 10, 0);//temp
        }
        else if (gunTypeIndex == 2)
        {
            sprite.sprite = sniper;
           // transform.localScale = new Vector3(1, 1, 0);//temp
        }
        else if (gunTypeIndex == 3)
        {
            sprite.sprite = smg;
           // transform.localScale = new Vector3(10, 10, 0);//temp
        }
        if (elementIndex == 0)
        {
            sprite.color = clense;
        }
        else if (elementIndex == 1)
            sprite.color = fire;
        else if (elementIndex == 2)
            sprite.color = water;
        else if (elementIndex == 3)
            sprite.color = earth;
        else if (elementIndex == 4)
            sprite.color = air;
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
