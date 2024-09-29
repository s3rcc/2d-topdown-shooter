using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Warg : Enemy
{
    [SerializeField] float acceleration = 0.1f;
    [SerializeField] float maxSpeed = 4f;
    [SerializeField] float biteDamage = 20f;
    [SerializeField] float slowDownFactor = 0.5f;
    private bool isBiting = false;
    public float biteDuration = 1f;

    private float currentSpeed;

    protected override void Update()
    {
        base.Update(); // Call the base Update for shared functionality

        if (!isDead && target != null)
        {
            if (!isBiting)
            {
                AccelerateTowardsPlayer();
            }
        }
    }

    protected override void MoveTowardsPlayer()
    {
        Vector3 direction = (target.position - transform.position);
        direction.Normalize();

        float distance = Vector3.Distance(transform.position, target.position);

        // Start biting if within a certain range
        if (distance < 1.5f && !isBiting)
        {
            StartCoroutine(Attack());
        }
        else
        {
            transform.position += direction * speed * Time.deltaTime;
            if (direction.x != 0)
            {
                spriteRenderer.flipX = direction.x < 0;
            }
            anim.SetFloat("velocity", direction.magnitude);
        }
    }

    private void AccelerateTowardsPlayer()
    {
        // Gradually increase speed towards maxSpeed
        currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.deltaTime, maxSpeed);
        MoveTowardsPlayer();
    }

    private IEnumerator Attack()
    {
        isBiting = true;
        currentSpeed = 0;  // Stop moving while biting
        anim.SetTrigger("attack");

        // Inflict damage to the player
        if (target != null)
        {
            Player player = target.GetComponent<Player>();
            if (player != null)
            {
                player.Hit((int)biteDamage); // Apply bite damage
            }
        }

        yield return new WaitForSeconds(biteDuration); // Wait for bite animation

        isBiting = false;

        // Slow down after bite
        StartCoroutine(SlowDownAfterBite());
    }

    private IEnumerator SlowDownAfterBite()
    {
        currentSpeed = maxSpeed * slowDownFactor;

        // Gradually restore the speed to normal
        while (currentSpeed < speed)
        {
            currentSpeed += acceleration * Time.deltaTime;
            yield return null;
        }

        currentSpeed = speed; // Restore to normal speed
    }

}
