using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Playables;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager GM;
    public enemySpawn[] enemyspawners;
    public randomGun[] randomGuns;
    public GameObject player; //we're going to spawn the player from GM. //no we arent keklord
    public playerMove playerScript;
    public bool playerdead;
    public Camera cam;
    public Text respawnText;

    [Header("UI")]
    public Canvas GunStatUICanvas;
    public PlayableDirector GunStatUIPlayableDirector;
    public Animator GunStatUIAnimator;
    public TextMeshProUGUI damageUItext;
    public TextMeshProUGUI RoFUItext;
    public TextMeshProUGUI shotSpeedUItext;
    public TextMeshProUGUI elementUItext;
    public TextMeshProUGUI effectUItext;
    public TextMeshProUGUI spreadUItext;
    public TextMeshProUGUI triggerUItext;
    public Image gunUIImage; //gun swap

    
    public Image[] gunUI = new Image[4]; //gun showcase
    //public Image[] gunUIHighlight = new Image[4];
    public Image gunUISelect;

    public float timer;
    public float timerOG;

    //public bool started = false;
    public Text titleHeader;
    public Text titleFooter;

    public bool end;

    public int maxScore;

    public List<GameObject> enemiesToReset = new List<GameObject>();
    public List<GameObject> rGunsToReset = new List<GameObject>();
    public List<GameObject> droppedAmmoToReset = new List<GameObject>();

    public AudioClip pickupSound;
    public AudioSource pickupSource;

    public Text hpText;
    public Image HPImage;
    public Sprite[] HPsprites;

    public int playerX, playerY;

    public RoomGenerator currentRoom;
    public GameObject playButton;
    public GameObject backgroundImage;

    public GameObject dummyObject;
    public GameObject minimapBackground;
    public List<minimapObject> minimapObjects;
    /// <summary>
    /// Representation of the player on the room grid. (CAN SWITCH TO ENUMS)
    /// </summary>
    public string[,] playerRoomGrid = new string[10, 10] {
        { "null", "null", "null", "null", "null", "null", "null", "null", "null", "null" },
        { "null", "null", "null", "null", "null", "null", "null", "null", "null", "null" },
        { "null", "null", "null", "null", "null", "null", "null", "null", "null", "null" },
        { "null", "null", "null", "null", "null", "null", "null", "null", "null", "null" },
        { "null", "null", "null", "null", "null", "null", "null", "null", "null", "null" },
        { "null", "null", "null", "null", "null", "null", "null", "null", "null", "null" },
        { "null", "null", "null", "null", "null", "null", "null", "null", "null", "null" },
        { "null", "null", "null", "null", "null", "null", "null", "null", "null", "null" },
        { "null", "null", "null", "null", "null", "null", "null", "null", "null", "null" },
        { "null", "null", "null", "null", "null", "null", "null", "null", "null", "null" }};

    public GenerationManager genManage;


    public GameObject randomGunToDrop;
    public GameObject ammoToDrop;
    public GameObject healthToDrop;

    public Sprite[] pistol, shotgun, sniper, smg;
    public Sprite pistolH, shotgunH, sniperH, smgH;

    public Coroutine minimapCheckingNumerator;

    public bool NextFloorRan;

    public GameObject endscreenUI;
    public TextMeshProUGUI floorText;
    public TextMeshProUGUI enemiesKilledUI;
    public int enemiesKilled;
    public GameObject ammoCount;
    public TextMeshProUGUI floorHighScoreText;
    public TextMeshProUGUI enemiesKilledHighScoreUI;
    public int floorHighScore = -1;
    public int enemiesKilledHighScore = -1;
    public bool newFloorHS;
    public bool newEnemyHS;




    //public static GameObject null;

    public enum dropsEmum
    {
        gun,
        ammo,
        health
    }

    public static List<GameObject> drops;

    private void Awake()
    {
        if (GM == null)
        {
            DontDestroyOnLoad(this); //this means it will exist if you switch scenes.
            GM = this;
        }
        else
        {
            Destroy(gameObject);
        }
        timer = timerOG;
        //maxScore = 5000;
    }

    // Start is called before the first frame update
    void Start()
    {
        //clense = playergun.gunScript.clense;
        playerScript = player.GetComponent<playerMove>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        genManage = GameObject.FindWithTag("GameController").GetComponent<GenerationManager>();
        GunStatUIAnimator = GunStatUICanvas.transform.GetChild(0).gameObject.GetComponent<Animator>();//GetComponentInChildren<Animator>();


        playerX = 4;
        playerY = 4;
        playerRoomGrid[playerX, playerY] = "true";

        makeMinimap();


        floorHighScore = PlayerPrefs.GetInt("floorHS");
        enemiesKilledHighScore = PlayerPrefs.GetInt("enemyHS");

        respawn();
        //CheckMinimapObjects();

        //randomGunToDrop = GameObject.Find("randomGun");
        //ammoToDrop = GameObject.Find("Ammo");

        //drops.Add(randomGunToDrop);
        //drops.Add(ammoToDrop);
        //drops.Add(null);
        //randomGuns = GameObject.FindGameObjectsWithTag("Gun");
    }

    public void makeMinimap() //redo this somehow to fit more screens
    {
        float height = 5f;// * 1.75f;
        float width = 5f;// * 1.75f;
        int xForDebug = 0;
        int yForDebug = 0;

        for (float y = height * 4 * 2; y >= -height * 5 * 2; y -= height * 2) //(start y(baseNum * 4) * 2, y >= end y(-baseNum * 5) * 2, base height num * 2)
        {
            for (float x = -width * 4 * 2; x <= width * 5 * 2; x += width * 2) //(start x(-baseNum*4) * 2, y >= end x(baseNum*5) * 2, base width num * 2)
            {
                GameObject g = Instantiate(dummyObject, new Vector3(minimapBackground.transform.position.x + x-5, minimapBackground.transform.position.y + y+5, 0), Quaternion.identity, minimapBackground.transform);
                g.GetComponent<minimapObject>().x = xForDebug;
                g.GetComponent<minimapObject>().y = yForDebug;
                minimapObjects.Add(g.GetComponent<minimapObject>());
                xForDebug++;
            }
            xForDebug = 0;
            yForDebug++;
        }

        //StartCoroutine(minimapCheckDelay());
    }


    /// <summary>
    /// Currently used for moving the reference of where the player is. Eventually can also be used to implement minimap.
    /// </summary>
    /// <param name="xDifference"></param>
    /// <param name="yDifference"></param>
    public void RenderMinimap(int xDifference, int yDifference) //should be called by moving between doors.
    {
        if (currentRoom.type == RoomGenerator.roomType.RegularX2)
        {
            foreach(GameObject g in currentRoom.doors)
            {
                DoorControl dC = g.GetComponent<DoorControl>();
                playerRoomGrid[dC.posX, dC.posY] = "true";
            }
        }



        playerRoomGrid[playerX, playerY] = "false";
        playerX += xDifference;
        playerY += yDifference;
        playerRoomGrid[playerX, playerY] = "true";

        minimapCheckingNumerator = StartCoroutine(CheckMinimapObjects("renderMini",0));
        //for(int y = 0; y < playerRoomGrid.length(1); y++){for(int x = 0; x < playerroomgrid.length; x++){}}
    }
    /// <summary>
    /// Used for switching between bigrooms.
    /// </summary>
    /// <param name="xDifference"></param>
    /// <param name="yDifference"></param>
    /// <param name="setX"></param>
    /// <param name="setY"></param>
    public void renderMinimap(int xDifference, int yDifference, int setX, int setY) //should be called by moving between doors.
    {
        
        playerRoomGrid[playerX, playerY] = "false";
        playerX = setX + xDifference;
        playerY = setY + yDifference;
        playerRoomGrid[playerX, playerY] = "true";



        minimapCheckingNumerator = StartCoroutine(CheckMinimapObjects("renderMini",0));
        //for(int y = 0; y < playerRoomGrid.length(1); y++){for(int x = 0; x < playerroomgrid.length; x++){}}
    }
    public void derenderBigRoom()
    {
        if (currentRoom.type == RoomGenerator.roomType.RegularX2)
        {
            foreach (GameObject g in currentRoom.doors)
            {
                try
                {
                    DoorControl dC = g.GetComponent<DoorControl>();
                    playerRoomGrid[dC.posX, dC.posY] = "false";
                }
                catch 
                {
                    //Debug.Break();
                    print("ee~ etto...");
                }
                
            }
        }
    }
    public IEnumerator CheckMinimapObjects(string location, float wait)
    {
        print(location);
        yield return new WaitForSeconds(wait);
        for(int i = 0; i < currentRoom.doors.Count; i++)
        {
            // if(currentRoom.doors[i] != null){
            //     print("im not null guys: "+ currentRoom.doors[i]);
            // }
            // else{
            //     print("im null as fuck bro"+ currentRoom.doors[i]);
            // }

            // while(currentRoom.doors[i] == null){
            //     i++;
            //     if(i == 4){
            //         i = 0;
            //     }
            //     print(i);
            // }
            try
            {
                if (currentRoom.doors[i].GetComponent<DoorControl>() != null && currentRoom.doors[i].GetComponent<DoorControl>().active)
                {
                    DoorControl dC = currentRoom.doors[i].GetComponent<DoorControl>();
                    if (dC.doorConnectedToControl != null)
                    {
                        playerRoomGrid[dC.doorConnectedToControl.posX, dC.doorConnectedToControl.posY] = "false";
                    }
                    else{
                        print("connected door is null: " + dC.doorConnectedToControl);
                        Debug.Break();
                        // if(minimapCheckingNumerator != null)
                        // StopCoroutine(minimapCheckingNumerator);
                        // minimapCheckingNumerator = StartCoroutine(CheckMinimapObjects("recursive"));
                        //break;
                    }
                }
                else{
                    // print("door is null: "+g);
                    // Debug.Break();
                    // CheckMinimapObjects("recursive");
                    //break;
                }
                    
                }
            catch
            {
                
                Debug.Break();
                // if(minimapCheckingNumerator != null)
                // StopCoroutine(minimapCheckingNumerator);
                // minimapCheckingNumerator = StartCoroutine(CheckMinimapObjects("recursive"));
                print("ee~ etto... bleh: "+ currentRoom.doors[i]);
            }
        }
        foreach(minimapObject miniO in minimapObjects)
        {
            miniO.Check();
        }

        yield return null;
    }

    //public IEnumerator minimapCheckDelay()
    //{
    //    yield return new WaitForSeconds(2.85f);
    //    CheckMinimapObjects("delayCoroutine");
    //}
    public void updateGunUI(List<gun> guns, int gunIndex)
    {
        for (int i = 0; i < gunUI.Length; i++)
        {
            try
            {
                if (guns[i] == null)
                {
                    //gunUI[i].color = new Color(1,1,1,0);
                    break;
                }
            }
            catch
            {
                break;
            }
            
        switch (guns[i].gunType)
        {
            case gunEnumScript.gunType.Pistol:
                gunUI[i].sprite = pistolH;
                //gunUIHighlight[i].sprite = pistolH;
                break;
            case gunEnumScript.gunType.Shotgun:
                gunUI[i].sprite = shotgunH;
                //gunUIHighlight[i].sprite = shotgunH;
                break;
            case gunEnumScript.gunType.Sniper:
                gunUI[i].sprite = sniperH;
                //gunUIHighlight[i].sprite = sniperH;
                break;
            case gunEnumScript.gunType.SMG:
                gunUI[i].sprite = smgH;
                //gunUIHighlight[i].sprite = smgH;
                break;
        }
            //gunUI[i].sprite = playergun.gunScript.gunSprites[(int)guns[i].gunType];
            gunUI[i].color = playergun.elementalColors[(int)guns[i].element];
        }

        gunUISelect.transform.position = gunUI[gunIndex].transform.position;

    }
    public void resetGunUI()
    {
        for (int i = 1; i < 4; i++)
        {
            gunUI[i].color = new Color(1, 1, 1, 0);
        }
    }





    public void playPickupSound() //gun switch sound
    {
        pickupSource.clip = pickupSound;
        pickupSource.Play();
    }
