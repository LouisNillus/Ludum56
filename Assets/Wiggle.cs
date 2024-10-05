using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class Wiggle : MonoBehaviour
{
    Tween _wiggleTween = null;

    [Button]
    public void Play()
    {
        _wiggleTween.Rewind();
        _wiggleTween = this.transform.DOPunchRotation(Vector3.forward * 10, 1f).SetLoops(-1);
    }
    public void Stop()
    {
        _wiggleTween.Rewind();
        _wiggleTween.Kill();
    }
}
