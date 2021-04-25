using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderLeg : MonoBehaviour
{


    [SerializeField]
    float stepDis = 1;

    [SerializeField]
    float legLen = 4;

    [SerializeField]
    float stepHeight = 0.7f;

    [SerializeField]
    float stepSpeed = 5f;

    [SerializeField]
    Transform Target;
    [SerializeField]
    Transform RayFrom;
    [SerializeField]
    LayerMask RayLayers;

    Vector3 targetPos;

    Vector3 lastPos;

    float lerp = 0;

    void Update()
    {
        Debug.DrawLine(transform.position, Target.position, Color.cyan);
        


        RaycastHit hit;
        Vector3 bestPos = RayFrom.position - RayFrom.up * legLen;
        if (Physics.SphereCast(RayFrom.position,0.2f, -RayFrom.up, out hit, legLen, RayLayers))
        {

            bestPos = hit.point + RayFrom.up * 0.01f;

            Debug.DrawLine(transform.position, bestPos, Color.blue);

            Vector3 offset = (lastPos - bestPos).normalized;
    

            if (((Vector3.Distance(bestPos, targetPos) > stepDis || !isTargetOnFloor()) && (group != null ? group.EnoughFeetOnGroundToDoStep() : true)))
            {
                lerp = 0f;
                targetPos = bestPos;
            }
      


        }
        else
        {
            lerp = 1;

            targetPos = bestPos;
        }



        if (lerp < 1)
        {
            Vector3 pos = Vector3.Lerp(lastPos, targetPos, lerp);
            pos += transform.up * Mathf.Sin(lerp * Mathf.PI) * stepHeight;

            Target.position = pos;
            lerp += Time.deltaTime * stepSpeed;
        }
        else
        {
            Target.position = targetPos;
            lastPos = targetPos;
        }



    }


    public bool isFootOnTarget()
    {

        return (lerp >= 1);
    }
    public bool isTargetOnFloor()
    {

        return (Physics.OverlapSphere(targetPos, 0.1f, RayLayers).Length > 0);
    }


    void Start()
    {
        RaycastHit hit;
        Target.position = RayFrom.position - RayFrom.up * legLen;
        if (Physics.Raycast(RayFrom.position, -RayFrom.up, out hit, legLen, RayLayers))
        {
            Target.position = hit.point + RayFrom.up * 0.01f;

        }

        lastPos = Target.position;
        targetPos = Target.position;

        if (group != null)
            group.legs.Add(this);

    }


    public SpiderLegGroup group;


  
}
