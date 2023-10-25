using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeStamina : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private bool isFading = false;
    private StaminaBar stamina;
    private Coroutine coroutine;

    private void Start()
    {
        stamina = StaminaBar.instance;
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1.0f;
    }

    private void Update()
    {
        if (stamina.GetCurrentStamina() >= 100f && !isFading)
        {
            coroutine = StartCoroutine(FadeOut(true));
        }
        else if (stamina.GetCurrentStamina() < 100f && isFading)
        {
            coroutine = StartCoroutine(FadeOut(false));
        }
    }

    private IEnumerator FadeOut(bool fade)
    {
        if (fade)
        {
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                canvasGroup.alpha = Mathf.Clamp01(i);
                yield return null;
                if (stamina.GetCurrentStamina() < 100f)
                    break;
            }
            isFading = true;
        }
        else 
        {
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                canvasGroup.alpha = Mathf.Clamp01(i);
                yield return null;
            }
            isFading = false;
        }
    }
}
