using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    public Text levelNumber;

    public void StartLevel()
    {
        SceneManager.LoadScene("Level" + levelNumber.text);
    }
}
