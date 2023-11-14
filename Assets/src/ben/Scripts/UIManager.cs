using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
public class UIManager : MonoBehaviour
{
    // Singleton Section
    public static UIManager Instance { get; private set; }
    private void Awake()
    {
        // Make sure there's only one instance of UI Manager
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
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

    public void QuitToMainMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
        AudioManager.Instance.PauseMusic();
    }

    /*************************************************************************
                                UI Element Visibility
    ************************************************************************/
    public void SetUIElementVisibility(VisualElement uiElement, bool isVisible)
    {
        if (uiElement == null)
            return;

        uiElement.style.display = (isVisible) ? DisplayStyle.Flex : DisplayStyle.None;
    }
}

