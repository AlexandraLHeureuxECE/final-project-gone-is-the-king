using UnityEngine;

public class FistAnimatorController : MonoBehaviour
{
    // Reference to the Animator component
    private Animator fistAnimator;
    
    // Offset distance for the fist movement
    public float punchOffset = 1f;

    // Duration for the fist to stay in the offset position
    public float punchDuration = 0.2f;

    // Damage dealt to enemies when the fist hits
    public int damageAmount = 10;

    // Original position of the fist
    private Vector3 originalPosition;

    void Start()
    {
        // Get the Animator component attached to the GameObject
        fistAnimator = GetComponent<Animator>();

        // Store the original position of the fist
        originalPosition = transform.localPosition;

        // Check if the Animator component is found
        if (fistAnimator == null)
        {
            Debug.LogError("Animator component not found on the GameObject.");
        }
    }

    void Update()
    {
        // Check for mouse down (left-click)
        if (Input.GetMouseButtonDown(0))
        {
            // Trigger the animation using the "Punch" trigger parameter
            fistAnimator.SetTrigger("Punch");

            // Move the fist forward
            MoveFist();
        }
    }

    private void MoveFist()
    {
        // Move the fist in the forward direction (if using 2D, consider transform.right)
        transform.localPosition += transform.forward * punchOffset;

        // Schedule returning the fist to its original position after punchDuration
        Invoke(nameof(ResetFistPosition), punchDuration);
    }

    private void ResetFistPosition()
    {
        // Reset the fist to its original position
        transform.localPosition = originalPosition;
    }

    // This method uses 2D collision detection to determine when the punch hits an enemy.
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collided object has the tag "Enemy"
        if (other.CompareTag("Enemy"))
        {
            // Get the Enemy component from the collided object
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                // When the punch hits, call TakeDamage on the enemy.
                // In your Enemy script, TakeDamage triggers the Hurt animation.
                enemy.TakeDamage(damageAmount);
                Debug.Log("Fist hit enemy! Enemy is hurt.");
            }
        }
    }
}
