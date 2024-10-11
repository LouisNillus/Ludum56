using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeadStack : MonoBehaviour
{
    [Header("Frame")]
    public Color _frameColor = Color.white;
    public Image _counterBackground = null;
    public TextMeshProUGUI _counterText = null;

    [Header("Head")]
    public HeadType _headType = HeadType.None;
    public SpriteRenderer _headRenderer = null;
    public Sprite _headSprite = null;
    public Sprite _frameOpened = null;
    public Sprite _frameClosed = null;

    private SpriteRenderer _frameRenderer = null;
    private Collider2D _collider = null;

    private Head _spawnHead = null;


    public void Release()
    {
        _spawnHead = null;

        RefreshAvailability();
    }

    public void RefreshAvailability()
    {
        RefreshCounterText();
        _headRenderer.color = _headRenderer.color.ChangeAlpha(HeadStackManager.Instance.Has(_headType) ? 1f : 0.2f);
        _frameRenderer.sprite = HeadStackManager.Instance.Has(_headType) || _spawnHead != null ? _frameOpened : _frameClosed;
    }

    private void RefreshCounterText()
    {
        _counterText.text = (HeadStackManager.Instance.GetCount(_headType) + (_spawnHead != null ? 1 : 0)).ToString();
    }

    private void HeadStackManager_OnLevelLoaded()
    {
        RefreshAvailability();
    }

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        _frameRenderer = GetComponent<SpriteRenderer>();

        _counterBackground.color = _frameColor;
        _frameRenderer.color = _frameColor;
        _headRenderer.sprite = _headSprite;

        RefreshCounterText();

        HeadStackManager.Instance.OnLevelLoaded.AddListener(HeadStackManager_OnLevelLoaded);
        HeadStackManager.Instance.OnHeadsCountChanged.AddListener(RefreshAvailability);
    }

    private void Update()
    {
        if (_spawnHead == null && _collider.bounds.Contains(Utilities.MousePosition2D().ChangeZ(this.transform.position.z)))
        {
            if (HeadStackManager.Instance.Has(_headType))
            {
                _spawnHead = HeadStackManager.Instance.PickHead(_headType);
                _spawnHead.ParentStack = this;

                RefreshAvailability();

                _spawnHead.transform.position = this.transform.position;
            }
        }
    }

    private void OnDestroy()
    {
        HeadStackManager.Instance.OnLevelLoaded.RemoveListener(HeadStackManager_OnLevelLoaded);
        HeadStackManager.Instance.OnHeadsCountChanged.RemoveListener(RefreshAvailability);
    }
}
