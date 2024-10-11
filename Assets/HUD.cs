using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelIndexText = null;

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
}
