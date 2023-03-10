using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HeroHealth : MonoBehaviour
{
    private int health = 0;
    public int maxHealth = 40;
    public Image healthBar;

    private void Start()
    {
        health = maxHealth;
    }

    public void UpdateHealth(int mod)
    {
        health += mod;

        if (health > maxHealth)
        {
            health = maxHealth;
        }
        else if (health <= 0)
        {
            Destroy(gameObject);
            SceneManager.LoadScene("Menu");
        }
        else
        {
            healthBar.GetComponent<ChangeImage>().ChangeSprite(health / 10 - 1);
        }
    }
}
