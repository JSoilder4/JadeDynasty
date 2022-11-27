using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
    public static GameManager GM;
    public enemySpawn[] enemyspawners;
    public randomGun[] randomGuns;
    public GameObject player; //we're going to spawn the player from GM.
    public playerMove playerScript;
    public bool playerdead;
    public Camera cam;
    public Text respawnText;
    public Text scoreText;
    public Text wintext;
    public int score;

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
    public Image gunUIImage;
    


    public float timer;
    public float timerOG;

    public bool started = false;
    public Text titleHeader;
    public Text titleFooter;

    public bool end;

    public int maxScore;

    public List<GameObject> enemiesToReset = new List<GameObject>();

    public AudioClip pickupSound;
    public AudioSource pickupSource;

    public Color clense;
    public Color fire = new Color(255, 0, 0);
    public Color water = new Color(0, 130, 255);
    public Color earth = new Color(0, 255, 0);
    public Color air = new Color(255, 255, 0);

    public Text hpText;

    public int playerX, playerY;

    public RoomGenerator currentRoom;
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
        clense = playergun.gunScript.clense;
        playerScript = player.GetComponent<playerMove>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        genManage = GameObject.FindWithTag("GameController").GetComponent<GenerationManager>();


        playerX = 4;
        playerY = 4;
        playerRoomGrid[playerX, playerY] = "true";


        //randomGuns = GameObject.FindGameObjectsWithTag("Gun");
    }
    /// <summary>
    /// Currently used for moving the reference of where the player is. Eventually can also be used to implement minimap.
    /// </summary>
    /// <param name="xDifference"></param>
    /// <param name="yDifference"></param>
    public void renderMinimap(int xDifference, int yDifference) //should be called by moving between doors.
    {
        playerRoomGrid[playerX, playerY] = "false";
        playerX += xDifference;
        playerY += yDifference;
        playerRoomGrid[playerX, playerY] = "true";
        //for(int y = 0; y < playerRoomGrid.length(1); y++){for(int x = 0; x < playerroomgrid.length; x++){}}
    }
    /// <summary>
    /// Used for switching between bigrooms. Will need to adjust minimap to account for big rooms later.
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
        //for(int y = 0; y < playerRoomGrid.length(1); y++){for(int x = 0; x < playerroomgrid.length; x++){}}
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
        print("trouble is doubled");
        playPickupSound();
        playergun.gunScript.equippedGuns.Add(rGunG);
        playergun.gunScript.gunIndex = playergun.gunScript.equippedGuns.Count;
        gunSwapUI(rGunG, true);
        rGun.SetActive(false);
    }

    public void gunSwapUI(gun gun, bool temp)
    {
        if (gun.gunType == gunEnumScript.gunType.Shotgun)
        {
            damageUItext.text = "Damage: " + gun.damage + " x " + ((gun.numShots *2)+1);
        }
        else if (gun.numShots > 0)
        {
            damageUItext.text = "Damage: " + gun.damage + " x " + (gun.numShots + 1);
        }
        else
        {
            damageUItext.text = "Damage: " + gun.damage;
        }

        RoFUItext.text = "Bullets Per Second: "+(Mathf.Round((1- gun.bSTog)*100.0f)/100.0f)+"";
        shotSpeedUItext.text = "Shot Speed: "+ (Mathf.Round((/*1 - */gun.shotSpeed) * 100.0f) / 100.0f);
        elementUItext.text = "Element: " + gun.element;
        effectUItext.text = "Special: " + gun.effect;
        spreadUItext.text = "Spread: " + gun.spread;
        gunUIImage.sprite = playergun.gunScript.gunSprites[(int)gun.gunType];
        switch (gun.element)
        {
            case gunEnumScript.element.Nothing:
                gunUIImage.color = clense;
                break;
            case gunEnumScript.element.Fire:
                gunUIImage.color = fire;
                break;
            case gunEnumScript.element.Water:
                gunUIImage.color = water;
                break;
            case gunEnumScript.element.Earth:
                gunUIImage.color = earth;
                break;
            case gunEnumScript.element.Air:
                gunUIImage.color = air;
                break;
        }
        //gunUIImage.color = playergun.gunScript.sprite.color;
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
       
        player.transform.position = new Vector3(0, 0, 0);
        cam.transform.position = new Vector3(0,0,-10);
        playerScript.enabled = true;
        playerScript.hp.currentHP = 5;
        playergun.gunScript.enabled = true;
        player.GetComponent<SpriteRenderer>().enabled = true;
        playergun.gunScript.sprite.enabled = true;
        player.GetComponent<CircleCollider2D>().enabled = true;
        rerollGuns();
        playergun.gunScript.resetGun();
        //spawnEnemies();
    }
    public void rerollGuns()
    {
        for (int i = 0; i < randomGuns.Length; i++)
        {
            randomGuns[i].gameObject.SetActive(true);
            randomGuns[i].theGun.roll();//rollGun();
        }
    }
    public void dead()
    {
        playerdead = true;
        respawnText.enabled = true;
    }
    public void alive()
    {
        playerdead = false;
        respawnText.enabled = false;
    }
    public void sceneReset()
    {
        for (int i = enemiesToReset.Count-1; i >= 0; i--)
        {
            //enemiesToReset.Remove(enemiesToReset[i]);
           // print("die");
            Destroy(enemiesToReset[i]);
            
        }
        enemiesToReset.Clear();
        resetScore();
    }
    public void addSpawnedObject(GameObject g)
    {
        enemiesToReset.Add(g);
    }
    public void Update()
    {
        if(!end)
        scoreTime();
        if (playerdead)
        {
            if(Input.GetButtonDown("Fire1"))
            {
                respawn();
                alive();
            }
        }
        if (!started && Input.GetButtonDown("Jump"))
        {
            start();
        }

        hpText.text = "HP: " + playerScript.hp.currentHP;


        //checkForEnemies();
    }
    public void start()
    {
        started = true;
        scoreText.enabled = true;
        titleHeader.enabled = false;
        titleFooter.enabled = false;
        respawn();

    }
    public void updateScore(int s)
    {
        score += s;
        if (score < 0)
        {
            score = 0;
        }
        scoreText.text = score.ToString();
    }
    public void resetScore()
    {
        score = 0;
        scoreText.text = score.ToString();
    }
    public void scoreTime()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            timer = timerOG;
            updateScore(-5);
        }
        
    }
    public void checkForEnemies()
    {
        //print(enemiesToReset.Count);
        if (enemiesToReset.Count == 0)
        {
            win();
        }
    }
    public void win()
    {
        scoreText.enabled = false;
        wintext.enabled = true;
        end = true;
        wintext.text = "All Enemies Defeated! Final Score: " + score; 
    }
    // public void spawnEnemies()
    // {
    //     for (int i = 0; i < enemyspawners.Length-1; i++)
    //     {
    //         enemyspawners[i].spawnenemy();
    //     }
    // }
}
