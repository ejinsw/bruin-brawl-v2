using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject roarPrefab;
    private bool attackCalled = false;
    private float lastAttackTime;
    private bool cooldown = false;

    private void Update()
    {
        if (attackCalled && !cooldown)
        {
            SendRoar();
            lastAttackTime = Time.time;
            attackCalled = false;
            cooldown = true;
        }

        if (Time.time - lastAttackTime >= 1)
        {
            cooldown = false;
        }
    }

    private void SendRoar()
    {
        GameObject roar = Instantiate(roarPrefab, transform.position, transform.rotation);
        Animator anim = roar.GetComponent<Animator>();
        anim.Play("RoarAnim");

        float animLength = anim.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        StartCoroutine(DestroyRoarAfterAnimation(roar, animLength));
    }

    private IEnumerator DestroyRoarAfterAnimation(GameObject roar, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(roar);
    }

    private void OnAttack(InputValue inputValue)
    {
        if (inputValue.isPressed && !attackCalled)
        {
            attackCalled = true;
        }
    }
}
