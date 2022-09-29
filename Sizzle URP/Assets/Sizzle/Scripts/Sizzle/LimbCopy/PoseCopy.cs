using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoseCopy : MonoBehaviour
{
    [Tooltip("What bone is being influnced by this object's direction")]
    [SerializeField] protected Transform boneTarget;
    [SerializeField] protected Vector3 rotOffset;

    public virtual Quaternion TargetValue
    {
        get
        {
            return boneTarget.rotation;
        }
    }

    /// <summary>
    /// Sets the rotation of the target to this rotation 
    /// </summary>
    /// <param name="originalValue"></param>
    public virtual void UpdateTarget(Quaternion originalValue)
    {
        boneTarget.rotation = Quaternion.Inverse(this.transform.rotation);
    }
}
