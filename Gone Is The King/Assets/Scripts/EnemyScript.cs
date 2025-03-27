using UnityEngine;
using System.Collections;

public class Enemy : NonPlayerCharacterScript
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private Collider2D bodyCollider;  

    [Header("Stats")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [Header("Jump Attack Settings")]
    [SerializeField] private float jumpDuration = 0.5f;         
    [SerializeField] private int jumpAttackDamage = 10;
    private bool isJumpAttacking;

    private bool isDead;

    private void Start()
    {
        currentHealth = maxHealth;

        GameObject protagonist = GameObject.FindGameObjectWithTag("Protagonist");
        if (protagonist != null)
        {
            StartJumpAttack(protagonist.transform.position);
        }
        else
        {
            Debug.LogError("No GameObject tagged 'Protagonist' found in the scene!");
        }
    }


    public override void MoveTo(int[] position)
    {
        // Convert the provided int array into a target position.
        Vector2 targetPos = new Vector2(position[0], position[1]);
        
        // If not already in a jump (and not dead), initiate a jump toward the target.
        if (!isJumpAttacking && !isDead)
        {
            StartJumpAttack(targetPos);
        }
    }

    private void StartJumpAttack(Vector2 targetPos)
    {
        isJumpAttacking = true;
        // Trigger the jump animation (ensure your Animator has a "Jump" trigger)
        animator.SetTrigger("Jump");

        // Stop any existing routines and start the jump routine toward the target
        StopAllCoroutines();
        StartCoroutine(JumpAttackRoutine(targetPos));
    }

    private IEnumerator JumpAttackRoutine(Vector2 targetPos)
    {
        float timer = 0f;
        Vector2 startPos = transform.position;

        // Lerp the enemy's position from start to target over jumpDuration
        // Adding an arc effect for a natural jump using a sine wave for height.
        while (timer < jumpDuration)
        {
            timer += Time.deltaTime;
            float t = timer / jumpDuration;
            float height = Mathf.Sin(t * Mathf.PI) * 1f; // Adjust "1f" for desired jump height
            Vector2 currentPos = Vector2.Lerp(startPos, targetPos, t);
            transform.position = new Vector3(currentPos.x, currentPos.y + height, transform.position.z);
            yield return null;
        }

        isJumpAttacking = false;
    }

    // This method is called if the enemy's collider touches something during the jump
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isJumpAttacking) return;
        if (other.CompareTag("Protagonist"))
        {
            // Call Damage Method Here
            Debug.Log("Enemy jumped and hit the Protagonist!");
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        StopAllCoroutines();
        animator.SetTrigger("Death");
    }

    public void OnDeathAnimationEnd()
    {
        Destroy(gameObject);
    }
}
