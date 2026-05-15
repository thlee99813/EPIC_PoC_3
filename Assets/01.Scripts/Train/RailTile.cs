using UnityEngine;

public class RailTile : MonoBehaviour
{
    public enum RailType
    {
        Straight,
        Curve
    }

    public enum RailConnection
    {
        North,
        East,
        South,
        West
    }

    [SerializeField] private RailType _railType;
    [SerializeField] private RailConnection _connectionA;
    [SerializeField] private RailConnection _connectionB;

    public RailType Type => _railType;
    public bool IsCurve => _railType == RailType.Curve;

    public bool TryGetExitDirection(Vector3 enterDirection, out Vector3 exitDirection)
    {
        Vector3 entrySide = -Flatten(enterDirection);
        Vector3 connectionA = GetWorldConnectionDirection(_connectionA);
        Vector3 connectionB = GetWorldConnectionDirection(_connectionB);

        float dotA = Vector3.Dot(entrySide, connectionA);
        float dotB = Vector3.Dot(entrySide, connectionB);

        const float connectionThreshold = 0.9f;

        if (dotA < connectionThreshold && dotB < connectionThreshold)
        {
            Debug.LogError($"Invalid Rail Entry / Rail:{name}, Y:{transform.eulerAngles.y}, Enter:{enterDirection}, EntrySide:{entrySide}, A:{_connectionA}/{connectionA}, B:{_connectionB}/{connectionB}, DotA:{dotA}, DotB:{dotB}");
            exitDirection = Vector3.zero;
            return false;
        }

        exitDirection = dotA > dotB ? connectionB : connectionA;
        return true;
    }

    private Vector3 GetWorldConnectionDirection(RailConnection connection)
    {
        Vector3 localDirection = connection switch
        {
            RailConnection.North => Vector3.forward,
            RailConnection.East => Vector3.right,
            RailConnection.South => Vector3.back,
            RailConnection.West => Vector3.left,
            _ => Vector3.forward
        };

        return Flatten(transform.TransformDirection(localDirection));
    }

    private Vector3 Flatten(Vector3 direction)
    {
        direction.y = 0f;
        return direction.normalized;
    }
}
