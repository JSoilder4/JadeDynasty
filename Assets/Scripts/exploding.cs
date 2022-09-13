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

    //public Color fire = new Color(255, 0, 0);
    //public Color water = new Color(0, 130, 255);
    //public Color earth = new Color(0, 255, 0);
    //public Color air = new Color(255, 255, 0);
    //public int damage;

    public gunEnumScript.element shotElement;
    // Start is called before the first frame update
    void Start()
    {
        damage = gun.gunScript.damage;
        sprite = GetComponent<SpriteRenderer>();
        scaleModVector = new Vector3(scaleMod, scaleMod, 0);
        mySource = GetComponent<AudioSource>();
        playExplosionSound();
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
        transform.localScale += scaleModVector;
        switch (shotElement)
        {
            case gunEnumScript.element.Nothing:
                //sprite.color = clense;
                break;
            case gunEnumScript.element.Fire:
                sprite.color = fire;
                break;
            case gunEnumScript.element.Water:
                sprite.color = water;
                break;
            case gunEnumScript.element.Earth:
                sprite.color = earth;
                break;
            case gunEnumScript.element.Air:
                sprite.color = air;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Destructable"))
        {
            Destroy(collision.gameObject);
        }
    }
}
