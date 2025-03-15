using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyR : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public GameObject smallExplosionEffect;

    public AudioSource characterHitSound;
    public AudioSource destructionSound;

    private NavMeshAgent agent;
    private Transform player;

    public int damage = 10;
    public float attackCooldown = 2f;
    private bool canAttack = true;

    public GameObject projectilePrefab; 
    public Transform firePoint; 
    public float projectileSpeed = 10f;
    public float attackRange = 15f; 

    private void Start()
    {
        currentHealth = maxHealth;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogError("Player not found! Make sure your player has the 'Player' tag.");
        }

        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component not found on enemy!");
        }
    }

    private void Update()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance <= attackRange)
            {
                ShootProjectile(); 
            }
            else if (agent != null)
            {
                agent.SetDestination(player.position);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (characterHitSound != null)
        {
            characterHitSound.Play();
        }

        if (currentHealth <= 0)
        {
            DestroyObject();
        }
    }

    private void ShootProjectile()
    {
        if (!canAttack) return;

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = firePoint.forward * projectileSpeed;
        }

        StartCoroutine(AttackCooldown());
    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private void DestroyObject()
    {
        if (destructionSound != null)
        {
            destructionSound.Play();
        }

        if (smallExplosionEffect != null)
        {
            Instantiate(smallExplosionEffect, transform.position, transform.rotation);
        }

        Destroy(gameObject);
    }
}
