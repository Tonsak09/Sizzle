using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKLegPoseCopy : PoseCopy
{
    [SerializeField] List<Transform> legJoints;
    [SerializeField] Transform targetPoint;


    public override void UpdateTarget(Quaternion originalValue)
    {
        base.UpdateTarget(originalValue);
    }

}
