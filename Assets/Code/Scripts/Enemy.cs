using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int maxHealth = 50;
    [SerializeField] float speed = 2f;

    private int currentHealth;
    private SpriteRenderer spriteRenderer;
    private new Collider2D collider2D;

    Animator anim;
    Transform target;

    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;
        target = GameObject.Find("Player").transform;
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2D = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (isDead) return;

        if (target != null)
        {
            Vector3 direction = target.position - transform.position;
            direction.Normalize();

            transform.position += direction * speed * Time.deltaTime;

            if (direction.x != 0)
            {
                spriteRenderer.flipX = direction.x < 0;
            }

            anim.SetFloat("velocity", direction.magnitude);
        }
        else
        {
            anim.SetFloat("velocity", 0);
        }
    }

    public void Hit(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        anim.SetTrigger("hit");

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        spriteRenderer.sortingOrder = spriteRenderer.sortingOrder;

        if (collider2D != null)
        {
            collider2D.enabled = false;
        }

        anim.SetTrigger("die");
        GetComponent<LootBag>().InstantiateLoot(transform.position);
        Destroy(gameObject, 1f);
    }

    public bool IsDead()
    {
        return isDead;
    }
}
