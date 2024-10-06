using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ProximityRule", menuName = "Rule/Proximity")]
public class ProximityRule : Rule
{
    public HeadType _headType = HeadType.None;
    public int _distance = 1;
    public bool _includeDiagonals = true;

    public override void Verify(
        Head rule_owner
        )
    {
        if (GridManager.Instance.GetAdjacentCells(rule_owner.AssignedCell, _distance, _includeDiagonals)
                                .Any(cell => cell != null && cell.Head != null && cell.Head.HeadType == _headType)
            )
        {
            rule_owner.EmotionalState = EmotionalState.Angry;
        }
        else
        {
            rule_owner.EmotionalState = EmotionalState.Happy;
        }
    }
}
