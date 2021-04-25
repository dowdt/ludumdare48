using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{



    float shakeDuration = 0f;

    [SerializeField]
    float shakeAmount = 1f;
    [SerializeField]
    float decreaseFactor = 3.0f;
    [SerializeField]
    float smooth = 20f;

    float zShake;

    public void Shake(float amount = 8f)
    {
        zShake = amount;
        shakeDuration = 1f;

    }


    void Update()
    {
        float zPos = Mathf.Lerp(0f,zShake* shakeAmount, shakeDuration); ;
        if (shakeDuration > 0)
        {
            shakeDuration -= Time.deltaTime * decreaseFactor;
        }

        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,transform.localEulerAngles.y, Mathf.Lerp(transform.localEulerAngles.z, zPos,Time.deltaTime* smooth));
    }
}
