using UnityEngine;
using ScriptLibrary.Inputs;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : ScriptLibrary.Inputs.Vector2Input
{
    [Header("Movement Constraints")]
    [SerializeField] private Transform xConstraint1;
    [SerializeField] private Transform xConstraint2;

    [Header("Movement Settings")]
    public float moveSpeed = 1f;

    private InputHandler inputHandler;
    private Rigidbody2D rb;
    private Camera mainCamera;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        inputHandler = GetComponent<InputHandler>();
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;

        if (xConstraint1 == null || xConstraint2 == null)
            Debug.LogError("Player X Constraints are not assigned.");
        if (inputHandler == null)
            Debug.LogError("No InputHandler Assigned to Player");
        if (mainCamera == null)
            Debug.LogError("No Main Camera Found.");

        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
            Debug.LogError("No SpriteRenderer found on Player.");
    }

    void FixedUpdate()
    {
        float inputX = VectorInput.x;
        float targetX = rb.position.x + (inputX * moveSpeed * Time.fixedDeltaTime);
        float clampedX = Mathf.Clamp(targetX, xConstraint1.position.x, xConstraint2.position.x);

        if (inputX > 0.01f)
            spriteRenderer.flipX = false;
        else if (inputX < -0.01f)
            spriteRenderer.flipX = true;

        rb.MovePosition(new Vector2(clampedX, rb.position.y));
    }
}
