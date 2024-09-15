using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class TimerManagerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject blackOutPanel;
    [SerializeField] private GameObject nightPanel;
    [SerializeField] private TextMeshProUGUI nightPassedText;
    [SerializeField] private List<TextMeshProUGUI> upgradeTextList;
    [SerializeField] private List<TextMeshProUGUI> upgradeDifferenceTextList;
    [SerializeField] private TextMeshProUGUI nightText;

    public TextMeshProUGUI GetTimerText() => timerText;
    public GameObject GetBlackOutPanel() => blackOutPanel;
    public GameObject GetNightPanel() => nightPanel;
    public TextMeshProUGUI GetNightPassedText() => nightPassedText;
    public List<TextMeshProUGUI> GetUpgradeTextList() => upgradeTextList;
    public List<TextMeshProUGUI> GetUpgradeDifferenceTextList() => upgradeDifferenceTextList;
    public TextMeshProUGUI GetNightText() => nightText;

    public void BlackOut(float duration, bool isIn)
    {
        StartCoroutine(FadeBlackOutPanel(duration, isIn));
    }
    private IEnumerator FadeBlackOutPanel(float duration, bool isIn)
    {
        CanvasGroup canvasGroup = blackOutPanel.GetComponent<CanvasGroup>();

        float time = 0f;

        canvasGroup.alpha = isIn ? 0f : 1f;
        blackOutPanel.SetActive(true);

        while (time < duration)
        {
            time += Time.deltaTime;
            if (isIn)
            {
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, time / duration);
            }
            else
            {
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, time / duration);
            }
            yield return null;
        }

        canvasGroup.alpha = isIn ? 1f : 0f;
    }
    public void MoveNightPanel(Vector2 pos, float duration)
    {
        nightPanel.GetComponent<RectTransform>().DOAnchorPos(pos, duration).SetEase(Ease.InQuint);
    }

}
