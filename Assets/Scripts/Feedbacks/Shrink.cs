using DG.Tweening;
using NaughtyAttributes;
using System;
using UnityEngine;

public class Shrink : MonoBehaviour
{
    Tween _growTween = null;

    [Button]
    public void Play(
        Action on_complete = null
        )
    {
        _growTween.Rewind();
        _growTween = this.transform.DOScale(Vector3.zero, 0.25f).OnComplete(() => on_complete?.Invoke());
    }

    public void Stop()
    {
        _growTween = this.transform.DOScale(Vector3.one, 0.25f);
    }
}
