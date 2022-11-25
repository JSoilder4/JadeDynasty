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
    public int spread;
    public gunEnumScript.trigger triggerType; // 0 Semi, 1 Auto, 2 Burst
    
    [Header("Shot")]
    public float shotSpeed;
    public int numShots;
    public float scatterAngle;
    public gunEnumScript.element element;        //nothing 0, fire 1, water 2, earth 3, air 4
    [Header("Shot Effect")]
    public gunEnumScript.effect effect;         //nothing 0, sciShot 1, split 2, explode 3, radiation 4, 

    
    

    
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


    public gunEnumScript.trigger FlipATrigger(gunEnumScript.trigger trigger0, gunEnumScript.trigger trigger1) //god john your such a fucking dumbass
    {
        int r = UnityEngine.Random.Range(0,2);
        if(r == 0)
        {
            return trigger0;
        }
        

        return trigger1;
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
                numShots = UnityEngine.Random.Range(0, 3);


                damage = 10;
                betweenShotTimer = UnityEngine.Random.Range(0.15f, 0.35f);
                reloadTimer = UnityEngine.Random.Range(0.5f, 1f);
                magazine = UnityEngine.Random.Range(6,13);
                shotSpeed = UnityEngine.Random.Range(0.1f, 0.4f);
                
                scatterAngle = 1;
                spread = UnityEngine.Random.Range(0, 5);
                triggerType = (gunEnumScript.trigger)UnityEngine.Random.Range(0, System.Enum.GetNames(typeof(gunEnumScript.trigger)).Length);
                break;
            case gunEnumScript.gunType.Shotgun:
                numShots = UnityEngine.Random.Range(1, 4);


                damage = 5;
                betweenShotTimer = UnityEngine.Random.Range(0.35f, 0.55f);
                reloadTimer = UnityEngine.Random.Range(0.5f, 1f);
                magazine = UnityEngine.Random.Range(2,7);
                shotSpeed = UnityEngine.Random.Range(0.1f, 0.3f);
                
                scatterAngle = 5;
                spread = UnityEngine.Random.Range(0, 5);
                triggerType = FlipATrigger(gunEnumScript.trigger.Semi, gunEnumScript.trigger.Burst);
                break;
            case gunEnumScript.gunType.Sniper:
                numShots = 0;


                damage = 30;
                betweenShotTimer = UnityEngine.Random.Range(0.55f, 0.75f);
                reloadTimer = UnityEngine.Random.Range(0.5f, 1f);
                magazine = UnityEngine.Random.Range(1,5);
                shotSpeed = UnityEngine.Random.Range(0.3f, 0.5f);
                
                scatterAngle = 1;
                spread = UnityEngine.Random.Range(0, 2);
                triggerType = FlipATrigger(gunEnumScript.trigger.Semi, gunEnumScript.trigger.Burst);
                break;
            case gunEnumScript.gunType.SMG:
                numShots = UnityEngine.Random.Range(0, 2);


                damage = 2;
                betweenShotTimer = UnityEngine.Random.Range(0.05f, 0.15f);
                reloadTimer = UnityEngine.Random.Range(0.5f, 1f);
                magazine = 30;//UnityEngine.Random.Range(6,13);
                shotSpeed = UnityEngine.Random.Range(0.1f, 0.4f);
                
                scatterAngle = 1;
                spread = UnityEngine.Random.Range(0, 5);
                triggerType = gunEnumScript.trigger.Auto;//find a way to make burst balanced on smg?     //FlipATrigger(gunEnumScript.trigger.Auto, gunEnumScript.trigger.Burst);
                break;
        }
        magAmmo = magazine;
        bSTog = betweenShotTimer;

    }

}