/// <summary>
/// Swap firstparam gun with secondparam gun (pick up Random Gun Implementation)
/// </summary>
/// <param name="mainGun"></param>
/// <param name="otherGun"></param>
/// <param name="rGun"></param>
    public void swapGunAndOtherGun(gun mainGun, gun otherGun, randomGun rGun)
    {
        //guntypeIndex
        //damage
        //betweenshottimer(remember BSTOG)
        //reloadtimer
        //shotspeed
        //element
        //effect
        //rGun.col.enabled = false;
        playPickupSound();

        gun theGun = new gun();

        theGun = mainGun; 

        mainGun = otherGun; //main = other

        otherGun = theGun; //other = main

        playergun.gunScript.equippedGuns[playergun.gunScript.gunIndex] = mainGun;
        rGun.theGun = otherGun;


        //playergun.gunScript.activeGun = mainGun;
        
        //playergun.gunScript.equippedGuns.Add(mainGun);

        gunSwapUI(mainGun, true);
    }
    //public void swapGunAndOtherGun(gun mainGun, gun otherGun)
    //{
    //    //guntypeIndex
    //    //damage
    //    //betweenshottimer(remember BSTOG)
    //    //reloadtimer
    //    //shotspeed
    //    //element
    //    //effect
    //    playPickupSound();

    //    gun theGun = new gun();

    //    theGun = mainGun;

    //    mainGun = otherGun;

    //    otherGun = theGun;

    //    playergun.gunScript.activeGun = mainGun;
    //    //playergun.gunScript.equippedGuns.add(otherGun);



    //     gunSwapUI(mainGun);
    //}
    /// <summary>
    /// used to pickup guns if under max number of guns
    /// </summary>
    public void gunPickup(GameObject rGun, gun rGunG)
    {
        //print("trouble is doubled");
        playPickupSound();
        playergun.gunScript.equippedGuns.Add(rGunG);
        playergun.gunScript.gunIndex = playergun.gunScript.equippedGuns.Count;
        gunSwapUI(rGunG, true);
        rGun.SetActive(false);
    }

    public void gunSwapUI(gun gun, bool temp)
    {
        // if (gun.gunType == gunEnumScript.gunType.Shotgun)
        // {
        //     damageUItext.text = "Damage: " + gun.damage + " x " + (gun.numShots);
        // }

        GunStatUIAnimator.SetTrigger("UIEnabled");

        if (gun.numShots > 1)
        {
            damageUItext.text = "Damage: " + (Mathf.Round(gun.damage * 100.0f) / 100.0f) + " x " + (gun.numShots);
        }
        else
        {
            damageUItext.text = "Damage: " + (Mathf.Round(gun.damage * 100.0f) / 100.0f);//gun.damage.ToString("F2");
        }

        //RoFUItext.text = "Bullets Per Second: "+(Mathf.Round((1- gun.bSTog)*100.0f)/100.0f)+"";
        //shotSpeedUItext.text = "Shot Speed: "+ (Mathf.Round((/*1 - */gun.shotSpeed) * 100.0f) / 100.0f);
        triggerUItext.text = "Trigger: " + gun.triggerType;
        spreadUItext.text = "Spread: " + gun.spread;
        elementUItext.text = "Element: " + gun.element;
        effectUItext.text = "Special: " + gun.effect;
        
        switch(gun.gunType){
            case gunEnumScript.gunType.Pistol:
            gunUIImage.sprite = pistolH;
            break;
            case gunEnumScript.gunType.Shotgun:
            gunUIImage.sprite = shotgunH;
            break;
            case gunEnumScript.gunType.Sniper:
            gunUIImage.sprite = sniperH;
            break;
            case gunEnumScript.gunType.SMG:
            gunUIImage.sprite = smgH;
            break;
        }
        //gunUIImage.sprite = playergun.gunScript.gunSprites[(int)gun.gunType];
        gunUIImage.color = playergun.elementalColors[(int)gun.element];
        if(temp){
            GunStatUIPlayableDirector.Stop();
            GunStatUIPlayableDirector.time = 0.0;
            GunStatUIPlayableDirector.Play();
        }
        if(!temp)
        {   
            print("crying in the club rn: "+GunStatUIPlayableDirector.duration);
            GunStatUIPlayableDirector.Stop();
            GunStatUIPlayableDirector.time = GunStatUIPlayableDirector.duration/2;
            //GunStatUIPlayableDirector.RebuildGraph();
            //GunStatUIPlayableDirector.Pause();
            GunStatUIPlayableDirector.Play();
            GunStatUIPlayableDirector.Pause();
        }

    }
    //public void gunSwapUI(playergun gun)
    //{
    //    if (gun.theGun.gunType != gunEnumScript.gunType.Shotgun)
    //    {
    //        damageUItext.text = "Damage: " + playergun.gunScript.theGun.damage + " x " + ((playergun.gunScript.theGun.numShots * 2) + 1);
    //    }
    //    else
    //    {
    //        damageUItext.text = "Damage: " + playergun.gunScript.theGun.damage;
    //    }

    //    RoFUItext.text = "Bullets Per Second: " + (Mathf.Round((1 - playergun.gunScript.theGun.bSTog) * 100.0f) / 100.0f) + "";
    //    shotSpeedUItext.text = "Shot Speed: " + (Mathf.Round((1 - playergun.gunScript.theGun.shotSpeed) * 100.0f) / 100.0f);
    //    elementUItext.text = "Element: " + playergun.gunScript.theGun.element;
    //    effectUItext.text = "Special: " + playergun.gunScript.theGun.effect;
    //    gunUIImage.sprite = gun.sprite.sprite;
    //    gunUIImage.color = gun.sprite.color;
    //    GunStatUIPlayableDirector.Stop();
    //    GunStatUIPlayableDirector.time = 0.0;
    //    GunStatUIPlayableDirector.Play();
    //}
    //IEnumerator Fade()
    //{
    //    Color c = GunStatUICanvas.color;
    //    for (float alpha = 1f; alpha >= 0; alpha -= 0.1f)
    //    {
    //        c.a = alpha;
    //        .color = c;
    //        yield return null;
    //    }
    //}

    public void respawn()
    {
        //sceneReset();
        player.transform.position = new Vector3(0, 0, 0);// genManage.roomPositions[4,4];
        cam.transform.position = new Vector3(0, 0, 0); //genManage.roomPositions[4,4];
        cam.GetComponent<CameraFollow>().cam = CameraFollow.CamType.gridBased;
        playerX = 4;
        playerY = 4;
        //playerScript.enabled = true;
        playerScript.hp.currentHP = 5;
        //playergun.gunScript.enabled = true;
        player.GetComponent<SpriteRenderer>().enabled = true;
        playergun.gunScript.sprite.enabled = true;
        player.GetComponent<CircleCollider2D>().enabled = true;
        //rerollGuns();
        playergun.gunScript.resetGun(); //check
        playergun.gunScript.resetAmmo();
        if(minimapCheckingNumerator != null)
        StopCoroutine(minimapCheckingNumerator);
        genManage.RetryLevel();
        genManage.floor = 0;
        sceneReset();
        resetMinimap();
        ResetHighScore();

        minimapCheckingNumerator = StartCoroutine(CheckMinimapObjects("respawn", 0.75f));
        //spawnEnemies();
    }
    public void nextFloor()
    {
        if(!NextFloorRan){
            player.transform.position = new Vector3(0, 0, 0);// genManage.roomPositions[4,4];
            cam.transform.position = new Vector3(0, 0, 0); //genManage.roomPositions[4,4];
            playerX = 4;
            playerY = 4;
            player.GetComponent<SpriteRenderer>().enabled = true;
            playergun.gunScript.sprite.enabled = true;
            player.GetComponent<CircleCollider2D>().enabled = true;
            genManage.floor += 1;
            print("current floor: "+genManage.floor);
            if(minimapCheckingNumerator != null)
            StopCoroutine(minimapCheckingNumerator);
            genManage.RetryLevel();
            sceneReset();
            resetMinimap();

            minimapCheckingNumerator = StartCoroutine(CheckMinimapObjects("nextlvl", 0.75f));

            NextFloorRan = true;
        }
        
        //spawnEnemies();
    }
    public void resetMinimap()
    {
        if(minimapCheckingNumerator != null)
        StopCoroutine(minimapCheckingNumerator);
        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                playerRoomGrid[x, y] = "null";
            }
        }
        playerRoomGrid[4, 4] = "true";
        minimapCheckingNumerator = StartCoroutine(CheckMinimapObjects("resetminimap", 0.75f));
        //StartCoroutine(minimapCheckDelay());
        //CheckMinimapObjects("");
    }
    public void rerollGuns()
    {
        for (int i = 0; i < randomGuns.Length; i++)
        {
            randomGuns[i].gameObject.SetActive(true);
            randomGuns[i].theGun.Roll("");//rollGun();
        }
    }
    public void dead()
    {
        playerdead = true;
        endscreenUI.SetActive(true);// = true;
        ammoCount.SetActive(false);
    }
    public void alive()
    {
        playerdead = false;
        endscreenUI.SetActive(false);// = false;
        ammoCount.SetActive(true);
    }
    public void sceneReset()
    {
        for (int i = enemiesToReset.Count-1; i >= 0; i--)
        {
            //enemiesToReset.Remove(enemiesToReset[i]);
           // print("die");
            Destroy(enemiesToReset[i]);
            
        }
        for (int i = rGunsToReset.Count - 1; i >= 0; i--)
        {
            //enemiesToReset.Remove(enemiesToReset[i]);
            // print("die");
            Destroy(rGunsToReset[i]);

        }
        for (int i = droppedAmmoToReset.Count - 1; i >= 0; i--)
        {
            //enemiesToReset.Remove(enemiesToReset[i]);
            // print("die");
            Destroy(droppedAmmoToReset[i]);

        }
        enemiesToReset.Clear();
        rGunsToReset.Clear();
        droppedAmmoToReset.Clear();
        //resetScore();
    }
    public void addSpawnedObject(GameObject g)
    {
        enemiesToReset.Add(g);
    }
    public void tryagainButton()
    {
        respawn();
        alive();
    }
    public void Update()
    {
        if(!end)
        setEndText();
        if (playerdead)
        {
            // if(Input.GetButtonDown("Fire1"))
            // {
            //     respawn();
            //     alive();
            // }
        }
        else{
            if(currentRoom.posX == 4 && currentRoom.posY == 4){
                if(Input.GetAxisRaw("Horizontal") != 0 && Input.GetAxisRaw("Vertical") != 0){
                    //minimapCheckingNumerator = StartCoroutine(CheckMinimapObjects("stupid solution", 0));
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        //CheckMinimapObjects("Update()");
        // if (!started && Input.GetButtonDown("Jump"))
        // {
        //     start();
        // }

        //hpText.text = "HP: " + playerScript.hp.currentHP;
        switch (playerScript.hp.currentHP)
        {
            case 5:
                HPImage.sprite = HPsprites[0];
                break;
            case 4:
                HPImage.sprite = HPsprites[1];
                break;
            case 3:
                HPImage.sprite = HPsprites[2];
                break;
            case 2:
                HPImage.sprite = HPsprites[3];
                break;
            case 1:
                HPImage.sprite = HPsprites[4];
                break;
            case 0:
                HPImage.sprite = HPsprites[5];
                break;
            default:
                HPImage.sprite = HPsprites[0];
                break;
        }

        //checkForEnemies();
    }


    public void start()
    {
        //started = true;
        //scoreText.enabled = true;
        titleHeader.enabled = false;
        titleFooter.enabled = false;
        playButton.SetActive(false);// = false;
        backgroundImage.SetActive(false);// = false;
        respawn();

    }
    // public void updateScore(int s)
    // {
    //     score += s;
    //     if (score < 0)
    //     {
    //         score = 0;
    //     }
    //     scoreText.text = score.ToString();
    // }
    public void setEndText()
    {
        floorText.text = "Floor Reached: "+(genManage.floor+1);
        enemiesKilledUI.text = "Enemies Defeated: "+enemiesKilled;

        if(genManage.floor > floorHighScore)
        {
            floorHighScore = genManage.floor;
            newFloorHS = true;
            PlayerPrefs.SetInt("floorHS", floorHighScore);
            PlayerPrefs.Save();
        }
        if(enemiesKilled > enemiesKilledHighScore)
        {
            enemiesKilledHighScore = enemiesKilled;
            newEnemyHS = true;
            PlayerPrefs.SetInt("enemyHS", enemiesKilledHighScore);
            PlayerPrefs.Save();
        }
        if(newFloorHS){
            floorHighScoreText.text = "NEW High Score: " + (floorHighScore+1);
        }
        else{
            floorHighScoreText.text = "High Score: " + floorHighScore;
        }
        if(newEnemyHS){
            enemiesKilledHighScoreUI.text = "NEW High Score: " + enemiesKilledHighScore;
        }
        else{
            enemiesKilledHighScoreUI.text = "High Score: " + enemiesKilledHighScore;
        }
        
        

    }
    public void ResetHighScore(){
        newFloorHS = false;
        newEnemyHS = false;
    }
    // public void scoreTime()
    // {
    //     timer -= Time.deltaTime;
    //     if(timer <= 0)
    //     {
    //         timer = timerOG;
    //         updateScore(-5);
    //     }
        
    // }
    // public void checkForEnemies()
    // {
    //     //print(enemiesToReset.Count);
    //     if (enemiesToReset.Count == 0)
    //     {
    //         win();
    //     }
    // }
    // public void win()
    // {
    //     floorText.enabled = false;
    //     wintext.enabled = true;
    //     end = true;
    //     wintext.text = "All Enemies Defeated! Final Score: " + score; 
    // }



    public static Dictionary<dropsEmum, int> enemyDropTable = new Dictionary<dropsEmum, int>()
    {
        {dropsEmum.gun, 40},
        {dropsEmum.ammo, 50},
        {dropsEmum.health, 10}
    };
    public static dropsEmum RollDrops()
    {
        int[] weights = enemyDropTable.Values.ToArray();

        int randomWeight = Random.Range(0, weights.Sum());


        for (int i = 0; i < weights.Length; i++)
        {
            // Debug.Log(weights[i] + " "+i);
            randomWeight -= weights[i];
            if (randomWeight < 0)
            {
                return (dropsEmum) i;//the drop
            }
        }
        return dropsEmum.ammo;
    }
    // public void spawnEnemies()
    // {
    //     for (int i = 0; i < enemyspawners.Length-1; i++)
    //     {
    //         enemyspawners[i].spawnenemy();
    //     }
    // }
}
