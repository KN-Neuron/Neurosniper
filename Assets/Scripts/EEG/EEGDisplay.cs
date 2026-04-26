using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EEGDisplay : MonoBehaviour
{
    [Header("UI Sliders")]
    public Slider attentionSlider;
    public Slider meditationSlider;

    [Header("UI Labels")]
    public TextMeshProUGUI attentionText;
    public TextMeshProUGUI meditationText;

    private int lastAttention = -1;
    private int lastMeditation = -1;

    private void Update()
    {
        if (EEGManager.Instance != null)
        {
            int currentAttention = EEGManager.Instance.Attention;
            int currentMeditation = EEGManager.Instance.Meditation;

            // Zabezpieczenie wartości
            if (currentAttention < 0) currentAttention = 0;
            if (currentMeditation < 0) currentMeditation = 0;

            if (currentAttention != lastAttention)
            {
                UpdateAttentionUI(currentAttention);
                lastAttention = currentAttention;
            }

            // Aktualizuj UI TYLKO jeśli wartość Relaksu się zmieniła
            if (currentMeditation != lastMeditation)
            {
                UpdateMeditationUI(currentMeditation);
                lastMeditation = currentMeditation;
            }
        }
    }

    private void UpdateAttentionUI(int value)
    {
        if (attentionSlider != null) attentionSlider.value = value;
        if (attentionText != null) attentionText.text = $"Attention Level: {value}%";
    }

    private void UpdateMeditationUI(int value)
    {
        if (meditationSlider != null) meditationSlider.value = value;
        if (meditationText != null) meditationText.text = $"Meditation Level: {value}%";
    }
}