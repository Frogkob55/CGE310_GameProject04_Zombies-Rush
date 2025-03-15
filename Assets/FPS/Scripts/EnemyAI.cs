using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum MaterialType
{
    Enemy1,
    Enemy2,
    Enemy3
}

public class EnemyAI : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public MaterialType materialType;
    public GameObject smallExplosionEffect;

    private AudioSource audioSource;
    public AudioClip hitSoundClip;
    public AudioClip destructionSoundClip;

    private NavMeshAgent agent;
    private Transform player;

    public int damage = 10;
    public float attackCooldown = 1.5f;
    private bool canAttack = true;

    public float moveSpeed = 3.5f;

    private bool isDying = false; 

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
        else
        {
            agent.speed = moveSpeed;
        }

        
        if (GetComponent<AudioSource>() == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (player != null && agent != null)
        {
            agent.SetDestination(player.position);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDying) return; 

        currentHealth -= damage;

        
        if (hitSoundClip != null && audioSource != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(hitSoundClip);
        }

        if (currentHealth <= 0)
        {
            DestroyObject();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Enemy collided with: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Player") && canAttack)
        {
            Debug.Log("Enemy hit Player!");

            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                StartCoroutine(AttackPlayer(playerHealth));
            }
        }
    }

    private IEnumerator AttackPlayer(PlayerHealth playerHealth)
    {
        canAttack = false;
        agent.isStopped = true;

        playerHealth.TakeDamage(damage);

        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;
        agent.isStopped = false;
    }

    private EnemySpawner spawner;

    public void SetSpawner(EnemySpawner _spawner)
    {
        spawner = _spawner;
    }

    private void DestroyObject()
    {
        if (isDying) return; 
        isDying = true;

        if (spawner != null)
        {
            spawner.RemoveEnemy(gameObject);
        }

        
        if (smallExplosionEffect != null)
        {
            Instantiate(smallExplosionEffect, transform.position, transform.rotation);
        }

        
        if (destructionSoundClip != null && audioSource != null)
        {
            AudioSource.PlayClipAtPoint(destructionSoundClip, transform.position);
        }

        
        Destroy(gameObject);
    }
}
