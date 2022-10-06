using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public enum CamType
    {
        gridBased,
        followAndMouse
    }
    public CamType cam;

    public GameObject player;
    // public Camera cam;

    public float leftrightNum;

    public Camera camVar;

    public Vector3 mousePos;
    public GameObject camBetween;

    // Start is called before the first frame update
    void Start()
    {
        camVar = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = camVar.ScreenToWorldPoint(Input.mousePosition);
        switch (cam)
        {
            case CamType.gridBased:
                if (player.transform.position.x >= transform.position.x + leftrightNum)
                {
                    transform.position = new Vector3(transform.position.x + (leftrightNum * 2), transform.position.y, transform.position.z);
                    playerMove.pms.coords[0] += 1;
                }
                else if (player.transform.position.x <= transform.position.x - leftrightNum)
                {
                    transform.position = new Vector3(transform.position.x - (leftrightNum * 2), transform.position.y, transform.position.z);
                    playerMove.pms.coords[0] -= 1;
                }
                else if (player.transform.position.y >= transform.position.y + 5f)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y + 10f, transform.position.z);
                    playerMove.pms.coords[1] += 1;
                }
                else if (player.transform.position.y <= transform.position.y - 5f)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y - 10f, transform.position.z);
                    playerMove.pms.coords[1] -= 1;
                }
                break;
            case CamType.followAndMouse:
                //camBetween.transform.position = player.transform.position;// + mousePos;
                break;
        }
        
    }
}
