using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelnest.BulletML;

public class badcatSurprise : enemy
{

    //public Animator anim;
    public BulletSourceScript bml;
    public TextAsset bmlXML;
    public float timer = 1f;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        anim = GetComponent<Animator>();
        bml = GetComponent<BulletSourceScript>();
    }

    // Update is called once per frame
    public override void Update()
    {
        //base.Update();
        timer -= Time.deltaTime;

    }

    public override void die()
    {
        //myState = state.dead;
        dead = true;
        //anim.SetInteger("state", (int)myState);
        StopAllCoroutines();
        anim.SetTrigger("die");
        StartCoroutine(dieKnockback());
        StartCoroutine(deathCoroutine(2f));
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall") && timer <= 0)
        {
            if (hp.currentHP > 25)
            {
                bml.xmlFile = bmlXML;
                bml.Reset();
                print("SpawnIt");
                Destroy(gameObject);
            }
            else if (hp.currentHP <= 25)
            {
                //spawn explosion
                print("ExplodeIt");
                Destroy(gameObject);
            }
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") && timer <= 0)
        {
            if (hp.currentHP > 25)
            {
                bml.xmlFile = bmlXML;
                bml.Reset();
                print("SpawnItCol");
                Destroy(gameObject);
            }
            else if (hp.currentHP <= 25)
            {
                //spawn explosion
                print("ExplodeItCol");
                Destroy(gameObject);
            }
        }
    }
}
