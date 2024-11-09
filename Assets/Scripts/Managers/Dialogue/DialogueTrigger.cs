using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    private event EventHandler OnPlayerInRange;

    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    private bool playerInRange;

    private void Awake() {
        playerInRange = false;
        visualCue.SetActive(false);
    }

    private void Start() {
        OnPlayerInRange += DialogueTrigger_OnPlayerInRange;

        GameInputManager.Instance.OnInteractAction += GameInputManager_OnInteractAction;
        DialogueManager.Instance.OnDialogueFinished += DialogueManager_OnDialogueFinished; ;
    }

    // Shows a Visual Cue For Interaction
    private void DialogueTrigger_OnPlayerInRange(object sender, EventArgs e) {
        if (playerInRange && !DialogueManager.Instance.IsDialoguePlaying()) {
            visualCue.SetActive(true);
        } else {
            visualCue.SetActive(false);
        }
    }

    private void GameInputManager_OnInteractAction(object sender, EventArgs e) {
        if (playerInRange && !DialogueManager.Instance.IsDialoguePlaying()) {
            DialogueManager.Instance.EnterDialogueMode(inkJSON);
            visualCue.SetActive(false);
        }
    }

    private void DialogueManager_OnDialogueFinished(object sender, EventArgs e) {
        if (playerInRange) {
            visualCue.SetActive(true);
        } else {
            visualCue.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.GetComponent<PlayerController>() != null) {
            playerInRange = true;
            OnPlayerInRange?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        playerInRange = false;
        OnPlayerInRange?.Invoke(this, EventArgs.Empty);
    }
}
