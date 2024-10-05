using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class Grow : MonoBehaviour
{
    Tween _growTween = null;

    [Button]
    public void Play()
    {
        _growTween.Rewind();
        _growTween = this.transform.DOScale(Vector3.one * 1.1f, 0.25f);
    }

    public void Stop()
    {
        _growTween = this.transform.DOScale(Vector3.one, 0.25f);
    }
}
