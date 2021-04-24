using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderLegGroup : MonoBehaviour
{
    public List<SpiderLeg> legs = new List<SpiderLeg>();

    [Range(1,10)]
    public int StepsAtOnce = 2;

    public int FeetOnFround()
    {
        int i = 0;
        foreach (SpiderLeg l in legs)
        {
            if (l.isFootOnTarget())
                i++;
        }

        return i;
    }


    public bool EnoughFeetOnGroundToDoStep()
    {
        return (FeetOnFround() > legs.Count - StepsAtOnce);

    }
}
