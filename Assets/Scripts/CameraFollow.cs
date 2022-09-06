using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public GameObject player;
    // public Camera cam;

    public float leftrightNum;

    // Start is called before the first frame update
    void Start()
    {
        //cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player.transform.position.x >= transform.position.x + leftrightNum)
        {
            transform.position = new Vector3(transform.position.x+ (leftrightNum*2), transform.position.y,transform.position.z);
            playerMove.pms.coords[0] += 1;
        }
        else if(player.transform.position.x <= transform.position.x - leftrightNum)
        {
            transform.position = new Vector3(transform.position.x - (leftrightNum*2), transform.position.y, transform.position.z);
            playerMove.pms.coords[0] -= 1;
        }
        else if (player.transform.position.y >= transform.position.y+5f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y+10f, transform.position.z);
            playerMove.pms.coords[1] += 1;
        }
        else if (player.transform.position.y <= transform.position.y - 5f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y-10f, transform.position.z);
            playerMove.pms.coords[1] -= 1;
        }
    }
}
