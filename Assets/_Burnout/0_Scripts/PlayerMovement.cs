using UnityEngine;
using ScriptLibrary.Inputs;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Constraints")]
    [SerializeField] private Transform xConstraint1;
    [SerializeField] private Transform xConstraint2;

    [Header("Movement Settings")]
    [SerializeField] private float lerpSpeed = 1f;

    private InputHandler inputHandler;
    private Rigidbody2D rb;
    private Camera mainCamera;

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
    }

    void FixedUpdate()
    {
        if (inputHandler.ClickActionValue > 0f)
        {
            Vector2 worldTarget = mainCamera.ScreenToWorldPoint(inputHandler.PointerLocationValue);

            float clampedX = Mathf.Clamp(worldTarget.x, xConstraint1.position.x, xConstraint2.position.x);
            Vector2 targetPos = new Vector2(clampedX, rb.position.y);

            rb.MovePosition(Vector2.Lerp(rb.position, targetPos, lerpSpeed * Time.fixedDeltaTime));
        }
    }
}
