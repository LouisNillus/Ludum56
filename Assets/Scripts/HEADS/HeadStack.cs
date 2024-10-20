using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeadStack : MonoBehaviour
{
    [Header("Counter")]
    public Image _counterBackground = null;
    public TextMeshProUGUI _counterText = null;

    [Header("Head")]
    public HeadData HeadData = null;
    public Image HeadVisual = null;

    [Header("Frame")]
    public Image FrameRenderer = null;
    public Sprite _frameOpened = null;
    public Sprite _frameClosed = null;

    private Head _currentHead = null;

    public void RefreshAvailability()
    {
        bool has_head_type = HeadStackManager.Instance.Has(HeadData.Type);

        _counterText.text = HeadStackManager.Instance.GetCount(HeadData.Type).ToString();
        HeadVisual.color = HeadVisual.color.ChangeAlpha(has_head_type ? 1f : 0.2f);
        FrameRenderer.sprite = has_head_type ? _frameOpened : _frameClosed;
    }

    private void HeadStackManager_OnLevelLoaded()
    {
        RefreshAvailability();
    }

    private void Start()
    {
        _counterBackground.color = HeadData.MainColor;
        FrameRenderer.color = HeadData.MainColor;
        HeadVisual.sprite = HeadData.Sprite;

        RefreshAvailability();

        HeadStackManager.Instance.OnLevelLoaded.AddListener(HeadStackManager_OnLevelLoaded);
        HeadStackManager.Instance.OnHeadsCountChanged.AddListener(RefreshAvailability);
    }

    // Called from UI button
    public void Spawn()
    {
        if (HeadStackManager.Instance.Has(HeadData.Type))
        {
            _currentHead = HeadStackManager.Instance.PickHead(HeadData.Type);
            _currentHead.ParentStack = this;
            _currentHead.transform.position = this.transform.position;

            RefreshAvailability();

            _currentHead.StartDrag();
        }
    }

    private void OnDestroy()
    {
        HeadStackManager.Instance.OnLevelLoaded.RemoveListener(HeadStackManager_OnLevelLoaded);
        HeadStackManager.Instance.OnHeadsCountChanged.RemoveListener(RefreshAvailability);
    }
}
