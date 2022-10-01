using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControl : MonoBehaviour
{
    private GameObject room;    //the parent room
    private GameObject hatch;   //the actual "door" that moves
    public Material hatchMat;   //the material of the door part
    public Material wallMat;    //the material of the surrounding wall
    public Material lockedMat;  //the material when locked
    public Animator doorAnim;   //reference to the animator on the door
    public bool open = false;   //is this door currently open?
    public bool locked = false; //is this door currently locked?
    public bool active = false; //is this door currently active(real)?
    public int posX;            //the x position of the parent room in the roomGrid array
    public int posY;            //the y position of the parent room in the roomGrid array

    private void Start()
    {
        //assign variables
        room = transform.parent.gameObject;
        posX = room.GetComponent<RoomGenerator>().posX;
        posY = room.GetComponent<RoomGenerator>().posY;

        hatch = transform.GetChild(3).gameObject;
        doorAnim = hatch.GetComponent<Animator>();

        locked = false;
    }

    private void Update()
    {
        //update material
        if (!active)
        {
            hatch.GetComponent<MeshRenderer>().material = wallMat;
        }

        else if (locked)
        {
            hatch.GetComponent<MeshRenderer>().material = lockedMat;
        }

        else if (!locked)
        {
            hatch.GetComponent<MeshRenderer>().material = hatchMat;
        }
    }

    public void OpenDoor() //plays animation when the door is opened
    {
        //seperate animations for different door prefabs cuz I'm dumb, but it would be more work to fix the original mistake, so it stays
        if(gameObject.name == "door1" || gameObject.name == "door1 (1)")
        {
            doorAnim.CrossFade("Open", 0.01f);
        }

        if (gameObject.name == "door2" || gameObject.name == "door2 (1)")
        {
            doorAnim.CrossFade("Open2", 0.01f);
        }

        open = true;
    }
}
