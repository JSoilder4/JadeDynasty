using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[Serializable]
public class gun //: MonoBehaviour
{
    // damage eventually = (base type damage + (multi * floornumber)) / (Number of Shots/2) ????????
    // damage = within a range???????


    [Header("Gun")]
    public gunEnumScript.gunType gunType;//Pistol 0, Shotgun 1, Sniper 2, SMG 3
    //public gunType2 name = gunType2.Pistol;
    [Header("Gun Modifiers")]
    public float damage;
    public float betweenShotTimer;

    public float dps;
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
                                                //

    public int floor;

    
    

    
    public gun()
    {
        gunType = gunEnumScript.gunType.Pistol;
        damage = 10;
        bSTog = 0.25f;
        reloadTimer = 1f;
        magazine = 6;
        shotSpeed = 0.22f;
        numShots = 1;
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


    public gunEnumScript.trigger FlipATrigger(gunEnumScript.trigger trigger0, gunEnumScript.trigger trigger1) //god john you're such a fucking dumbass
    {
        int r = UnityEngine.Random.Range(0,2);
        if(r == 0)
        {
            return trigger0;
        }
        

        return trigger1;
    }
    public void Roll(string codeName)
    {
       //resetRoll();
        gunType = (gunEnumScript.gunType)UnityEngine.Random.Range(0, System.Enum.GetNames(typeof(gunEnumScript.gunType)).Length);
        element = (gunEnumScript.element)UnityEngine.Random.Range(0, System.Enum.GetNames(typeof(gunEnumScript.element)).Length);

        if(codeName == ""){
            effect = RollEffect();//(gunEnumScript.effect)UnityEngine.Random.Range(0, System.Enum.GetNames(typeof(gunEnumScript.effect)).Length);
        }
        

        
        switch (gunType)
        {
            case gunEnumScript.gunType.Pistol:
                //dps = UnityEngine.Random.Range(28.0f, 66.0f);
                numShots = UnityEngine.Random.Range(1, 4); // 1 - 3 shots
                triggerType = (gunEnumScript.trigger)UnityEngine.Random.Range(0, System.Enum.GetNames(typeof(gunEnumScript.trigger)).Length);

                damage = 10 + (2*floor);

                //betweenShotTimer = (float)1/(dps/damage);

                if(numShots != 1)
                {
                    float numShotMulti = (1+(numShots/10.0f));
                    damage = (damage*numShotMulti)/numShots;
                }
                
                
                betweenShotTimer = UnityEngine.Random.Range(0.15f, 0.35f);

                dps = (damage*(numShots))*(1/betweenShotTimer);

                magazine = UnityEngine.Random.Range(6,13);
                reloadTimer = 0.25f*magazine;//UnityEngine.Random.Range(0.25f*magazine, 0.5f*magazine);
                shotSpeed = UnityEngine.Random.Range(0.22f, 0.65f);
                
                scatterAngle = 1;
                spread = UnityEngine.Random.Range(1, 6);
                
                break;
            case gunEnumScript.gunType.Shotgun:
                numShots = UnityEngine.Random.Range(3, 10);//3 - 9 shots
                triggerType = FlipATrigger(gunEnumScript.trigger.Semi, gunEnumScript.trigger.Burst);

                damage = 10 + (2 * floor);
                if(numShots != 1)
                {
                    float numShotMulti = (1+(numShots/10.0f)) * 2;
                    damage = (damage*numShotMulti)/numShots;
                }
                dps = (damage*(numShots))*(1/betweenShotTimer);
                
                betweenShotTimer = UnityEngine.Random.Range(0.35f, 0.55f);
                magazine = UnityEngine.Random.Range(2,7);
                reloadTimer = 0.5f*magazine;//UnityEngine.Random.Range(0.25f*magazine, 0.5f*magazine);
                shotSpeed = UnityEngine.Random.Range(0.22f, 0.4f);
                
                scatterAngle = 1;//used to be 5
                spread = UnityEngine.Random.Range(3, 7); //3 - 6
                
                break;
            case gunEnumScript.gunType.Sniper:
                numShots = 1;
                triggerType = FlipATrigger(gunEnumScript.trigger.Semi, gunEnumScript.trigger.Burst);

                damage = 30 + (2 * floor);
                
                if(numShots != 1)
                {
                    float numShotMulti = (1+(numShots/10.0f));
                    damage = (damage*numShotMulti)/numShots;
                }
                dps = (damage*(numShots))*(1/betweenShotTimer);

                betweenShotTimer = UnityEngine.Random.Range(0.55f, 0.75f);
                
                magazine = UnityEngine.Random.Range(1,5); // 1 - 4
                reloadTimer = 0.5f*magazine;//UnityEngine.Random.Range(0.5f*magazine, 1f*magazine);
                shotSpeed = UnityEngine.Random.Range(0.3f, 1f);
                
                scatterAngle = 1;
                spread = UnityEngine.Random.Range(1, 3);
                
                break;
            case gunEnumScript.gunType.SMG:
                numShots = UnityEngine.Random.Range(1, 3);//1 - 2 shots
                triggerType = gunEnumScript.trigger.Auto;//find a way to make burst balanced on smg?     //FlipATrigger(gunEnumScript.trigger.Auto, gunEnumScript.trigger.Burst);

                damage = 5 + (2 * floor);
                
                if(numShots != 1)
                {
                    float numShotMulti = (1+(numShots/10.0f));
                    damage = (damage*numShotMulti)/numShots;
                }
                dps = (damage*(numShots))*(1/betweenShotTimer);

                betweenShotTimer = UnityEngine.Random.Range(0.05f, 0.15f);
                
                magazine = UnityEngine.Random.Range(30,46); // 30 - 45
                reloadTimer = 0.03f*magazine;//UnityEngine.Random.Range(0.03f*magazine, 0.05f*magazine);
                shotSpeed = UnityEngine.Random.Range(0.22f, 0.4f);
                
                scatterAngle = 1;
                spread = UnityEngine.Random.Range(1, 6);
                
                break;
        }

        switch (effect)
        {
            case gunEnumScript.effect.Infinity: //look into cases of infinity + stasis, could be too strong
                //magazine = int.MaxValue;
                //damage /= 2;
                break;
            case gunEnumScript.effect.Backwards:
                scatterAngle = 180;
                spread = 5;
                damage *= 1.25f;
                break;
            case gunEnumScript.effect.BackShot:
                scatterAngle = 180;
                spread = 4;
                if(numShots == 1){
                    numShots += 1;
                }
                
                break;
            case gunEnumScript.effect.Broken:
                //scatterAngle = 180;
                spread = 180;
                //numShots *= 2;UnityEngine.Random.Range(1, 10);
                
                break;

            default:
                break;
        }
        magAmmo = magazine;
        bSTog = betweenShotTimer;

    }

    public static gunEnumScript.effect RollEffect()
    {
        int[] weights = gunEnumScript.effectWeightTable.Values.ToArray();

        int randomWeight = UnityEngine.Random.Range(0, weights.Sum());


        for(int i = 0; i < weights.Length; i++)
        {
           // Debug.Log(weights[i] + " "+i);
            randomWeight -= weights[i];
            if (randomWeight < 0)
            {
                return (gunEnumScript.effect) i;
            }
        }
        return gunEnumScript.effect.Nothing;
    }


    public static int RollNumShots()
    {
        int[] weights = numShotWeightTable.Values.ToArray();

        int randomWeight = UnityEngine.Random.Range(0, weights.Sum());


        for(int i = 1; i <= weights.Length; i++)
        {
           // Debug.Log(weights[i] + " "+i);
            randomWeight -= weights[i];
            if (randomWeight < 0)
            {
                return i;//(gunEnumScript.effect) i;
            }
        }
        return 1;
    }
    public static Dictionary<int, int> numShotWeightTable = new Dictionary<int, int>()
    {
        {1, 50},
        {2, 30},
        {3, 30},
        {4, 30},
        {5, 30},
        //{effect.Nothing, 30},
        //{effect.Nothing, 30},



    };


}
