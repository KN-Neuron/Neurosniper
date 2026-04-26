using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class BodyHitBox : MonoBehaviour, IDamageable
{
    private float damageMultiplayer = 1f;
    [SerializeField] private float hitAnimationDuration = 0.8f;
    private const float DEFAULT_BULLET_FORCE = 5f;

    private HealthController healthController;
    private IRagdoll ragdoll;
    private Rigidbody rb;
    private Animator characterAnimator;

    private void Start()
    {
        healthController = GetComponentInParent<HealthController>();
        characterAnimator = GetComponentInParent<Animator>();
        ragdoll = GetComponentInParent<IRagdoll>();

        damageMultiplayer = DamageMultipliers.GetDamageMultiplayer(gameObject.tag);

        //rigidbody setup
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        //layer setup
        gameObject.layer = GameLayers.HITBOX_LAYER;
    }

    public void TakeDamage(float damage)
    {
        healthController.TakeDamage(damage * damageMultiplayer);
    }

    public void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitDirection, float bulletForce = DEFAULT_BULLET_FORCE)
    {
        TakeDamage(damage);
        if (healthController.IsAlive)
        {
            TriggerHitAnimation(hitDirection);
        }
        else
        {
            ragdoll.EnableRagdoll();
            ragdoll.ApplyForceToRagdoll(hitPoint, hitDirection, bulletForce);
        }
    }
    private void TriggerHitAnimation(Vector3 hitDirection)
    {
        int hitLayerIndex = characterAnimator.GetLayerIndex(AnimationParameters.HIT_ANIMATION_LAYER_NAME);

        // Enable the hit animation layer
        characterAnimator.SetLayerWeight(hitLayerIndex, 1f);

        // Determine hit direction relative to character
        Transform characterTransform = characterAnimator.transform;
        Vector3 normalizedHitDir = -hitDirection.normalized;

        // Calculate dot products to determine direction
        float forwardDot = Vector3.Dot(normalizedHitDir, characterTransform.forward);
        float rightDot = Vector3.Dot(normalizedHitDir, characterTransform.right);

        string triggerToUse = AnimationParameters.HIT_FRONT_PARAM;
        
        // Check if it's more front/back or left/right
        if (Mathf.Abs(forwardDot) > Mathf.Abs(rightDot))
        {
            triggerToUse = forwardDot > 0 ? AnimationParameters.HIT_FRONT_PARAM : AnimationParameters.HIT_BACK_PARAM;
        }
        else
        {
            triggerToUse = rightDot > 0 ? AnimationParameters.HIT_RIGHT_PARAM : AnimationParameters.HIT_LEFT_PARAM;
        }
        
        // Trigger the appropriate animation
        characterAnimator.SetTrigger(triggerToUse);
        
        Debug.Log($"Hit animation triggered from direction: {triggerToUse}");
        
        // Reset animation state after duration
        StartCoroutine(ResetHitAnimation(hitLayerIndex, hitAnimationDuration));
    }
    
    private IEnumerator ResetHitAnimation(int layerIndex, float delay)
    {
        // Wait for the animation to complete
        yield return new WaitForSeconds(delay);
        
        // Gradually fade out the hit animation layer
        float fadeTime = 0.4f;
        float startTime = Time.time;
        float weight = 1f;
        
        while (Time.time < startTime + fadeTime)
        {
            weight = Mathf.Lerp(1f, 0f, (Time.time - startTime) / fadeTime);
            characterAnimator.SetLayerWeight(layerIndex, weight);
            yield return null;
        }
        
        characterAnimator.SetLayerWeight(layerIndex, 0f);
    }
}
