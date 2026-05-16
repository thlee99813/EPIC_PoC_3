using UnityEngine;

[CreateAssetMenu(menuName = "Epic/Dialogue/Dialogue Sequence")]
public class DialogueSequenceData : ScriptableObject
{
    [SerializeField] private DialogueEntryData[] _entries;

    public DialogueEntryData[] Entries => _entries;
}