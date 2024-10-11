using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NeighboursRule", menuName = "Rule/Neighbours")]
public class NeighboursRule : Rule
{
    public List<HeadType> _neighbourTypes = new();
    public ConditionType _condition = ConditionType.OR;
    public int _count = 0;

    public override void Verify(
        Head rule_owner
        )
    {
        foreach (HeadType head_type in _neighbourTypes)
        {
            int neighbours_count = GridManager.Instance.GetAdjacentCells(rule_owner.AssignedCell, 1, include_diagonals: true)
                                                    .Count(cell => cell != null && cell.Head != null && cell.Head.HeadType == head_type);

            if (neighbours_count >= _count)
            {
                rule_owner.EmotionalState = EmotionalState.Happy;

                if (_condition == ConditionType.OR)
                {
                    return;
                }

            }
            else
            {
                rule_owner.EmotionalState = EmotionalState.Angry;

                if (_condition == ConditionType.AND)
                {
                    return;
                }
            }

        }

        rule_owner.EmotionalState = EmotionalState.Angry;
    }
}

public enum ConditionType
{
    OR = 0,
    AND = 1
}
