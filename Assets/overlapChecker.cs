using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class overlapChecker : MonoBehaviour
{

    public BoxCollider2D bc2D;

    public GenerationManager genManage;

    public Collider2D[] col2Ds;

    

    // Start is called before the first frame update
    void Start()
    {
        bc2D = GetComponent<BoxCollider2D>();

        genManage = GameObject.FindWithTag("GameController").GetComponent<GenerationManager>();

        col2Ds = new Collider2D[2];
    }

    // Update is called once per frame
    void Update()
    {
        //if (bc2D.OverlapCollider(new ContactFilter2D(), col2Ds) > 1)
        //{



        //    //print(bc2D.OverlapCollider(new ContactFilter2D(), col2Ds));
            
        //    //
        //}

        for (int i = 0; i < col2Ds.Length; i++)
        {
            //print(col2Ds[i].transform.parent.name +" "+ i);
        }
        if (col2Ds[0].transform.parent.name != col2Ds[1].transform.parent.name)
        {
            Debug.Break();
            print("Overlap Detected, Retrying.");
            genManage.RetryLevel();
        }
        else
        {
            //print("Deleting");
            col2Ds[1] = null;
        }

    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("roomCol"))
        {
           // Debug.Break();
            print("Overlap Detected, Retrying.");
            genManage.RetryLevel();
        }
    }

}
