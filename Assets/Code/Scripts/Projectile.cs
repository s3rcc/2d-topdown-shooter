using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float speed = 12f;
    private void FixedUpdate()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("PT collider trigger");
        var enemy = collision.gameObject.GetComponent<Enemy>();
        if(enemy != null)
        {
            //Debug.Log("PT collider enemy");
            Destroy(gameObject);
            enemy.Hit(25);
        }
    }
}
