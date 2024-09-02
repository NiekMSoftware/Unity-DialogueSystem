using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogueLineNode", menuName = "Dialogue/Line Node")]
public class DialogueLineNode : DialogueNode
{
    public DialogueNode nextNode;

    public override void EnterNode()
    {
        DialogueManager.Instance.DisplayDialogue(characterName, dialogueText);
        DialogueManager.Instance.OnDialogueLineComplete += ProceedToNextNode;
    }

    private void ProceedToNextNode()
    {
        DialogueManager.Instance.OnDialogueLineComplete -= ProceedToNextNode;
        DialogueManager.Instance.SetCurrentNode(nextNode);
    }
}
