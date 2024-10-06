using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ProximityRule", menuName = "Rule/Proximity")]
public class ProximityRule : Rule
{
    public HeadType _headType = HeadType.None;
    public ProximityType _proximityType = ProximityType.All;
    public int _distance = 1;

    public override void Verify(
        Head rule_owner
        )
    {
        bool proximity_confirmed = _proximityType switch
        {
            ProximityType.All => GridManager.Instance.GetAdjacentCells(rule_owner.AssignedCell, _distance, true)
                                                     .Any(cell => cell != null && cell.Head != null && cell.Head.HeadType == _headType),
            ProximityType.Straight => GridManager.Instance.GetAdjacentCells(rule_owner.AssignedCell, _distance, false)
                                                     .Any(cell => cell != null && cell.Head != null && cell.Head.HeadType == _headType),
            ProximityType.Diagonals => GridManager.Instance.GetDiagonalCells(rule_owner.AssignedCell, _distance)
                                                     .Any(cell => cell != null && cell.Head != null && cell.Head.HeadType == _headType),
            _ => false
        };


        rule_owner.EmotionalState = proximity_confirmed ? EmotionalState.Angry : EmotionalState.Happy;
    }
}

public enum ProximityType
{
    All = 0,
    Straight = 1,
    Diagonals = 2
}
