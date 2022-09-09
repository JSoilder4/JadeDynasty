﻿using System.Collections;
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
    public Image gunUIImage;


    public float timer;
    public float timerOG;

    public bool started = false;
    public Text titleHeader;
    public Text titleFooter;

    public bool end;

    public int maxScore;

    public List<GameObject> enemiesToReset = new List<GameObject>();
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
        maxScore = 5000;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void swapGun(gun gun, randomGun rGun)
    {
        //guntypeIndex
        //damage
        //betweenshottimer(remember BSTOG)
        //reloadtimer
        //shotspeed
        //element
        //effect
        rGun.playPickupSound();

        gunEnumScript.gunType gT = gun.gunType;
        int damage = gun.damage;
        float betweenshottimer = gun.bSTog;
        float reloadtimer = gun.reloadTimer;
        float shotspeed = gun.shotSpeed;
        gunEnumScript.element element = gun.element;
        gunEnumScript.effect effect = gun.effect;

        gun.gunType = rGun.gunType;
        gun.damage = rGun.damage;
        gun.bSTog = rGun.betweenShotTimer;
        gun.reloadTimer = rGun.reloadTimer;
        gun.shotSpeed = rGun.shotSpeed;
        gun.element = rGun.element;
        gun.effect = rGun.effect;

        rGun.gunType = gT;
        rGun.damage = damage;
        rGun.betweenShotTimer = betweenshottimer;
        rGun.reloadTimer = reloadtimer;
        rGun.shotSpeed = shotspeed;
        rGun.element = element;
        rGun.effect = effect;

        gunSwapUI(rGun);
    }

    public void gunSwapUI(randomGun rGun)
    {
        
       damageUItext.text = "Damage: "+ gun.gunScript.damage;
       RoFUItext.text = "Bullets Per Second: "+(Mathf.Round((1-gun.gunScript.bSTog)*100.0f)/100.0f)+"";
       shotSpeedUItext.text = "Shot Speed: "+ (Mathf.Round((1 - gun.gunScript.shotSpeed) * 100.0f) / 100.0f);
       elementUItext.text = "Element: " + gun.gunScript.element;
       effectUItext.text = "effect: " + gun.gunScript.effect;
        gunUIImage.sprite = rGun.sprite.sprite;
        gunUIImage.color = rGun.sprite.color;
        GunStatUIPlayableDirector.Stop();
        GunStatUIPlayableDirector.time = 0.0;
        GunStatUIPlayableDirector.Play();
    }
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
        sceneReset();
        player.transform.position = new Vector3(0, 0, 0);
        cam.transform.position = new Vector3(0,0,-10);
        player.GetComponent<playerMove>().enabled = true;
        gun.gunScript.enabled = true;
        player.GetComponent<SpriteRenderer>().enabled = true;
        gun.gunScript.sprite.enabled = true;
        player.GetComponent<CircleCollider2D>().enabled = true;
        rerollGuns();
        gun.gunScript.resetGun();
        spawnEnemies();
    }
    public void rerollGuns()
    {
        for (int i = 0; i < randomGuns.Length-1; i++)
        {
            randomGuns[i].rollGun();
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
        checkForEnemies();
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
    public void spawnEnemies()
    {
        for (int i = 0; i < enemyspawners.Length-1; i++)
        {
            enemyspawners[i].spawnenemy();
        }
    }
}
