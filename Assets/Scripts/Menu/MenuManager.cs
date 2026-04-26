using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenuPanel;

    [SerializeField]
    private GameObject levelSelectPanel;

    [SerializeField]
    private GameObject weaponSelectPanel;

    [SerializeField]
    private GameObject settingsPanel;

    public PanelSlider slider;

    public void StartGame()
    {
        slider.Slide(mainMenuPanel.GetComponent<RectTransform>(), levelSelectPanel.GetComponent<RectTransform>());
    }

    public void OpenSettings()
    {
        settingsPanel.GetComponent<SettingsManager>().LoadSettings();
        slider.Slide(mainMenuPanel.GetComponent<RectTransform>(), settingsPanel.GetComponent<RectTransform>());
    }

    public void ExitGame()
    {
        Debug.Log("Leaving the game...");
        Application.Quit();
    }

    public void BackToMainFromSettings()
    {
        settingsPanel.GetComponent<SettingsManager>().SaveSettings();
        slider.Slide(settingsPanel.GetComponent<RectTransform>(), mainMenuPanel.GetComponent<RectTransform>(), PanelSlider.SlideDirection.LeftToRight);
    }

    public void BackToMainFromLevelSelection()
    {
        slider.Slide(levelSelectPanel.GetComponent<RectTransform>(), mainMenuPanel.GetComponent<RectTransform>(), PanelSlider.SlideDirection.LeftToRight);
    }

    public void BackToLevelSelect()
    {
        slider.Slide(weaponSelectPanel.GetComponent<RectTransform>(), levelSelectPanel.GetComponent<RectTransform>(), PanelSlider.SlideDirection.LeftToRight);
    }

    public void WeaponSelect()
    {
        slider.Slide(levelSelectPanel.GetComponent<RectTransform>(), weaponSelectPanel.GetComponent<RectTransform>());
    }
}
