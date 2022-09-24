using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbCopy : MonoBehaviour
{
    [SerializeField] string animationGroup;
    [SerializeField] Transform boneTarget;
    [SerializeField] bool isConfigurableJoint;

    [SerializeField] Vector3 rotOffset;

    private ConfigurableJoint CJoint;


    /// <summary>
    /// Gets the targets current rotational value
    /// </summary>
    public Quaternion targetValue
    {
        get
        {
            if (isConfigurableJoint)
            {
                return CJoint.targetRotation;
            }
            else
            {
                return boneTarget.rotation;
            }
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        if(isConfigurableJoint)
        {
            CJoint = boneTarget.GetComponent<ConfigurableJoint>();
        }
    }

    public void UpdateTarget(Quaternion originalValue)
    {
        // Makes the target copy this bone rotations
        if(isConfigurableJoint)
        {
            CJoint.targetRotation = Quaternion.Inverse(Quaternion.Euler(this.transform.eulerAngles + rotOffset));
        }
        else
        {
            boneTarget.rotation = Quaternion.Inverse(this.transform.rotation);
        }
    }
}
