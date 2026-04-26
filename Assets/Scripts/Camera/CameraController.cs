using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineCamera mainCamera;
    [SerializeField] private CinemachineCamera aimCamera;
    private CinemachineBasicMultiChannelPerlin mainCameraNoise;
    private CinemachineBasicMultiChannelPerlin aimCameraNoise;

    /// <summary>
    /// Speed at which the camera shake transitions between different states.
    /// This value controls how quickly the camera noise amplitude and frequency change when switching between idle, walking, running, and aiming states.
    /// A higher value results in a faster transition, while a lower value results in a smoother, slower transition.
    /// </summary>
    [SerializeField] private float shakeTransitionSpeed = 1f;
    [SerializeField] private float idleAmplitude = 0.1f;
    [SerializeField] private float idleFrequency = 0.3f;
    [SerializeField] private float walkAmplitude = 0.3f;
    [SerializeField] private float walkFrequency = 1.5f;
    [SerializeField] private float runAmplitude = 0.6f;
    [SerializeField] private float runFrequency = 3f;

    [SerializeField] private float aimMaxFOV = 30f;
    [SerializeField] private float aimMinFOV = 10f;
    [SerializeField] private float normalFOV = 60f;
    [SerializeField] private float zoomSpeed = 10f;
    private float maxAimAmplitude = 0f;
    private float minAimAmplitude = 0f;
    private float minAimFrequency = 0f;
    private float maxAimFrequency = 0f;

    public float AimMaxFov { get => aimMaxFOV; set => aimMaxFOV = value; }
    public float AimMinFov { get => aimMinFOV; set => aimMinFOV = value; }

    public float MinAimAmplitude { get => minAimAmplitude; set => minAimAmplitude = value; }
    public float MaxAimAmplitude { get => maxAimAmplitude; set => maxAimAmplitude = value; }
    public float MinAimFrequency { get => minAimFrequency; set => minAimFrequency = value; }
    public float MaxAimFrequency { get => maxAimFrequency; set => maxAimFrequency = value; }
    private bool isAiming = false;
    private float targetFOV;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (mainCamera != null && aimCamera != null)
        {
            mainCameraNoise = mainCamera.GetCinemachineComponent(CinemachineCore.Stage.Noise) as CinemachineBasicMultiChannelPerlin;
            aimCameraNoise = aimCamera.GetCinemachineComponent(CinemachineCore.Stage.Noise) as CinemachineBasicMultiChannelPerlin;
        }
    }

    private void Update()
    {
        HandleCameraShake();
        HandleAiming();
    }

    /// <summary>
    /// Handles frequency and amplitude of cinemachine camera noise based on player state
    /// </summary>
    private void HandleCameraShake()
    {
        if (GameInput.Instance.IsAiming())
        {
            CalculateAimAmplitude(out float targetAmplitude, out float targetFrequency);

            SetCameraNoise(aimCameraNoise, targetAmplitude, targetFrequency);
        }
        else if (GameInput.Instance.IsRunning())
        {
            SetCameraNoise(mainCameraNoise, runAmplitude, runFrequency);
        }
        else if (GameInput.Instance.IsWalking())
        {
            SetCameraNoise(mainCameraNoise, walkAmplitude, walkFrequency);
        }
        else
        {
            SetCameraNoise(mainCameraNoise, idleAmplitude, idleFrequency);
        }
    }

    /// <summary>
    /// Handles zooming in and out when aiming.
    /// </summary>
    private void HandleAiming()
    {
        if (GameInput.Instance.IsAiming())
        {
            aimCamera.Priority = 11; // Set the aim camera to be active
            if (!isAiming)
            {
                targetFOV = aimMaxFOV;
                isAiming = true;
            }

            HandleZoom();
            mainCamera.Lens.FieldOfView = Mathf.Lerp(
                mainCamera.Lens.FieldOfView,
                normalFOV,
                Time.deltaTime * zoomSpeed
            );

            aimCamera.Lens.FieldOfView = Mathf.Lerp(
                aimCamera.Lens.FieldOfView,
                targetFOV,
                Time.deltaTime * zoomSpeed
            );
        }
        else
        {
            aimCamera.Priority = 0; // Set the aim camera to be inactive
            isAiming = false;
            // Smoothly return to normal FOV
            targetFOV = normalFOV;

            aimCamera.Lens.FieldOfView = Mathf.Lerp(
                aimCamera.Lens.FieldOfView,
                normalFOV,
                Time.deltaTime * zoomSpeed
            );
            
            mainCamera.Lens.FieldOfView = Mathf.Lerp(
                mainCamera.Lens.FieldOfView,
                normalFOV,
                Time.deltaTime * zoomSpeed
            );
        }
    }

    /// <summary>
    /// Adjusts the targetFOV based on the scroll input, clamping it between aimMinFOV and aimMaxFOV.
    /// </summary>
    private void HandleZoom()
    {
        Vector2 scrollInput = GameInput.Instance.GetMouseScrollWheel();
        float scrollY = scrollInput.y;

        if (Mathf.Abs(scrollY) > 0.01f)
        {
            // Scroll to adjust targetFOV when aiming
            targetFOV -= scrollY * zoomSpeed;
            targetFOV = Mathf.Clamp(targetFOV, aimMinFOV, aimMaxFOV);
        }
    }

    /// <summary>
    /// Sets the cinemachine camera noise amplitude and frequency.
    /// </summary>
    /// <param name="amplitude"></param>
    /// <param name="frequency"></param>
    private void SetCameraNoise(CinemachineBasicMultiChannelPerlin noiseChannel, float amplitude, float frequency)
    {
        if (noiseChannel != null)
        {
            noiseChannel.AmplitudeGain = Mathf.Lerp(noiseChannel.AmplitudeGain, amplitude, Time.deltaTime * shakeTransitionSpeed);
            noiseChannel.FrequencyGain = Mathf.Lerp(noiseChannel.FrequencyGain, frequency, Time.deltaTime * shakeTransitionSpeed);
        }
    }
    
    /// <summary>
    /// Calculates cinemachine camera noise amplitude and frequency based on EEGManager values.
    /// </summary>
    /// <param name="amplitude"></param>
    /// <param name="frequency"></param>
    private void CalculateAimAmplitude(out float amplitude, out float frequency)
    {
        if (EEGManager.Instance.IsConnected)
        {
            //calculate differences between max and min amplitude and frequency for aiming
            float amplitudeDif = maxAimAmplitude - minAimAmplitude;
            float frequencyDif = maxAimFrequency - minAimFrequency;
            amplitude = maxAimAmplitude - (EEGManager.Instance.Meditation * amplitudeDif / 100);
            frequency = maxAimFrequency - (EEGManager.Instance.Attention * frequencyDif / 100);
        }
        else
        {
            //if EEGManager is not connected, use default values
            amplitude = maxAimAmplitude;
            frequency = maxAimFrequency;
        }
    }
}
