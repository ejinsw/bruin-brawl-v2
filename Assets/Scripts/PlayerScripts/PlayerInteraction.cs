using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Transform grabPoint;
    [SerializeField] private Transform rayPoint;
    [SerializeField] private float rayDistance;

    [SerializeField] private DialogueUI dialogueUI;
    public DialogueUI DialogueUI => dialogueUI;
    public IInteractable Interactable { get; set; }


    private Vector3 rayDirection;
    private GameObject grabbedObject;
    private int objectsLayer;
    private int interactableLayer;


    void Start()
    {
        objectsLayer = LayerMask.NameToLayer("Objects");
        interactableLayer = LayerMask.NameToLayer("Interactable");
    }

    private void OnInteract(InputValue inputValue)
    {
        // Get Direction
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        
        if (x > 0)
        {
            rayDirection = transform.right;
        }
        else if (x < 0)
        {
            rayDirection = -transform.right;
        }
        else if (y > 0)
        {
            rayDirection = transform.up;
        }
        else if (y < 0)
        {
            rayDirection = -transform.up;
        }

        // Check Collision
        RaycastHit2D hitInfo = Physics2D.Raycast(rayPoint.position, rayDirection, rayDistance, LayerMask.GetMask("Objects", "Interactable"));
        Debug.DrawRay(rayPoint.position, rayDirection * rayDistance);

        // Object Pickup Interaction
        if (hitInfo.collider != null && hitInfo.collider.gameObject.layer == objectsLayer || grabbedObject != null)
        {
            if (inputValue.isPressed && grabbedObject == null)
            {
                grabbedObject = hitInfo.collider.gameObject;
                grabbedObject.GetComponent<Rigidbody2D>().isKinematic = true;
                grabbedObject.GetComponent<Rigidbody2D>().simulated = false;
                StartCoroutine(SetPosition());
                grabbedObject.transform.SetParent(transform);
            }
            else if (inputValue.isPressed)
            {
                grabbedObject.GetComponent<Rigidbody2D>().isKinematic = false;
                grabbedObject.GetComponent<Rigidbody2D>().simulated = true;
                grabbedObject.transform.SetParent(null);
                grabbedObject = null;
                StopCoroutine(SetPosition());
            }
        }


        // Dialogue Interaction
        if (hitInfo.collider != null && hitInfo.collider.gameObject.layer == interactableLayer)
        {
            if (inputValue.isPressed && !dialogueUI.IsOpen)
            {
                Interactable?.Interact(this);
            }
        }
    }

    private IEnumerator SetPosition()
    {
        while (grabbedObject != null)
        {
            grabbedObject.transform.position = grabPoint.position;

            yield return null;
        }
    }
}
