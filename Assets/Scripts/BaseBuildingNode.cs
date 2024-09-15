using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class BuildingCost
{
    public ResourceType ResourceType;
    public int Cost;
}
public abstract class BaseBuildingNode : InteractableNode
{
    [SerializeField] protected int maxProgress;
    [SerializeField] protected int currentProgress;
    [SerializeField] protected Transform progress;
    [SerializeField] protected List<MeshRenderer> progressBarList = new List<MeshRenderer>();
    [SerializeField] protected Material fullProgressMaterial;
    [SerializeField] protected List<BuildingCost> buildingCost = new List<BuildingCost>();
    [SerializeField] protected TextMeshPro requirementText;

    protected bool canBuild = false;

    private Dictionary<ResourceType, int> resourceTypeSpriteIndex = new Dictionary<ResourceType, int>
    {
        {ResourceType.Gold, 0},
        {ResourceType.Wood, 1},
        {ResourceType.Stone, 2},
        {ResourceType.Iron, 3},
        {ResourceType.Berry, 4},
    };

    protected override void Start()
    {
        base.Start();

        requirementText.text = "";

        foreach (var cost in buildingCost)
        {
            string textToAdd;
            textToAdd = $"<sprite={resourceTypeSpriteIndex[cost.ResourceType]}> {cost.Cost} ";
            requirementText.text += textToAdd;
        }
    }

    public override void Interact()
    {
        if (!isInteractable) return;
        foreach(var cost in buildingCost)
        {
            if(GameResources.resourceTypes[cost.ResourceType] >= cost.Cost)
            {
                canBuild = true;
            }
            else
            {
                canBuild = false;
                break;
            }
        }

        if (!canBuild) return;

        currentProgress = Mathf.Clamp(currentProgress + 1, 0, maxProgress);
        
        progressBarList[currentProgress - 1].material = fullProgressMaterial;

        PlaySoundRandomPitch();

        foreach (var cost in buildingCost)
        {
            GameResources.ChangeResourceAmount(cost.ResourceType, -cost.Cost);
        }

        if (currentProgress == maxProgress)
        {
            HandleProgressFull();
        }
    }

    private void Update()
    {
        ShowInteractQuad(isInInteractRange && isInteractable);
    }
    protected abstract void HandleProgressFull();
    protected void DisableInteraction()
    {
        progress.gameObject.SetActive(false);
        interactQuadTransform.gameObject.SetActive(false);
        isInteractable = false;
    }
}
