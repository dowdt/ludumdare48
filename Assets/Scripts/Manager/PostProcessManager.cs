using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessManager : MonoBehaviour
{
    [SerializeField]
    PostProcessVolume pp;
    

    Vignette vignette;
    private void Update()
    {
       
        float vignetteIntensity = (0.25f + 0.25f * (1f - 1f/GameManager.playerInstance.maxHealth*GameManager.playerInstance.health));

        if (vignette)
            vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, vignetteIntensity, Time.deltaTime*10f);
        else
            pp.profile.TryGetSettings(out vignette);
    }





    public static PostProcessManager instance;


    private void Awake()
    {
       
        instance = this;
    }
}
