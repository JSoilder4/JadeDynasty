using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    [Header("Which doors exist (Regular)")]
    public bool south = true; 
    public bool west = true;
    public bool north = true; 
    public bool east = true;
    [Header("Which doors exist (BigRoom)")] //directionSubRoomNum
    public bool south3 = true; 
    public bool south2 = true; 
    public bool west2 = true; 
    public bool west0 = true; 
    public bool north0 = true; 
    public bool north1 = true; 
    public bool east1 = true; 
    public bool east3 = true; 

    public enum roomType
    {
        Regular,
        Boss,
        Treasure,
        RegularX2,
    }
    [Header("The rest (ask before touching)")]
    public roomType type;

    public int roomIndexKilled; //used for boss/treasue/special rooms

    public int posX;          //the x position of this room in the roomGrid array
    public int posY;          //the y position of this room in the roomGrid array
    public int roomIndex = 0; //this room's index in the array of rooms in the scene, used as an identifier
    public bool done = false; //is this room done creating rooms?

    [SerializeField] private GameObject corridor;             //storage of the corridor prefab
    public GameObject[] rooms;                                //storage of the different room prefabs
    public GameObject[] bigRooms;
    public List<GameObject> doors = new List<GameObject>();   //the doors in this room
    public List<GameObject> enemySpawns = new List<GameObject>(); //the enemyspawners in this room
    public List<enemySpawn> enemySpawnerScripts = new List<enemySpawn>();
    public List<GameObject> enemies = new List<GameObject>();
    private GenerationManager genManage;                      //reference to the manager
    public GameObject roomRef;                               //debug: for remembering what room begat what other room
    public bool completed = false;

    public bool start;

    public List<GameObject> roomsMade;

    public int doorsConnected;

    public bool cleared;

    [Header("Big Room :(")]
    public int[] posXBig = new int[4], posYBig = new int[4]; //0 = NorthWest, 1 = NorthEast, 2 = SouthWest, 3 = SouthEast



    public void enemiesDebug(){
        for(int i = 0; i < enemies.Count; i++){
            print(enemies[i] + " "+i);
        }
    }
    public void enemiesNullRemover(){
        for(int i = 0; i < enemies.Count; i++){
            if(enemies[i] == null){
                enemies.RemoveAt(i);
            }
        }
    }
    public void deadRemover(List<GameObject> l)
    {
        for(int i = 0; i < l.Count; i++){
            if(l[i].GetComponent<enemy>().dead){
                l.RemoveAt(i);
            }
        }
    }

    void Start()
    {
        start = true;
        //assign variables
        genManage = GameObject.FindWithTag("GameController").GetComponent<GenerationManager>();
        rooms = genManage.rooms;
        bigRooms = genManage.bigRooms;

        doors = FindChildrenWithTag(gameObject.transform, "door");
        enemySpawns = FindChildrenWithTag(gameObject.transform, "enemyspawn");

        foreach(GameObject g in enemySpawns)
        {
            enemySpawnerScripts.Add(g.GetComponent<enemySpawn>());
        }
        

        if (type != roomType.RegularX2)
        {
            posX = GetRoomPosition()[0];
            posY = GetRoomPosition()[1];
        }
        else
        {
            posXBig = new int[4];
            posYBig = new int[4];
            //print(transform.position + new Vector3(genManage.roomWidth, genManage.roomHeight, 0));
            //print(transform.position + new Vector3(-genManage.roomWidth, genManage.roomHeight, 0));
            //print(transform.position + new Vector3(genManage.roomWidth, -genManage.roomHeight, 0));
            //print(transform.position + new Vector3(-genManage.roomWidth, -genManage.roomHeight, 0));
            GetRoomPosition(roomType.RegularX2);
            //for (int i = 0; i < posXBig.Length; i++)
            //{
            //    print(name + "\ni: " + i + "\nX Pos: " + posXBig[i]);
            //}
        }
    }

    void Update()
    {
        if (start && (type == roomType.Regular || type == roomType.RegularX2))
        {
            name = name + " " + roomIndex;
            start = false;
        }
        else if (start && type == roomType.Boss)
        {
            name = name + " " + roomIndexKilled;
            start = false;
        }


        if (type == roomType.Boss)
        {
            doorsConnected = 0;
            for (int i = 0; i < doors.Count; i++)
            {
                if (doors[i].GetComponent<DoorControl>().active)
                {
                    doorsConnected++;
                }
            }
            if (doorsConnected >= 2)
            {
                print("hi" + doorsConnected);
                //genManage.RetryLevel();
            }
        }
        
        //main room creation code. It tries to make more rooms every frame until it is finished, or there are enough rooms
        try
        {
            if (genManage.roomsCreated < genManage.minRooms && !done && type != roomType.Boss)
            {
                int roomsToCreate = Mathf.FloorToInt(Random.Range(1, 4)); //how many rooms am I going to make
                genRooms(roomsToCreate);
                
            }
        }
        catch
        {
            //if the generation tries to go out of the grid, give up trying to make it
            done = true;
        }
        //manually reset level
        if (Input.GetKeyDown(KeyCode.T))
        {
            GameManager.GM.respawn();
            // genManage.RetryLevel();
            // GameManager.GM.sceneReset();
        }

            completed = true;


        //print(enemies);
        //if (transform.position == genManage.roomPositions[GameManager.GM.playerX,GameManager.GM.playerY])// && enemies.)
        //{

        //}
        if(GameManager.GM.currentRoom == this){
            //enemiesDebug();
            //enemiesNullRemover();
            
            genManage.nullRemover(enemies);
            deadRemover(enemies);
            
            if(enemies.Count <= 0){
                cleared = true;
            }
        }

        if(cleared)
        {
            foreach(GameObject g in doors){
                g.GetComponent<DoorControl>().locked = false;
            }
        }

        //COMEBACK TO THIS LATER????

            // for(int i = 0; i < doors.Count; i++)
            // {
            //    doors[i].GetComponent<DoorControl>().locked = false;
            // }
    }
    public void genRooms(int numOfRooms)
    {
        if (numOfRooms == 0)
        {
            return;
        }

        int roomDir;
        int typeOfRoom;

        float roomHeight = genManage.roomHeight;// - 0.5f; // - 0.5f because odd number for grid room height, would need to adjust if room size differ
        float roomWidth = genManage.roomWidth;

            roomDir = Mathf.FloorToInt(Random.Range(0, 4)); //which direction am I going to make this room in?
            typeOfRoom = Mathf.FloorToInt(Random.Range(1, 101)); //which type of room am I generating? (BIG? L?) (0 = big  for now)

            if((!south && roomDir == 0) || (!west && roomDir == 1) || (!north && roomDir == 2) || (!east && roomDir == 3))
            {
                genRooms(numOfRooms);
            }

            // while(!south && roomDir == 0)
            // {
            //     roomDir = Mathf.FloorToInt(Random.Range(0, 4));
            // }
            // while(!west && roomDir == 1)
            // {
            //     roomDir = Mathf.FloorToInt(Random.Range(0, 4));
            // }
            // while(!north && roomDir == 2)
            // {
            //     roomDir = Mathf.FloorToInt(Random.Range(0, 4));
            // }
            // while(!east && roomDir == 3)
            // {
            //     roomDir = Mathf.FloorToInt(Random.Range(0, 4));
            // }

        if (typeOfRoom <= 13)
        {
            int coinFlip;
            coinFlip = Mathf.FloorToInt(Random.Range(0, 2));//0 = negative, 1 = positive

            if (type == roomType.Regular)
            {
                if (doors[roomDir].gameObject.name == "doorSouth") //interprets the value from 0-3 as a door object in the room, since the doors are always in the same position relative to each other
                {
                    if (!genManage.roomGrid[posX, posY + 1] && !genManage.roomGrid[posX - 1, posY + 1] && !genManage.roomGrid[posX + 1, posY + 1] && !genManage.roomGrid[posX, posY + 2] && !genManage.roomGrid[posX - 1, posY + 2] && !genManage.roomGrid[posX + 1, posY + 2]) //is there already a room there or adjacent?
                    {
                        if (coinFlip == 0)//neg
                        {
                            roomRef = Instantiate(bigRooms[Mathf.FloorToInt(Random.Range(1, bigRooms.Length))], genManage.roomPositions[posX, posY + 1] + new Vector3(-roomWidth, -roomHeight, 0), Quaternion.identity); //instantiate the room
                            genManage.roomGrid[posX, posY + 1] = true; //update grid
                            genManage.roomGrid[posX - 1, posY + 1] = true; //update grid
                            genManage.roomGrid[posX, posY + 2] = true; //update grid
                            genManage.roomGrid[posX - 1, posY + 2] = true; //update grid
                        }
                        if (coinFlip == 1)//pos
                        {
                            roomRef = Instantiate(bigRooms[Mathf.FloorToInt(Random.Range(1, bigRooms.Length))], genManage.roomPositions[posX, posY + 1] + new Vector3(roomWidth, -roomHeight, 0), Quaternion.identity); //instantiate the room
                            genManage.roomGrid[posX, posY + 1] = true; //update grid
                            genManage.roomGrid[posX + 1, posY + 1] = true; //update grid
                            genManage.roomGrid[posX, posY + 2] = true; //update grid
                            genManage.roomGrid[posX + 1, posY + 2] = true; //update grid
                        }


                        doors[roomDir].GetComponent<DoorControl>().active = true; //obsolete I think
                        done = true; //done with generation
                        genManage.roomsCreated += 2; //increment how many rooms have been made

                        //print(gameObject.name + ": " + roomRef.name);
                        roomsMade.Add(roomRef);
                    }
                }

                if (doors[roomDir].gameObject.name == "doorWest")
                {
                    if (!genManage.roomGrid[posX - 1, posY] && !genManage.roomGrid[posX - 1, posY + 1] && !genManage.roomGrid[posX - 1, posY - 1] && !genManage.roomGrid[posX - 2, posY] && !genManage.roomGrid[posX - 2, posY + 1] && !genManage.roomGrid[posX - 2, posY - 1])
                    {
                        if (coinFlip == 0) //neg
                        {
                            roomRef = Instantiate(bigRooms[Mathf.FloorToInt(Random.Range(1, bigRooms.Length))], genManage.roomPositions[posX - 1, posY] + new Vector3(-roomWidth, -roomHeight, 0), Quaternion.identity); //instantiate the room
                            genManage.roomGrid[posX - 1, posY] = true; //update grid
                            genManage.roomGrid[posX - 1, posY + 1] = true; //update grid
                            genManage.roomGrid[posX - 2, posY] = true; //update grid
                            genManage.roomGrid[posX - 2, posY + 1] = true; //update grid
                        }
                        if (coinFlip == 1)//pos
                        {
                            roomRef = Instantiate(bigRooms[Mathf.FloorToInt(Random.Range(1, bigRooms.Length))], genManage.roomPositions[posX - 1, posY] + new Vector3(-roomWidth, roomHeight, 0), Quaternion.identity); //instantiate the room
                            genManage.roomGrid[posX - 1, posY] = true; //update grid
                            genManage.roomGrid[posX - 1, posY - 1] = true; //update grid
                            genManage.roomGrid[posX - 2, posY] = true; //update grid
                            genManage.roomGrid[posX - 2, posY - 1] = true; //update grid
                        }


                        doors[roomDir].GetComponent<DoorControl>().active = true; //obsolete I think
                        done = true; //done with generation
                        genManage.roomsCreated += 2; //increment how many rooms have been made

                        //print(gameObject.name + ": " + roomRef.name);
                        roomsMade.Add(roomRef);
                    }
                }

                if (doors[roomDir].gameObject.name == "doorNorth")
                {
                    if (!genManage.roomGrid[posX, posY - 1] && !genManage.roomGrid[posX - 1, posY - 1] && !genManage.roomGrid[posX + 1, posY + 1] && !genManage.roomGrid[posX, posY - 2] && !genManage.roomGrid[posX - 1, posY - 2] && !genManage.roomGrid[posX + 1, posY + 2])
                    {
                        if (coinFlip == 0) //neg
                        {
                            roomRef = Instantiate(bigRooms[Mathf.FloorToInt(Random.Range(1, bigRooms.Length))], genManage.roomPositions[posX, posY - 1] + new Vector3(-roomWidth, roomHeight, 0), Quaternion.identity); //instantiate the room
                            genManage.roomGrid[posX, posY - 1] = true; //update grid
                            genManage.roomGrid[posX - 1, posY - 1] = true; //update grid
                            genManage.roomGrid[posX, posY - 2] = true; //update grid
                            genManage.roomGrid[posX - 1, posY - 2] = true; //update grid
                        }
                        if (coinFlip == 1)//pos
                        {
                            roomRef = Instantiate(bigRooms[Mathf.FloorToInt(Random.Range(1, bigRooms.Length))], genManage.roomPositions[posX, posY - 1] + new Vector3(roomWidth, roomHeight, 0), Quaternion.identity); //instantiate the room
                            genManage.roomGrid[posX, posY - 1] = true; //update grid
                            genManage.roomGrid[posX + 1, posY - 1] = true; //update grid
                            genManage.roomGrid[posX, posY - 2] = true; //update grid
                            genManage.roomGrid[posX + 1, posY - 2] = true; //update grid
                        }


                        doors[roomDir].GetComponent<DoorControl>().active = true; //obsolete I think
                        done = true; //done with generation
                        genManage.roomsCreated += 2; //increment how many rooms have been made

                        //print(gameObject.name + ": " + roomRef.name);
                        roomsMade.Add(roomRef);
                    }
                }

                if (doors[roomDir].gameObject.name == "doorEast")
                {
                    if (!genManage.roomGrid[posX + 1, posY] && !genManage.roomGrid[posX + 1, posY + 1] && !genManage.roomGrid[posX + 1, posY - 1] && !genManage.roomGrid[posX + 2, posY] && !genManage.roomGrid[posX + 2, posY + 1] && !genManage.roomGrid[posX + 2, posY - 1])
                    {
                        if (coinFlip == 0) //neg
                        {
                            roomRef = Instantiate(bigRooms[Mathf.FloorToInt(Random.Range(1, bigRooms.Length))], genManage.roomPositions[posX + 1, posY] + new Vector3(roomWidth, -roomHeight, 0), Quaternion.identity); //instantiate the room
                            genManage.roomGrid[posX + 1, posY] = true; //update grid
                            genManage.roomGrid[posX + 1, posY + 1] = true; //update grid
                            genManage.roomGrid[posX + 2, posY] = true; //update grid
                            genManage.roomGrid[posX + 2, posY + 1] = true; //update grid
                        }
                        if (coinFlip == 1)//pos
                        {
                            roomRef = Instantiate(bigRooms[Mathf.FloorToInt(Random.Range(1, bigRooms.Length))], genManage.roomPositions[posX + 1, posY] + new Vector3(roomWidth, roomHeight, 0), Quaternion.identity); //instantiate the room
                            genManage.roomGrid[posX + 1, posY] = true; //update grid
                            genManage.roomGrid[posX + 1, posY - 1] = true; //update grid
                            genManage.roomGrid[posX + 2, posY] = true; //update grid
                            genManage.roomGrid[posX + 2, posY - 1] = true; //update grid
                        }


                        doors[roomDir].GetComponent<DoorControl>().active = true; //obsolete I think
                        done = true; //done with generation
                        genManage.roomsCreated += 2; //increment how many rooms have been made

                        //print(gameObject.name + ": " + roomRef.name);
                        roomsMade.Add(roomRef);
                    }
                }
            }
            if (type == roomType.RegularX2)
            {
                //int smallRoom = Mathf.FloorToInt(Random.Range(0, 4)); // 0 = NorthWest, 1 = NorthEast, 2 = SouthWest, 3 = SouthEast

                //posXBig[smallRoom], posYBig[smallRoom];

                roomDir = Mathf.FloorToInt(Random.Range(0, 8)); //0 SouthEastDown, 1 SouthWestDown, 2 SouthWestLeft, 3 NorthWestLeft, 4 NorthWestUp, 5 NorthEastUp, 6 NorthEastRight, 7 SouthEastRight

                int subRoom = -1;
                if (roomDir == 0 || roomDir == 7)
                {
                    subRoom = 3;
                }
                if (roomDir == 1 || roomDir == 2)
                {
                    subRoom = 2;
                }
                if (roomDir == 3 || roomDir == 4)
                {
                    subRoom = 0;
                }
                if (roomDir == 5 || roomDir == 6)
                {
                    subRoom = 1;
                }

                if (doors[roomDir].gameObject.name == "doorSouth")
                {
                    if (!genManage.roomGrid[posXBig[subRoom], posYBig[subRoom] + 1] && !genManage.roomGrid[posXBig[subRoom] - 1, posYBig[subRoom] + 1] && !genManage.roomGrid[posXBig[subRoom] + 1, posYBig[subRoom] + 1] && !genManage.roomGrid[posXBig[subRoom], posYBig[subRoom] + 2] && !genManage.roomGrid[posXBig[subRoom] - 1, posYBig[subRoom] + 2] && !genManage.roomGrid[posXBig[subRoom] + 1, posYBig[subRoom] + 2]) //is there already a room there or adjacent?
                    {
                        if (coinFlip == 0)//neg
                        {
                            roomRef = Instantiate(bigRooms[Mathf.FloorToInt(Random.Range(1, bigRooms.Length))], genManage.roomPositions[posXBig[subRoom], posYBig[subRoom] + 1] + new Vector3(-roomWidth, -roomHeight, 0), Quaternion.identity); //instantiate the room
                            genManage.roomGrid[posXBig[subRoom], posYBig[subRoom] + 1] = true; //update grid
                            genManage.roomGrid[posXBig[subRoom] - 1, posYBig[subRoom] + 1] = true; //update grid
                            genManage.roomGrid[posXBig[subRoom], posYBig[subRoom] + 2] = true; //update grid
                            genManage.roomGrid[posXBig[subRoom] - 1, posYBig[subRoom] + 2] = true; //update grid
                        }
                        if (coinFlip == 1)//pos
                        {
                            roomRef = Instantiate(bigRooms[Mathf.FloorToInt(Random.Range(1, bigRooms.Length))], genManage.roomPositions[posXBig[subRoom], posYBig[subRoom] + 1] + new Vector3(roomWidth, -roomHeight, 0), Quaternion.identity); //instantiate the room
                            genManage.roomGrid[posXBig[subRoom], posYBig[subRoom] + 1] = true; //update grid
                            genManage.roomGrid[posXBig[subRoom] + 1, posYBig[subRoom] + 1] = true; //update grid
                            genManage.roomGrid[posXBig[subRoom], posYBig[subRoom] + 2] = true; //update grid
                            genManage.roomGrid[posXBig[subRoom] + 1, posYBig[subRoom] + 2] = true; //update grid
                        }


                        doors[roomDir].GetComponent<DoorControl>().active = true; //obsolete I think
                        done = true; //done with generation
                        genManage.roomsCreated += 2; //increment how many rooms have been made

                        //print(gameObject.name + ": " + roomRef.name);
                        roomsMade.Add(roomRef);
                    }
                }

                if (doors[roomDir].gameObject.name == "doorWest")
                {
                    if (!genManage.roomGrid[posXBig[subRoom] - 1, posYBig[subRoom]] && !genManage.roomGrid[posXBig[subRoom] - 1, posYBig[subRoom] + 1] && !genManage.roomGrid[posXBig[subRoom] - 1, posYBig[subRoom] - 1] && !genManage.roomGrid[posXBig[subRoom] - 2, posYBig[subRoom]] && !genManage.roomGrid[posXBig[subRoom] - 2, posYBig[subRoom] + 1] && !genManage.roomGrid[posXBig[subRoom] - 2, posYBig[subRoom] - 1])
                    {
                        if (coinFlip == 0) //neg
                        {
                            roomRef = Instantiate(bigRooms[Mathf.FloorToInt(Random.Range(1, bigRooms.Length))], genManage.roomPositions[posXBig[subRoom] - 1, posYBig[subRoom]] + new Vector3(-roomWidth, -roomHeight, 0), Quaternion.identity); //instantiate the room
                            genManage.roomGrid[posXBig[subRoom] - 1, posYBig[subRoom]] = true; //update grid
                            genManage.roomGrid[posXBig[subRoom] - 1, posYBig[subRoom] + 1] = true; //update grid
                            genManage.roomGrid[posXBig[subRoom] - 2, posYBig[subRoom]] = true; //update grid
                            genManage.roomGrid[posXBig[subRoom] - 2, posYBig[subRoom] + 1] = true; //update grid
                        }
                        if (coinFlip == 1)//pos
                        {
                            roomRef = Instantiate(bigRooms[Mathf.FloorToInt(Random.Range(1, bigRooms.Length))], genManage.roomPositions[posXBig[subRoom] - 1, posYBig[subRoom]] + new Vector3(-roomWidth, roomHeight, 0), Quaternion.identity); //instantiate the room
                            genManage.roomGrid[posXBig[subRoom] - 1, posYBig[subRoom]] = true; //update grid
                            genManage.roomGrid[posXBig[subRoom] - 1, posYBig[subRoom] - 1] = true; //update grid
                            genManage.roomGrid[posXBig[subRoom] - 2, posYBig[subRoom]] = true; //update grid
                            genManage.roomGrid[posXBig[subRoom] - 2, posYBig[subRoom] - 1] = true; //update grid
                        }


                        doors[roomDir].GetComponent<DoorControl>().active = true; //obsolete I think
                        done = true; //done with generation
                        genManage.roomsCreated += 2; //increment how many rooms have been made

                        //print(gameObject.name + ": " + roomRef.name);
                        roomsMade.Add(roomRef);
                    }
                }

                if (doors[roomDir].gameObject.name == "doorNorth") //interprets the value from 0-3 as a door object in the room, since the doors are always in the same position relative to each other
                {
                    if (!genManage.roomGrid[posXBig[subRoom], posYBig[subRoom] - 1] && !genManage.roomGrid[posXBig[subRoom] - 1, posYBig[subRoom] - 1] && !genManage.roomGrid[posXBig[subRoom] + 1, posYBig[subRoom] + 1] && !genManage.roomGrid[posXBig[subRoom], posYBig[subRoom] - 2] && !genManage.roomGrid[posXBig[subRoom] - 1, posYBig[subRoom] - 2] && !genManage.roomGrid[posXBig[subRoom] + 1, posYBig[subRoom] + 2])
                    {
                        if (coinFlip == 0) //neg
                        {
                            roomRef = Instantiate(bigRooms[Mathf.FloorToInt(Random.Range(1, bigRooms.Length))], genManage.roomPositions[posXBig[subRoom], posYBig[subRoom] - 1] + new Vector3(-roomWidth, roomHeight, 0), Quaternion.identity); //instantiate the room
                            genManage.roomGrid[posXBig[subRoom], posYBig[subRoom] - 1] = true; //update grid
                            genManage.roomGrid[posXBig[subRoom] - 1, posYBig[subRoom] - 1] = true; //update grid
                            genManage.roomGrid[posXBig[subRoom], posYBig[subRoom] - 2] = true; //update grid
                            genManage.roomGrid[posXBig[subRoom] - 1, posYBig[subRoom] - 2] = true; //update grid
                        }
                        if (coinFlip == 1)//pos
                        {
                            roomRef = Instantiate(bigRooms[Mathf.FloorToInt(Random.Range(1, bigRooms.Length))], genManage.roomPositions[posXBig[subRoom], posYBig[subRoom] - 1] + new Vector3(roomWidth, roomHeight, 0), Quaternion.identity); //instantiate the room
                            genManage.roomGrid[posXBig[subRoom], posYBig[subRoom] - 1] = true; //update grid
                            genManage.roomGrid[posXBig[subRoom] + 1, posYBig[subRoom] - 1] = true; //update grid
                            genManage.roomGrid[posXBig[subRoom], posYBig[subRoom] - 2] = true; //update grid
                            genManage.roomGrid[posXBig[subRoom] + 1, posYBig[subRoom] - 2] = true; //update grid
                        }


                        doors[roomDir].GetComponent<DoorControl>().active = true; //obsolete I think
                        done = true; //done with generation
                        genManage.roomsCreated += 2; //increment how many rooms have been made

                        //print(gameObject.name + ": " + roomRef.name);
                        roomsMade.Add(roomRef);
                    }
                }

                if (doors[roomDir].gameObject.name == "doorEast")
                {
                    if (!genManage.roomGrid[posXBig[subRoom] + 1, posYBig[subRoom]] && !genManage.roomGrid[posXBig[subRoom] + 1, posYBig[subRoom] + 1] && !genManage.roomGrid[posXBig[subRoom] + 1, posYBig[subRoom] - 1] && !genManage.roomGrid[posXBig[subRoom] + 2, posYBig[subRoom]] && !genManage.roomGrid[posXBig[subRoom] + 2, posYBig[subRoom] + 1] && !genManage.roomGrid[posXBig[subRoom] + 2, posYBig[subRoom] - 1])
                    {
                        if (coinFlip == 0) //neg
                        {
                            roomRef = Instantiate(bigRooms[Mathf.FloorToInt(Random.Range(1, bigRooms.Length))], genManage.roomPositions[posXBig[subRoom] + 1, posYBig[subRoom]] + new Vector3(roomWidth, -roomHeight, 0), Quaternion.identity); //instantiate the room
                            genManage.roomGrid[posXBig[subRoom] + 1, posYBig[subRoom]] = true; //update grid
                            genManage.roomGrid[posXBig[subRoom] + 1, posYBig[subRoom] + 1] = true; //update grid
                            genManage.roomGrid[posXBig[subRoom] + 2, posYBig[subRoom]] = true; //update grid
                            genManage.roomGrid[posXBig[subRoom] + 2, posYBig[subRoom] + 1] = true; //update grid
                        }
                        if (coinFlip == 1)//pos
                        {
                            roomRef = Instantiate(bigRooms[Mathf.FloorToInt(Random.Range(1, bigRooms.Length))], genManage.roomPositions[posXBig[subRoom] + 1, posYBig[subRoom]] + new Vector3(roomWidth, roomHeight, 0), Quaternion.identity); //instantiate the room
                            genManage.roomGrid[posXBig[subRoom] + 1, posYBig[subRoom]] = true; //update grid
                            genManage.roomGrid[posXBig[subRoom] + 1, posYBig[subRoom] - 1] = true; //update grid
                            genManage.roomGrid[posXBig[subRoom] + 2, posYBig[subRoom]] = true; //update grid
                            genManage.roomGrid[posXBig[subRoom] + 2, posYBig[subRoom] - 1] = true; //update grid
                        }


                        doors[roomDir].GetComponent<DoorControl>().active = true; //obsolete I think
                        done = true; //done with generation
                        genManage.roomsCreated += 2; //increment how many rooms have been made

                        //print(gameObject.name + ": " + roomRef.name);
                        roomsMade.Add(roomRef);
                    }
                }
            }

        }
        else
        {
            if (type == roomType.Regular)
            {
                if (doors[roomDir].gameObject.name == "doorSouth") //interprets the value from 0-3 as a door object in the room, since the doors are always in the same position relative to each other
                {
                    if (!genManage.roomGrid[posX, posY + 1] && !genManage.roomGrid[posX - 1, posY + 1] && !genManage.roomGrid[posX + 1, posY + 1]) //is there already a room there or adjacent?
                    {
                        roomRef = Instantiate(rooms[Mathf.FloorToInt(Random.Range(1, rooms.Length))], genManage.roomPositions[posX, posY + 1], Quaternion.identity); //instantiate the room
                        doors[roomDir].GetComponent<DoorControl>().active = true; //obsolete I think
                        genManage.roomGrid[posX, posY + 1] = true; //update grid
                        done = true; //done with generation
                        genManage.roomsCreated++; //increment how many rooms have been made

                        //print(gameObject.name + ": " + roomRef.name);
                        roomsMade.Add(roomRef);
                    }
                }

                if (doors[roomDir].gameObject.name == "doorWest")
                {
                    if (!genManage.roomGrid[posX - 1, posY] && !genManage.roomGrid[posX - 1, posY + 1] && !genManage.roomGrid[posX - 1, posY - 1])
                    {
                        roomRef = Instantiate(rooms[Mathf.FloorToInt(Random.Range(1, rooms.Length))], genManage.roomPositions[posX - 1, posY], Quaternion.identity);
                        doors[roomDir].GetComponent<DoorControl>().active = true;
                        genManage.roomGrid[posX - 1, posY] = true;
                        done = true;
                        genManage.roomsCreated++;

                        //print(gameObject.name + ": " + roomRef.name);
                        roomsMade.Add(roomRef);
                    }
                }

                if (doors[roomDir].gameObject.name == "doorNorth")
                {
                    if (!genManage.roomGrid[posX, posY - 1] && !genManage.roomGrid[posX - 1, posY - 1] && !genManage.roomGrid[posX + 1, posY + 1])
                    {
                        roomRef = Instantiate(rooms[Mathf.FloorToInt(Random.Range(1, rooms.Length))], genManage.roomPositions[posX, posY - 1], Quaternion.identity);
                        doors[roomDir].GetComponent<DoorControl>().active = true;
                        genManage.roomGrid[posX, posY - 1] = true;
                        done = true;
                        genManage.roomsCreated++;

                        //print(gameObject.name + ": " + roomRef.name);
                        roomsMade.Add(roomRef);
                    }
                }

                if (doors[roomDir].gameObject.name == "doorEast")
                {
                    if (!genManage.roomGrid[posX + 1, posY] && !genManage.roomGrid[posX + 1, posY + 1] && !genManage.roomGrid[posX + 1, posY - 1])
                    {
                        roomRef = Instantiate(rooms[Mathf.FloorToInt(Random.Range(1, rooms.Length))], genManage.roomPositions[posX + 1, posY], Quaternion.identity);
                        doors[roomDir].GetComponent<DoorControl>().active = true;
                        genManage.roomGrid[posX + 1, posY] = true;
                        done = true;
                        genManage.roomsCreated++;

                        //print(gameObject.name + ": " + roomRef.name);
                        roomsMade.Add(roomRef);
                    }
                }
            }
            if (type == roomType.RegularX2)
            {
                //int smallRoom = Mathf.FloorToInt(Random.Range(0, 4)); // 0 = NorthWest, 1 = NorthEast, 2 = SouthWest, 3 = SouthEast

                //posXBig[smallRoom], posYBig[smallRoom];

                roomDir = Mathf.FloorToInt(Random.Range(0, 8)); //0 SouthEastDown, 1 SouthWestDown, 2 SouthWestLeft, 3 NorthWestLeft, 4 NorthWestUp, 5 NorthEastUp, 6 NorthEastRight, 7 SouthEastRight

                int subRoom = -1;
                if (roomDir == 0 || roomDir == 7)
                {
                    subRoom = 3;
                }
                if (roomDir == 1 || roomDir == 2)
                {
                    subRoom = 2;
                }
                if (roomDir == 3 || roomDir == 4)
                {
                    subRoom = 0;
                }
                if (roomDir == 5 || roomDir == 6)
                {
                    subRoom = 1;
                }

                if (doors[roomDir].gameObject.name == "doorSouth")
                {
                    if (!genManage.roomGrid[posXBig[subRoom], posYBig[subRoom] + 1] && !genManage.roomGrid[posXBig[subRoom] - 1, posYBig[subRoom] + 1] && !genManage.roomGrid[posXBig[subRoom] + 1, posYBig[subRoom] + 1]) //is there already a room there or adjacent?
                    {
                        roomRef = Instantiate(rooms[Mathf.FloorToInt(Random.Range(1, rooms.Length))], genManage.roomPositions[posXBig[subRoom], posYBig[subRoom] + 1], Quaternion.identity); //instantiate the room
                        doors[roomDir].GetComponent<DoorControl>().active = true; //obsolete I think
                        genManage.roomGrid[posXBig[subRoom], posYBig[subRoom] + 1] = true; //update grid
                        done = true; //done with generation
                        genManage.roomsCreated++; //increment how many rooms have been made

                        //print(gameObject.name + ": " + roomRef.name);
                        roomsMade.Add(roomRef);
                    }
                }

                if (doors[roomDir].gameObject.name == "doorWest")
                {
                    if (!genManage.roomGrid[posXBig[subRoom] - 1, posYBig[subRoom]] && !genManage.roomGrid[posXBig[subRoom] - 1, posYBig[subRoom] - 1] && !genManage.roomGrid[posXBig[subRoom] - 1, posYBig[subRoom] + 1])
                    {
                        roomRef = Instantiate(rooms[Mathf.FloorToInt(Random.Range(1, rooms.Length))], genManage.roomPositions[posXBig[subRoom] - 1, posYBig[subRoom]], Quaternion.identity);
                        doors[roomDir].GetComponent<DoorControl>().active = true;
                        genManage.roomGrid[posXBig[subRoom] - 1, posYBig[subRoom]] = true;
                        done = true;
                        genManage.roomsCreated++;

                        //print(gameObject.name + ": " + roomRef.name);
                        roomsMade.Add(roomRef);
                    }
                }
                
                if (doors[roomDir].gameObject.name == "doorNorth") //interprets the value from 0-3 as a door object in the room, since the doors are always in the same position relative to each other
                {
                    if (!genManage.roomGrid[posXBig[subRoom], posYBig[subRoom] - 1] && !genManage.roomGrid[posXBig[subRoom] - 1, posYBig[subRoom] - 1] && !genManage.roomGrid[posXBig[subRoom] + 1, posYBig[subRoom] - 1]) //is there already a room there or adjacent?
                    {
                        roomRef = Instantiate(rooms[Mathf.FloorToInt(Random.Range(1, rooms.Length))], genManage.roomPositions[posXBig[subRoom], posYBig[subRoom] - 1], Quaternion.identity); //instantiate the room
                        doors[roomDir].GetComponent<DoorControl>().active = true; //obsolete I think
                        genManage.roomGrid[posXBig[subRoom], posYBig[subRoom] - 1] = true; //update grid
                        done = true; //done with generation
                        genManage.roomsCreated++; //increment how many rooms have been made

                        //print(gameObject.name + ": " + roomRef.name);
                        roomsMade.Add(roomRef);
                    }
                }

                if (doors[roomDir].gameObject.name == "doorEast")
                {
                    if (!genManage.roomGrid[posXBig[subRoom] + 1, posYBig[subRoom]] && !genManage.roomGrid[posXBig[subRoom] + 1, posYBig[subRoom] - 1] && !genManage.roomGrid[posXBig[subRoom] + 1, posYBig[subRoom] + 1])
                    {
                        roomRef = Instantiate(rooms[Mathf.FloorToInt(Random.Range(1, rooms.Length))], genManage.roomPositions[posXBig[subRoom] + 1, posYBig[subRoom]], Quaternion.identity);
                        doors[roomDir].GetComponent<DoorControl>().active = true;
                        genManage.roomGrid[posXBig[subRoom] + 1, posYBig[subRoom]] = true;
                        done = true;
                        genManage.roomsCreated++;

                        //print(gameObject.name + ": " + roomRef.name);
                        roomsMade.Add(roomRef);
                    }
                }
            }
        }
        
        genRooms(numOfRooms - 1);
    }
    private int[] GetRoomPosition() //finds room's position on the grid based on its position in the scene and returns it as an array of 2 values; [0] is x and [1] is y
    {
        int[] pos = new int[2]; //array to return

        //iterates through the lookup table to find the position that matches the room's position and then, when it's found, breaks out of the loop and returns it
        for (int x = 0; x < genManage.roomPositions.GetLength(0); x++)
        {
            for (int y = 0; y < genManage.roomPositions.GetLength(1); y++)
            {
                if (genManage.roomPositions[x, y] == transform.position)
                {
                    pos[0] = x;
                    pos[1] = y;
                    goto Done;
                }
            }
        }

    Done:
        return pos;
    }
    private void GetRoomPosition(roomType theType) //finds room's position on the grid based on its position in the scene and returns it as an array of 2 values; [0] is x and [1] is y
    {
        //int[] pos = new int[2]; //array to return

        //iterates through the lookup table to find the position that matches the room's position and then, when it's found, breaks out of the loop and returns it
        for (int x = 0; x < genManage.roomPositions.GetLength(0); x++)
        {
            for (int y = 0; y < genManage.roomPositions.GetLength(1); y++)
            {

                if (genManage.roomPositions[x, y] == transform.position + new Vector3(-genManage.roomWidth, genManage.roomHeight, 0))
                {
                    posXBig[0] = x;
                    posYBig[0] = y;
                    //goto Done;
                }
                if (genManage.roomPositions[x, y] == transform.position + new Vector3(genManage.roomWidth, genManage.roomHeight, 0))
                {
                    posXBig[1] = x;
                    posYBig[1] = y;
                    //goto Done;
                }
                if (genManage.roomPositions[x, y] == transform.position + new Vector3(-genManage.roomWidth, -genManage.roomHeight, 0))
                {
                    posXBig[2] = x;
                    posYBig[2] = y;
                    //goto Done;
                }
                if (genManage.roomPositions[x, y] == transform.position + new Vector3(genManage.roomWidth, -genManage.roomHeight, 0))
                {
                    posXBig[3] = x;
                    posYBig[3] = y;
                    //goto Done;
                }
            }
        }

    //Done:
        //return pos;
    }

    public void RegenMe(bool bigRoom)
    {
        print("I NEED HEALING");
        roomsMade.Remove(gameObject);

        if(bigRoom)
        {
            roomRef = Instantiate(bigRooms[Mathf.FloorToInt(Random.Range(1, bigRooms.Length))], genManage.roomPositions[posX, posY], Quaternion.identity);
        }
        else
        {
            roomRef = Instantiate(rooms[Mathf.FloorToInt(Random.Range(1, rooms.Length))], genManage.roomPositions[posX, posY], Quaternion.identity); //instantiate the room
        }
        

        roomsMade.Add(roomRef);

        Destroy(gameObject);
    }

    public void spawnEnemies()
    {
        //genManage.nullRemover(doors);
        if(!cleared && enemySpawns.Count > 0)
        {
            foreach (enemySpawn eS in enemySpawnerScripts) 
            {
                eS.spawnenemy();
            }
            foreach (enemySpawn eS in enemySpawnerScripts)
            {
                enemies.Add(eS.spawnedReference);
            }
            foreach (GameObject g in doors)
            {
                g.GetComponent<DoorControl>().locked = true;
            }
        }

    }

    public List<GameObject> FindChildrenWithTag(Transform parent, string tag) //simple function to find all children of a game object "parent" with tag "tag" (why is this not built in???)
    {
        List<GameObject> children = new List<GameObject>();

        for(int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i).gameObject.CompareTag(tag))
            {
                children.Add(parent.GetChild(i).gameObject);
            }
        }

        return children;
    }
}
