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
    public int hp;
    public int OGhp;
    public int[] coords;
    public bool visable;

    public bool onfire;
    public float fireTimer;
    public Color colorChange;
    public bool earthed;
    public float earthTimer;
    public bool watered;
    public float waterTimer;
    public bool aired;
    public float airedTimer;
    
    public AudioClip hitSound;
    public AudioClip fireClip;
    public AudioSource mySource;

    // Start is called before the first frame update
    public virtual void Start()
    {
        colorChange = new Color(255,255,255);
    mySource = GetComponent<AudioSource>();

        //knockback = 15000f;
        OGhp = hp;
        speedOG = speed;
        coords = new int[2];
        player = GameObject.Find("player");
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        elementEffect();
        sprite.color = colorChange;
        checkHealth();
        colorNormalize();
    }
    public void elementEffect()
    {
        if (earthed)
        {
            earthTimer -= Time.deltaTime;
        }
        else
        {
            facePlayer();
        }
        if (earthTimer <= 0)
        {
            earthed = false;
        }
        if (watered)
        {
            waterTimer -= Time.deltaTime;
            speed = speedOG / 2;
        }
        else
        {
            speed = speedOG;
        }
        if (waterTimer <= 0)
        {
            watered = false;
        }
        if (aired)
        {
            speed = -speedOG;
            airedTimer -= Time.deltaTime;
        }
        else
        {
            speed = speedOG;
        }
        if (airedTimer <= 0)
        {
            aired = false;
        }
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
    public void colorNormalize()
    {
    if (!onfire)
    {
        if (colorChange.g < 240 && colorChange.b < 240)
        {
            colorChange.g += Time.deltaTime;
            colorChange.b += Time.deltaTime;
        }
        else if (colorChange.g >= 240 && colorChange.b >= 240)
        {

        }
    }
        if (!watered)
        {
            if (colorChange.g < 240 && colorChange.r < 240)
            {
                colorChange.g += Time.deltaTime;
                colorChange.r += Time.deltaTime;
            }
            else if (colorChange.g >= 240 && colorChange.r >= 240)
            {

            }
        }
        if (!earthed)
        {
            if (colorChange.b < 240 && colorChange.r < 240)
            {
                colorChange.b += Time.deltaTime;
                colorChange.r += Time.deltaTime;
            }
            else if (colorChange.b >= 240 && colorChange.r >= 240)
            {

            }
        }
        if (!aired)
        {
            if (colorChange.b < 240)
            {
                colorChange.b += Time.deltaTime;
            }
            else if (colorChange.b >= 240)
            {

            }
        }
    }
    public void checkHealth()
    {
        if (onfire)
        {
            
            sprite.color = colorChange;
            hp -= 1;
            fireTimer -= Time.deltaTime;
            if (fireTimer <= 0)
            {
                onfire = false;
            }
        }
        if (hp <= 0)
        {
            print("die" + hp);
            GameManager.GM.updateScore(GameManager.GM.maxScore / GameManager.GM.enemiesToReset.Count);
            GameManager.GM.enemiesToReset.Remove(gameObject);
            Destroy(gameObject);
        }
    }
    public void fireSound()
    {
        mySource.clip = fireClip;
        mySource.Play();
    }
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("shot"))
        {
            playHitSound();
            if(collision.name != "Explosion")
            {
                hp -= collision.GetComponent<shot>().damage;
            }
            else
            {
                hp -= collision.GetComponent<exploding>().damage;
            }
            
            switch (collision.GetComponent<shot>().elementindex)
            {
                case 0:
                    {
                        break;
                    }
                case 1:
                    {
                        fireSound();
                        //print(fire);
                        colorChange.b = 0;
                        colorChange.g = 0;
                        fireTimer = 0.25f;
                        onfire = true;
                        
                        break;
                    }
                case 2:
                    {
                        colorChange.r = 0;
                        colorChange.g = 0;
                        waterTimer = 0.25f;
                        watered = true;
                        break;
                    }
                case 3:
                    {
                        colorChange.b = 0;
                        colorChange.r = 0;
                        earthTimer = 0.25f;
                        earthed = true;
                        break;
                    }
                case 4:
                    {
                        colorChange.b = 0;
                        airedTimer = 0.75f;
                        aired = true;
                        //print(hp);
                        //print(gun.gunScript.damage + "Damage");
                        //print(collision.GetComponent<shot>().lookDirection * knockback);
                        //rb.AddForce(collision.GetComponent<shot>().lookDirection.normalized * knockback);
                        //aired = false;
                        //rb.AddForce(-collision.GetComponent<shot>().lookDirection.normalized * knockback);
                        break;
                    }
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
