using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderMove : MonoBehaviour
{


    [Header("General - must be asigned")]

    [SerializeField]
    Rigidbody rb;

    [SerializeField]
    Transform body;

    [Header("Target")]

    [SerializeField]
    Transform target;

    [SerializeField]
    float Speed = 10;

    [SerializeField]
    float targetRange = 1;

    [SerializeField]
    float TurnSpeed = 5;

    [Header("Base Movement")]

    [SerializeField]
    float RotationSpeed = 10;

    [SerializeField]
    float wallClipRange = 3.5f;

    [SerializeField]
    LayerMask RayLayers;



    [SerializeField]
    float aboveGround = 3;

    [SerializeField]
    float aboveGroundTol = 0.3f;

    [SerializeField]
    float GroundPullForce = 30f;





    void FixedUpdate()
    {
        Move();


    }

    void Move() {
        Vector3 closetsNormal = Vector3.up;
        Quaternion closetsRot = Quaternion.LookRotation(Vector3.forward);
        Vector3 closetsPoint = transform.position - Vector3.up*10f;
        float closetsDistance = wallClipRange;

        RaycastHit hit;
        Vector3[] points = new Vector3[] { -transform.up, transform.up, -transform.right, transform.right, -transform.forward, transform.forward };
        foreach (Vector3 pos in points)
            if (Physics.Raycast(transform.position+body.forward, pos, out hit, wallClipRange, RayLayers) || Physics.Raycast(transform.position, pos, out hit, wallClipRange, RayLayers))
            {
                
                Vector3 point = hit.point;
                float dis = Vector3.Distance(point, transform.position);
                if (dis < closetsDistance)
                {
                    closetsDistance = dis;
                    closetsNormal = hit.normal;
                    closetsPoint = point;

                    if (pos == -transform.up)
                        break;
                }

            }

        Debug.DrawLine(transform.position, closetsPoint, Color.red);

        float distance = Vector3.Distance(closetsPoint, transform.position);
        if (distance < aboveGround)
            rb.AddForce(closetsNormal* GroundPullForce * Time.fixedDeltaTime, ForceMode.VelocityChange);
        if (distance > aboveGround+ aboveGroundTol)
            rb.AddForce(-closetsNormal * GroundPullForce * Time.fixedDeltaTime, ForceMode.VelocityChange);

        rb.AddForce(-closetsNormal * GroundPullForce * Random.Range(0.1f,-0.1f) * Time.fixedDeltaTime, ForceMode.VelocityChange);

        Quaternion rot = Quaternion.FromToRotation(Vector3.up, closetsNormal);


        transform.rotation = Quaternion.Lerp(transform.rotation,rot, RotationSpeed * Time.fixedDeltaTime);


        if (target == null)
            return;

        if (Vector3.Distance(transform.position,new Vector3(target.position.x,transform.position.y,target.position.z)) < targetRange)
            return;

        Vector3 dir = (target.position - transform.position).normalized;
        body.rotation = Quaternion.Lerp(body.rotation, Quaternion.LookRotation(dir,Vector3.up),Time.fixedDeltaTime * TurnSpeed);

        
        rb.AddForce(body.forward * Time.fixedDeltaTime * Speed *10, ForceMode.Acceleration);

        body.localEulerAngles = new Vector3(0f, body.localEulerAngles.y, 0f);

     
    }
}
