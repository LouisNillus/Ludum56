using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class Wiggle : MonoBehaviour
{
    Tween _wiggleTween = null;

    [Button]
    public void Play(
        int loops_count = -1
        )
    {
        _wiggleTween.Rewind();
        _wiggleTween = this.transform.DOPunchRotation(Vector3.forward * 10, 1f).SetLoops(loops_count);
    }

    public void Stop()
    {
        _wiggleTween.Rewind();
        _wiggleTween.Kill();
    }
}
