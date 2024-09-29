using Assets.Code.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI expText;
    [SerializeField] float moveSpeed = 6;
    [SerializeField] int maxHealth = 100;

    Animator animator;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;

    int currentHealth;

    int currentExp = 0;
    int currentLevel = 1;

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

        UpdateExpText();
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

        if (movement.x != 0)
            spriteRenderer.flipX = movement.x < 0;
    }

    private void FixedUpdate()
    {
        rb.velocity = movement * moveSpeed;
    }

    public void Hit(int damage)
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

    public void CollectExp(int amount)
    {
        currentExp += amount;
        CheckLevelUp();
    }

    void UpdateExpText()
    {
        expText.text = $"EXP: {currentExp}";
    }

    void CheckLevelUp()
    {
        int expRequired = currentLevel * 10;
        if (currentExp >= expRequired)
        {
            currentLevel++;
            currentExp = 0;
            maxHealth += 1;
            currentHealth +=1;
            healthText.text = currentHealth.ToString();
            Debug.Log($"Level up! New level: {currentLevel}");
        }

        UpdateExpText();
    }
}
