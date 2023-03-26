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
    public Color clense = new Color(1, 1, 1);
    public Color fire = new Color(1, 0, 0);
    public Color water = new Color(0, 0, 1);
    public Color lightning = new Color(1, 1, 0);
    public Color stasis = new Color(1, 0, 1);

    public static Color[] elementalColors = new Color[5];

    //public float scatterAngle;
    //public int numShots;

    [Header("Audio")]
    public AudioClip shootSound;
    public AudioClip shotgunShootSound;
    public AudioClip shotgunPumpSound;
    public AudioSource mySource;


    public TextMeshProUGUI reloadText;
    public TextMeshProUGUI AmmoCount;

    public Coroutine reloadIEnum;
    public Coroutine shotgunPumpIEnum;
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

        elementalColors[(int)gunEnumScript.element.Nothing] = clense;
        elementalColors[(int)gunEnumScript.element.Fire] = fire;
        elementalColors[(int)gunEnumScript.element.Water] = water;
        elementalColors[(int)gunEnumScript.element.Lightning] = lightning;
        elementalColors[(int)gunEnumScript.element.Stasis] = stasis;
    }

private void FixedUpdate() {
    gunIndex = Mathf.Clamp(gunIndex, 0, equippedGuns.Count-1);

    GameManager.GM.updateGunUI(equippedGuns, gunIndex);    //im lazy?
    activeGun.betweenShotTimer -= Time.deltaTime;
}
    // Update is called once per frame
    void Update()
    {
        //print(equippedGuns.Count);
        
        
        // lookDirection = playerMove.pms.lookDir;

        if (GameManager.GM.started && !playerMove.pms.dodging && !reloading)
        {
            // if (activeGun.triggerType == gunEnumScript.trigger.Semi && Input.GetButtonDown("Fire1")) //semi-auto
            // {
            //     if (activeGun.betweenShotTimer <= 0 && activeGun.magAmmo > 0)
            //     {
            //         if (activeGun.gunType == gunEnumScript.gunType.Pistol || activeGun.gunType == gunEnumScript.gunType.Sniper)
            //         {
            //             shoot(false);
            //         }
            //         else if (activeGun.gunType == gunEnumScript.gunType.Shotgun)
            //         {
            //             shoot(true);
            //         }

            //     }
            //     else if(activeGun.magAmmo <= 0)
            //     {
            //         StartCoroutine(reload());
            //     }
            // }
            // if (activeGun.triggerType == gunEnumScript.trigger.Auto && Input.GetMouseButton(0)) //full auto
            // {
            //     if (activeGun.betweenShotTimer <= 0 && activeGun.magAmmo > 0)
            //     {
            //         // if (activeGun.gunType == gunEnumScript.gunType.SMG)
            //         // {
            //             shoot(false);
            //         // }
            //     }
            //     else if(activeGun.magAmmo <= 0)
            //     {
            //         StartCoroutine(reload());
            //     }

            // }

            if(activeGun.gunType == gunEnumScript.gunType.Shotgun && activeGun.betweenShotTimer == activeGun.bSTog)
            {
                StartCoroutine(shotgunPump());
            }

            switch(activeGun.triggerType)
            {
                case gunEnumScript.trigger.Semi:
                    if(Input.GetButtonDown("Fire1"))
                    {
                        if (activeGun.betweenShotTimer <= 0 && activeGun.magAmmo > 0)
                        {

                            StartCoroutine(shoot(activeGun.gunType));
                            StartCoroutine(shotgunPump());

                        }
                        else if(activeGun.magAmmo <= 0)
                        {
                            StartCoroutine(reload());
                        }
                    }
                break;
                case gunEnumScript.trigger.Auto:
                    if(Input.GetMouseButton(0))
                    {
                        if (activeGun.betweenShotTimer <= 0 && activeGun.magAmmo > 0)
                        {
                            StartCoroutine(CameraFollow.CF.Shaking(0.10f,CameraFollow.ShakeCurveType.shootGun));
                            StartCoroutine(shoot(activeGun.gunType));

                        }
                        else if(activeGun.magAmmo <= 0)
                        {
                            StartCoroutine(reload());
                        }
                    }
                break;
                case gunEnumScript.trigger.Burst:
                    if(Input.GetButtonDown("Fire1"))
                    {
                        if (activeGun.betweenShotTimer <= 0 && activeGun.magAmmo > 0)
                        {
                            StartCoroutine(burstFire());
                        }
                        else if(activeGun.magAmmo <= 0)
                        {
                            StartCoroutine(reload());
                        }
                    }
                break;
            }


            if(Input.GetKeyDown(KeyCode.R) && activeGun.magAmmo != activeGun.magazine)
            {
                reloadIEnum = StartCoroutine(reload());
            }

        }
        
        //if(!reloading)
        //{
            if (Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")) > 0.1f)
            {

            }
            else if (Input.GetAxis("Mouse ScrollWheel") == -0.1f)
            {
            //++
            if (reloading)
            {
                StopCoroutine(reloadIEnum);
                reloadText.enabled = false;
                reloadText.text = "Reloading";
                reloading = false;
            }
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
            GameManager.GM.gunSwapUI(equippedGuns[gunIndex], true);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") == 0.1f)
            {
            //--
            if (reloading)
            {
                StopCoroutine(reloadIEnum);
                reloadText.enabled = false;
                reloadText.text = "Reloading";
                reloading = false;
            }
            gunIndex--;
            if (gunIndex < 0)
            {
                gunIndex = equippedGuns.Count-1;
            }
            GameManager.GM.gunSwapUI(equippedGuns[gunIndex], true);
            }
            activeGun = equippedGuns[Mathf.Clamp(gunIndex, 0, equippedGuns.Count-1)];
        //}
        //print(Input.GetAxis("Mouse ScrollWheel"));
        

        //print(activeGun.magAmmo+"/"+activeGun.magazine);
        //print("Pistol Ammo:"+pistolAmmo+"\nSniper Ammo:"+sniperAmmo+"\nSMG Ammo: "+smgAmmo+"\nShotgun Ammo: "+shotgunAmmo);
        spriteUpdate();
        updateAmmoUI();
    }

    IEnumerator burstFire()
    {   
        print(Mathf.Round(1/activeGun.bSTog));
        for(int i = 0; i < Mathf.Round(1/activeGun.bSTog); i++)
        {
            StartCoroutine(CameraFollow.CF.Shaking(0.10f,CameraFollow.ShakeCurveType.shootGun));
            StartCoroutine(shoot(activeGun.gunType));
            if(activeGun.magAmmo <= 0)
            {
                //reloadIEnum = StartCoroutine(reload());
                break;
            }
            yield return new WaitForSeconds(activeGun.bSTog/Mathf.Round(1/activeGun.bSTog));
        }

        StartCoroutine(shotgunPump());
        yield return null;
    }

    public void updateAmmoUI(){
        switch(activeGun.gunType){
            case gunEnumScript.gunType.Pistol:
            AmmoCount.text = activeGun.magAmmo+"/"+pistolAmmo;
            break;
            case gunEnumScript.gunType.Sniper:
            AmmoCount.text = activeGun.magAmmo+"/"+sniperAmmo;
            break;
            case gunEnumScript.gunType.SMG:
            AmmoCount.text = activeGun.magAmmo+"/"+smgAmmo;
            break;
            case gunEnumScript.gunType.Shotgun:
            AmmoCount.text = activeGun.magAmmo+"/"+shotgunAmmo;
            break;
        }
        AmmoCount.color = sprite.color;
    }
    public void ammoPickup()//GameObject ammoObject)
    {
        int ammoBefore;
        print("expected gain: "+ activeGun.magazine);
        switch(activeGun.gunType)
        {
            case gunEnumScript.gunType.Pistol:
            ammoBefore = pistolAmmo;
            pistolAmmo += activeGun.magazine*4;
            print("actual gain: "+(pistolAmmo - ammoBefore));
            break;

            case gunEnumScript.gunType.Shotgun:
            ammoBefore = shotgunAmmo;
            shotgunAmmo += activeGun.magazine*4;
            print("actual gain: "+(shotgunAmmo - ammoBefore));
            break;

            case gunEnumScript.gunType.Sniper:
            ammoBefore = sniperAmmo;
            sniperAmmo += activeGun.magazine*4;
            print("actual gain: "+(sniperAmmo - ammoBefore));
            break;

            case gunEnumScript.gunType.SMG:
            ammoBefore = smgAmmo;
            smgAmmo += activeGun.magazine*4;
            print("actual gain: "+(smgAmmo - ammoBefore));
            break;
        }
        //Destroy(ammoObject);


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
        //updateAmmoUI();
    }

    public void resetGun()
    {
        equippedGuns.Clear();
        activeGun = new gun();
        activeGun.roll();
        while(activeGun.element == gunEnumScript.element.Stasis) //maybe temp? crude way to prevent spawning with stasis
        {
            activeGun.roll();
        }
        equippedGuns.Add(activeGun);
        GameManager.GM.updateGunUI(equippedGuns, gunIndex);
        GameManager.GM.resetGunUI();


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
    public void resetAmmo()
    {
        pistolAmmo = pistolAmmoMax;
        shotgunAmmo = shotgunAmmoMax;
        sniperAmmo = sniperAmmoMax;
        smgAmmo = smgAmmoMax;
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
            case gunEnumScript.element.Lightning:
                sprite.color = lightning;
                break;
            case gunEnumScript.element.Stasis:
                sprite.color = stasis;
                break;
        }
    }
    // public void playShootSound()
    // {
    //     mySource.clip = shootSound;
    //     mySource.Play();
    // }
    public int shotgunShots;
    public IEnumerator shotgunPump()
    {
        if(shotgunShots == 0)
        {
            StopCoroutine(shotgunPump());
        }
        else
        {
            yield return new WaitForSeconds(0.20f);
            StartCoroutine(shotgunPumpForReal(shotgunShots));
        }
        
        //mySource.PlayOneShot(shotgunPumpSound);
    }
    public IEnumerator shotgunPumpForReal(int shotgunShooties)
    {
        for(int i = 0; i < shotgunShots; i++)
        {
            yield return new WaitForSeconds(activeGun.bSTog/shotgunShooties);
            shotgunShots--;
            print("ShotgunShots: " + shotgunShots);
            mySource.PlayOneShot(shotgunPumpSound);
        }
        //shotgunShots = 0;
    }
    public IEnumerator shoot(gunEnumScript.gunType gunType)
    {

        switch(gunType)
        {
            default:
                mySource.PlayOneShot(shootSound);//sumner stinky
            break;
            case gunEnumScript.gunType.Pistol:
            break;
            case gunEnumScript.gunType.Shotgun:
                mySource.PlayOneShot(shotgunShootSound);
                shotgunShots++;
                
            break;
            case gunEnumScript.gunType.Sniper:
            break;
            case gunEnumScript.gunType.SMG:
            break;
        }
        activeGun.betweenShotTimer = activeGun.bSTog;
        activeGun.magAmmo--;
        if(activeGun.magAmmo <= 0)
        {
            reloadIEnum = StartCoroutine(reload());
        }
        //updateAmmoUI();

        // if (activeGun.numShots >= 2 && !shotgun) 
        // {
        //     shotgun = true;
        //     activeGun.numShots -= 1;
        // }
        for (int i = 0; i < activeGun.numShots; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                if(i > 0)
                {
                    //look at later, might not be the best final solution
                    bullet.transform.Rotate(bullet.transform.forward, activeGun.scatterAngle * activeGun.spread*((Random.Range(0,2)*2)-1) + Random.Range(-activeGun.spread, activeGun.spread+1)); //* (Random.Range(-activeGun.spread, activeGun.spread)) + Random.Range(-activeGun.spread, activeGun.spread+1));
                }
                else
                {
                    bullet.transform.Rotate(bullet.transform.forward, activeGun.scatterAngle + Random.Range(-activeGun.spread, activeGun.spread+1));
                }
                

                bullet.GetComponent<shot>().effect = activeGun.effect;
                //bullet.GetComponent<shot>().sprite.color = elementalColors[(int)activeGun.element];
                Destroy(bullet, 10f);
                if(Random.Range(0, 4) > 0){
                    yield return new WaitForSeconds(Random.Range(0.0001f, 0.001f));
                }
                
            }
        // if (activeGun.numShots >= 3)
        // {
        //     if(activeGun.numShots % 2 == 0)
        //     {
        //         int g;
        //         int badMath = 0;
        //         for(int i = 1; i <= activeGun.numShots; i++)
        //         {
        //             if(i >= 4)
        //             {
        //                 if(i % 2 == 0)
        //                 {
        //                     badMath++;
        //                 }
        //             }
        //         }
        //         for (int i = 0; i < activeGun.numShots; i++)
        //         {
        //             if(i < activeGun.numShots/2)
        //             {
        //                 g = i-(activeGun.numShots/2);
        //             }
        //             else
        //             {
        //                 g = i-badMath;
        //             }
                    
        //             GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        //             bullet.transform.Rotate(bullet.transform.forward, activeGun.scatterAngle * g + Random.Range(-activeGun.spread, activeGun.spread+1));

        //             bullet.GetComponent<shot>().effect = activeGun.effect;
        //             Destroy(bullet, 10f);
        //         }
        //     }
        //     else
        //     {
        //         int numShotgun = activeGun.numShots-2;
        //         for (int i = -numShotgun; i < numShotgun; i++)
        //         {
        //             GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        //             bullet.transform.Rotate(bullet.transform.forward, activeGun.scatterAngle * i + Random.Range(-activeGun.spread, activeGun.spread+1));

        //             bullet.GetComponent<shot>().effect = activeGun.effect;
        //             Destroy(bullet, 10f);
        //         }
        //     }
            
        // }
        // else if (activeGun.numShots <= 2)
        // {
        //     for (int i = 0; i < activeGun.numShots; i++)
        //     {
        //         GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        //         bullet.transform.Rotate(bullet.transform.forward, activeGun.scatterAngle * (i*Random.Range(1, 6)*(Random.Range(0, 2)*2-1)) + Random.Range(-activeGun.spread, activeGun.spread+1));

        //         bullet.GetComponent<shot>().effect = activeGun.effect;
        //         Destroy(bullet, 10f);
        //     }
        // }

        yield return null;
    
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
