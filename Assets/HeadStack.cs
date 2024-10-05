using UnityEngine;

public class HeadStack : MonoBehaviour
{
    [Header("Frame")]
    public Color _frameColor = Color.white;

    [Header("Head")]
    public HeadType _headType = HeadType.None;
    public SpriteRenderer _headRenderer = null;
    public Sprite _headSprite = null;

    private SpriteRenderer _frameRenderer = null;
    private Collider2D _collider = null;

    private Head _spawnHead = null;


    public void Release()
    {
        _spawnHead = null;
    }

    public void RefreshAvailability(
        HeadType changed_head_type
        )
    {
        if (changed_head_type == _headType)
        {
            _headRenderer.color = _headRenderer.color.ChangeAlpha(HeadStackManager.Instance.Has(_headType) ? 1f : 0.25f);
        }
    }

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        _frameRenderer = GetComponent<SpriteRenderer>();

        _frameRenderer.color = _frameColor;
        _headRenderer.sprite = _headSprite;

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

                _spawnHead.transform.position = this.transform.position;
            }
        }
    }

    private void OnDestroy()
    {
        HeadStackManager.Instance.OnHeadsCountChanged.RemoveListener(RefreshAvailability);
    }
}
