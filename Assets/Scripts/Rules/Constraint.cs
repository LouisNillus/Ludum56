using UnityEngine;

public abstract class Constraint : ScriptableObject
{
    public ConstraintType _constraintType;

    public abstract bool Perform(ConstraintType constraint_type, Head constraint_owner);
}

public enum ConstraintType
{
    Global = 0,
    Selection = 1
}
