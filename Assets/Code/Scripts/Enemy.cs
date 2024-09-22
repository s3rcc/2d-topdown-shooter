using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int maxHealth = 50;
    public int damage = 10;
    public float speed = 2f;
    
    

    protected int currentHealth;
    protected SpriteRenderer spriteRenderer;
    protected new Collider2D collider2D;

    protected Animator anim;
    protected Transform target;

    protected bool isDead = false;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        target = FindPlayer();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2D = GetComponent<Collider2D>();
    }

    protected Transform FindPlayer()
    {
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            return player.transform;
        }
        return null;
    }

    protected virtual void Update()
    {
        if (isDead) return;

        if (!isDead && target != null)
        {
                MoveTowardsPlayer();
        }
        else
        {
            anim.SetFloat("velocity", 0);
        }
    }

    protected virtual void MoveTowardsPlayer()
    {
        // Calculate direction to the player
        Vector3 direction = (target.position - transform.position);
        direction.Normalize();
            // Move the enemy towards the player
            transform.position += direction * speed * Time.deltaTime;
            if (direction.x != 0)
            {
                spriteRenderer.flipX = direction.x < 0;
            }
            anim.SetFloat("velocity", direction.magnitude);
    }


    public void Hit(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        anim.SetTrigger("hit");

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    

    protected virtual void Die()
    {
        if (isDead) return;
        isDead = true;

        // Keep the enemy on the same sorting layer during death animation
        spriteRenderer.sortingOrder = spriteRenderer.sortingOrder;

        // Disable collider to prevent interaction after death
        if (collider2D != null)
        {
            collider2D.enabled = false;
        }

        // Trigger death animation
        anim.SetTrigger("die");
        Destroy(gameObject, 1f);
    }

    public bool IsDead()
    {
        return isDead;
    }
}
