using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private Weapon currentWeapon;
    [SerializeField] private GameObject crossHair;
    [SerializeField] private RectTransform scatterAreaUI;
    [SerializeField] private CinemachineCamera aimCamera;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Recoil recoil;

    private void Awake()
    {
        cameraController.AimMaxFov = currentWeapon.MaxZoomFOV;
        cameraController.AimMinFov = currentWeapon.MinZoomFOV;
        cameraController.MinAimAmplitude = currentWeapon.MinAimNoiseAmplitude;
        cameraController.MaxAimAmplitude = currentWeapon.MaxAimNoiseAmplitude;
        cameraController.MinAimFrequency = currentWeapon.MinAimNoiseFrequency;
        cameraController.MaxAimFrequency = currentWeapon.MaxAimNoiseFrequency;
    }

    private void FixedUpdate()
    {
        if (currentWeapon == null || crossHair == null) return;

        HandleShooting();
        HandleAiming();
    }

    private void HandleShooting()
    {
        // Check if the player is trying to shoot
        if (GameInput.Instance.IsShooting())
        {
            Vector3 shootingDirection = aimCamera.State.GetFinalOrientation() * Vector3.forward;
            currentWeapon.SetDirection(shootingDirection);

            Vector3 shootingPosition = CalculateShootingOffset();
            currentWeapon.SetOffset(shootingPosition);

            if (currentWeapon.Fire())
            {
                recoil.AddRecoil(currentWeapon.RecoilForce);
            }
        }
    }

    private void HandleAiming()
    {
        // Dodatkowe zabezpieczenie
        if (crossHair == null) return;

        if (GameInput.Instance.IsAiming())
        {
            crossHair.SetActive(true);
        }
        else
        {
            crossHair.SetActive(false);
        }
    }

    private Vector3 CalculateShootingOffset()
    {
        float distraction = EEGManager.Instance.IsConnected ? (100.0f - EEGManager.Instance.Attention) / 100.0f : 1;

        float maxRadius = distraction * currentWeapon.MaxScatterRadius * 0.1f;

        float angle = Random.Range(0f, Mathf.PI * 2f);
        float radius = maxRadius * Mathf.Sqrt(Random.Range(0f, 1f));

        float x = radius * Mathf.Cos(angle);
        float y = radius * Mathf.Sin(angle);

        return new Vector3(x, y, 0);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        crossHair = null;

        if (scene.name == "Level1")
        {
            GameObject foundCrosshair = GameObject.Find("Scope Variant");

            if (foundCrosshair != null)
            {
                crossHair = foundCrosshair;
                crossHair.SetActive(false);
                Debug.Log("Znalazłem i przypisałem Scope Variant!");
            }
            else
            {
                // To logujemy tylko jeśli jesteśmy w Level1, a mimo to nie ma celownika
                Debug.LogError("BŁĄD: Nie znaleziono obiektu 'Scope Variant' w scenie Level1!");
            }
        }
    }
}