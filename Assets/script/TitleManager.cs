using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public void ChangeToMainGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void ChangeToSimModeScene()
    {
        SceneManager.LoadScene("SimMode");
    }
    public void ChangeToMainMenuScene()
    {
        SceneManager.LoadScene("Title");
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
