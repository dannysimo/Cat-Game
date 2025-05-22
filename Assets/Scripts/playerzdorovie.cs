using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10;
    private int currentHealth;

    public TMP_Text healthText; // Referință la Text Mesh Pro din Canvas

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player took damage. Current health: " + currentHealth);

        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player died!");
        // Adăugați aici logica pentru ce se întâmplă când jucătorul moare
        // De exemplu, puteți reîncărca scena, afișa un ecran de game over, etc.
        FindObjectOfType<PlayerDeathController>()?.ShowDeathUI(); // Apelez direct metoda de afișare a UI-ului de moarte
    }

    // Optional: Adăugați o metodă pentru a vindeca jucătorul
    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        Debug.Log("Player healed. Current health: " + currentHealth);
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        healthText.text = currentHealth.ToString(); // Convertim întregul în string
    }
}