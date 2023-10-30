using UnityEngine;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{

    public void PlayGame(string sceneName)
    {
#if UNITY_EDITOR
            if (Application.isPlaying)
#endif
        SceneManager.LoadSceneAsync(sceneName);
    }

    //Restart level
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // closeAllMenus();
    }

    

    public void Quit()
    {
        Application.Quit(); //Quits the game (only works in build)

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; //Exits play mode (will only be executed in the editor)
#endif
    }
}

