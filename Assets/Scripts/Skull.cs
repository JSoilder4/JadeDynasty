using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skull : enemy
{

    public float timer;
    public float timerOG;
    public Vector2 maxVelocity;
    public Vector2 velocity;

   // public Camera cam;
    // Start is called before the first frame update
    public override void Start()
    {
        timer = timerOG;
        base.Start();
       // sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        //timer -= Time.deltaTime;
        //facePlayer();
       
        
    }
    private void FixedUpdate()
    {
        if (visable)
        {
            //if (!earthed)
                attackPlayer();
            //if (earthed)
            //{
            //    rb.AddForce(-rb.velocity);
            //}
            //checkHealth();
        }
    }
    public void attackPlayer()
    {
        //float x = player.transform.position.x - transform.position.x;
        //float y = player.transform.position.y - transform.position.y;
        velocity = player.transform.position - transform.position;//new Vector2(x,y);
        if(timer <= 0)
        {
            //rb.AddForce(-rb.velocity * 5);
            timer = timerOG;
        }
        else
        {
            
        }
        //if (!aired)
        //rb.AddForce(Vector3.Normalize(velocity) * 2);
        //if (aired)
        //    rb.AddForce(-Vector3.Normalize(velocity)*2);
        /*if (Mathf.Abs(velocity.x) > 10f || Mathf.Abs(velocity.y) > 10f)
        {
            rb.AddForce(velocity * 4);
        }
        if (Mathf.Abs(velocity.x) < 10f || Mathf.Abs(velocity.y) < 10f)
        {
            rb.AddForce(Vector3.Normalize(velocity) * 2);
        }
        if(Mathf.Abs(rb.velocity.x) > Mathf.Abs(maxVelocity.x) && Mathf.Abs(rb.velocity.y) > Mathf.Abs(maxVelocity.y))
        {
            timer -= Time.deltaTime;
        }*/

        /*if ((rb.velocity.x > (maxVelocity.x) || rb.velocity.y > (maxVelocity.y)))
        {
            rb.AddForce(Vector3.Normalize(velocity)*10);

        }
        else if ((rb.velocity.x > (maxVelocity.x * 2) || rb.velocity.y > (maxVelocity.y * 2)))
        {
            rb.AddForce(-rb.velocity*2);
        }
        else
        {
            rb.AddForce(Vector3.Normalize(velocity));
        }

        if(Mathf.Abs(transform.position.x) > Mathf.Abs(player.transform.position.x)+15 || Mathf.Abs(transform.position.y) > Mathf.Abs(player.transform.position.y)+15)
        {
           // rb.AddForce(-rb.velocity);
        }*/
    }
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);


    }
    public override void OnBecameVisible()
    {
        base.OnBecameVisible();
    }
    public override void OnBecameInvisible()
    {
        base.OnBecameInvisible();
    }

    public override void die()
    {
        throw new System.NotImplementedException();
    }
}
