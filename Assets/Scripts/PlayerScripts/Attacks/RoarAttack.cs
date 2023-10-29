using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoarAttack : MonoBehaviour
{

    private Animator animator;
    public static RoarAttack instance;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void StartRoar()
    {
        StartCoroutine(StartAttack());
    }

    private IEnumerator StartAttack()
    {
        animator.Play("RoarGrow");
        new WaitForSeconds(0.5f * Time.deltaTime);
        animator.Play("RoarShrink");
        yield return null;
    }
}
