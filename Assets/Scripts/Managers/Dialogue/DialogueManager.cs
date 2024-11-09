using Ink.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour {

    public static DialogueManager Instance { get; private set; }

    private const string SPEAKER_TAG = "speaker";
    private const string LAYOUT_TAG = "layout";

    public event EventHandler OnDialogueFinished;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject backGround;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choiceButtons;
    private TextMeshProUGUI[] choicesTexts;

    private Story currentStory;

    private bool dialogueIsPlaying;
    private bool isDisplayingChoices;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        backGround.SetActive(false);

        GetChoicesText();

        GameInputManager.Instance.OnContinueAction += GameInputManager_OnContinueAction;
    }

    private void GameInputManager_OnContinueAction(object sender, EventArgs e) {
        if (dialogueIsPlaying && !isDisplayingChoices) {
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON) {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        backGround.SetActive(true);

        ContinueStory();
    }

    private IEnumerator ExitDialogueMode() {
        yield return new WaitForSeconds(0.2f);

        OnDialogueFinished?.Invoke(this, EventArgs.Empty);

        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        backGround.SetActive(false);
        dialogueText.text = "";
    }

    private void ContinueStory() {
        if (currentStory.canContinue) {
            dialogueText.text = currentStory.Continue();
            DisplayChoices();
            HandleTags(currentStory.currentTags);
        } else {
            StartCoroutine(ExitDialogueMode());
        }
    }

    private void DisplayChoices() {
        List<Choice> currentChoices = currentStory.currentChoices;

        isDisplayingChoices = currentChoices.Count > 0;

        if (!isDisplayingChoices) {
            if (choiceButtons[0].gameObject.activeSelf) {
                foreach (GameObject choiceButton in choiceButtons) {
                    choiceButton.SetActive(false);
                }
            }
            return;
        }

        if (currentChoices.Count > choicesTexts.Length) {
            Debug.LogError("More choices available than UI can support. Number of choices given: " + currentChoices.Count);
        }

        int index = 0;
        foreach (Choice choice in currentChoices) {
            choiceButtons[index].gameObject.SetActive(true);
            choicesTexts[index].text = choice.text;
            index++;
        }
    }

    private void GetChoicesText() {
        choicesTexts = new TextMeshProUGUI[choiceButtons.Length];

        int index = 0;
        foreach (GameObject choiceButton in choiceButtons) {
            choicesTexts[index] = choiceButton.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    public void MakeChoice(int choiceIndex) {
        isDisplayingChoices = false;
        EventSystem.current.SetSelectedGameObject(null);

        currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
    }

    private void HandleTags(List<string> currentTags) {
        foreach (string tag in currentTags) {
            string[] splitTag = tag.Split(':');

            if (splitTag.Length != 2) {
                Debug.LogFormat("Tag could not be parsed: {0}", tag);
            }

            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch (tagKey) {
                case SPEAKER_TAG:
                    Debug.Log("Speaker=" + tagValue);
                    break;
                case LAYOUT_TAG:
                    Debug.Log("Layout=" + tagValue);
                    break;
                default:
                    Debug.LogFormat("Unknown tag type: {0}", tagKey);
                    break;
            }
        }

    }

    public bool IsDialoguePlaying() {
        return dialogueIsPlaying;
    }

    private void OnDestroy() {
        GameInputManager.Instance.OnContinueAction -= GameInputManager_OnContinueAction;
    }
}