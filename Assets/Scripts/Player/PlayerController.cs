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
    }

    private void Update() {
        PlayerInput();
    }

    private void FixedUpdate() {
        HandlePlayerMovement();
    }

    private void PlayerInput() {
        movement = GameInputManager.Instance.GetPlayerMovementNormalized();
    }

    private void HandlePlayerMovement() {
        if (rb != null) {
            rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
        } else {
            Debug.LogWarning("Rigidbody2D not found on player");
        }
    }
}
