using UnityEngine;

public class PlayerDeathController : MonoBehaviour
{
    public GameObject deathUIContainer;
    private PlayerHealth playerHealth; // Modificăm tipul aici la PlayerHealth

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>(); // Asigurăm că obținem PlayerHealth
            if (playerHealth == null)
            {
                Debug.LogError("PlayerHealth component not found on the player.");
            }
        }
        else
        {
            Debug.LogError("Player GameObject not found.");
        }
    }

    public void ShowDeathUI()
    {
        if (deathUIContainer != null)
        {
            deathUIContainer.SetActive(true);
        }
    }
}