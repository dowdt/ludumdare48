using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAnim : MonoBehaviour
{
    [Header("Bob")]
    [SerializeField]
    float BobAmount = 0.02f;
    [SerializeField]
    float BobSpeed = 1.4f;
    [SerializeField]
    Movement playerMove;
    [Header("Sway")]
    [SerializeField]
    float SwayAmount = 0.02f;
    [SerializeField]
    float SwayMaxAmount = 0.1f;
    [Header("General")]
    [SerializeField]
    float Smoothing = 6f;

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.localPosition;
    }

    float time = 0f;
    void LateUpdate()
    {

        //bob
        time += BobSpeed * (playerMove.GetVerticalMove()) * Time.deltaTime;
        float yOffset = (Mathf.Sin(time) * BobAmount);
        
    
        //sway
        float mouseMoveX = -Input.GetAxis("Mouse X") * SwayAmount;
        float mouseMoveY = -Input.GetAxis("Mouse Y") * SwayAmount;

        if (!InGameUserInterface.instance.canMove())
        {
            mouseMoveX = 0f;
            mouseMoveY = 0f;
        }

        mouseMoveX = Mathf.Clamp(mouseMoveX, -SwayMaxAmount, SwayMaxAmount);
        mouseMoveY = Mathf.Clamp(mouseMoveY, -SwayMaxAmount, SwayMaxAmount);

        //adding effects
        Vector3 offsetPosition = new Vector3(mouseMoveX, mouseMoveY + yOffset, 0f);
        transform.localPosition = Vector3.Lerp(transform.localPosition, offsetPosition + initialPosition, Time.deltaTime * Smoothing);



    }
}
