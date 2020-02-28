using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Fade: MonoBehaviour
{
    public static IEnumerator FadeTo(float aValue, float aTime, Image fadeImg)
    {
        float alpha = fadeImg.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(0, 0, 0, Mathf.Lerp(alpha, aValue, t));
            fadeImg.color = newColor;
            yield return null;
        }
    }

    public static IEnumerator FadeTo(float aValue, float aTime, TMPro.TextMeshProUGUI fadeTxt)
    {
        float alpha = fadeTxt.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color textColor = new Color(1, 0.45f, 0, Mathf.Lerp(alpha, aValue, t));
            fadeTxt.color = textColor;
            yield return null;
        }
    }

}