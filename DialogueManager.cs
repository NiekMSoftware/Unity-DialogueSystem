using System;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    public event Action OnDialogueLineComplete;

    public DialogueNode currentNode;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public void StartDialogue(DialogueNode startNode)
    {
        SetCurrentNode(startNode);
    }

    private void EndDialogue()
    {
        UIManager.Instance.HideDialogue();
    }

    public void SetCurrentNode(DialogueNode node)
    {
        if (node == null)
        {
            EndDialogue();
            return;
        }

        currentNode = node;
        currentNode.EnterNode();
    }

    public void OnDialogueLineCompleted()
    {
        OnDialogueLineComplete?.Invoke();
    }

    // this method can be called from UI, e.g., when player clicks continue
    public void ContinueDialogue()
    {
        if (currentNode is DialogueLineNode lineNode)
        {
            if (lineNode.nextNode == null)
            {
                EndDialogue();
            }
            else
                OnDialogueLineCompleted();
        }
    }

    // Handling choice selection
    public void ChooseOption(DialogueChoiceNode.Choice choice)
    {
        SetCurrentNode(choice.nextNode);
    }

    public void DisplayDialogue(string characterName, string text)
    {
        UIManager.Instance.ShowDialogue(characterName, text);
    }

    public void DisplayChoices(string characterName, string text, System.Collections.Generic.List<DialogueChoiceNode.Choice> choices)
    {
        UIManager.Instance.ShowChoices(characterName, text, choices);
    }
}
