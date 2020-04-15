using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameController : MonoBehaviour
{
    public Tilemap coinsTilemap;
    public static GameController Instance = null;
    private int coinsLeft = 1;


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
    }

    // Use this for initialization
    void Start()
    {
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
            Debug.Log("WIN!");
        }
    }

    private int CountCoins()
    {
        return coinsTilemap.gameObject.transform.childCount;
    }
}