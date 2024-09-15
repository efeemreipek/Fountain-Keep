using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Text.RegularExpressions;

public class TimerManager : MonoBehaviour
{
    public static event Action OnGameEnded;

    [SerializeField] private float time = 120f;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField, Range(0, 100)] private int soldierDeathPercent = 50;

    private int nightCount = 1;

    private Timer timer;
    private TimerManagerUI timerManagerUI;

    private void Awake()
    {
        timer = GetComponent<Timer>();
        timerManagerUI = GetComponent<TimerManagerUI>();

        Timer.OnTimerEnd += Timer_OnTimerEnd;
    }
    private void Update()
    {
        int remainingTime = timer.GetRemainingTimeAsInt();
        TimeSpan timeSpan = TimeSpan.FromSeconds(remainingTime);
        string time = timeSpan.ToString("mm':'ss");
        timerManagerUI.GetTimerText().text = time;
    }
    public void StartGame()
    {
        timer.StartTimer(time);
    }
    private void Timer_OnTimerEnd()
    {
        GameManager.Instance.ChangeIsInputActive(false);

        timerManagerUI.BlackOut(fadeDuration, true);

        StartCoroutine(NightProgressing());
    }
    private IEnumerator NightProgressing()
    {
        timerManagerUI.GetNightText().gameObject.SetActive(true);

        timerManagerUI.GetNightText().text = $"NIGHT {nightCount}";
        yield return new WaitForSeconds(1f);
        timerManagerUI.GetNightText().text = $"NIGHT {nightCount}.";
        yield return new WaitForSeconds(1f);
        timerManagerUI.GetNightText().text = $"NIGHT {nightCount}..";
        yield return new WaitForSeconds(1f);
        timerManagerUI.GetNightText().text = $"NIGHT {nightCount}...";
        yield return new WaitForSeconds(1f);

        timerManagerUI.GetNightText().gameObject.SetActive(false);
        EnemyDamageUpgrades();

        NightPassPanel();
    }
    private void NightPassPanel()
    {
        timerManagerUI.GetNightPassedText().text = $"NIGHT {nightCount++} PASSED";
        ListUpgrades();

        timerManagerUI.MoveNightPanel(new Vector2(0f, -40f), fadeDuration);
        GameManager.Instance.PlaySoundRandomPitch();

        foreach (ResourceNode node in GameManager.Instance.GetResourceNodeList())
        {
            node.RegenerateResourceImmediately();
        }
    }
    public void Reverse()
    {
        GameManager.Instance.ChangeIsInputActive(true);

        timerManagerUI.BlackOut(fadeDuration, false);
        timerManagerUI.MoveNightPanel(new Vector2(2869f, -40f), fadeDuration);
        GameManager.Instance.PlaySoundRandomPitch();

        Upgrades.MatchUpgradeLevelsToLast();

        if (Upgrades.upgradeTypesLevels[UpgradeType.SoldierCount] <= 0)
        {
            EndGame();
            return;
        }

        timer.StartTimer(time);
    }
    private void ListUpgrades()
    {
        for(int i = 0; i < timerManagerUI.GetUpgradeTextList().Count; i++)
        {
            string text = "";
            text += $"{PascalCaseToSentence(Upgrades.upgradeTypes[i].ToString())}: ";
            text += $"{Upgrades.upgradeTypesLevels[Upgrades.upgradeTypes[i]]} ";

            timerManagerUI.GetUpgradeTextList()[i].text = text;

            int diff = Upgrades.FindDifferenceInUpgradeLevels()[i];
            string difference = diff < 0 ? $"<color=#FF0000>{diff}" : diff > 0 ? $"<color=#00FF00>+{diff}" : $"{diff}";

            timerManagerUI.GetUpgradeDifferenceTextList()[i].text = difference;
        }
    }
    private void EnemyDamageUpgrades()
    {
        int militaryForce = Upgrades.upgradeTypesLevels[UpgradeType.Cannon] * 2 + Upgrades.upgradeTypesLevels[UpgradeType.ArcherTower] * 1;
        if(militaryForce == 0)
        {
            int r = UnityEngine.Random.Range(0, 100);
            militaryForce = r <= 50 ? militaryForce + Upgrades.buildingFortificationLevel : 0;
        }

        int rand = UnityEngine.Random.Range(0, 100);
        if (rand <= soldierDeathPercent)
        {
            Upgrades.ChangeUpgradeLevel(UpgradeType.SoldierCount, -1);
            Upgrades.ChangeUpgradeLevel(UpgradeType.SoldierEquipment, -1);
        }

        int equipmentShortage = Upgrades.upgradeTypesLevels[UpgradeType.SoldierCount] - Upgrades.upgradeTypesLevels[UpgradeType.SoldierEquipment];

        if(equipmentShortage > 6)
        {
            int diff = -1;
            diff = Mathf.Clamp(diff + militaryForce, diff, 0);

            if (Upgrades.upgradeTypesLevels[UpgradeType.SoldierEquipment] == 0) diff *= 2;

            Upgrades.ChangeUpgradeLevel(UpgradeType.SoldierCount, diff - 1);
            Upgrades.ChangeUpgradeLevel(UpgradeType.SoldierEquipment, diff - 2);
            Upgrades.ChangeUpgradeLevel(UpgradeType.BuildingFortification, diff);
        }
        else if(equipmentShortage > 4)
        {
            int diff = -1;
            diff = Mathf.Clamp(diff + militaryForce, diff, 0);

            if (Upgrades.upgradeTypesLevels[UpgradeType.SoldierEquipment] == 0) diff *= 2;

            Upgrades.ChangeUpgradeLevel(UpgradeType.SoldierCount, diff);
            Upgrades.ChangeUpgradeLevel(UpgradeType.SoldierEquipment, diff - 1);
        }
        else if (equipmentShortage > 2)
        {
            int diff = -1;
            diff = Mathf.Clamp(diff + militaryForce, diff, 0);

            Upgrades.ChangeUpgradeLevel(UpgradeType.SoldierEquipment, diff);
        }
        else if (equipmentShortage <= 2 && equipmentShortage >= 0)
        {
            rand = UnityEngine.Random.Range(0, 100);
            if (rand <= soldierDeathPercent)
            {
                Upgrades.ChangeUpgradeLevel(UpgradeType.SoldierCount, -1);
                Upgrades.ChangeUpgradeLevel(UpgradeType.SoldierEquipment, -1);
            }
        }
    }
    private void EndGame()
    {
        GameManager.Instance.ChangeIsInputActive(false);
        OnGameEnded?.Invoke();
    }
    private string PascalCaseToSentence(string text)
    {
        return Regex.Replace(text, "(?<!^)([A-Z])", " $1");
    }
}
