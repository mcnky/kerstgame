using UnityEngine;
using UnityEngine.SceneManagement;

public class Mainmenu : MonoBehaviour
{
    public void playGame()
    {
        SceneManager.LoadScene(1);
        Debug.Log("started game");
    }

    public void Quit()
    {
        PlayerPrefs.Save();
        Application.Quit();
        Debug.Log("quit");
    }

}