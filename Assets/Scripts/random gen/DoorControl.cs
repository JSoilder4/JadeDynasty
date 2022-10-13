using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControl : MonoBehaviour
{
    private GameObject room;    //the parent room
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

    public bool open = false;   //is this door currently open?
    public bool locked = false; //is this door currently locked?
    public bool active = false; //is this door currently active(real)?
    public int posX;            //the x position of the parent room in the roomGrid array
    public int posY;            //the y position of the parent room in the roomGrid array

    public SpriteRenderer spriteR;

    public bool bigRoom;
    public int bigRoomNum; //0 NorthWest, 1 NorthEast, 2 SouthWest, 3 SouthEast



    private void Start()
    {

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
        room = transform.parent.gameObject;
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

        locked = false;
    }

    private void Update()
    {
        spriteR.enabled = active;
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
                RaycastHit2D raycastN = Physics2D.Raycast(transform.position, Vector2.up, 3, layermaskN);
                Debug.DrawRay(transform.position, Vector3.up * 3, Color.green);
                try
                {
                    doorConnectedTo = raycastN.transform.gameObject;
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
                }
                catch
                {
                    print("I'm litteraly crying rn");
                }
                break;
            case doorDir.south:
                int layermaskS = LayerMask.GetMask("Default");
                RaycastHit2D raycastS = Physics2D.Raycast(transform.position, Vector2.down, 3, layermaskS);
                Debug.DrawRay(transform.position, Vector3.down * 3, Color.green);
                try
                {
                    doorConnectedTo = raycastS.transform.gameObject;
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
                    break;
                case doorDir.east:
                    collision.transform.position = doorConnectedTo.transform.position + Vector3.right;
                    break;
                case doorDir.south:
                    collision.transform.position = doorConnectedTo.transform.position + Vector3.down;
                    break;
                case doorDir.west:
                    collision.transform.position = doorConnectedTo.transform.position + Vector3.left;
                    break;
            }
            
        }
    }


}
