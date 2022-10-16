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
    public GameObject virtualCam;

    public GameObject player;
    // public Camera cam;

    public float leftrightNum;

    public Camera camVar;

    public Vector3 mousePos;
    public GameObject camBetween;
    public GenerationManager genManage;

    public int x, y;

    // Start is called before the first frame update
    void Start()
    {
        camVar = GetComponent<Camera>();
        genManage = GameObject.FindGameObjectWithTag("GameController").GetComponent<GenerationManager>();
        virtualCam = GameObject.FindGameObjectWithTag("virtualCam");
        player = GameObject.FindGameObjectWithTag("Player");
        x = 4;
        y = 4;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            x = 4;
            y = 4;
        }
        mousePos = camVar.ScreenToWorldPoint(Input.mousePosition);
        switch (cam)
        {
            case CamType.gridBased:
                // if (player.transform.position.x >= transform.position.x + leftrightNum)
                // {
                //     transform.position = new Vector3(transform.position.x + (leftrightNum * 2), transform.position.y, transform.position.z);
                //     playerMove.pms.coords[0] += 1;
                // }
                // else if (player.transform.position.x <= transform.position.x - leftrightNum)
                // {
                //     transform.position = new Vector3(transform.position.x - (leftrightNum * 2), transform.position.y, transform.position.z);
                //     playerMove.pms.coords[0] -= 1;
                // }
                // else if (player.transform.position.y >= transform.position.y + 5f)
                // {
                //     transform.position = new Vector3(transform.position.x, transform.position.y + 10f, transform.position.z);
                //     playerMove.pms.coords[1] += 1;
                // }
                // else if (player.transform.position.y <= transform.position.y - 5f)
                // {
                //     transform.position = new Vector3(transform.position.x, transform.position.y - 10f, transform.position.z);
                //     playerMove.pms.coords[1] -= 1;
                // }
                virtualCam.transform.position = new Vector3(genManage.roomPositions[x,y].x, genManage.roomPositions[x,y].y, -10);
                break;
            case CamType.followAndMouse:
                virtualCam.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);//player.transform.position;// + mousePos;
                break;
        }
        
    }
}
