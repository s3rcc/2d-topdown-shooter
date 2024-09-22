using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] GameObject muzzle;
    [SerializeField] Transform gunPivot;
    [SerializeField] Transform muzzlePosition;
    [SerializeField] GameObject projectile;

    [Header("Config")]
    //[SerializeField] float rotationSpeed = 5f;
    [SerializeField] float fireDistance = 10;
    [SerializeField] float fireRate = 0.1f;

    Player playerScript;
    Transform player;
    Vector2 offset;

    private SpriteRenderer gunSpriteRenderer;

    private float timeSinceLastShot = 0f;
    Transform closestEnemy;
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        timeSinceLastShot = fireRate;
        player = GameObject.Find("Player").transform;
        playerScript = player.GetComponent<Player>();
        gunSpriteRenderer = GetComponent<SpriteRenderer>();
        SetOffset(new Vector2(0,0));
    }

    private void Update()
    {
        if (playerScript != null && playerScript.dead)
        {
            gameObject.SetActive(false); // Deactivate the gun when the player dies
            return;
        }
        transform.position = (Vector2)player.position + offset;
        //FindClosestEnemy();
        //AimAtEnemy();
        Aim();
        Shooting();
    }

    void FindClosestEnemy()
    {
        closestEnemy = null;
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        float closestDistance = fireDistance;

        foreach (Enemy enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance <= closestDistance && !enemy.IsDead())
            {
                closestEnemy = enemy.transform;
                closestDistance = distance;
            }
        }
    }

    void Aim()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - gunPivot.position;
        direction.Normalize();

        // Calculate angle in degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate the gun to face the mouse
        gunPivot.rotation = Quaternion.Euler(0, 0, angle);

        // Flip the gun if necessary (based on aiming direction)
        if (direction.x < 0) // Aiming to the left
        {
            gunPivot.localScale = new Vector3(1, -1, 1); // Flip vertically
        }
        else // Aiming to the right
        {
            gunPivot.localScale = new Vector3(1, 1, 1); // Default orientation
        }

        timeSinceLastShot += Time.deltaTime;
        if (timeSinceLastShot >= fireRate)
        {
            Shoot();
            timeSinceLastShot = 0;
        }
    }

    void AimAtEnemy()
    {
        if (closestEnemy != null)
        {
            Vector3 direction = closestEnemy.position - transform.position;
            direction.Normalize();

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if (direction.x < 0)
            {
                gunSpriteRenderer.flipY = true;  // Flip the gun sprite vertically
            }
            else
            {
                gunSpriteRenderer.flipY = false;
            }


            transform.rotation = Quaternion.Euler(0, 0, angle);
            transform.position = (Vector2)player.position + offset;
        }
    }

    void Shooting()
    {
        if (closestEnemy == null) return;

        timeSinceLastShot += Time.deltaTime;
        if(timeSinceLastShot >= fireRate)
        {
            Shoot();
            timeSinceLastShot = 0;
        }
    }

    void Shoot()
    {
        // Instantiate muzzle flash at the muzzle position
        var muzzleGo = Instantiate(muzzle, muzzlePosition.position, gunPivot.rotation);
        muzzleGo.transform.SetParent(gunPivot);
        Destroy(muzzleGo, 0.05f); // Destroy muzzle flash after 0.05 seconds

        // Instantiate projectile at the muzzle position and rotate based on gunPivot's rotation
        var projectileGo = Instantiate(projectile, muzzlePosition.position, gunPivot.rotation);
        Destroy(projectileGo, 3); // Destroy projectile after 3 seconds
    }
    
    public void SetOffset(Vector2 o)
    {
        offset = o;
    }
}
