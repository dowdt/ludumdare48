using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterfaceManager : MonoBehaviour
{
    [SerializeField]
    CanvasGroup[] windows;
    [SerializeField]
    AnimationCurve FadeAnimationCurve;

    public int currentWindow = -1;

    public void openWindow(int i) {
        StopCoroutine("ToScreen");

        StartCoroutine(ToScreen(i));
    }

    private void Start()
    {
        if (currentWindow >= 0)
            openWindow(currentWindow);
    }

    IEnumerator ToScreen(int i)
    {

        if (currentWindow >= 0)
        {
            float t = 1f;

            while (t > 0f)
            {
                t -= Time.deltaTime * 1f;
                float a = FadeAnimationCurve.Evaluate(t);
                windows[currentWindow].alpha = Mathf.Clamp(a, 0, 1);
                yield return 0;

            }
            windows[currentWindow].gameObject.SetActive(false);
        }

        currentWindow = i;


        if (currentWindow >= 0)
        {

         
            windows[currentWindow].gameObject.SetActive(true);
            float t = 0f;

            while (t < 1f)
            {
                t += Time.deltaTime * 1f;
                float a = FadeAnimationCurve.Evaluate(t);
      
                windows[currentWindow].alpha = Mathf.Clamp(a,0,1);
                yield return 0;

            }
        }

    }

    

}

