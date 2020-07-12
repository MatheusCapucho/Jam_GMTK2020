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
    public int weaponIndex;
    public Transform attackPoint; 
    public float weaponRange = 10f;
    bool isFacingRight;
    public float attackRate = 1f;
    public float nextAttack = 0f;
    public BoxCollider2D col;
    public Animator animator;
    public LayerMask groundMask;
    public LayerMask enemies;
    int weaponDamage = 60;
    int currentWeaponDamage;
    int health = 2;
    public int currrentHealth;

    private Transform espada0;
    private Transform espada1;
    private Transform lanca2;


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
        espada0 = transform.Find("Espada0anim");
        espada1 = transform.Find("Espada1anim");
        lanca2 = transform.Find("Lanca2anim");

        weaponIndex = 0;
        trocar = auxtrocar;
        isFacingRight = true;
        
    }

    void Update()
    {
        movementInput = Input.GetAxis("Horizontal");

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
                animator.SetTrigger("Jump");
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
                animator.SetTrigger("Jump");
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
        if (weaponIndex == 0)
        {
            animator.SetTrigger("Attack0");
            
        } else if (weaponIndex == 1)
        {
            animator.SetTrigger("Attack0");
        } else if (weaponIndex == 2)
        {
            animator.SetTrigger("Attack2");
        }
        else if (weaponIndex == 3)
        {
            animator.SetTrigger("Attack3");
        }

        Collider2D[] hit = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, enemies);
        foreach( Collider2D enemy in hit)
        {
            enemy.GetComponent<EnemyController>().TakeDamage(currentWeaponDamage);
        }
    }

    public void ChangeWeapon0()
    {
        StartCoroutine(ChangeWeapon());
    }
    IEnumerator ChangeWeapon()
    {
        if (weaponIndex == 0)
        {
            yield return new WaitForSeconds (1f);
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(2).gameObject.SetActive(false);
            transform.GetChild(3).gameObject.SetActive(false);
            
        }
        if (weaponIndex == 1)
        {
            yield return new WaitForSeconds(1f);
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(true);
            transform.GetChild(3).gameObject.SetActive(false);
        }
        if (weaponIndex == 2)
        {
            yield return new WaitForSeconds(1f);
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(false);
            transform.GetChild(3).gameObject.SetActive(true);
        }
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
