using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI References")]
    public GameObject missionSuccessPanel;
    public GameObject missionFailedPanel;
    public GameObject pausePanel; // NOWE: Referencja do panelu pauzy

    private bool isGameActive;
    private bool isPaused; // NOWE: Zmienna sprawdzająca, czy gra jest spauzowana

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (missionSuccessPanel != null)
            missionSuccessPanel.SetActive(false);
        if (missionFailedPanel != null)
            missionFailedPanel.SetActive(false);

        if (pausePanel != null)
            pausePanel.SetActive(false);

        Time.timeScale = 1;
        isGameActive = true;
        isPaused = false;
    }

    private void Update()
    {
        if (isGameActive)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePause();
            }
        }
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGameUser();
        }
    }

    // NOWE: Funkcja wznawiająca grę (podpinasz ją też pod przycisk "Resume")
    public void ResumeGame()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);

        Time.timeScale = 1;
        isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // NOWE: Wewnętrzna funkcja pauzująca dla gracza
    private void PauseGameUser()
    {
        if (pausePanel != null)
            pausePanel.SetActive(true);

        PauseInternal();
    }

    // Zmieniłem nazwę ze starego "PauseGame" na "PauseInternal", żeby się nie myliło
    private void PauseInternal()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ShowMissionSuccess(string reason)
    {
        if (!isGameActive) return;

        isGameActive = false;

        if (missionSuccessPanel != null)
        {
            missionSuccessPanel.SetActive(true);
            Transform successReasonTransform = missionSuccessPanel.transform.Find("SuccessReason");
            if (successReasonTransform != null)
            {
                TextMeshProUGUI successReasonText = successReasonTransform.GetComponent<TextMeshProUGUI>();
                successReasonText.text = reason;
            }
        }

        PauseInternal();
    }

    public void ShowMissionFailed(string reason)
    {
        if (!isGameActive) return;

        isGameActive = false;

        if (missionFailedPanel != null)
        {
            missionFailedPanel.SetActive(true);
            Transform failReasonTransform = missionFailedPanel.transform.Find("FailReason");
            if (failReasonTransform != null)
            {
                TextMeshProUGUI failReasonText = failReasonTransform.GetComponent<TextMeshProUGUI>();
                failReasonText.text = reason;
            }
        }

        PauseInternal();
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        isPaused = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene("MainMenu");

        Destroy(gameObject);
    }
}