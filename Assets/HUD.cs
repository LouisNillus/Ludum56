using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelIndexText = null;
    [SerializeField] private Image _stacksPanel = null;

    private void Start()
    {
        GridManager.Instance.OnLevelGenerated.AddListener(SetLevelIndexText);
    }

    private void OnDestroy()
    {
        GridManager.Instance.OnLevelGenerated.RemoveListener(SetLevelIndexText);
    }

    public void SetLevelIndexText(
        int new_index
        )
    {
        _levelIndexText.text = $"{new_index + 1}/{GridManager.Instance.LevelsCount}";
    }

    public float GetSidePanelWidthRatio()
    {
        return Mathf.InverseLerp(0, this.GetComponent<RectTransform>().rect.width, _stacksPanel.rectTransform.rect.width);
    }
}
