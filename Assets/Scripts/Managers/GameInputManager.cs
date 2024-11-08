using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInputManager : MonoBehaviour {

    public static GameInputManager Instance { get; private set; }

    public event EventHandler OnInteractAction;

    private PlayerInputActions playerInputActions;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }

        playerInputActions = new PlayerInputActions();
    }

    // Create Event action
    private void Start() {
        playerInputActions.Player.Interaction.performed += ctx => OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    private void OnEnable() {
        playerInputActions.Enable();

    }

    private void OnDisable() {
        playerInputActions.Disable();
    }

    public Vector2 GetPlayerMovementNormalized() {
        return playerInputActions.Player.Movement.ReadValue<Vector2>().normalized;
    }
}
