using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class playergun : MonoBehaviour
{
    //gunType, damage, bST, reload, ammo?, shotspeed, element, effect, numshots, burstfire?, auto?,
    //enum, int, float, float, int, float, enum, enum, int, bool?, bool?


    public gun activeGun;
    public List<gun> equippedGuns;
    public int gunIndex;

    public const int pistolAmmoMax = 144, sniperAmmoMax = 48, smgAmmoMax = 360, shotgunAmmoMax = 72; //mag size: 6-12, 1-4, 30?, 2-6
    public int pistolAmmo, sniperAmmo, smgAmmo, shotgunAmmo;
    public bool reloading;

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

    //public float scatterAngle;
    //public int numShots;

    public AudioClip shootSound;
    public AudioSource mySource;

    public TextMeshProUGUI reloadText;
    // Start is called before the first frame update
    void Start()
    {
        mySource = GetComponent<AudioSource>();
        pms = GetComponentInParent<playerMove>();
        firePoint = GetComponentInParent<Transform>();
        sprite = GetComponent<SpriteRenderer>();
        gunScript = this;
        clense = sprite.color;
        reloadText = pms.gameObject.GetComponentInChildren<TextMeshProUGUI>();


        activeGun = new gun();
        equippedGuns.Add(activeGun);

        pistolAmmo = pistolAmmoMax;
        shotgunAmmo = shotgunAmmoMax;
        sniperAmmo = sniperAmmoMax;
        smgAmmo = smgAmmoMax;
    }

    // Update is called once per frame
    void Update()
    {
        //print(equippedGuns.Count);
        gunIndex = Mathf.Clamp(gunIndex, 0, equippedGuns.Count-1);
        activeGun.betweenShotTimer -= Time.deltaTime;
        // lookDirection = playerMove.pms.lookDir;

        if (GameManager.GM.started && !playerMove.pms.dodging && !reloading)
        {
            if (Input.GetButtonDown("Fire1")) //semi-auto
            {
                if (activeGun.betweenShotTimer <= 0 && activeGun.magAmmo > 0)
                {
                    if (activeGun.gunType == gunEnumScript.gunType.Pistol || activeGun.gunType == gunEnumScript.gunType.Sniper)
                    {
                        shoot(false);
                    }
                    else if (activeGun.gunType == gunEnumScript.gunType.Shotgun)
                    {
                        shoot(true);
                    }

                }
                else if(activeGun.magAmmo <= 0)
                {
                    StartCoroutine(reload());
                }
            }
            if (Input.GetMouseButton(0)) //full auto
            {
                if (activeGun.betweenShotTimer <= 0 && activeGun.magAmmo > 0)
                {
                    if (activeGun.gunType == gunEnumScript.gunType.SMG)
                    {
                        shoot(false);
                    }
                }
                else if(activeGun.magAmmo <= 0)
                {
                    StartCoroutine(reload());
                }

            }

            if(Input.GetKeyDown(KeyCode.R) && activeGun.magAmmo != activeGun.magazine)
            {
                StartCoroutine(reload());
            }

        }
        
        if(!reloading)
        {
            if (Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")) > 0.1f)
            {

            }
            else if (Input.GetAxis("Mouse ScrollWheel") == 0.1f)
            {
            //++
            if(equippedGuns.Count > 1 && gunIndex != equippedGuns.Count-1)
            {
                gunIndex++;
            }
            else if (gunIndex == equippedGuns.Count - 1)
            {
                gunIndex = 0;
            }
            
            if (gunIndex >= equippedGuns.Count)
            {
                gunIndex = 0;
            }
            GameManager.GM.gunSwapUI(equippedGuns[gunIndex]);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") == -0.1f)
            {
            //--
            gunIndex--;
            if (gunIndex < 0)
            {
                gunIndex = equippedGuns.Count-1;
            }
            GameManager.GM.gunSwapUI(equippedGuns[gunIndex]);
            }
            activeGun = equippedGuns[Mathf.Clamp(gunIndex, 0, equippedGuns.Count-1)];
        }
        //print(Input.GetAxis("Mouse ScrollWheel"));
        

        print(activeGun.magAmmo+"/"+activeGun.magazine);
        print("Pistol Ammo:"+pistolAmmo+"\nSniper Ammo:"+sniperAmmo+"\nSMG Ammo: "+smgAmmo+"\nShotgun Ammo: "+shotgunAmmo);
        spriteUpdate();
    }

    public void ammoPickup()
    {

    }
    IEnumerator reload()
    {
        reloading = true;
        reloadText.enabled = true;
        print("reload timer:"+activeGun.reloadTimer);
        reloadText.text = "Reloading";
        yield return new WaitForSeconds(activeGun.reloadTimer/4);
        reloadText.text = "Reloading.";
        yield return new WaitForSeconds(activeGun.reloadTimer/4);
        reloadText.text = "Reloading..";
        yield return new WaitForSeconds(activeGun.reloadTimer/4);
        reloadText.text = "Reloading...";
        yield return new WaitForSeconds(activeGun.reloadTimer/4);

        reload(activeGun.gunType);
        reloadText.enabled = false;
        reloadText.text = "Reloading";
        yield return null;
    }
    public void reload(gunEnumScript.gunType type) //use with animator later? or IEnumerator coroutine
    {
        print("reloading");
        switch(type)
        {
            case gunEnumScript.gunType.Pistol:
            if(pistolAmmo <= 0)
            {
                print("Reload Fail: No ammo");
                break;
            } 
            else if(pistolAmmo < (activeGun.magazine - activeGun.magAmmo))
            {
                print("Reload Partial Fail: Not enough ammo");
                activeGun.magAmmo = pistolAmmo;
                pistolAmmo -= activeGun.magAmmo;
                break;
            }
            else
                pistolAmmo -= (activeGun.magazine - activeGun.magAmmo);
                activeGun.magAmmo = activeGun.magazine;
            break;

            case gunEnumScript.gunType.Sniper:
            if(sniperAmmo <= 0)
            {
                print("Reload Fail: No ammo");
                break;
            } 
            else if(sniperAmmo < (activeGun.magazine - activeGun.magAmmo))
            {
                print("Reload Partial Fail: Not enough ammo");
                activeGun.magAmmo = sniperAmmo;
                sniperAmmo -= activeGun.magAmmo;
                break;
            }
            else
                sniperAmmo -= (activeGun.magazine - activeGun.magAmmo);
                activeGun.magAmmo = activeGun.magazine;
            break;

            case gunEnumScript.gunType.SMG:
            if(smgAmmo <= 0)
            {
                print("Reload Fail: No ammo");
                break;
            } 
            else if(smgAmmo < (activeGun.magazine - activeGun.magAmmo))
            {
                print("Reload Partial Fail: Not enough ammo");
                activeGun.magAmmo = smgAmmo;
                smgAmmo -= activeGun.magAmmo;
                break;
            }
            else
                smgAmmo -= (activeGun.magazine - activeGun.magAmmo);
                activeGun.magAmmo = activeGun.magazine;
            break;

            case gunEnumScript.gunType.Shotgun:
            if(shotgunAmmo <= 0)
            {
                print("Reload Fail: No ammo");
                break;
            } 
            else if(shotgunAmmo < (activeGun.magazine - activeGun.magAmmo))
            {
                print("Reload Partial Fail: Not enough ammo");
                activeGun.magAmmo = shotgunAmmo;
                shotgunAmmo -= activeGun.magAmmo;
                break;
            }
            else
                shotgunAmmo -= (activeGun.magazine - activeGun.magAmmo);
                activeGun.magAmmo = activeGun.magazine;
            break;

        }
        reloading = false;
        
    }

    public void resetGun()
    {
        equippedGuns.Clear();
        activeGun = new gun();
        equippedGuns.Add(activeGun);


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
    public void shoot(bool shotgun)
    {
        mySource.PlayOneShot(shootSound); //sumner stinky
        activeGun.betweenShotTimer = activeGun.bSTog;
        activeGun.magAmmo--;

        if (activeGun.numShots >= 2 && !shotgun) 
        {
            shotgun = true;
            activeGun.numShots -= 1;
        }
        
        if (shotgun)
        {
            for (int i = -activeGun.numShots; i <= activeGun.numShots; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

                bullet.transform.Rotate(bullet.transform.forward, activeGun.scatterAngle * i + Random.Range(-activeGun.spread, activeGun.spread+1));

                bullet.GetComponent<shot>().effect = activeGun.effect;
                Destroy(bullet, 10f);
            }
        }
        else
        {
            for (int i = 0; i <= activeGun.numShots; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

                bullet.transform.Rotate(bullet.transform.forward, activeGun.scatterAngle * (i*Random.Range(1, 6)*(Random.Range(0, 2)*2-1)) + Random.Range(-activeGun.spread, activeGun.spread+1));

                bullet.GetComponent<shot>().effect = activeGun.effect;
                Destroy(bullet, 10f);
            }
        }


    
    }
    //public void shoot(gunEnumScript.gunType shotgun)
    //{
    //    mySource.PlayOneShot(shootSound); //sumner stinky
    //    activeGun.betweenShotTimer = activeGun.bSTog;
    //    activeGun.magAmmo--;
    //    for (int i = -activeGun.numShots; i <= activeGun.numShots; i++)
    //    {
    //        //int i2 = i - 1;
    //        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    //        //GameManager.GM.addSpawnedObject(bullet);

    //        bullet.transform.Rotate(bullet.transform.forward, activeGun.scatterAngle * i);


    //        //bullet.GetComponent<shot>().overrideDirection = new Vector3(lookDirection.x + Mathf.Cos(scatterAngle)*i, lookDirection.y + Mathf.Sin(scatterAngle)*i, lookDirection.z);
            
    //        bullet.GetComponent<shot>().effect = activeGun.effect;
    //        Destroy(bullet, 5f);
    //    }
    //}
    //public void shotgunShoot()
    //{
    //    //numshots
    //    //scatterAngle
    //    playShootSound();
    //    activeGun.betweenShotTimer = activeGun.bSTog;
    //    for (int i = -activeGun.numShots; i <= activeGun.numShots; i++)
    //    {
    //        //int i2 = i - 1;
    //        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    //        //GameManager.GM.addSpawnedObject(bullet);

    //        bullet.transform.Rotate(bullet.transform.forward, scatterAngle*i);


    //        //bullet.GetComponent<shot>().overrideDirection = new Vector3(lookDirection.x + Mathf.Cos(scatterAngle)*i, lookDirection.y + Mathf.Sin(scatterAngle)*i, lookDirection.z);
    //        bullet.GetComponent<shot>().shotgun = true;
    //        bullet.GetComponent<shot>().effect = activeGun.effect;
    //        Destroy(bullet, 5f);
    //    }
    //}

}
