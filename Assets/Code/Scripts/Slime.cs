using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    public float atkCooldown = 5f;
    public float atkRange = 5f;
    public GameObject projectile;
    public int numberOfProjectiles = 6;
    public float projectileSpeed = 5f;

    private bool isAttack = false;
    private bool isJumping = false;

    private Coroutine attackCycleCoroutine; // Store the coroutine reference

    protected override void Start()
    {
        base.Start();
        attackCycleCoroutine = StartCoroutine(JumpAttackCycle()); // Start the cycle for jump attacks
    }

    protected override void Update()
    {
        if (isDead) return;

        if (!isJumping)
        {
            base.Update();
        }
    }

    // Coroutine that handles the cycle of jump attacks
    private IEnumerator JumpAttackCycle()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(atkCooldown);

            if (target != null && Vector3.Distance(transform.position, target.position) <= atkRange && !isAttack)
            {
                StartCoroutine(JumpAttack()); // Perform the jump attack if the player is within range
            }
        }
    }

    private IEnumerator JumpAttack()
    {
        if (isDead) yield break; // Prevent the jump attack if the slime is dead

        isAttack = true;
        isJumping = true;
        anim.SetTrigger("attack");

        yield return new WaitForSeconds(0.5f);

        Shoot();
        isJumping = false;
        isAttack = false;
    }

    private void Shoot()
    {
        if (isDead) return; // Prevent shooting if the slime is dead

        float angleStep = 360f / numberOfProjectiles;
        float angle = 0f;

        for (int i = 0; i < numberOfProjectiles; i++)
        {
            // Calculate the direction for each projectile
            float projectileDirX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180);
            float projectileDirY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180);

            Vector3 projectileVector = new Vector3(projectileDirX, projectileDirY, 0);
            Vector3 projectileMoveDir = (projectileVector - transform.position).normalized;

            // Calculate the rotation angle for the projectile
            float projectileRotation = Mathf.Atan2(projectileMoveDir.y, projectileMoveDir.x) * Mathf.Rad2Deg;

            // Instantiate and set the direction and speed of the projectile
            GameObject proj = Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, projectileRotation));
            proj.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileMoveDir.x, projectileMoveDir.y) * projectileSpeed;

            Destroy(proj, 3); // Destroy projectile after 3 seconds

            angle += angleStep; // Increment the angle for the next projectile
        }
    }

    protected override void Die()
    {
        base.Die();
        if (attackCycleCoroutine != null) // Stop the attack cycle coroutine when the slime dies
        {
            StopCoroutine(attackCycleCoroutine);
        }
    }
}


