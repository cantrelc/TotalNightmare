using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float health;
    public float maxHealth;

    public SpriteRenderer spriteRenderer;
    public Sprite spriteDead;

    public PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        //if player makes contact with enemy, kill them and disable movement
        if (health <= 0)
        {
            spriteRenderer.sprite = spriteDead;
            playerMovement.enabled = false;
            PlayerDied();
        }
    }
    private void PlayerDied()
    {
        LevelManager.instance.GameOver();
        gameObject.SetActive(false);
    }
}
