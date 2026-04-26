using UnityEngine;
using TMPro;
using System.Collections;

public class DetectionUI : MonoBehaviour
{
    [Header("Ustawienia UI")]
    [SerializeField] private TextMeshProUGUI warningText; 
    [SerializeField] private float displayTime = 7.0f;

    private Coroutine hideCoroutine;

    private void Awake()
    {
        if (warningText != null)
        {
            warningText.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        DroneSpotlight.OnPlayerSpotted += ShowWarningMessage;
    }

    private void OnDisable()
    {
        DroneSpotlight.OnPlayerSpotted -= ShowWarningMessage;
    }

    private void ShowWarningMessage()
    {
        if (warningText == null) return;

        warningText.text = "You have been detected!";
        warningText.gameObject.SetActive(true);


        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }

        hideCoroutine = StartCoroutine(HideTextRoutine());
    }

    private IEnumerator HideTextRoutine()
    {
        yield return new WaitForSeconds(displayTime);

        warningText.gameObject.SetActive(false);
        hideCoroutine = null;
    }
}