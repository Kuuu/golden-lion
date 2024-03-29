﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameController : MonoBehaviour
{
    private Tilemap coinsTilemap;
    public static GameController Instance = null;
    private int coinsLeft = 1;
    public DeviceType deviceType;


    private void Awake()
    {
        if (Instance)
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Instance = this;
        }

        deviceType = SystemInfo.deviceType;
    }

    // Use this for initialization
    void Start()
    {
        coinsTilemap = GameObject.Find("CoinTilemap").GetComponent<Tilemap>();
        coinsLeft = CountCoins();
        Debug.Log("Counted " + coinsLeft + " coins!");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitToMenu();
        }
    }

    public void ExitToMenu()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {

    }

    public void CollectedCoin()
    {
        coinsLeft--;
        CheckWin();
    }
    
    private void CheckWin()
    {
        if (coinsLeft == 0)
        {
            SceneManager.LoadScene("LevelSelect");
        }
    }

    private int CountCoins()
    {
        return coinsTilemap.gameObject.transform.childCount;
    }
}