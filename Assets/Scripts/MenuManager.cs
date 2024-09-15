using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject startMenuGO;
    [SerializeField] private GameObject endMenuGO;
    [SerializeField] private GameObject settingsMenuGO;
    [SerializeField] private GameObject textAndButtonsGO;
    [SerializeField] private float fadeDuration = 0.5f;

    private TimerManager timerManager;

    private void Awake()
    {
        timerManager = GetComponent<TimerManager>();

        TimerManager.OnGameEnded += TimerManager_OnGameEnded;
    }

    private void TimerManager_OnGameEnded()
    {
        endMenuGO.SetActive(true);
    }
    public void Settings()
    {
        settingsMenuGO.SetActive(true);
        startMenuGO.SetActive(false);
    }
    public void Back()
    {
        settingsMenuGO.SetActive(false);
        startMenuGO.SetActive(true);
    }
    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void StartGame()
    {
        StartCoroutine(StartGameCoroutine());
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private IEnumerator StartGameCoroutine()
    {
        textAndButtonsGO.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0f, -1152f), fadeDuration).SetEase(Ease.InQuint);
        yield return new WaitForSeconds(fadeDuration);
        StartCoroutine(FadePanel(fadeDuration * 2));
        yield return new WaitForSeconds(fadeDuration);

        GameManager.Instance.ChangeIsInputActive(true);

        timerManager.StartGame();
    }
    private IEnumerator FadePanel(float duration)
    {
        CanvasGroup canvasGroup = startMenuGO.GetComponent<CanvasGroup>();

        float time = 0f;

        canvasGroup.alpha = 1f;
        startMenuGO.SetActive(true);

        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, time / duration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
    }
}
