using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFabrik : MonoBehaviour
{

    public int ChainLength = 2;


    public Transform Target;
    public Transform PullTowards;


    [Header("Solve")]
    public int Iterations = 10;

    public float Delta = 0.001f;


    [Range(0, 1)]
    public float SnapBackStrength = 1f;



    float CompleteLength;
    Transform[] Legs;

    Vector3[] Points;
    Vector3[] StartDirectionSucc;
    Quaternion[] StartRotationBone;
    Quaternion StartRotationTarget;
    Transform RootTranform;
    float[] LengthsOfLegs;

    //init stuff
    void Awake()
    {
        InitValues();
    }
    //update after other update
    void LateUpdate()
    {

        if (LengthsOfLegs.Length != ChainLength || Target == null)
        {
            InitValues();
            return;
        }
        DoFabrikIK();
    }


    void InitValues()
    {

        if (Target == null)
        {
            Debug.LogError("No target!");
            return;
        }


        Legs = new Transform[ChainLength + 1];
        Points = new Vector3[ChainLength + 1];
        LengthsOfLegs = new float[ChainLength];
        StartDirectionSucc = new Vector3[ChainLength + 1];
        StartRotationBone = new Quaternion[ChainLength + 1];

        
        RootTranform = transform;
        for (var i = 0; i <= ChainLength; i++)
        {
            if (RootTranform == null)
                Debug.LogError("ChainLength is too high!");
            else
                RootTranform = RootTranform.parent;
        }

        StartRotationTarget = GetRotationRootSpace(Target);



        //init data
        var current = transform;
        CompleteLength = 0;
        for (var i = Legs.Length - 1; i >= 0; i--)
        {
            Legs[i] = current;
            StartRotationBone[i] = GetRotationRootSpace(current);

            if (i == Legs.Length - 1)
            {
                //leaf
                StartDirectionSucc[i] = GetPositionRootSpace(Target) - GetPositionRootSpace(current);
            }
            else
            {
                //mid bone
                StartDirectionSucc[i] = GetPositionRootSpace(Legs[i + 1]) - GetPositionRootSpace(current);
                LengthsOfLegs[i] = StartDirectionSucc[i].magnitude;
                CompleteLength += LengthsOfLegs[i];
            }

            current = current.parent;
        }



    }


    private void DoFabrikIK()
    {
        //fabric IK thing

        //setup points

        for (int i = 0; i < Legs.Length; i++)
            Points[i] = GetPositionRootSpace(Legs[i]);

        Vector3 targetPosition = GetPositionRootSpace(Target);
        Quaternion targetRotation = GetRotationRootSpace(Target);

        //check if can reach
        if (Vector3.Distance(targetPosition,GetPositionRootSpace(Legs[0])) >= CompleteLength)
        {

            Vector3 direction = (targetPosition - Points[0]).normalized;
            for (int i = 1; i < Points.Length; i++)
                Points[i] = Points[i - 1] + direction * LengthsOfLegs[i - 1];
        }
        else
        {
            //do fabrik
            for (int i = 0; i < Points.Length - 1; i++)
                Points[i + 1] = Vector3.Lerp(Points[i + 1], Points[i] + StartDirectionSucc[i], SnapBackStrength);

            //loop through unity target fond or out of time
            for (int iteration = 0; iteration < Iterations; iteration++)
            {
 
                //backwards
                for (int i = Points.Length - 1; i > 0; i--)
                {
                    if (i == Points.Length - 1)
                        Points[i] = targetPosition; 
                    else
                        Points[i] = Points[i + 1] + (Points[i] - Points[i + 1]).normalized * LengthsOfLegs[i]; 
                }

            
                //forwards
                for (int i = 1; i < Points.Length; i++)
                    Points[i] = Points[i - 1] + (Points[i] - Points[i - 1]).normalized * LengthsOfLegs[i - 1];


                //check if close enough
                if ((Points[Points.Length - 1] - targetPosition).sqrMagnitude < Delta * Delta)
                    break;
            }
        }

        //putt leg towards pull towards
        if (PullTowards != null)
        {
            var polePosition = GetPositionRootSpace(PullTowards);
            for (int i = 1; i < Points.Length - 1; i++)
            {
                var plane = new Plane(Points[i + 1] - Points[i - 1], Points[i - 1]);
                var projectedPole = plane.ClosestPointOnPlane(polePosition);
                var projectedBone = plane.ClosestPointOnPlane(Points[i]);
                var angle = Vector3.SignedAngle(projectedBone - Points[i - 1], projectedPole - Points[i - 1], plane.normal);
                Points[i] = Quaternion.AngleAxis(angle, plane.normal) * (Points[i] - Points[i - 1]) + Points[i - 1];
            }
        }

        //points to transfarm
        for (int i = 0; i < Points.Length; i++)
        {
            if (i == Points.Length - 1)
                SetRotationRootSpace(Legs[i], Quaternion.Inverse(targetRotation) * StartRotationTarget * Quaternion.Inverse(StartRotationBone[i]));
            else
                SetRotationRootSpace(Legs[i], Quaternion.FromToRotation(StartDirectionSucc[i], Points[i + 1] - Points[i]) * Quaternion.Inverse(StartRotationBone[i]));
            SetPositionRootSpace(Legs[i], Points[i]);
        }
    }

    private Vector3 GetPositionRootSpace(Transform current)
    {
        if (RootTranform == null)
            return current.position;
        else
            return Quaternion.Inverse(RootTranform.rotation) * (current.position - RootTranform.position);
    }

    private void SetPositionRootSpace(Transform current, Vector3 position)
    {
        if (RootTranform == null)
            current.position = position;
        else
            current.position = RootTranform.rotation * position + RootTranform.position;
    }

    private Quaternion GetRotationRootSpace(Transform current)
    {
        //inverse(after) * before => rot: before -> after
        if (RootTranform == null)
            return current.rotation;
        else
            return Quaternion.Inverse(current.rotation) * RootTranform.rotation;
    }

    private void SetRotationRootSpace(Transform current, Quaternion rotation)
    {
        if (RootTranform == null)
            current.rotation = rotation;
        else
            current.rotation = RootTranform.rotation * rotation;
    }

    void OnDrawGizmos()
    {
        //deub thing
        #if UNITY_EDITOR
        var current = this.transform;
        for (int i = 0; i < ChainLength && current != null && current.parent != null; i++)
        {
            Gizmos.DrawLine(current.parent.position, current.position);
            current = current.parent;
        }
        #endif
    }


}
