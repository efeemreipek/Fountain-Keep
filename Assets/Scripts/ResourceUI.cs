using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI woodText;
    [SerializeField] private TextMeshProUGUI stoneText;
    [SerializeField] private TextMeshProUGUI ironText;
    [SerializeField] private TextMeshProUGUI berryText;

    private void Awake()
    {
        GameResources.OnResourceChanged += GameResources_OnResourceChanged;
    }
    private void Start()
    {
        goldText.text = $"<sprite=0>0";
        woodText.text = $"<sprite=1>0";
        stoneText.text = $"<sprite=2>0";
        ironText.text = $"<sprite=3>0";
        berryText.text = $"<sprite=4>0";
    }

    private void GameResources_OnResourceChanged(ResourceType resourceType, int amount)
    {
        switch (resourceType)
        {
            case ResourceType.Gold:
                goldText.text = $"<sprite=0>{amount}";
                break;
            case ResourceType.Wood:
                woodText.text = $"<sprite=1>{amount}";
                break;
            case ResourceType.Stone:
                stoneText.text = $"<sprite=2>{amount}";
                break;
            case ResourceType.Iron:
                ironText.text = $"<sprite=3>{amount}";
                break;
            case ResourceType.Berry:
                berryText.text = $"<sprite=4>{amount}";
                break;
        }
    }
}
