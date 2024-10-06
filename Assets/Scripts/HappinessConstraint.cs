using UnityEngine;

[CreateAssetMenu(fileName = "HappinessConstraint", menuName = "Constraint/Happiness")]
public class HappinessConstraint : Constraint
{
    public override bool Perform(ConstraintType constraint_type, Head rule_owner)
    {
        if (constraint_type != ConstraintType.Global && constraint_type != _constraintType)
        {
            return true;
        }

        return rule_owner.EmotionalState == EmotionalState.Angry;
    }
}
