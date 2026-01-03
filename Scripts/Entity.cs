using System;
using System.Collections;
using UnityEngine;

//Notes:
// -> && significa "E" lógico
// -> || significa "OU" lógico
// -> != significa "diferente de"   posso escrever if (!canMove)  = a if(canMove == false)
// -> == significa "igual a"   posso escrever if (canMove)  = a if(canMove == true)
// public Collider2D[] colliders;// tamanho fixo ao criar // Não podemos tirar ou colocar itens enquanto roda // Mais rápido
//public List<Collider2D> example; // podemos mudar o tamanho // Podemos por ou tirar itens enquanto roda // Mais Lento

public class Entity : MonoBehaviour
{
    protected Animator anime;
    protected Rigidbody2D rb;
    protected Collider2D col;
    protected SpriteRenderer sr;

    [Header("Health")]
    [SerializeField] private int maxHealth = 1;
    [SerializeField] private int currentHealth;
    [SerializeField] private Material damageMaterial;
    [SerializeField] private float damageFeedbackDurations = 0.1f;
    private Coroutine damageFeedbackCoroutine;

    [Header("Attack Details")]
    [SerializeField] protected float attackRadius;
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected LayerMask WhatIsTarget;

    [Header("Collision Details")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField]public LayerMask whatIsGround;
    protected bool isGrounded;

    //Fancing direction details
    protected int facingD = 1; //1 = right, -1 = left
    protected bool canMove = true;
    protected bool facingRight = true;


    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anime = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();

        currentHealth = maxHealth;
    }

    protected virtual void Update()
    {
        HandleCollision();
        HandleMovement();
        HandleAnimations();
        HandleFlip();

    }

    public void DamageTargets()
    {
        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, WhatIsTarget);


        foreach  (Collider2D enemy in enemyColliders)
        {
            Entity entityTarget = enemy.GetComponent<Entity>();
            entityTarget.TakeDamage();
            

        }
    }

    private void TakeDamage()
    {
        currentHealth--;
        PlayDamageFeedback();

        if (currentHealth <= 0)
            Die();

    }

    private void PlayDamageFeedback()
    {
        if (damageFeedbackCoroutine != null)
            StopCoroutine(damageFeedbackCoroutine);


        StartCoroutine(DamageFeedbackCo());
    }

    private IEnumerator DamageFeedbackCo()
    {
        Material originalMat = sr.material;

        sr.material = damageMaterial;

        yield return new WaitForSeconds(damageFeedbackDurations);

        sr.material = originalMat;
    }

    protected virtual void Die()
    {
        anime.enabled = false;
        col.enabled = false;

        rb.gravityScale = 12;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 15);

        Destroy(gameObject, 3);
    }

    public virtual void EnableMovement(bool enable)
    {
        canMove = enable;
    }

    protected void HandleAnimations()
    {
        
        anime.SetFloat("xVelocity", rb.linearVelocity.x);
        anime.SetFloat("yVelocity", rb.linearVelocity.y);
        anime.SetBool("IsGrounded", isGrounded);
    }

    

    protected virtual void HandleAttack()
    {
        if (isGrounded)
            anime.SetTrigger("attack");
    }

    

    

    protected virtual void HandleMovement()     // = a escrever 'canMove == true'
    { 
    }

    protected virtual void HandleCollision()      // verifica se está no chão de um ponto especifico para baixo e verefica se colide com o layer do chão a uma certa distancia
    {                                           // funciona sem desenhar o raycast, mas é util para visualização
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    

    protected virtual void HandleFlip()
    {
        if (rb.linearVelocity.x > 0 && facingRight == false)
            Flip();
        else if (rb.linearVelocity.x < 0 && facingRight == true)
            Flip();
    }

    public void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
        facingD *= -1;
    }

    private void OnDrawGizmos() // desenha a linha do raycast no editor apenas serve para visualização
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance));

        if (attackPoint!= null)
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}

