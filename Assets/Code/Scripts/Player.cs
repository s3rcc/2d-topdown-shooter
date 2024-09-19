using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] float moveSpeed = 6;

    Animator animator;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;

    int maxHealth = 100;
    int currentHealth;

    public bool dead = false;

    float moveHorizontal, moveVertical;
    Vector2 movement;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        currentHealth = maxHealth;
        healthText.text = maxHealth.ToString();
    }

    private void Update()
    {
        if (dead)
        {
            movement = Vector2.zero;
            animator.SetFloat("velocity", 0);
            return;
        }

        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        movement = new Vector2(moveHorizontal, moveVertical).normalized;

        animator.SetFloat("velocity", movement.magnitude);

        // Change player facing direction by fliping the x axis
        if(movement.x != 0)
        {
                spriteRenderer.flipX = movement.x < 0;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = movement * moveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();

        if (enemy != null)
        {
            Hit(20);
        }
    }

    void Hit(int damage)
    {
        if (dead) return;
        animator.SetTrigger("hit");
        currentHealth -= damage;
        healthText.text = Mathf.Clamp(currentHealth, 0, maxHealth).ToString();

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        if (dead) return;
        dead = true;
        animator.SetTrigger("die");
        Destroy(gameObject, 2f);
    }
}
