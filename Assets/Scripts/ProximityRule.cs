using UnityEngine;

[CreateAssetMenu(fileName = "ProximityRule", menuName = "Rule/Proximity")]
public class ProximityRule : Rule
{
    public HeadType _headType = HeadType.None;
    public int _distance = 1;

    public override void Verify(
        Head rule_owner
        )
    {
        rule_owner.EmotionalState = EmotionalState.Happy;
    }
}
