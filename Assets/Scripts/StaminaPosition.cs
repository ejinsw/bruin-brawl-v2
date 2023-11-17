using UnityEngine;

public class StaminaPosition : MonoBehaviour
{
    [SerializeField] private Transform player;
    private void FixedUpdate()
    {
        float yOffset = 0.5f;
        
        Vector3 staminaPosition = new Vector3(player.position.x, player.position.y + yOffset, transform.position.z);
        transform.position = staminaPosition;
    }
}
