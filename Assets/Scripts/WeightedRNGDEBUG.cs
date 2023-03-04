using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[ExecuteAlways]
public class WeightedRNGDEBUG : MonoBehaviour
{
    

    //public gunEnumScript.effect effect;

    private gunEnumScript.effect effect2;

    private GameManager.dropsEmum fun;
    //public float percentChance;
    
    private float testWeight;
    private float testSum;

    //public string[] effectNames;

    private float percentSum;



    // Start is called before the first frame update
    
    void Start()
    {
        Debug.Log("TestStart");

        //effectNames = gunEnumScript.effectWeightTable.;

        testSum = gunEnumScript.effectWeightTable.Values.Sum();
        for(int i = 0; i < gunEnumScript.effectWeightTable.Count; i++)
        {
            effect2 = (gunEnumScript.effect)i;
            testWeight = gunEnumScript.effectWeightTable[effect2];

            float percent = (Mathf.Round(((testWeight / testSum) * 100)*100.0f)/100.0f);
            Debug.Log(effect2+": " + percent+"%");
            percentSum += percent;
        }
            Debug.Log("Total :"+percentSum+"%"); //technicaly useless but also fun in a self serving way



        testDropChance();

    }

    public void testDropChance()
    {
        percentSum = 0;
        testSum = GameManager.enemyDropTable.Values.Sum();
        for (int i = 0; i < GameManager.enemyDropTable.Count; i++)
        {
            fun = (GameManager.dropsEmum)i;
            testWeight = GameManager.enemyDropTable[fun];

            float percent = (Mathf.Round(((testWeight / testSum) * 100) * 100.0f) / 100.0f);
            Debug.Log(fun + ": " + percent + "%");
            percentSum += percent;
        }



        Debug.Log("Total :" + percentSum + "%");
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("TestUpdate: "+ gunEnumScript.effectWeightTable.Values.Sum());

        //testWeight = gunEnumScript.effectWeightTable[effect];
        //testSum = gunEnumScript.effectWeightTable.Values.Sum();

        //percentChance = Mathf.Round(((testWeight / testSum) * 100)*100.0f)/100.0f;
    }
}
