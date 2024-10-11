using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class Shake : MonoBehaviour
{
    Tween _shakeTween = null;

    [Button]
    public void Play(
        int loops_count = -1
        )
    {
        _shakeTween.Rewind();
        _shakeTween = this.transform.DOBlendablePunchRotation(Vector3.forward * 15f, 1f).SetLoops(loops_count);
    }

    public void Stop()
    {
        _shakeTween.Rewind();
        _shakeTween.Kill();
    }
}
