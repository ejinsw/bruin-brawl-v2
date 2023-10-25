using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GrabObject : MonoBehaviour
{
    [SerializeField] private Transform grabPoint;
    [SerializeField] private Transform rayPoint;
    [SerializeField] private float rayDistance;


    private Vector3 rayDirection;
    private GameObject grabbedObject;
    private int layerIndex;
    // Start is called before the first frame update
    void Start()
    {
        layerIndex = LayerMask.NameToLayer("Objects");
    }

    // Update is called once per frame
    void Update()
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
        RaycastHit2D hitInfo = Physics2D.Raycast(rayPoint.position, rayDirection, rayDistance, LayerMask.GetMask("Objects"));

        // Pick up and drop Object
        if (hitInfo.collider != null && hitInfo.collider.gameObject.layer == layerIndex || grabbedObject != null)
        {
            if (Input.GetKeyDown(KeyCode.Space) && grabbedObject == null)
            {
                grabbedObject = hitInfo.collider.gameObject;
                grabbedObject.GetComponent<Rigidbody2D>().isKinematic = true;
                grabbedObject.GetComponent<Rigidbody2D>().simulated = false;
                StartCoroutine(SetPosition());
                grabbedObject.transform.SetParent(transform);
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                grabbedObject.GetComponent<Rigidbody2D>().isKinematic = false;
                grabbedObject.GetComponent<Rigidbody2D>().simulated = true;
                grabbedObject.transform.SetParent(null);
                grabbedObject = null;
                StopCoroutine(SetPosition());
            }
        }
        Debug.DrawRay(rayPoint.position, rayDirection * rayDistance);
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
