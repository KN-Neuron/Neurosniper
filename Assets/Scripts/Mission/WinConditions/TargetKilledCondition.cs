using NeuroSniper.Mission;
using UnityEngine;

public class TargetKilledCondition : MonoBehaviour, IWinCondition
{
    private HealthController targetHealthController;
    public string GetConditionDescription()
    {
        return "Eliminate the target.";
    }

    public bool IsConditionMet()
    {
        return targetHealthController != null && !targetHealthController.IsAlive;
    }

    void Start()
    {
        targetHealthController = GetComponent<HealthController>();
    }
}
