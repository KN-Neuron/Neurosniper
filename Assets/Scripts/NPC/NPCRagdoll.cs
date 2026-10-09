using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class NPCRagdoll : MonoBehaviour, IRagdoll
{
    private Animator animator;
    private NavMeshAgent agent;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }
    public void EnableRagdoll()
    {
        if (animator != null) animator.enabled = false;
        if (agent != null) agent.enabled = false;

        Rigidbody[] allBodies = GetComponentsInChildren<Rigidbody>();
        Collider collider = GetComponent<Collider>();
        if (collider != null)
            collider.enabled = false;

        // Bodies are kinematic while alive (see BodyHitBox), so they have no leftover velocity.
        // Interpolation and continuous collision are only needed once the ragdoll is simulated.
        foreach (var body in allBodies)
        {
            body.isKinematic = false;
            body.interpolation = RigidbodyInterpolation.Interpolate;
            body.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }
    }

    public void ApplyForceToRagdoll(Vector3 hitPoint, Vector3 hitDirection, float bulletForce)
    {
        Rigidbody[] allBodies = GetComponentsInChildren<Rigidbody>();
        Rigidbody closest = null;
        float minDist = float.MaxValue;
        foreach (var body in allBodies)
        {
            float d = Vector3.Distance(hitPoint, body.worldCenterOfMass);
            if (d < minDist)
            {
                minDist = d;
                closest = body;
            }
        }

        if (closest != null)
            closest.AddForceAtPosition(hitDirection.normalized * bulletForce, hitPoint, ForceMode.Impulse);
    }
}