using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Upgrades
{
    private const int MAX_LEVEL = 100;

    public static int archerTowerCount = 0;
    public static int cannonCount = 0;
    public static int soldierCount = 10;
    public static int soldierEquipmentLevel = 0;
    public static int buildingFortificationLevel = 0;

    public static Dictionary<UpgradeType, int> upgradeTypesLevels = new Dictionary<UpgradeType, int>
    {
        {UpgradeType.SoldierEquipment, soldierEquipmentLevel},
        {UpgradeType.BuildingFortification, buildingFortificationLevel},
        {UpgradeType.SoldierCount, soldierCount},
        {UpgradeType.ArcherTower, archerTowerCount},
        {UpgradeType.Cannon, cannonCount},
    };

    public static UpgradeType[] upgradeTypes = new UpgradeType[]
    {
        UpgradeType.SoldierEquipment,
        UpgradeType.BuildingFortification,
        UpgradeType.SoldierCount,
        UpgradeType.ArcherTower,
        UpgradeType.Cannon,
    };

    public static Dictionary<UpgradeType, int> upgradeTypesLevelLast = new Dictionary<UpgradeType, int>
    {
        {UpgradeType.SoldierEquipment, soldierEquipmentLevel},
        {UpgradeType.BuildingFortification, buildingFortificationLevel},
        {UpgradeType.SoldierCount, soldierCount},
        {UpgradeType.ArcherTower, archerTowerCount},
        {UpgradeType.Cannon, cannonCount},
    };

    public static void ChangeUpgradeLevel(UpgradeType upgradeType, int amount)
    {
        if (upgradeTypesLevels.ContainsKey(upgradeType))
        {
            upgradeTypesLevels[upgradeType] = Mathf.Clamp(upgradeTypesLevels[upgradeType] + amount, 0, MAX_LEVEL);
        }
    }
    public static List<int> FindDifferenceInUpgradeLevels()
    {
        List<int> differences = new List<int>();

        for(int i = 0; i < upgradeTypesLevels.Count; i++)
        {
            differences.Add(upgradeTypesLevels[upgradeTypes[i]] - upgradeTypesLevelLast[upgradeTypes[i]]); 
        }

        return differences;
    }
    public static void MatchUpgradeLevelsToLast()
    {
        for(int i = 0; i < upgradeTypesLevels.Count; i++)
        {
            upgradeTypesLevelLast[upgradeTypes[i]] = upgradeTypesLevels[upgradeTypes[i]];
        }
    }
}
