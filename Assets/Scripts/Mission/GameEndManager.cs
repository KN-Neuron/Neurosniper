using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NeuroSniper.Mission;
using UnityEngine;

public class GameEndManager : MonoBehaviour
{
    private List<IWinCondition> iWinConditions = new List<IWinCondition>();
    private List<ILoseCondition> iLoseConditions = new List<ILoseCondition>();

    /// <summary>
    /// Seconds between the win condition being met and the victory screen appearing,
    /// so the player can watch the target's ragdoll fall.
    /// Keep it below HealthController's timeToDie, otherwise the body disappears before the screen shows.
    /// </summary>
    [SerializeField] private float victoryScreenDelay = 3f;

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
        // Set immediately so no lose condition (e.g. the timer) can fire during the delay
        gameEnded = true;
        StartCoroutine(ShowMissionSuccessAfterDelay(reason));
    }

    /// <summary>
    /// Waits before showing the victory screen. Uses scaled time, so the wait also stops while the game is paused.
    /// </summary>
    private IEnumerator ShowMissionSuccessAfterDelay(string reason)
    {
        yield return new WaitForSeconds(victoryScreenDelay);
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
