using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public string levelName;
    public bool isUnlocked;
    public int buildIndex;
}

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] private List<LevelData> levels = new List<LevelData>();
    public IReadOnlyList<LevelData> Levels => levels.AsReadOnly();

    public int SelectedLevelIndex { get; private set; } = -1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        LoadLevelProgress();
    }

    public void SelectLevel(int levelIndex)
    {
        SelectedLevelIndex = levelIndex;
        PlayerPrefs.SetInt("SelectedLevelIndex", levelIndex);
    }

    public LevelData GetSelectedLevel()
    {
        if (SelectedLevelIndex < 0 || SelectedLevelIndex >= levels.Count)
            return null;
        return levels[SelectedLevelIndex];
    }

    public void UnlockLevel(int index)
    {
        if (index >= 0 && index < levels.Count)
            levels[index].isUnlocked = true;
    }

    public void SaveLevelProgress()
    {
        for (int i = 0; i < levels.Count; i++)
        {
            PlayerPrefs.SetInt($"Level_{i}_Unlocked", levels[i].isUnlocked ? 1 : 0);
        }
        PlayerPrefs.Save();
    }

    private void LoadLevelProgress()
    {
        for (int i = 0; i < levels.Count; i++)
        {
            levels[i].isUnlocked = PlayerPrefs.GetInt($"Level_{i}_Unlocked", i == 0 ? 1 : 0) == 1;
        }
    }
}
