using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Transform interactionStartTransform;
    [SerializeField] private Transform interactionEndTransform;
    [SerializeField] private float radius = 0.5f;
    [SerializeField] private Animator animator;

    private List<InteractableNode> interactableNodesInRange = new List<InteractableNode>();

    private void Update()
    {
        foreach (var node in interactableNodesInRange)
        {
            node.ChangeIsInInteractRange(false);
        }

        interactableNodesInRange.Clear();

        Collider[] colliders = Physics.OverlapCapsule(interactionStartTransform.position, interactionEndTransform.position, radius);
        foreach (Collider collider in colliders)
        {
            if(collider.TryGetComponent(out InteractableNode interactableNode))
            {
                interactableNode.ChangeIsInInteractRange(true);
                interactableNodesInRange.Add(interactableNode);

                if (GameManager.Instance.GetIsInputActive() && Input.GetKeyDown(KeyCode.E))
                {
                    interactableNode.Interact();
                    if (interactableNode.GetIsInteractable())
                    {
                        animator.SetTrigger("Pick");
                    }
                }
            }

            //if (collider.TryGetComponent(out IInteractable interactable) && Input.GetKeyDown(KeyCode.E))
            //{
            //    interactable.Interact();
            //    animator.SetTrigger("Pick");
            //}
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionStartTransform.position, radius);
        Gizmos.DrawWireSphere(interactionEndTransform.position, radius);
    }
}
