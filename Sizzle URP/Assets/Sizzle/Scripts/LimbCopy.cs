using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbCopy : MonoBehaviour
{
    [SerializeField] string animationGroup;
    [SerializeField] Transform boneTarget;
    [SerializeField] bool isConfigurableJoint;

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
    void Start()
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
            CJoint.targetRotation = this.transform.rotation;
        }
        else
        {

        }
    }
}
