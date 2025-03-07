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

    public AudioSource characterHitSound;
    public AudioSource destructionSound;

    private NavMeshAgent agent;
    private Transform player;

    public int damage = 10;
    public float attackCooldown = 1.5f;
    private bool canAttack = true;

    public float moveSpeed = 3.5f; 

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
            UnityEngine.Debug.LogError("Player not found! Make sure your player has the 'Player' tag.");
        }

        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            UnityEngine.Debug.LogError("NavMeshAgent component not found on enemy!");
        }
        else
        {
            agent.speed = moveSpeed; 
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


    private void OnCollisionEnter(Collision collision)
    {
        UnityEngine.Debug.Log("Enemy collided with: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Player") && canAttack)
        {
            UnityEngine.Debug.Log("Enemy hit Player!");

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
        if (spawner != null)
        {
            spawner.RemoveEnemy(gameObject);
        }

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
