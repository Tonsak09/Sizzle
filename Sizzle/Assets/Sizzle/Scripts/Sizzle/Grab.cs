using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{


    [Header("Neck")]
    [SerializeField] ConfigurableJoint neckJoint;

    [SerializeField] Vector3 neckDefaultRot;
    [SerializeField] Vector3 neckTargetRot;

    [SerializeField] AnimationCurve neckOpenAnimCurve;
    [SerializeField] float neckOpenSpeed;

    [SerializeField] AnimationCurve neckCloseAnimCurve;
    [SerializeField] float neckCloseSpeed;

    [Header("Jaw")]
    [SerializeField] ConfigurableJoint jawJoint;

    [SerializeField] Vector3 jawDefaultRot;
    [SerializeField] Vector3 jawTargetRot;

    [SerializeField] AnimationCurve jawOpenAnimCurve;
    [SerializeField] float JawOpenSpeed;

    [SerializeField] AnimationCurve jawCloseAnimCurve;
    [SerializeField] float JawCloseSpeed;

    [Header("Grabbing")]
    [Tooltip("The threshold the lerp must surpass before Sizzle can grab anything")]
    [SerializeField] float jawLerpThreshold;

    [SerializeField] Vector3 detectStartOffset;
    [SerializeField] Vector3 detectTargetOffset;
    [SerializeField] AnimationCurve detectOffsetAnimCurve;
    [SerializeField] float detectOffsetSpeed;

    [SerializeField] Vector3 detectStartSize;
    [SerializeField] Vector3 detectTargetSize;


    // The lerp between a mouth open and closed 
    private float neckLerp;
    private float jawLerp;

    private void Update()
    {
        AnimateJaw();

        if(Input.GetMouseButtonDown(1))
        {
            if (jawLerp >= jawLerpThreshold)
            {

            }
        }
    }

    private void AnimateJaw()
    {
        // On right click hold 
        if (Input.GetMouseButton(1))
        {
            // Continues unless fully open 
            if (neckLerp < 1)
            {
                neckLerp += neckOpenSpeed * Time.deltaTime;
            }
            if (jawLerp < 1)
            {
                jawLerp += JawOpenSpeed * Time.deltaTime;
            }

            // Applys the rotation 
            neckJoint.targetRotation = Quaternion.Euler(Vector3.Lerp(neckDefaultRot, neckTargetRot, neckOpenAnimCurve.Evaluate(neckLerp)));
            jawJoint.targetRotation = Quaternion.Euler(Vector3.Lerp(jawDefaultRot, jawTargetRot, jawOpenAnimCurve.Evaluate(jawLerp)));
        }
        else
        {
            // Continues unless fully closed 
            if (neckLerp > 0)
            {
                neckLerp -= neckCloseSpeed * Time.deltaTime;
            }
            if (jawLerp > 0)
            {
                jawLerp -= neckOpenSpeed * Time.deltaTime;
            }

            // Applys the rotation 
            neckJoint.targetRotation = Quaternion.Euler(Vector3.Lerp(neckDefaultRot, neckTargetRot, neckCloseAnimCurve.Evaluate(neckLerp)));
            jawJoint.targetRotation = Quaternion.Euler(Vector3.Lerp(jawDefaultRot, jawTargetRot, jawCloseAnimCurve.Evaluate(jawLerp)));
        }
    }
}
