using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class InteractableNode : MonoBehaviour, IInteractable
{
    [SerializeField] protected Transform interactQuadTransform;
    [SerializeField] protected TextMeshPro interactTextMesh;
    [SerializeField] protected string interactText;
    [SerializeField] protected List<AudioClip> interactSounds;

    protected AudioSource audioSource;
    protected bool isInteractable = true;
    protected bool isInInteractRange = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    protected virtual void Start()
    {
        interactTextMesh.text = interactText;
    }

    public abstract void Interact();
    public void ChangeIsInInteractRange(bool cond) => isInInteractRange = cond;
    protected void ShowInteractQuad(bool cond) => interactQuadTransform.gameObject.SetActive(cond);
    protected void PlaySoundRandomPitch()
    {
        audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        audioSource.clip = interactSounds[UnityEngine.Random.Range(0, interactSounds.Count - 1)];
        audioSource.Play();
    }
    public bool GetIsInteractable() => isInteractable;

}
