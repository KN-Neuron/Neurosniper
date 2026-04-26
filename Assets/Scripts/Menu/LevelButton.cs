using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public int levelIndex;
    public Button button;
    public GameObject lockIcon;

    private void Start()
    {
        var buttonSound = GetComponent<ButtonSound>();
        var levelData = LevelManager.Instance.Levels[levelIndex-1];
        button.interactable = levelData.isUnlocked;
        lockIcon.SetActive(!levelData.isUnlocked);

        if (!levelData.isUnlocked && buttonSound != null)
            buttonSound.enabled = false;

        button.onClick.AddListener(() =>
        {
            LevelManager.Instance.SelectLevel(levelIndex);
        });
    }
}
