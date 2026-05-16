using UnityEngine;

public class BaseZone : MonoBehaviour
{
    [SerializeField] private float _interactionRadius = 8f;
    [SerializeField] private BaseZoneDialogueData _dialogueData;
    [SerializeField] private BaseZoneShopData _shopData;

    public float InteractionRadius => _interactionRadius;
    public BaseZoneDialogueData DialogueData => _dialogueData;
    public BaseZoneShopData ShopData => _shopData;

    public bool Contains(Vector3 worldPosition)
    {
        Vector3 offset = worldPosition - transform.position;
        offset.y = 0f;
        return offset.sqrMagnitude <= _interactionRadius * _interactionRadius;
    }
}