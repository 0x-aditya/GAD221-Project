using UnityEngine;
using ScriptLibrary.Inputs;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Constraints")]
    [SerializeField] private Transform xConstraint1;
    [SerializeField] private Transform xConstraint2;

    [Header("Movement Settings")]
    public float lerpSpeed = 1f;

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
        if (inputHandler.ClickActionValue > 0f)
        {
            Vector2 worldTarget = mainCamera.ScreenToWorldPoint(inputHandler.PointerLocationValue);

            float clampedX = Mathf.Clamp(worldTarget.x, xConstraint1.position.x, xConstraint2.position.x);
            Vector2 targetPos = new Vector2(clampedX, rb.position.y);

            // 👇 FLIP LOGIC
            float direction = targetPos.x - rb.position.x;

            if (direction > 0.01f)
                spriteRenderer.flipX = false; // facing right
            else if (direction < -0.01f)
                spriteRenderer.flipX = true;  // facing left

            rb.MovePosition(Vector2.Lerp(rb.position, targetPos, lerpSpeed * Time.fixedDeltaTime));
        }
    }
}
