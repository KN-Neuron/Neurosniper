using UnityEngine;

public interface IRagdoll
{
    void EnableRagdoll();
    void ApplyForceToRagdoll(Vector3 hitPoint, Vector3 hitDirection, float force);
}