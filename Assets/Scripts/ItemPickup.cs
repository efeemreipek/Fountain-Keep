using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour, IPickable
{
    private ResourceType resourceType;

    private List<GameObject> meshGameObjects = new List<GameObject>();

    private void Awake()
    {
        var meshGO = transform.GetChild(0);
        foreach(Transform child in meshGO)
        {
            meshGameObjects.Add(child.gameObject);
        }
    }

    public void Init(ResourceType resourceType)
    {
        this.resourceType = resourceType;

        switch (resourceType)
        {
            case ResourceType.Gold:
                meshGameObjects[0].SetActive(true);
                break;
            case ResourceType.Wood:
                meshGameObjects[1].SetActive(true);
                break;
            case ResourceType.Stone:
                meshGameObjects[2].SetActive(true);
                break;
            case ResourceType.Iron:
                meshGameObjects[3].SetActive(true);
                break;
            case ResourceType.Berry:
                meshGameObjects[4].SetActive(true);
                break;
        }
    }

    public void Pick()
    {
        print($"PICKED: {name}; {resourceType}");

        switch (resourceType)
        {
            case ResourceType.Gold:
                GameResources.ChangeResourceAmount(ResourceType.Gold, 1);
                break;
            case ResourceType.Wood:
                GameResources.ChangeResourceAmount(ResourceType.Wood, 2);
                break;
            case ResourceType.Stone:
                GameResources.ChangeResourceAmount(ResourceType.Stone, 3);
                break;
            case ResourceType.Iron:
                GameResources.ChangeResourceAmount(ResourceType.Iron, 2);
                break;
            case ResourceType.Berry:
                GameResources.ChangeResourceAmount(ResourceType.Berry, 3);
                break;
        }

        Destroy(gameObject);
    }
}
