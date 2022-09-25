using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public GameObject player;
    public SpriteRenderer sprite;
    public Rigidbody2D rb;

    public float speed;
    public float speedOG;
    //public int hp;
    //public int OGhp;

    public hp hp;


    public int[] coords;
    public bool visable;

    public AudioClip hitSound;
    
    public AudioSource mySource;

    // Start is called before the first frame update
    public virtual void Start()
    {
        hp = GetComponent<hp>();
    mySource = GetComponent<AudioSource>();

        //knockback = 15000f;
        //OGhp = hp;
        speedOG = speed;
        coords = new int[2];
        player = GameObject.Find("player");
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        facePlayer();
    }
    
    public void playHitSound()
    {
        mySource.clip = hitSound;
        mySource.Play();
    }
    public void facePlayer()
    {
        if (player.transform.position.x >= transform.position.x)
        {
            sprite.flipX = false;
        }
        else if (player.transform.position.x <= transform.position.x)
        {
            
            sprite.flipX = true;
        }
    }
    
    
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("shot"))
        {
            playHitSound();

            switch (collision.GetComponent<shot>().effect)
            {
                case gunEnumScript.effect.EXPLOSION:


                    break;

                default:
                    hp.takeDamage(collision.GetComponent<shot>().damage);// -= collision.GetComponent<shot>().damage;
                    break;
            }
            

            
            
            //Destroy(collision.gameObject);
        }
    }
    public virtual void OnBecameVisible()
    {
        visable = true;
    }
    public virtual void OnBecameInvisible()
    {
        visable = false;
    }
    public void die()
    {
        GameManager.GM.enemiesToReset.Remove(gameObject);
        Destroy(gameObject);
    }
}
