using UnityEngine;

[CreateAssetMenu(menuName = "Epic/Dialogue/Dialogue Entry")]
public class DialogueEntryData : ScriptableObject
{
    [SerializeField] private string _speakerName;
    [SerializeField] private Sprite _characterSprite;
    [SerializeField] private Sprite _dialogueBoxSprite;
    [SerializeField, TextArea] private string _text;

    public string SpeakerName => _speakerName;
    public Sprite CharacterSprite => _characterSprite;
    public Sprite DialogueBoxSprite => _dialogueBoxSprite;
    public string Text => _text;
}