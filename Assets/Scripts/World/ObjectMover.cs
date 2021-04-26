using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    [SerializeField]
    Transform pos1;

    [SerializeField]
    Transform pos2;


    [SerializeField]
    float speed = 1f;

    float lerp = 0;
    bool swotch;

    private void Update()
    {
        transform.position = Vector3.Lerp(pos1.position,pos2.position,lerp);
        lerp += Time.deltaTime * speed * (swotch ? -1 : 1);

        if(lerp > 1)
            swotch = true;
        if (lerp < 0)
            swotch = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(pos1.position, pos2.position);
    }
}
