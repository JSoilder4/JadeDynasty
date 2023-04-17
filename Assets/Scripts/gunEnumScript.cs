using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum gunType2
{
    Pistol,
    Shotgun,
    Sniper,
    SMG
}


public class gunEnumScript : MonoBehaviour
{
    //public static gunEnumScript gunEnums;
    public enum gunType
    {
        Pistol,
        Shotgun,
        Sniper,
        SMG
    }
    public enum element
    {
        Nothing,
        Fire,
        Water,
        Lightning,
        Stasis
    }
    public enum effect
    {
        Nothing, 
        Boomerang, 
        EXPLOSION, 
        Comet, 
        BigBullets,
        Infinity,
    }
    public enum trigger
    {
        Semi,
        Auto,
        Burst
    }

    public static Dictionary<effect, int> effectWeightTable = new Dictionary<effect, int>()
    {
        {effect.Nothing, 50},
        {effect.Boomerang, 30},
        {effect.EXPLOSION, 30},
        {effect.Comet, 30},
        {effect.BigBullets, 30},
        {effect.Infinity, 30},
        //{effect.Nothing, 30},



    };

    // Start is called before the first frame update
    //void Awake()
    //{
    //    gunEnums = this;
    //}
}
