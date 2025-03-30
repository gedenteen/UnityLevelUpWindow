using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;

public class WindowLevelUp : MonoBehaviour
{
    [SerializeField] private CanvasGroup _myCanvasGroup;
    [SerializeField] private Button _buttonWatchAd;
    [SerializeField] private Button _buttonClaim;

    private void Awake()
    {
        _buttonWatchAd.onClick.AddListener(CloseWindow);
        _buttonClaim.onClick.AddListener(CloseWindow);
    }

    private void OnDestroy()
    {
        _buttonWatchAd.onClick.RemoveListener(CloseWindow);
        _buttonClaim.onClick.RemoveListener(CloseWindow);
    }

    public void CloseWindow()
    {
        Activate(false, 0.25f);
    }
    
    public void OpenWindow()
    {
        Activate(true, 0.25f);
    }

    private void Activate(bool isActive, float seconds = 0f)
    {
        if (_myCanvasGroup != null)
        {
            ChangeAlpha(isActive, seconds).Forget();
            _myCanvasGroup.interactable = isActive; // Устанавливаем интерактивность в зависимости от isActive
            // _myCanvasGroup.blocksRaycasts = isActive; // Устанавливаем блокировку событий мыши в зависимости от isActive
        }
        else
        {
            Debug.LogError("UiPanel: CanvasGroup is not assigned or found on this GameObject.");
        }
    }

    private async UniTask ChangeAlpha(bool isActive, float seconds)
    {
        if (isActive) // если включаем, то включаем gameObject сразу
        {
            _myCanvasGroup.gameObject.SetActive(isActive);
        }

        if (seconds == 0f)
        {
            _myCanvasGroup.alpha = isActive ? 1 : 0; // Устанавливаем прозрачность в 1 (включено) или 0 (выключено)
        }
        else
        {
            if (isActive)
            {
                _myCanvasGroup.alpha = 0;
                await SmoothChangeAlpha(seconds, 0f, 1f);
            }
            else
            {
                _myCanvasGroup.alpha = 1f;
                await SmoothChangeAlpha(seconds, 1f, 0f);
            }
        }

        if (!isActive) // если выключаем, то выключаем gameObject в конце
        {
            _myCanvasGroup.gameObject.SetActive(isActive);
        }
    }

    private async UniTask SmoothChangeAlpha(float seconds, float startAlpha, float endAlpha)
    {
        if (seconds <= 0f)
        {
            Debug.LogError($"SmoothChangeAlpha: invalid seconds = {seconds}");
            return;
        }

        float timer = 0f;
        while (timer < seconds)
        {
            _myCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, timer / seconds);
            timer += Time.deltaTime;
            await UniTask.Yield();
        }

        _myCanvasGroup.alpha = endAlpha;
    }
}
