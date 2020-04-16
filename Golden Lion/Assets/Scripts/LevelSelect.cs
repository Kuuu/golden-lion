using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    public void StartLevel(int levelNumber)
    {
        SceneManager.LoadScene("Level" + levelNumber);
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
