using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    public static SceneFader instance;
    public Image image;
    public AnimationCurve animcurve;


    void Start()
    {

        instance = this;

        FadeIn();
        Time.timeScale = 1f;

    }
    //turn screen off
    public void FadeIn()
    {
        StartCoroutine(fadeIn());
    }
    //turns screen on
    public void FadeOut()
    {
        StartCoroutine(fadeIn());
    }

    public void fadeToNewScene(string scene)
    {

        StartCoroutine(fadeOut(scene));
    }





    IEnumerator fadeOut(string ToScene)
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * 1.2f;
            image.color = new Color(0f, 0f, 0f, animcurve.Evaluate(t));
            yield return 0;

        }

        SceneManager.LoadScene(ToScene);


    }


    IEnumerator fadeIn()
    {
        float t = 1f;

        while (t > 0f)
        {
            t -= Time.deltaTime * 1.2f;
            image.color = new Color(0f, 0f, 0f, animcurve.Evaluate(t));
            yield return 0;

        }


    }
}
