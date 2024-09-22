using Assets.Code.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int maxHealth = 50;
    [SerializeField] float speed = 2f;
    [SerializeField] GameObject exp1;
    [SerializeField] GameObject exp2;
    [SerializeField] GameObject exp3;



    private int currentHealth;
    private SpriteRenderer spriteRenderer;
    private new Collider2D collider2D;

    Animator anim;
    Transform target;

    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;
        target = Follow();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2D = GetComponent<Collider2D>();
    }

    private Transform Follow()
    {
        var target = GameObject.Find("Player").transform;
        if (target != null)
            return target;
        else
            return null;
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
                spriteRenderer.flipX = direction.x < 0;

            anim.SetFloat("velocity", direction.magnitude);
        }
        else
            anim.SetFloat("velocity", 0);
    }

    public void Hit(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        anim.SetTrigger("hit");

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        spriteRenderer.sortingOrder = spriteRenderer.sortingOrder;

        if (collider2D != null)
            collider2D.enabled = false;

        anim.SetTrigger("die");
        DropExp(); // Drop EXP when the enemy dies
        Destroy(gameObject, 1f);
    }

    void DropExp()
    {
        int expAmount = Exp.GetExpDrop();

        var expPrefab = GetExpPrefabByAmount(expAmount);

        var expDrop = Instantiate(expPrefab, transform.position, Quaternion.identity);

        expDrop.GetComponent<Exp>().Initialize(expAmount); ;

        if (expDrop.GetComponent<Exp>() == null)
            Debug.LogError($"Get Null Component");

        ExpManager.Instance.AddExp(expAmount); // Add EXP to the manager

        Debug.Log($"Dropped {expAmount} EXP.");
    }

    GameObject GetExpPrefabByAmount(int expAmount)
    {
        if (expAmount == 1)
            return exp1;
        else if (expAmount == 3)
            return exp2;
        else 
            return exp3;
    }

    public bool IsDead()
    {
        return isDead;
    }
}
