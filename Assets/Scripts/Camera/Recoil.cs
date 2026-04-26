using UnityEngine;

public class Recoil : MonoBehaviour
{
    private Vector3 currentRotation;
    private Vector3 targetRotation;

    [SerializeField] private float recoilSpeed = 10f;
    [SerializeField] private float returnSpeed = 5f;
    [SerializeField] private float randomness = 3f;

    private void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Lerp(currentRotation, targetRotation, recoilSpeed * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    /// <summary>
    /// Adds recoil to the object. Changes X-axis rotation based on the specified amount. Additionally, it adds some randomness to the Y and Z axes to simulate a more realistic recoil effect.
    /// The randomness is controlled by the 'randomness' field, which can be adjusted in the inspector.
    /// </summary>
    /// <param name="amount">
    /// number of degrees to add to x-axis rotation
    /// </param>
    public void AddRecoil(float amount)
    {
        targetRotation += new Vector3(-amount, Random.Range(-randomness, randomness), Random.Range(-randomness, randomness));
    }
}
