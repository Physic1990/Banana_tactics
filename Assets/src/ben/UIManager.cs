using UnityEngine;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    [Header("Unit Selection")]
    [SerializeField] private GameObject unitSelection;

    private void closeAllMenus()
    {
        // unitSelection.SetActive(false);
    }

    private void Awake()
    {
        closeAllMenus();
    }

    #region Unit Selection
    public void OpenUnitSelection()
    {
        unitSelection.SetActive(true);
    }

    public void CloseUnitSelection()
    {
        unitSelection.SetActive(false);
    }

    public bool GetIsUnitSelectionVisible()
    {
        return unitSelection.activeSelf;
    }
    #endregion
}

