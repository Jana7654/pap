using UnityEngine;

public class Player : Entity
{
    [Header("Movement Details")]
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 8;
    private bool canJump = true;
    private float xInput;

    override protected void Update()
    {
        base.Update();
        HandleInput();
    }

    private void HandleInput()
    {
        xInput = Input.GetAxis("Horizontal"); // a1) faz andar

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))// a2) saltar
            TryToJump();

        else if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.UpArrow) && rb.linearVelocity.y > 0)// a3) faz com que o jogador pule menos se soltar a tecla mais cedo
            StopJump();

        if (Input.GetKeyDown(KeyCode.Mouse0))
            HandleAttack();
    }

    private void StopJump()     //Para de saltar se soltar a tecla mais cedo
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
    }

    protected override void HandleMovement()
    {
        if (canMove)
            rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocity.y);
        else
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    private void TryToJump()   // Normalemente fica no script do jogador
    {
        if (isGrounded && canJump)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    public override void EnableMovement(bool enable)
    {
        base.EnableMovement(enable);
        canJump = enable;
    }

    protected override void Die()
    {
        base.Die();
        UI.instance.EnableGameOverUI();
    }
}
