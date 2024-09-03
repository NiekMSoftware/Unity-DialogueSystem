using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public TMP_Text characterNameText;
    public TMP_Text dialogueText;
    public GameObject choicesPanel;
    public GameObject dialoguePanel;
    public Button choiceButtonPrefab;

    [Header("Dialogue Typing")]
    public float typingSpeed = 0.05f;
    public float fastTypingSpeed = 0.01f;

    private Coroutine typingCoroutine;
    private bool isTypingFast = false;
    private bool isTypingComplete = false;
    private bool isChoiceNode = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public void HideDialogue()
    {
        dialoguePanel.SetActive(false);
        choicesPanel.SetActive(false);

        // Stop the coroutine if it's running
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
    }

    public void ShowDialogue(string characterName, string text)
    {
        isChoiceNode = false;
        dialoguePanel.SetActive(true);

        characterNameText.text = characterName;
        dialogueText.text = text;

        // Start typing effect
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeText(text));
        choicesPanel.SetActive(false);
    }

    public void ShowChoices(string characterName, string text, List<DialogueChoiceNode.Choice> choices)
    {
        isChoiceNode = true;
        characterNameText.text = characterName;
        dialogueText.text = text;

        // Start typing effect
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeText(text, choices));
    }

    private IEnumerator TypeText(string text, List<DialogueChoiceNode.Choice> choices = null)
    {
        isTypingComplete = false;
        dialogueText.text = ""; // Clear the text first

        foreach (char c in text.ToCharArray())
        {
            dialogueText.text += c;

            // If space bar is held down, type fast; otherwise, use normal speed
            float currentTypingSpeed = isTypingFast ? fastTypingSpeed : typingSpeed;

            yield return new WaitForSeconds(currentTypingSpeed);
        }

        isTypingComplete = true;
        typingCoroutine = null; // Reset coroutine reference after typing is complete

        if (isChoiceNode && choices != null)
        {
            PopulateChoices(choices);
        }
    }

    public void SetTypingSpeed(bool isFast)
    {
        isTypingFast = isFast;
    }

    public bool IsTypingComplete() => isTypingComplete;

    private void PopulateChoices(List<DialogueChoiceNode.Choice> choices)
    {
        // clear existing choises
        foreach (Transform child in choicesPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // Create new choice buttons
        foreach (var choice in choices)
        {
            Button button = Instantiate(choiceButtonPrefab, choicesPanel.transform);
            button.GetComponentInChildren<TMP_Text>().text = choice.choiceText;
            button.onClick.AddListener(() => OnChoiceSelected(choice));
        }

        choicesPanel.SetActive(true);
    }

    private void OnChoiceSelected(DialogueChoiceNode.Choice choice)
    {
        DialogueManager.Instance.ChooseOption(choice);
    }
}
