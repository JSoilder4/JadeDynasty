using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelnest.BulletML;

public class badcatBullet : MonoBehaviour
{

    //public BulletSourceScript bml;
    public TextAsset xml;

    public SpriteRenderer sprite;
    public BoxCollider2D col;

    public GameObject spawnBulletSource;

    // Start is called before the first frame update
    void Start()
    {
        //bml = GetComponent<BulletSourceScript>();
        sprite = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

private void OnCollisionEnter2D(Collision2D other) {
    
}
private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Wall"))
        {
            
            split();
            sprite.enabled = false;
            
        }
    }    

    public void split(){
        //Debug.Break();
        GameObject g = Instantiate(spawnBulletSource, transform.position, Quaternion.identity);
        g.GetComponent<BulletSourceScript>().xmlFile = xml;
        //bml.xmlFile = xml;
        //bml.Reset();
        col.enabled = false;
        Destroy(gameObject, 3f);
        Destroy(g, 5f);
        //print("shut the fuck up.");
    }
}
