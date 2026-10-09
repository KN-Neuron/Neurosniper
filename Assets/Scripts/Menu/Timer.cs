using System;
using NeuroSniper.Mission;
using TMPro;
using UnityEngine;

public class Timer : EndCondition, ILoseCondition
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float startTime = 120f;
    private float timeRemaining;
    private int lastDisplayedSeconds = -1;

    private void Start()
    {
        timeRemaining = startTime;
    }

    private void Update()
    {
        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            return;
        }
        else if (timeRemaining < 60)
        {
            timerText.color = Color.red;
        }

        timeRemaining -= Time.deltaTime;
        int minutes = Mathf.FloorToInt(timeRemaining / 60F);
        int seconds = Mathf.FloorToInt(timeRemaining - minutes * 60);

        // Only rebuild the text when the displayed value changes, not every frame
        int displayedSeconds = minutes * 60 + seconds;
        if (displayedSeconds == lastDisplayedSeconds) return;
        lastDisplayedSeconds = displayedSeconds;

        timerText.text = "Remaining time:\n" + string.Format("{0:0}:{1:00}", minutes, seconds);
    }

    public override bool IsConditionMet()
    {
        return timeRemaining <= 0;
    }
}
