using UnityEngine;

public class RailTile : MonoBehaviour
{
    public enum RailType
    {
        Straight,
        Curve
    }

    [SerializeField] private RailType _railType;

    public RailType Type => _railType;
    public bool IsCurve => _railType == RailType.Curve;

    public int GetTurnDirection(Vector3 trainDirection)
    {
        Vector3 localDirection = transform.InverseTransformDirection(trainDirection);
        localDirection.y = 0f;
        localDirection.Normalize();

        int turnDirection;

        if (Mathf.Abs(localDirection.z) > Mathf.Abs(localDirection.x))
        {
            turnDirection = localDirection.z > 0f ? 1 : -1;
        }
        else
        {
            turnDirection = localDirection.x > 0f ? 1 : -1;
        }

        return turnDirection;
    }

}
