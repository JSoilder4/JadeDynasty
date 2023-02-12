using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exploding : shot
{
    public float scaleMod;
    public Vector3 scaleModVector;

   // public SpriteRenderer sprite;

    public AudioClip explosion;
   // public AudioSource mySource;
    //public int damage;

    //public gunEnumScript.element shotElement;
    // Start is called before the first frame update
    void Start()
    {
        damage = playergun.gunScript.activeGun.damage;
        sprite = GetComponent<SpriteRenderer>();
        scaleModVector = new Vector3(scaleMod, scaleMod, 0);
        mySource = GetComponent<AudioSource>();
        playExplosionSound();
        sprite.color = playergun.elementalColors[(int)element];
        Destroy(gameObject, 0.25f);
    }
    public void playExplosionSound()
    {
        mySource.clip = explosion;
        mySource.Play();
    }
    // Update is called once per frame
    void Update()
    {
    
    }
    private void FixedUpdate()
    {
        transform.localScale += scaleModVector;
        // switch (shotElement)
        // {
        //     case gunEnumScript.element.Nothing:
        //         //sprite.color = clense;
        //         break;
        //     case gunEnumScript.element.Fire:
        //         sprite.color = fire;
        //         break;
        //     case gunEnumScript.element.Water:
        //         sprite.color = water;
        //         break;
        //     case gunEnumScript.element.Lightning:
        //         sprite.color = lightning;
        //         break;
        //     case gunEnumScript.element.Stasis:
        //         sprite.color = stasis;
        //         break;
        // }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Destructable"))
        {
            Destroy(collision.gameObject);
        }
    }
}
