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
    }

    public void ShowDialogue(string characterName, string text)
    {
        dialoguePanel.SetActive(true);

        characterNameText.text = characterName;
        dialogueText.text = text;
        choicesPanel.SetActive(false);
    }

    public void ShowChoices(string characterName, string text, List<DialogueChoiceNode.Choice> choices)
    {
        characterNameText.text = characterName;
        dialogueText.text = text;
        PopulateChoices(choices);
    }

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
