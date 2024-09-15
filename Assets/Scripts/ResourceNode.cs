using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum ResourceType
{
    Gold = 1,
    Wood = 2,
    Stone = 4,
    Iron = 8,
    Berry = 16,
}

public class ResourceNode : InteractableNode
{
    [SerializeField] private ResourceType resourceType;
    [SerializeField] private int maxResourceAmount;
    [SerializeField] private int currentResourceAmount;
    [SerializeField] private GameObject itemPickupPrefab;
    [SerializeField] private float resourceRegenTime;

    private bool canResource = true;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    protected override void Start()
    {
        base.Start();
        currentResourceAmount = maxResourceAmount;
    }
    private void Update()
    {
        ShowInteractQuad(isInInteractRange && canResource && isInteractable);
    }

    public override void Interact()
    {
        if (currentResourceAmount > 0 && canResource && isInteractable)
        {
            print($"INTERACTED: {name};{resourceType}");
            DecreaseFromCurrentResourceAmount();

            PlaySoundRandomPitch();

            InstantiateAndAddForceToItemPickup();

            if (currentResourceAmount <= 0)
            {
                canResource = false;
                StartCoroutine(RegenerateResource());
                isInteractable = false;
            }
        }
        else
        {
            //print($"{name} cannot be interacted");
        }
    }

    
    private void ChangeResourceAmount(int amount)
    {
        currentResourceAmount = Mathf.Clamp(currentResourceAmount + amount, 0, maxResourceAmount);
    }
    private void DecreaseFromCurrentResourceAmount() => ChangeResourceAmount(-1);
    private void IncreaseFromCurrentResourceAmount() => ChangeResourceAmount(1);
    private void InstantiateAndAddForceToItemPickup()
    {
        var pickup = Instantiate(itemPickupPrefab, transform.position, Quaternion.identity);
        var itemPickup = pickup.GetComponent<ItemPickup>();
        itemPickup.Init(resourceType);

        Vector3 randomOffset = new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), 1f, UnityEngine.Random.Range(-0.5f, 0.5f));
        pickup.transform.position += randomOffset;

        Rigidbody rb = pickup.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 randomDirection = new Vector3(UnityEngine.Random.Range(-1f, 1f), 1f, UnityEngine.Random.Range(-1f, 1f)).normalized;

            float jumpForce = 5f;
            rb.AddForce(randomDirection * jumpForce, ForceMode.Impulse);
        }
    }
    private IEnumerator RegenerateResource()
    {
        while(currentResourceAmount < maxResourceAmount)
        {
            yield return new WaitForSeconds(resourceRegenTime);
            IncreaseFromCurrentResourceAmount();
        }

        canResource = true;
        isInteractable = true;
    }
    public void RegenerateResourceImmediately()
    {
        StopCoroutine(RegenerateResource());

        currentResourceAmount = maxResourceAmount;
        canResource = true;
        isInteractable = true;
    }
}
