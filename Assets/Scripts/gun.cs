using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class gun //: MonoBehaviour
{
    [Header("Gun")]
    public gunEnumScript.gunType gunType;        //Pistol 0, Shotgun 1, Sniper 2, SMG 3
    [Header("Gun Modifiers")]
    public int damage;
    public float betweenShotTimer;
    public float bSTog;               //original of ^(betweenShotTimerOG) (CHANGE THIS WHEN SWITCHING WEAPONS)
    public float reloadTimer;
    public int magazine;
    public int magAmmo;
    [Header("Shot")]
    public float shotSpeed;
    public gunEnumScript.element element;        //nothing 0, fire 1, water 2, earth 3, air 4
    [Header("Shot Effect")]
    public gunEnumScript.effect effect;         //nothing 0, sciShot 1, split 2, explode 3, radiation 4, 

    public float scatterAngle;
    public int numShots;

    public int spread;
    public gun()
    {
        gunType = gunEnumScript.gunType.Pistol;
        damage = 10;
        bSTog = 0.25f;
        reloadTimer = 1f;
        magazine = 6;
        shotSpeed = 0.2f;
        numShots = 0;
        element = gunEnumScript.element.Nothing;
        effect = gunEnumScript.effect.Nothing;
        magAmmo = magazine;
    }
    public gun(gunEnumScript.gunType type, int dmg, float bST, float reload, int mag, float shspeed, int numOfShots, gunEnumScript.element elem, gunEnumScript.effect theEffect)
    {
        gunType = type;
        damage = dmg;
        bSTog = bST;
        reloadTimer = reload;
        magazine = mag;
        shotSpeed = shspeed;
        numShots = numOfShots;
        element = elem;
        effect = theEffect;
        magAmmo = magazine;
    }

    public void roll()
    {
       //resetRoll();
        gunType = (gunEnumScript.gunType)UnityEngine.Random.Range(0, System.Enum.GetNames(typeof(gunEnumScript.gunType)).Length);
        element = (gunEnumScript.element)UnityEngine.Random.Range(0, System.Enum.GetNames(typeof(gunEnumScript.element)).Length);
        effect = (gunEnumScript.effect)UnityEngine.Random.Range(0, System.Enum.GetNames(typeof(gunEnumScript.effect)).Length);

        
        switch (gunType)
        {
            case gunEnumScript.gunType.Pistol:
                damage = 10;
                betweenShotTimer = UnityEngine.Random.Range(0.15f, 0.35f);
                reloadTimer = UnityEngine.Random.Range(0.5f, 1f);
                magazine = UnityEngine.Random.Range(6,13);
                shotSpeed = UnityEngine.Random.Range(0.1f, 0.4f);
                numShots = UnityEngine.Random.Range(0, 3);
                scatterAngle = 1;
                spread = UnityEngine.Random.Range(0, 5);
                break;
            case gunEnumScript.gunType.Shotgun:
                damage = 5;
                betweenShotTimer = UnityEngine.Random.Range(0.35f, 0.55f);
                reloadTimer = UnityEngine.Random.Range(0.5f, 1f);
                magazine = UnityEngine.Random.Range(2,7);
                shotSpeed = UnityEngine.Random.Range(0.1f, 0.3f);
                numShots = UnityEngine.Random.Range(1, 4);
                scatterAngle = 5;
                spread = UnityEngine.Random.Range(0, 5);
                break;
            case gunEnumScript.gunType.Sniper:
                damage = 30;
                betweenShotTimer = UnityEngine.Random.Range(0.55f, 0.75f);
                reloadTimer = UnityEngine.Random.Range(0.5f, 1f);
                magazine = UnityEngine.Random.Range(1,5);
                shotSpeed = UnityEngine.Random.Range(0.3f, 0.5f);
                numShots = 0;
                scatterAngle = 1;
                spread = UnityEngine.Random.Range(0, 2);
                break;
            case gunEnumScript.gunType.SMG:
                damage = 2;
                betweenShotTimer = UnityEngine.Random.Range(0.05f, 0.15f);
                reloadTimer = UnityEngine.Random.Range(0.5f, 1f);
                magazine = 30;//UnityEngine.Random.Range(6,13);
                shotSpeed = UnityEngine.Random.Range(0.1f, 0.4f);
                numShots = UnityEngine.Random.Range(0, 2);
                scatterAngle = 1;
                spread = UnityEngine.Random.Range(0, 5);
                break;
        }
        magAmmo = magazine;


    }

}
