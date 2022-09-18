using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun
{
    [Header("Gun")]
    public gunEnumScript.gunType gunType;        //Pistol 0, Shotgun 1, Sniper 2, SMG 3
    [Header("Gun Modifiers")]
    public int damage;
    public float betweenShotTimer;
    public float bSTog;               //original of ^(betweenShotTimerOG) (CHANGE THIS WHEN SWITCHING WEAPONS)
    public float reloadTimer;
    [Header("Shot")]
    public float shotSpeed;
    public gunEnumScript.element element;        //nothing 0, fire 1, water 2, earth 3, air 4
    [Header("Shot Effect")]
    public gunEnumScript.effect effect;         //nothing 0, sciShot 1, split 2, explode 3, radiation 4, 

    public float scatterAngle;
    public int numShots;

    public gun()
    {
        gunType = gunEnumScript.gunType.Pistol;
        damage = 10;
        bSTog = 0.25f;
        reloadTimer = 1f;
        shotSpeed = 0.2f;
        numShots = 0;
        element = gunEnumScript.element.Nothing;
        effect = gunEnumScript.effect.Nothing;
    }
    public gun(gunEnumScript.gunType type, int dmg, float bST, float reload, float shspeed, int numOfShots, gunEnumScript.element elem, gunEnumScript.effect theEffect)
    {
        gunType = type;
        damage = dmg;
        bSTog = bST;
        reloadTimer = reload;
        shotSpeed = shspeed;
        numShots = numOfShots;
        element = elem;
        effect = theEffect;
    }



}
