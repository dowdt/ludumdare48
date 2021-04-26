using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{

    private void OnTriggerStay(Collider collider)
    {
        if(collider.gameObject == GameManager.playerInstance.gameObject)
            GameManager.instance.PlayerWin();
    }
}
