using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{

    public void StartGame()
    {
        SceneManager.LoadScene("CharacterSelection");
    }

    public void JoinGame()
    {
        // code to join an existing game
    }

    public void OpenSettings()
    {
        // code to open the settings menu
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}