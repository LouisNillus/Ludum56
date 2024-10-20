using NaughtyAttributes;
using UnityEngine;

public class CameraFocus : MonoBehaviour
{
    public GridManager _gridManager = null;
    public HUD _hud = null;

    [Button]
    public void Focus()
    {
        Camera camera = Camera.main;

        float left_bound = camera.ViewportToWorldPoint(new(0f, 0.5f, 0f)).x;
        float right_bound = camera.ViewportToWorldPoint(new(1f, 0.5f, 0f)).x;

        float view_width = right_bound - left_bound;

        float horizontal_offset = -((_hud.GetSidePanelWidthRatio() * view_width) / 2f);

        camera.transform.position = _gridManager.GetCenterPoint().OffsetX(horizontal_offset).ChangeZ(-10f);
    }

    private void GridManager_OnGridSetup()
    {
        Focus();
    }

    private void Awake()
    {
        _gridManager.OnGridInitialized.AddListener(GridManager_OnGridSetup);
    }

    private void OnDestroy()
    {
        _gridManager.OnGridInitialized.RemoveListener(GridManager_OnGridSetup);
    }
}
