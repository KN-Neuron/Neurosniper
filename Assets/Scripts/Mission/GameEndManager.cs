using System.Collections.Generic;
using System.Linq;
using NeuroSniper.Mission;
using UnityEngine;

public class GameEndManager : MonoBehaviour
{   
    private List<IWinCondition> iWinConditions = new List<IWinCondition>();
    private List<ILoseCondition> iLoseConditions = new List<ILoseCondition>();
    
    private bool gameEnded = false;

    void Start()
    {
        var allWinComponents = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .Where(component => component is IWinCondition)
            .Cast<IWinCondition>();
        
        foreach(var winCondition in allWinComponents)
        {
            if(!iWinConditions.Contains(winCondition))
            {
                iWinConditions.Add(winCondition);
            }
        }

        // Find all Lose Conditions
        var allLoseComponents = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .Where(component => component is ILoseCondition)
            .Cast<ILoseCondition>();
        
        foreach(var loseCondition in allLoseComponents)
        {
            if(!iLoseConditions.Contains(loseCondition))
            {
                iLoseConditions.Add(loseCondition);
            }
        }
    }

    void Update()
    {
        if (gameEnded) return;

        CheckWinConditions();
        CheckLoseConditions();
    }
    
    private void CheckWinConditions()
    {
        foreach(var condition in iWinConditions)
        {
            if(condition.IsConditionMet())
            {
                OnGameWon();
                return;
            }
        }
    }

    private void CheckLoseConditions()
    {
        foreach (var condition in iLoseConditions)
        {
            if (condition.IsConditionMet())
            {
                OnGameLost(condition.GetConditionDescription());
                return;
            }
        }
    }
    
    private void OnGameWon(string reason = "")
    {
        gameEnded = true;
        GameManager.Instance.ShowMissionSuccess(reason);
    }

    private void OnGameLost(string reason = "")
    {
        gameEnded = true;
        GameManager.Instance.ShowMissionFailed(reason);
    }
    
    public void AddWinCondition(IWinCondition condition)
    {
        iWinConditions.Add(condition);
    }
    
    public void AddLoseCondition(ILoseCondition condition)
    {
        iLoseConditions.Add(condition);
    }
    
    public void RemoveWinCondition(IWinCondition condition)
    {
        iWinConditions.Remove(condition);
    }
    
    public void RemoveLoseCondition(ILoseCondition condition)
    {
        iLoseConditions.Remove(condition);
    }
}
