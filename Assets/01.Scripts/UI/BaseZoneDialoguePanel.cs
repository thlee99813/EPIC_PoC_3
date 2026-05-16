using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseZoneDialoguePanel : MonoBehaviour
{
    [SerializeField] private GameObject _root;
    [SerializeField] private TMP_Text _speakerNameText;
    [SerializeField] private TMP_Text _dialogueText;
    [SerializeField] private Image _characterImage;
    [SerializeField] private Image _dialogueBoxImage;
    [SerializeField] private Button _completeButton;

    private DialogueEntryData[] _entries;
    private int _currentIndex;
    private Action _closed;

    private void Awake()
    {
        _completeButton.onClick.AddListener(Next);
        _root.SetActive(false);
    }

    public void Open(BaseZoneDialogueData data, Action closed)
    {
        _entries = data.DialogueSequence.Entries;
        _currentIndex = 0;
        _closed = closed;

        _root.SetActive(true);
        Refresh();
    }

    private void Next()
    {
        _currentIndex++;

        if (_currentIndex >= _entries.Length)
        {
            Close();
            return;
        }

        Refresh();
    }

    private void Refresh()
    {
        DialogueEntryData entry = _entries[_currentIndex];

        _speakerNameText.text = entry.SpeakerName;
        _dialogueText.text = entry.Text;
        _characterImage.sprite = entry.CharacterSprite;
        _dialogueBoxImage.sprite = entry.DialogueBoxSprite;
    }

    private void Close()
    {
        _root.SetActive(false);
        _closed?.Invoke();
        _closed = null;
    }

    private void OnDestroy()
    {
        _completeButton.onClick.RemoveListener(Next);
    }
}