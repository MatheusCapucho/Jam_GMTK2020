using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   
    private float movementInput;
    public float speed = 5f;
    public float jumpForce = 10f;
    public float dashForce;
    private Rigidbody2D rb;
    public Vector2 playerPos;
    private int auxtrocar = 1;
    public int trocar;
   // int weaponIndex = 0;
    public Transform attackPoint; 
    public float weaponRange = 10f;
    bool isFacingRight;
    public float attackRate = 1f;
    public float nextAttack = 0f;
    public BoxCollider2D col;
    public Animator animator;
    public LayerMask groundMask;
    public LayerMask enemies;
    int weaponDamage = 25;
    int currentWeaponDamage;
    int health = 2;
    public int currrentHealth;

    private void Awake()
    {
        currrentHealth = health;
        currentWeaponDamage = weaponDamage;
        animator = this.gameObject.GetComponent<Animator>();
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        col = this.gameObject.GetComponent<BoxCollider2D>();
    }
    private void Start()
    {
        trocar = auxtrocar;
        isFacingRight = true;
        
    }

    void Update()
    {
        movementInput = Input.GetAxis("Horizontal");

        //flip
        if (isFacingRight && movementInput * speed < 0)
        {
            animator.SetBool("Running", true);
            isFacingRight = false;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

        if (!isFacingRight && movementInput * speed > 0)
        {
            animator.SetBool("Running", true);
            isFacingRight = true;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

        if (movementInput == 0)
        {
            animator.SetBool("Running", false);
        }

        if (Time.time > nextAttack)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Attack();
                nextAttack = Time.time + attackRate;
            }
        }
        //pular ou dash - trocando
        if (trocar > 0)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && IsGrounded())
            {
                //animacao de pulo
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

            if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
            {
                rb.velocity = Vector2.up * jumpForce;
                //animacao de pulo
            }
        }

    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(movementInput * (speed + dashForce), rb.velocity.y);
    }

     private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(col.bounds.center, Vector2.down, col.bounds.extents.y + 0.1f, groundMask);
        return hit.collider != null;
    }

    private void Attack()
    {
        Debug.Log("Ataquei");
        animator.SetTrigger("Attack");

        Collider2D[] hit = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, enemies);//animacao
        foreach( Collider2D enemy in hit)
        {
            enemy.GetComponent<EnemyController>().TakeDamage(currentWeaponDamage);
        }
        //mudar o colider
        //diferentes armas (switch-case)
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, weaponRange);
    }
    IEnumerator Dash()
    {
        dashForce = 50f;
        yield return new WaitForSeconds(0.2f);
        dashForce = 0f;
    }

}
