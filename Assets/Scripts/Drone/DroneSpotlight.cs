using UnityEngine;
using System;

public class DroneSpotlight : MonoBehaviour
{
    [Header("Światło")]
    [SerializeField] private Light spotLight;
    [SerializeField] private Color normalColor = Color.yellow;
    [SerializeField] private Color detectedColor = Color.red;
    [SerializeField] private float colorChangeSpeed = 5f;

    [Header("Ruch Reflektora")]
    [SerializeField] private float scanSpeed = 2f;
    [SerializeField] private float scanAngle = 15f;
    [SerializeField] private bool autoScan = true;

    [Header("Detekcja")]
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private float detectionDistance = 20f;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private LayerMask detectionLayer = -1;

    
    public static event Action OnPlayerSpotted;

    
    private bool panicEventSent = false;
    

    private Color targetColor;
    private Quaternion startRotation;
    private float scanTime;
    private bool playerDetected = false;

    private void Start()
    {
        if (spotLight == null)
            spotLight = GetComponent<Light>();

        targetColor = normalColor;
        spotLight.color = normalColor;
        startRotation = transform.localRotation;

        if (spotLight.type != LightType.Spot)
        {
            spotLight.type = LightType.Spot;
        }
    }

    private void Update()
    {
        if (spotLight == null) return;

        
        if (autoScan && !playerDetected)
        {
            ScanArea();
        }

       
        DetectPlayer();

        
        targetColor = playerDetected ? detectedColor : normalColor;
        spotLight.color = Color.Lerp(spotLight.color, targetColor, Time.deltaTime * colorChangeSpeed);

        
        if (playerDetected && !panicEventSent)
        {
            
            panicEventSent = true; 
            OnPlayerSpotted?.Invoke(); 
            Debug.Log("Dron wysyła sygnał paniki!");
        }
        else if (!playerDetected && panicEventSent)
        {
           
            panicEventSent = false;
        }
    }

    private void ScanArea()
    {
        scanTime += Time.deltaTime * scanSpeed;
        float rotationOffset = Mathf.Sin(scanTime) * scanAngle;
        Quaternion scanRotation = Quaternion.Euler(rotationOffset, 0, 0);
        transform.localRotation = startRotation * scanRotation;
    }

    private void DetectPlayer()
    {
        RaycastHit[] hits = Physics.SphereCastAll(
            transform.position,
            detectionRadius,
            transform.forward,
            detectionDistance,
            detectionLayer
        );

        playerDetected = false;

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag(playerTag))
            {
                
                if (IsInSpotlightCone(hit.point))
                {
                    playerDetected = true;
                    break;
                }
            }
        }
    }

    private bool IsInSpotlightCone(Vector3 point)
    {
        Vector3 directionToPoint = (point - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, directionToPoint);

        return angle < spotLight.spotAngle / 2f;
    }
}