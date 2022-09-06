using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawn : MonoBehaviour
{

    public List<GameObject> enemiestospawn;

    public enemySpawn eSpawnScript;

    //public bool warrior;
    //public bool archer;
    //public bool skull;

    //public bool visible;

    // Start is called before the first frame update
    void Start()
    {
        eSpawnScript = this;
        GameObject g = Instantiate(enemiestospawn[Random.Range(0, enemiestospawn.Count)], transform.position, Quaternion.identity);
        GameManager.GM.addSpawnedObject(g);

    }

    // Update is called once per frame
    void Update()
    {
           /* if (Input.GetButtonDown("Fire1") && GameManager.GM.playerdead) 
            {
                GameObject g = Instantiate(enemiestospawn[Random.Range(0, enemiestospawn.Count)], transform.position, Quaternion.identity);
                GameManager.GM.addSpawnedObject(g);
            }*/
    }

    public void spawnenemy()
    {
        GameObject g = Instantiate(enemiestospawn[Random.Range(0, enemiestospawn.Count)], transform.position, Quaternion.identity);
        GameManager.GM.addSpawnedObject(g);
    }



    /*public void OnBecameVisible()
    {
        visible = true;
    }*/
}
