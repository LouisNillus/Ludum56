using UnityEngine;

public abstract class Rule : ScriptableObject
{
    public abstract void Verify(Head rule_owner);
}
