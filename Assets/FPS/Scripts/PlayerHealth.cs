using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public Text HPText;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHPText();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player HP: " + currentHealth);
        UpdateHPText();

        if (currentHealth <= 0) Die();
    }

    public void Heal(int amount) 
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log("Healed! Player HP: " + currentHealth);
        UpdateHPText();
    }

    private void Die()
    {
        Debug.Log("Player Died!");
        SceneManager.LoadScene("GameOver");
    }

    private void UpdateHPText()
    {
        if (HPText) HPText.text = "HP: " + currentHealth;
    }
}
