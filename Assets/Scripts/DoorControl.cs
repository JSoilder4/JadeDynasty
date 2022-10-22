using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControl : MonoBehaviour
{
    private GameObject room;    //the parent room
    public RoomGenerator roomScript;
  //  private GameObject hatch;   //the actual "door" that moves
   // public Material hatchMat;   //the material of the door part
   // public Material wallMat;    //the material of the surrounding wall
   // public Material lockedMat;  //the material when locked
    //public Animator doorAnim;   //reference to the animator on the door

    public enum doorDir
    {
        north,
        east,
        south,
        west
    }
    public doorDir direction;

    public GameObject doorConnectedTo;
    public DoorControl doorConnectedToControl;

    public bool connected = false;

    public bool open = false;   //is this door currently open?
    public bool locked = false; //is this door currently locked?
    public bool active = false; //is this door currently active(real)?
    public int posX;            //the x position of the parent room in the roomGrid array
    public int posY;            //the y position of the parent room in the roomGrid array

    public SpriteRenderer spriteR;

    public bool bigRoom;
    public int bigRoomNum; //0 NorthWest, 1 NorthEast, 2 SouthWest, 3 SouthEast

public CameraFollow camFollow;

    private void Start()
    {

        room = transform.parent.gameObject;
        roomScript = room.GetComponent<RoomGenerator>();
        if (name == "doorNorth")
        {
            direction = doorDir.north;
            
        }
        else if (name == "doorEast")
        {
            direction = doorDir.east;
            
        }
        else if (name == "doorSouth")
        {
            direction = doorDir.south;
            
        }
        else if (name == "doorWest")
        {
            direction = doorDir.west;
            
        }



        //assign variables
        spriteR = GetComponent<SpriteRenderer>();

        //parentRoomGen = room.GetComponent<RoomGenerator>();
        if(bigRoom)
        {
            if((transform.position.x > room.transform.position.x) && (transform.position.y > room.transform.position.y)) //top right
            {
                bigRoomNum = 1;
            }
            if((transform.position.x < room.transform.position.x) && (transform.position.y > room.transform.position.y)) //top left
            {
                bigRoomNum = 0;
            }
            if((transform.position.x > room.transform.position.x) && (transform.position.y < room.transform.position.y)) //bottom right
            {
                bigRoomNum = 3;
            }
            if((transform.position.x < room.transform.position.x) && (transform.position.y < room.transform.position.y)) //bottom left
            {
                bigRoomNum = 2;
            }
        }
        

        if (bigRoom)
        {
            posX = room.GetComponent<RoomGenerator>().posXBig[bigRoomNum];
            posY = room.GetComponent<RoomGenerator>().posYBig[bigRoomNum];
        }
        else
        {
            posX = room.GetComponent<RoomGenerator>().posX;
            posY = room.GetComponent<RoomGenerator>().posY;
        }


        //hatch = transform.GetChild(3).gameObject;
        //doorAnim = hatch.GetComponent<Animator>();
        camFollow = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();

        locked = false;
    }

    private void Update()
    {
        spriteR.enabled = active;
        if(connected)
        {
            switch(direction)
            {
                case doorDir.north:
                    if(!roomScript.north || !roomScript.north0 || !roomScript.north1)
                    {
                        Debug.Break();
                        roomScript.RegenMe(bigRoom);
                    }
                break;
                case doorDir.east:
                    if(!roomScript.east || !roomScript.east1 || !roomScript.east3)
                    {
                        Debug.Break();
                        roomScript.RegenMe(bigRoom);
                    }
                break;
                case doorDir.south:
                    if(!roomScript.south || !roomScript.south2 || !roomScript.south3)
                    {
                        Debug.Break();
                        roomScript.RegenMe(bigRoom);
                    }
                break;
                case doorDir.west:
                    if(!roomScript.west || !roomScript.west0 || !roomScript.west2)
                    {
                        Debug.Break();
                        roomScript.RegenMe(bigRoom);
                    }
                break;
            }
        }
        
        // gameObject.SetActive(active);


        //else if (locked)
        //{
        //    hatch.GetComponent<MeshRenderer>().material = lockedMat;
        //}

        //else if (!locked)
        //{
        //    hatch.GetComponent<MeshRenderer>().material = hatchMat;
        //}
    }

    public void ConnectToDoor()
    {
        switch (direction)
        {
            case doorDir.north:
                //Vector3 distToPlayerVect = player.transform.position - transform.position;
                // gameObject.layer = LayerMask.GetMask("Ignore Raycast");
                int layermaskN = LayerMask.GetMask("Default");
                RaycastHit2D raycastN = Physics2D.Raycast(transform.position, Vector2.up, 5, layermaskN);
                Debug.DrawRay(transform.position, Vector3.up * 3, Color.green);
                try
                {
                    doorConnectedTo = raycastN.transform.gameObject;
                    doorConnectedToControl = doorConnectedTo.GetComponent<DoorControl>();
                    connected = true;

                }
                catch
                {
                    print("I'm litteraly crying rn");
                }
                // gameObject.layer = LayerMask.GetMask("Default");

                break;
            case doorDir.east:
                int layermaskE = LayerMask.GetMask("Default");
                RaycastHit2D raycastE = Physics2D.Raycast(transform.position, Vector2.right, 2, layermaskE);
                Debug.DrawRay(transform.position, Vector3.right * 2, Color.green);
                try
                {
                    doorConnectedTo = raycastE.transform.gameObject;
                    doorConnectedToControl = doorConnectedTo.GetComponent<DoorControl>();
                    connected = true;
                }
                catch
                {
                    print("I'm litteraly crying rn");
                }
                break;
            case doorDir.south:
                int layermaskS = LayerMask.GetMask("Default");
                RaycastHit2D raycastS = Physics2D.Raycast(transform.position, Vector2.down, 5, layermaskS);
                Debug.DrawRay(transform.position, Vector3.down * 3, Color.green);
                try
                {
                    doorConnectedTo = raycastS.transform.gameObject;
                    doorConnectedToControl = doorConnectedTo.GetComponent<DoorControl>();
                    connected = true;
                }
                catch
                {
                    print("I'm litteraly crying rn");
                }
                break;
            case doorDir.west:
                int layermaskW = LayerMask.GetMask("Default");
                RaycastHit2D raycastW = Physics2D.Raycast(transform.position, Vector2.left, 2, layermaskW);
                Debug.DrawRay(transform.position, Vector3.left * 2, Color.green);
                try
                {
                    doorConnectedTo = raycastW.transform.gameObject;
                    doorConnectedToControl = doorConnectedTo.GetComponent<DoorControl>();
                    connected = true;
                }
                catch
                {
                    print("I'm litteraly crying rn");
                }

                break;

        }
    }

    public void OpenDoor() //plays animation when the door is opened
    {
        //seperate animations for different door prefabs cuz I'm dumb, but it would be more work to fix the original mistake, so it stays
        //if(gameObject.name == "door1" || gameObject.name == "door1 (1)")
        //{
        //    doorAnim.CrossFade("Open", 0.01f);
        //}

        //if (gameObject.name == "door2" || gameObject.name == "door2 (1)")
        //{
        //    doorAnim.CrossFade("Open2", 0.01f);
        //}

        open = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            switch (direction)
            {
                case doorDir.north:
                    collision.transform.position = doorConnectedTo.transform.position + Vector3.up;
                    if(doorConnectedToControl.bigRoom)
                    {
                        camFollow.cam = CameraFollow.CamType.followAndMouse;
                        camFollow.y--;
                    }
                    else
                    {
                        camFollow.cam = CameraFollow.CamType.gridBased;
                        camFollow.y = posY-1;
                        camFollow.x = posX;
                    }
                    camFollow.currentRoom = doorConnectedToControl.room.GetComponent<RoomGenerator>();
                    
                    break;
                case doorDir.east:
                    collision.transform.position = doorConnectedTo.transform.position + Vector3.right;
                    if(doorConnectedToControl.bigRoom)
                    {
                        camFollow.cam = CameraFollow.CamType.followAndMouse;
                        camFollow.x++;
                    }
                    else
                    {
                        camFollow.cam = CameraFollow.CamType.gridBased;
                        camFollow.x = posX+1;
                        camFollow.y = posY;
                    }
                    camFollow.currentRoom = doorConnectedToControl.room.GetComponent<RoomGenerator>();
                    break;
                case doorDir.south:
                    collision.transform.position = doorConnectedTo.transform.position + Vector3.down;
                    if(doorConnectedToControl.bigRoom)
                    {
                        camFollow.cam = CameraFollow.CamType.followAndMouse;
                        camFollow.y++;
                    }
                    else
                    {
                        camFollow.cam = CameraFollow.CamType.gridBased;
                        camFollow.y = posY+1;
                        camFollow.x = posX;
                    }
                    camFollow.currentRoom = doorConnectedToControl.room.GetComponent<RoomGenerator>();
                    break;
                case doorDir.west:
                    collision.transform.position = doorConnectedTo.transform.position + Vector3.left;
                    if(doorConnectedToControl.bigRoom)
                    {
                        camFollow.cam = CameraFollow.CamType.followAndMouse;
                        camFollow.x--;
                    }
                    else
                    {
                        camFollow.cam = CameraFollow.CamType.gridBased;
                        camFollow.x = posX-1;
                        camFollow.y = posY;
                    }
                    camFollow.currentRoom = doorConnectedToControl.room.GetComponent<RoomGenerator>();
                    break;
            }
            
        }
    }


}
