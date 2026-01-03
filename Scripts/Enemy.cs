using UnityEngine;

public class Enemy : Entity
{
    [Header("Movement Details")]
    [SerializeField] protected float moveSpeed = 5f;

    private bool playerDetected;

    protected override void Update()
    {
        base.Update();
         
        HandleAttack();
    }

    protected override void HandleAttack()
    {
        if (playerDetected)
            anime.SetTrigger("attack");
    }

    protected override void HandleMovement()
    {
        if (canMove)
            rb.linearVelocity = new Vector2(facingD * moveSpeed, rb.linearVelocity.y);
        else
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    protected override void HandleCollision()
    {
        base.HandleCollision();
        playerDetected = Physics2D.OverlapCircle(attackPoint.position, attackRadius, WhatIsTarget);
    }

    protected override void Die()
    {         
        base.Die();
        UI.instance.AddKillCount();
    }

}
