using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackPosition : MonoBehaviour
{
    [SerializeField] private Transform childObject;
    [SerializeField] private Transform parentObject;

    [SerializeField] private float distanceX = 0.2f;
    [SerializeField] private float distanceY = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        transform.localPosition = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if (x < 0)
        {
            childObject.position = new Vector3(parentObject.position.x + distanceX * x, parentObject.position.y, 0);
            childObject.eulerAngles = new Vector3(0, 0, 90);
        }
        else if (x > 0)
        {
            childObject.position = new Vector3(parentObject.position.x + distanceX * x, parentObject.position.y, 0);
            childObject.eulerAngles = new Vector3(0, 0, -90);
        }
        else if (y > 0)
        {
            childObject.position = new Vector3(parentObject.position.x, parentObject.position.y + distanceY + 0.2f, 0);
            childObject.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (y < 0)
        {
            childObject.position = new Vector3(parentObject.position.x, parentObject.position.y - distanceY, 0);
            childObject.eulerAngles = new Vector3(0, 0, 180);
        }
    }
}
