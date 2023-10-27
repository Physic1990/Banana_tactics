using UnityEngine;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    // [Header("Unit Selection")]
    // [SerializeField] private GameObject unitSelection;

    // private void closeAllMenus()
    // {
    // unitSelection.SetActive(false);
    // }

    // private void Awake()
    // {
    // closeAllMenus();
    // }

    #region Unit Selection
    public void OpenUnitSelection()
    {
        // unitSelection.SetActive(true);
    }

    public void CloseUnitSelection()
    {
        // unitSelection.SetActive(false);
    }

    public bool GetIsUnitSelectionVisible()
    {
        // return unitSelection.activeSelf;
    }
    #endregion

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
        closeAllMenus();
    }

    public void Quit()
    {
        Application.Quit(); //Quits the game (only works in build)

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; //Exits play mode (will only be executed in the editor)
#endif
    }
}

