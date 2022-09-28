using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfCatnip : enemy
{
    public Vector3 velocity;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        // sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        //facePlayer();
        if (visable)
        {
            //if(!earthed)
            attackPlayer();
            //checkHealth();
        }

    }
    public void attackPlayer()
    {
        velocity = player.transform.position - transform.position;

        rb.MovePosition(transform.position + Vector3.Normalize(velocity) * speed);
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
}
