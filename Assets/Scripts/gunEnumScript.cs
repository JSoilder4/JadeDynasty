using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Earth,
        Air
    }
    public enum effect
    {
        Nothing, 
        Boomerang, 
        EXPLOSION, 
        Comet, 
        BigBullets
    }
    public enum trigger
    {
        Semi,
        Auto,
        Burst
    }


    // Start is called before the first frame update
    //void Awake()
    //{
    //    gunEnums = this;
    //}
}
