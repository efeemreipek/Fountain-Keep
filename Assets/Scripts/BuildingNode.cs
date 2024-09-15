using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingNode : BaseBuildingNode
{
    [SerializeField] private GameObject buildingToSpawn;
    [SerializeField] private UpgradeType upgradeType;

    private Transform meshTransform;

    protected override void Start()
    {
        base.Start();

        meshTransform = transform.GetChild(1);
    }

    public override void Interact()
    {
        base.Interact();
    }

    protected override void HandleProgressFull()
    {
        DisableInteraction();

        foreach (Transform child in meshTransform)
        {
            Destroy(child.gameObject);
        }

        var building = Instantiate(buildingToSpawn, transform.position, Quaternion.identity);
        building.transform.parent = meshTransform;

        Upgrades.ChangeUpgradeLevel(upgradeType, 1);
    }
}
