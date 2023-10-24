using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StaminaBar : MonoBehaviour
{
    public Slider staminaBar;
    private float maxStamina = 100f;
    private float currentStamina;

    private WaitForSeconds regenTick = new WaitForSeconds(0.25f);
    private  Coroutine regen;

    public static StaminaBar instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentStamina = maxStamina;
        staminaBar.maxValue = maxStamina;
        staminaBar.value = maxStamina;
    }

    public void UseStamina(float amount)
    {
        if (currentStamina - amount >= 0f)
        {
            currentStamina -= amount;
            staminaBar.value = currentStamina;


            if (regen != null)
            {
                StopCoroutine(regen);
            }
            regen = StartCoroutine(RegenStamina());
        }
        else
        {
            Debug.Log("Not enough stamina!");
        }
    }

    private IEnumerator RegenStamina()
    {
        yield return new WaitForSeconds(0.5f); 

        while(currentStamina < maxStamina)
        {
            currentStamina += maxStamina / 20;
            staminaBar.value = currentStamina;
            yield return regenTick;
        }
        regen = null;
    }

    public float GetCurrentStamina()
    {
        return currentStamina;
    }
}
