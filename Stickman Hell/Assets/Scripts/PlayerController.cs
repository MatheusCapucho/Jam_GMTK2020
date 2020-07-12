using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   
    private float movementInput;
    private float speed = 5f;
    private float jumpForce = 10f;
    private float dashForce;
    private Rigidbody2D rb;
    public Transform attackPoint; 
    public Vector2 playerPos;
    bool isGrounded = true;
    bool trocar;
    int weaponIndex = 0;
    float weaponRange = 10f;
    bool isFacingRight;

    private void Awake()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        trocar = true;
    }
    private void start()
    {
        isFacingRight = true;
        
    }

    void Update()
    {
        movementInput = Input.GetAxisRaw("Horizontal");

        //flip
        if (isFacingRight && movementInput * speed < 0)
        {
            isFacingRight = false;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
        if (!isFacingRight && movementInput * speed > 0)
        {
            isFacingRight = true;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

        //attack
        if (Input.GetKeyDown(KeyCode.F)) 
        {
            Attack();
        }

        //pular ou dash - trocando
        if (trocar)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                rb.velocity = Vector2.up * jumpForce;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {    
                StartCoroutine(Dash());     
            }
        } else
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                StartCoroutine(Dash()); 
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.velocity = Vector2.up * jumpForce;
            }
        }

    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(movementInput * (speed + dashForce), rb.velocity.y);
    }

    private void Attack()
    {
        Physics2D.Raycast(playerPos, Vector2.right, weaponRange, 8);
        //animacao
        //mudar o colider
        //diferentes armas (switch-case)
        //if(enemyKilled) {  trocar = !trocar; speed = -speed; dashForce = -dashForce; }
    }
    IEnumerator Dash()
    {
        dashForce = 10f;
        yield return new WaitForSeconds(0.2f);
        dashForce = 0f;
    }

}
