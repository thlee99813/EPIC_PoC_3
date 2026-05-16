using UnityEngine;

[CreateAssetMenu(menuName = "Epic/Build/Rail Build Cost Data")]
public class RailBuildCostData : ScriptableObject
{
    [SerializeField] private float _straightRailCost = 5f;
    [SerializeField] private float _curveRailCost = 8f;

    [SerializeField] private float _refundRate = 0.5f;

    public float GetBuildCost(BuildMode buildMode)
    {
        return buildMode switch
        {
            BuildMode.StraightRail => _straightRailCost,
            BuildMode.CurveRail => _curveRailCost,
            _ => 0
        };
    }

    public float GetRefund(RailTile.RailType railType)
    {
        float buildCost = railType switch
        {
            RailTile.RailType.Straight => _straightRailCost,
            RailTile.RailType.Curve => _curveRailCost,
            _ => 0f
        };

        return buildCost * _refundRate;
    }
}