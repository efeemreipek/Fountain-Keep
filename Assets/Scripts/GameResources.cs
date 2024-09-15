using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameResources
{
    public static event Action<ResourceType, int> OnResourceChanged;

    public static Dictionary<ResourceType, int> resourceTypes = new Dictionary<ResourceType, int> 
    {
        {ResourceType.Gold, 0 },
        {ResourceType.Wood, 0 },
        {ResourceType.Stone, 0 },
        {ResourceType.Iron, 0 },
        {ResourceType.Berry, 0 },
    };

    public static int maxValue = 9999;

    public static void ChangeResourceAmount(ResourceType resourceType, int amount)
    {
        if (resourceTypes.ContainsKey(resourceType))
        {
            resourceTypes[resourceType] = Mathf.Clamp(resourceTypes[resourceType] + amount, 0, maxValue);
        }
        OnResourceChanged?.Invoke(resourceType, resourceTypes[resourceType]);
    }
}
