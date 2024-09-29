using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy
{
    private bool isAttack = false;
    private float attackDuration = 1f;

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (!isDead && target != null)
        {
            if (!isAttack)
            {
                MoveTowardsPlayer();
            }
        }
    }

    protected override void MoveTowardsPlayer()
    {
        // Calculate direction to the player
        Vector3 direction = (target.position - transform.position);
        direction.Normalize();

        float distance = Vector3.Distance(transform.position, target.position);
        if (distance < 1.5f && !isAttack)
        {
            StartCoroutine(Attack());
        }
        else
        {
            // Move the enemy towards the player
            transform.position += direction * speed * Time.deltaTime;
            if (direction.x != 0)
            {
                spriteRenderer.flipX = direction.x < 0;
            }
            anim.SetFloat("velocity", direction.magnitude);
        }

    }


    private IEnumerator Attack()
    {
        isAttack = true;
        anim.SetTrigger("attack");

        // Inflict damage to the player
        if (target != null)
        {
            Player player = target.GetComponent<Player>();
            if (player != null)
            {
                player.Hit((int)damage); // Apply bite damage
            }
        }

        yield return new WaitForSeconds(attackDuration); // Wait for bite animation

        isAttack = false;
    }
}
