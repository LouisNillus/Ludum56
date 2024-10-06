using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public List<TextMeshProUGUI> _letters = new List<TextMeshProUGUI>();

    public string _displayedMessage = string.Empty;
    public CanvasGroup _madeByCanvasGroup = null;
    public CanvasGroup _startGameCanvasGroup = null;
    public CanvasGroup _globalCanvasGroup = null;
    public CanvasGroup _blackCanvasGroup = null;

    public Button _startGameButton = null;

    public RectTransform _startGameRectTransform = null;

    private void Start()
    {
        StartCoroutine(DisplayMessage());
    }

    public IEnumerator DisplayMessage()
    {
        yield return new WaitForSeconds(0.75f);

        float duration = 0.5f;
        for (int char_index = 0; char_index < _displayedMessage.Length; char_index++)
        {
            char c = _displayedMessage[char_index];
            yield return new WaitForSeconds(duration);
            _letters[char_index].text = c.ToString();

            if (char_index == 3)
            {
                yield return new WaitForSeconds(duration);
            }
        }

        float t = 0f;

        while (t < 2f)
        {
            _madeByCanvasGroup.alpha = Mathf.InverseLerp(0f, 2f, t);

            yield return null;

            t += Time.deltaTime;
        }

        t = 0f;

        while (t < 0.75f)
        {
            _startGameRectTransform.sizeDelta = new(Mathf.Lerp(30f, 280f, t / 0.75f), _startGameRectTransform.sizeDelta.y);

            t += Time.deltaTime;

            yield return null;
        }

        t = 0f;

        while (t < 0.5f)
        {
            _startGameCanvasGroup.alpha = Mathf.InverseLerp(0f, 0.5f, t);

            yield return null;

            t += Time.deltaTime;
        }

        _startGameButton.interactable = true;
    }

    bool _canStart = true;
    public void StartGameFromButton()
    {
        if (_canStart)
        {
            StartCoroutine(StartGame());
        }
    }

    public IEnumerator StartGame()
    {
        _canStart = false;

        float t = 0f;

        while (t < 0.75f)
        {
            _globalCanvasGroup.alpha = Mathf.InverseLerp(0.75f, 0f, t);

            yield return null;

            t += Time.deltaTime;
        }

        _globalCanvasGroup.alpha = 0;
        _globalCanvasGroup.interactable = false;
        _globalCanvasGroup.blocksRaycasts = false;

        t = 0f;

        while (t < 0.5f)
        {
            _blackCanvasGroup.alpha = Mathf.InverseLerp(0.5f, 0f, t);

            yield return null;

            t += Time.deltaTime;
        }

        _blackCanvasGroup.alpha = 0;
        _blackCanvasGroup.interactable = false;
        _blackCanvasGroup.blocksRaycasts = false;

        GridManager.Instance.GenerateGrid();

        _canStart = true;
    }
}
