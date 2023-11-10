using UnityEngine;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{

    private GameScreen gameScreen;

    private void Awake()
    {
        gameScreen = GameObject.FindGameObjectWithTag("UIManager").GetComponent<GameScreen>();
    }

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
#if UNITY_EDITOR
        if (Application.isPlaying)
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadSceneAsync(scene.name);
        }
#endif
    }



    public void Quit()
    {
        Application.Quit(); //Quits the game (only works in build)

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; //Exits play mode (will only be executed in the editor)
#endif
    }
}

