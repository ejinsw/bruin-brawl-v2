using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GrabPosition : MonoBehaviour
{
    [SerializeField] private Transform childObject;
    [SerializeField] private Transform parentObject;

    [SerializeField] private float distanceX = 0.2f;
    [SerializeField] private float distanceY = 0.2f;

    void Start()
    {
        transform.localPosition = new Vector3(0, 0, 0);
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        
        if (x != 0)
        {
            childObject.position = new Vector3(parentObject.position.x + distanceX * x, parentObject.position.y, 0);
        }
        else if (y > 0)
        {
            childObject.position = new Vector3(parentObject.position.x, parentObject.position.y + distanceY + 0.2f, 0);
        }
        else if (y < 0)
        {
            childObject.position = new Vector3(parentObject.position.x, parentObject.position.y - distanceY, 0);
        }
    }
}
