using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationManager : MonoBehaviour
{
    public int roomsCreated = 0;            //how many rooms have been made
    public GameObject[] rooms;              //storage of different room prefabs
    public GameObject[] createdRooms;       //to keep track of the rooms that exist in the level
    public GameObject corridor;             //storage of the corridor prefab
    public GameObject[] bossRooms;          //storage of the boss room prefabs
    public GameObject treasureRoom;         //storage of the treasure room prefab
    private bool corridorsSpawned = false;  //have the corridors been instantiated yet?
    public GameObject[] doors;              //to keep track of all the doors in the scene
    public int floor = 0;                   //what floor the player is on

    //checklist for generation
    public bool genComplete = false;        //main generation of floor
    public bool bossRoomAssigned = false;   //assign boss room
    public bool tRoomAssigned = false;      //assign treasure room

    //Representation of the level as a grid. Each bool value corresponds to whether there is a room in that position on the grid
    public bool[,] roomGrid = new bool[10, 10] { 
        { false, false, false, false, false, false, false, false, false, false },
        { false, false, false, false, false, false, false, false, false, false },
        { false, false, false, false, false, false, false, false, false, false },
        { false, false, false, false, false, false, false, false, false, false },
        { false, false, false, false, false, false, false, false, false, false },
        { false, false, false, false, false, false, false, false, false, false },
        { false, false, false, false, false, false, false, false, false, false },
        { false, false, false, false, false, false, false, false, false, false },
        { false, false, false, false, false, false, false, false, false, false },
        { false, false, false, false, false, false, false, false, false, false }};

    //Lookup table of the actual in-engine transform positions for each position on the grid above
    public Vector3[,] roomPositions = new Vector3[10, 10] { 
        { new Vector3 (-200, 0, 200), new Vector3 (-150, 0, 200), new Vector3 (-100, 0, 200), new Vector3 (-50, 0, 200), new Vector3 (0, 0, 200), new Vector3 (50, 0, 200), new Vector3 (100, 0, 200), new Vector3 (150, 0, 200), new Vector3 (200, 0, 200), new Vector3 (250, 0, 200) },
        { new Vector3 (-200, 0, 150), new Vector3 (-150, 0, 150), new Vector3 (-100, 0, 150), new Vector3 (-50, 0, 150), new Vector3 (0, 0, 150), new Vector3 (50, 0, 150), new Vector3 (100, 0, 150), new Vector3 (150, 0, 150), new Vector3 (200, 0, 150), new Vector3 (250, 0, 150) },
        { new Vector3 (-200, 0, 100), new Vector3 (-150, 0, 100), new Vector3 (-100, 0, 100), new Vector3 (-50, 0, 100), new Vector3 (0, 0, 100), new Vector3 (50, 0, 100), new Vector3 (100, 0, 100), new Vector3 (150, 0, 100), new Vector3 (200, 0, 100), new Vector3 (250, 0, 100) },
        { new Vector3 (-200, 0, 50), new Vector3 (-150, 0, 50), new Vector3 (-100, 0, 50), new Vector3 (-50, 0, 50), new Vector3 (0, 0, 50), new Vector3 (50, 0, 50), new Vector3 (100, 0, 50), new Vector3 (150, 0, 50), new Vector3 (200, 0, 50), new Vector3 (250, 0, 50) },
        { new Vector3 (-200, 0, 0), new Vector3 (-150, 0, 0), new Vector3 (-100, 0, 0), new Vector3 (-50, 0, 0), new Vector3 (0, 0, 0), new Vector3 (50, 0, 0), new Vector3 (100, 0, 0), new Vector3 (150, 0, 0), new Vector3 (200, 0, 0), new Vector3 (250, 0, 0) },
        { new Vector3 (-200, 0, -50), new Vector3 (-150, 0, -50), new Vector3 (-100, 0, -50), new Vector3 (-50, 0, -50), new Vector3 (0, 0, -50), new Vector3 (50, 0, -50), new Vector3 (100, 0, -50), new Vector3 (150, 0, -50), new Vector3 (200, 0, -50), new Vector3 (250, 0, -50) },
        { new Vector3 (-200, 0, -100), new Vector3 (-150, 0, -100), new Vector3 (-100, 0, -100), new Vector3 (-50, 0, -100), new Vector3 (0, 0, -100), new Vector3 (50, 0, -100), new Vector3 (100, 0, -100), new Vector3 (150, 0, -100), new Vector3 (200, 0, -100), new Vector3 (250, 0, -100) },
        { new Vector3 (-200, 0, -150), new Vector3 (-150, 0, -150), new Vector3 (-100, 0, -150), new Vector3 (-50, 0, -150), new Vector3 (0, 0, -150), new Vector3 (50, 0, -150), new Vector3 (100, 0, -150), new Vector3 (150, 0, -150), new Vector3 (200, 0, -150), new Vector3 (250, 0, -150) },
        { new Vector3 (-200, 0, -200), new Vector3 (-150, 0, -200), new Vector3 (-100, 0, -200), new Vector3 (-50, 0, -200), new Vector3 (0, 0, -200), new Vector3 (50, 0, -200), new Vector3 (100, 0, -200), new Vector3 (150, 0, -200), new Vector3 (200, 0, -200), new Vector3 (250, 0, -200) },
        { new Vector3 (-200, 0, -250), new Vector3 (-150, 0, -250), new Vector3 (-100, 0, -250), new Vector3 (-50, 0, -250), new Vector3 (0, 0, -250), new Vector3 (50, 0, -250), new Vector3 (100, 0, -250), new Vector3 (150, 0, -250), new Vector3 (200, 0, -250), new Vector3 (250, 0, -250) }};

    //Debug: just translates the 2D array roomGrid into 10 1D arrays that can be viewed in the inspector
    [SerializeField] private bool[] roomGridColumn0 = new bool[10];
    [SerializeField] private bool[] roomGridColumn1 = new bool[10];
    [SerializeField] private bool[] roomGridColumn2 = new bool[10];
    [SerializeField] private bool[] roomGridColumn3 = new bool[10];
    [SerializeField] private bool[] roomGridColumn4 = new bool[10];
    [SerializeField] private bool[] roomGridColumn5 = new bool[10];
    [SerializeField] private bool[] roomGridColumn6 = new bool[10];
    [SerializeField] private bool[] roomGridColumn7 = new bool[10];
    [SerializeField] private bool[] roomGridColumn8 = new bool[10];
    [SerializeField] private bool[] roomGridColumn9 = new bool[10];

    void Start()
    {
        //create starting room
        Instantiate(rooms[0], roomPositions[4, 4], Quaternion.identity);
        roomGrid[4, 4] = true;
        roomsCreated = 1;
    }

    void Update()
    {
        //create corridors, once there are at least 7 rooms
        if(roomsCreated >= 7 && !corridorsSpawned)
        {
            //iterates through the roomGrid and checks for neighbors. If it finds them, instantiate a corridor in the appropriate position
            for(int x = 0; x < roomGrid.GetLength(0); x++)
            {
                for(int y = 0; y < roomGrid.GetLength(1); y++)
                {
                    if(roomGrid[x, y])
                    {
                        if(roomGrid[x + 1, y])
                        {
                            Instantiate(corridor, new Vector3(roomPositions[x, y].x, 2.02f, roomPositions[x, y].z - 25), Quaternion.AngleAxis(90, Vector3.up));
                        }

                        if (roomGrid[x, y + 1])
                        {
                            Instantiate(corridor, new Vector3(roomPositions[x, y].x + 25, 2.02f, roomPositions[x, y].z), Quaternion.identity);
                        }
                    }
                }
            }

            corridorsSpawned = true;
        }

        //just populates the above debug arrays
        for(int i = 0; i < roomGrid.GetLength(1); i++)
        {
            roomGridColumn0[i] = roomGrid[0, i];
            roomGridColumn1[i] = roomGrid[1, i];
            roomGridColumn2[i] = roomGrid[2, i];
            roomGridColumn3[i] = roomGrid[3, i];
            roomGridColumn4[i] = roomGrid[4, i];
            roomGridColumn5[i] = roomGrid[5, i];
            roomGridColumn6[i] = roomGrid[6, i];
            roomGridColumn7[i] = roomGrid[7, i];
            roomGridColumn8[i] = roomGrid[8, i];
            roomGridColumn9[i] = roomGrid[9, i];
        }

        //populates these arrays every frame, since the floor generation happens across multiple frames
        //doors = GameObject.FindGameObjectsWithTag("door");
        createdRooms = GameObject.FindGameObjectsWithTag("room");

        //determines whether doors should be "real" or not
        for(int i = 0; i < doors.Length; i++)
        {
            //checks the grid for neighbors to each door. If it finds one, sets the door to be active ("real")
            if(doors[i].name == "door1")
            {
                try
                {
                    if (roomGrid[doors[i].GetComponent<DoorControl>().posX, doors[i].GetComponent<DoorControl>().posY + 1])
                    {
                        doors[i].GetComponent<DoorControl>().active = true;
                    }
                }
                catch
                {
                    //in case the door borders the edge of the grid
                    print("exception caught");
                    doors[i].GetComponent<DoorControl>().active = false;
                }
            }

            if (doors[i].name == "door2")
            {
                try
                {
                    if (roomGrid[doors[i].GetComponent<DoorControl>().posX - 1, doors[i].GetComponent<DoorControl>().posY])
                    {
                        doors[i].GetComponent<DoorControl>().active = true;
                    }
                }
                catch
                {
                    //in case the door borders the edge of the grid
                    print("exception caught");
                    doors[i].GetComponent<DoorControl>().active = false;
                }
            }

            if (doors[i].name == "door1 (1)")
            {
                try
                {
                    if (roomGrid[doors[i].GetComponent<DoorControl>().posX, doors[i].GetComponent<DoorControl>().posY - 1])
                    {
                        doors[i].GetComponent<DoorControl>().active = true;
                    }
                }
                catch
                {
                    //in case the door borders the edge of the grid
                    print("exception caught");
                    doors[i].GetComponent<DoorControl>().active = false;
                }
            }

            if (doors[i].name == "door2 (1)")
            {
                try
                {
                    if (roomGrid[doors[i].GetComponent<DoorControl>().posX + 1, doors[i].GetComponent<DoorControl>().posY])
                    {
                        doors[i].GetComponent<DoorControl>().active = true;
                    }
                }
                catch
                {
                    //in case the door borders the edge of the grid
                    print("exception caught");
                    doors[i].GetComponent<DoorControl>().active = false;
                }
            }
        }

        //tells each room what its index is in the main array that tracks each room
        for(int i = 0; i < createdRooms.Length; i++)
        {
            createdRooms[i].GetComponent<RoomGenerator>().roomIndex = i;
        }

        //checklist update
        if(roomsCreated >= 7 && !genComplete)
        {
            genComplete = true;
        }

        if(genComplete && !bossRoomAssigned)
        {
            AssignBossRoom(floor);
        }

        if(genComplete && bossRoomAssigned && !tRoomAssigned)
        {
            AssignTreasureRoom();
        }
    }

    public void AssignBossRoom(int floor) //picks the room furthest from the player's starting room and replaces it with the appropriate boss room (based on floor)
    {
        int roomToReplaceIndex = 0; //which room gets replaced (identified by its index in the createdRooms array)
        float roomDist = 0f;        //stores the highest distance that has been found, to compare other rooms against

        for(int i = 0; i < createdRooms.Length; i++)
        {
            //iterate through all the rooms, every time a room is found that is further from the start than the previous furthest update which room to replace
            if(Vector3.Distance(createdRooms[i].transform.position, createdRooms[0].transform.position) > roomDist)
            {
                roomDist = Vector3.Distance(createdRooms[i].transform.position, createdRooms[0].transform.position);
                roomToReplaceIndex = i;
            }
        }

        //actually replace the room
        Instantiate(bossRooms[floor], createdRooms[roomToReplaceIndex].transform.position, Quaternion.identity);
        Destroy(createdRooms[roomToReplaceIndex]);

        //update checklist
        bossRoomAssigned = true;
    }

    public void AssignTreasureRoom() //picks a random room that isn't the starting room and isn't the boss room and replaces it with the treasure room
    {
        //because the boss room was just assigned, we know it is the last room in the array, and the starting room is always 0, so we just exclude the ends of the array
        int roomToReplaceIndex = Random.Range(1, createdRooms.Length - 1);

        //replace room
        Instantiate(treasureRoom, createdRooms[roomToReplaceIndex].transform.position, Quaternion.identity);
        Destroy(createdRooms[roomToReplaceIndex]);

        //update checklist
        tRoomAssigned = true;
    }

    public void RetryLevel() //retry the level generation, mostly for debug
    {
        //find all the stuff that was generated
        GameObject[] roomsToDestroy = GameObject.FindGameObjectsWithTag("room");
        GameObject[] corridorsToDestroy = GameObject.FindGameObjectsWithTag("corridor");

        //reset checklist
        genComplete = false;
        bossRoomAssigned = false;
        tRoomAssigned = false;

        //destroy all the stuff that was generated
        for (int i = 0; i < roomsToDestroy.Length; i++)
        {
            Destroy(roomsToDestroy[i]);
        }

        for (int i = 0; i < corridorsToDestroy.Length; i++)
        {
            Destroy(corridorsToDestroy[i]);
        }

        //reset the grid
        for (int x = 0; x < roomGrid.GetLength(0); x++)
        {
            for(int y = 0; y < roomGrid.GetLength(1); y++)
            {
                roomGrid[x, y] = false;
            }
        }

        //reset the last variables
        roomsCreated = 0;
        corridorsSpawned = false;

        //create new seed room
        Instantiate(rooms[0], roomPositions[4, 4], Quaternion.identity);
        roomGrid[4, 4] = true;
        roomsCreated = 1;

        //put the player back at the beginning
        GameObject.FindWithTag("Player").transform.position = new Vector3(0, 0, 0);
    }
}
