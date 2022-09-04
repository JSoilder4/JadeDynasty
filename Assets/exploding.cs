using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exploding : MonoBehaviour
{
    public float scaleMod;
    public Vector3 scaleModVector;

    public SpriteRenderer sprite;

    public AudioClip explosion;
    public AudioSource mySource;

    public Color fire = new Color(255, 0, 0);
    public Color water = new Color(0, 130, 255);
    public Color earth = new Color(0, 255, 0);
    public Color air = new Color(255, 255, 0);
    public int damage;
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
        if (gun.gunScript.elementIndex == 0)
        {

        }
        else if (gun.gunScript.elementIndex == 1)
            sprite.color = fire;
        else if (gun.gunScript.elementIndex == 2)
            sprite.color = water;
        else if (gun.gunScript.elementIndex == 3)
            sprite.color = earth;
        else if (gun.gunScript.elementIndex == 4)
            sprite.color = air;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Destructable"))
        {
            Destroy(collision.gameObject);
        }
    }
}
