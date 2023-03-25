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
    public RandomGun[] randomGuns;
    public GameObject player; //we're going to spawn the player from GM. //no we arent keklord
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
    public TextMeshProUGUI triggerUItext;
    public Image gunUIImage; //gun swap


    public Image[] gunUI = new Image[4]; //gun showcase
    public Image gunUISelect;

    public float timer;
    public float timerOG;

    public bool started = false;
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

    public int playerX, playerY;

    public RoomGenerator currentRoom;
    public GameObject playButton;
    public GameObject backgroundImage;
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
    //public static GameObject null;

    public enum dropsEmum
    {
        gun,
        ammo
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

        //randomGunToDrop = GameObject.Find("randomGun");
        //ammoToDrop = GameObject.Find("Ammo");

        //drops.Add(randomGunToDrop);
        //drops.Add(ammoToDrop);
        //drops.Add(null);
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


    public void updateGunUI(List<gun> guns, int gunIndex)
    {
        for (int i = 0; i < gunUI.Length; i++)
        {
            try
            {
                if (guns[i] == null)
                {
                    break;
                }
            }
            catch
            {
                break;
            }
            

            gunUI[i].sprite = playergun.gunScript.gunSprites[(int)guns[i].gunType];
            gunUI[i].color = playergun.elementalColors[(int)guns[i].element];
        }

        gunUISelect.transform.position = gunUI[gunIndex].transform.position;

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
    public void swapGunAndOtherGun(gun mainGun, gun otherGun, RandomGun rGun)
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
        
        
        gunUIImage.sprite = playergun.gunScript.gunSprites[(int)gun.gunType];
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
        genManage.RetryLevel();
        genManage.floor = 0;
        sceneReset();


        //spawnEnemies();
    }
    public void nextFloor()
    {
        player.transform.position = new Vector3(0, 0, 0);// genManage.roomPositions[4,4];
        cam.transform.position = new Vector3(0, 0, 0); //genManage.roomPositions[4,4];
        playerX = 4;
        playerY = 4;
        player.GetComponent<SpriteRenderer>().enabled = true;
        playergun.gunScript.sprite.enabled = true;
        player.GetComponent<CircleCollider2D>().enabled = true;
        genManage.floor += 1;
        print("current floor: "+genManage.floor);
        genManage.RetryLevel();
        sceneReset();


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
        // if (!started && Input.GetButtonDown("Jump"))
        // {
        //     start();
        // }

        hpText.text = "HP: " + playerScript.hp.currentHP;


        //checkForEnemies();
    }


    public void start()
    {
        started = true;
        scoreText.enabled = true;
        titleHeader.enabled = false;
        titleFooter.enabled = false;
        playButton.SetActive(false);// = false;
        backgroundImage.SetActive(false);// = false;
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



    public static Dictionary<dropsEmum, int> enemyDropTable = new Dictionary<dropsEmum, int>()
    {
        {dropsEmum.gun, 40},
        {dropsEmum.ammo, 60},
        //{null, 30}
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
