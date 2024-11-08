using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public static PlayerController Instance { get; private set; }

    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        rb = GetComponent<Rigidbody2D>();

        GameInputManager.Instance.OnInteractAction += GameInputManager_OnInteractAction;
    }

    // Event Listeners
    private void GameInputManager_OnInteractAction(object sender, System.EventArgs e) {
        Debug.Log("Interact");
    }

    private void Update() {
        PlayerInput();
    }

    private void FixedUpdate() {
        HandlePlayerMovement();
    }

    // Get Vector 2 Input from GameInputManager
    private void PlayerInput() {
        movement = GameInputManager.Instance.GetPlayerMovementNormalized();
    }

    // Handle Player movement base from vector 2 input from gameinputmanager
    private void HandlePlayerMovement() {
        if (rb != null) {
            rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
        } else {
            Debug.LogWarning("Rigidbody2D not found on player");
        }
    }
}
