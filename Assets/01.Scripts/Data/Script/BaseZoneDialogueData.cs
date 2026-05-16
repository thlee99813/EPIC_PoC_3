using UnityEngine;

[CreateAssetMenu(menuName = "Epic/Base Zone/Base Zone Dialogue")]
public class BaseZoneDialogueData : ScriptableObject
{
    [SerializeField] private string _zoneName;
    [SerializeField] private DialogueSequenceData _dialogueSequence;

    public string ZoneName => _zoneName;
    public DialogueSequenceData DialogueSequence => _dialogueSequence;
}