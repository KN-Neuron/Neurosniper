using UnityEngine;
using UnityEngine.UI;

public class EEGSignalIcon: MonoBehaviour
{
    [SerializeField] private Image brainIcon;

    private Gradient signalGradient;

    private void Awake()
    {
        // gradient from Red (0) → Orange → Yellow → Green (1), built once instead of every frame
        signalGradient = new Gradient();
        signalGradient.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(Color.red, 0.0f),
                new GradientColorKey(new Color(1f, 0.5f, 0f), 0.33f), // Orange
                new GradientColorKey(Color.yellow, 0.66f),
                new GradientColorKey(Color.green, 1.0f)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(1f, 1f)
            }
        );
    }

    private void Update()
    {
        // Clamp and normalize the signal value (0 = best, 200 = worst)

        int signal = EEGManager.Instance.PoorSignal;

        if(!EEGManager.Instance.IsConnected) signal = 200;

        float t = Mathf.InverseLerp(200f, 0f, signal); // t in range [0,1], where 0 = worst (red), 1 = best (green)
        brainIcon.color = signalGradient.Evaluate(t);
    }
}
