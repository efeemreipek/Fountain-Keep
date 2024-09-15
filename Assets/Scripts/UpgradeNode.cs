using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UpgradeType
{
    SoldierEquipment,
    SoldierCount,
    BuildingFortification,
    ArcherTower,
    Cannon,
}

public class UpgradeNode : BaseBuildingNode
{
    [SerializeField] private UpgradeType upgradeType;
    [SerializeField] private int upgradeAmount;
    [SerializeField] private Material emptyProgressMaterial;

    protected override void HandleProgressFull()
    {
        Upgrades.ChangeUpgradeLevel(upgradeType, upgradeAmount);

        foreach(var progressBar in progressBarList)
        {
            progressBar.material = emptyProgressMaterial;
        }
        currentProgress = 0;
    }
}
