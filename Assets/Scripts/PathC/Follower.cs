using UnityEngine;
using PathCreation;

public class Follower : MonoBehaviour
{
    public PathCreator pathCreator;
    public float speed = 10.0f;
    public float rotationOffset = 90f;

    [Header("Propeller Settings")]
    public Transform[] propellers; 
    public float propellerSpeed = 2000f;

    float distanceTravelled;

    void Update()
    {
        distanceTravelled += speed * Time.deltaTime;
        transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);

        Vector3 direction = pathCreator.path.GetDirectionAtDistance(distanceTravelled);

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            Vector3 eulerRotation = targetRotation.eulerAngles;
            transform.rotation = Quaternion.Euler(0, eulerRotation.y + rotationOffset, 25);
        }

        
        foreach (Transform propeller in propellers)
        {
            if (propeller != null)
            {
                
                propeller.Rotate(Vector3.forward, propellerSpeed * Time.deltaTime);
            }
        }
    }
}