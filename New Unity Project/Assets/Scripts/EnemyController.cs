using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public GameObject player;
    public SpriteRenderer sr;
    private Rigidbody2D rb; 
    private float speed = 10f;
    int health = 50;
    public int currentHealth = 50;

    void Start()
    {
        currentHealth = health;
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        rb.velocity = new Vector2(-speed, rb.velocity.y);
    }
    private void OnCollisionEnter2D (Collision2D other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
            speed = -speed;
            if (other.gameObject.CompareTag("Player"))
            {
                player.GetComponent<PlayerController>().currrentHealth--;
            }
        }
    }

    public void TakeDamage(int weaponDamage)
    {
        currentHealth -= weaponDamage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        player.GetComponent<PlayerController>().speed *= -1;
        player.GetComponent<PlayerController>().trocar *= -1;
        Destroy(this.gameObject);
        //morra

    }

}
