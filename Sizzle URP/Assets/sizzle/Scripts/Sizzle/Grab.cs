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

    [SerializeField] float closedJawLerp;

    [Header("Grabbing")]
    [Tooltip("The threshold the lerp must surpass before Sizzle can grab anything")]
    [SerializeField] Vector2 jawLerpRange;

    [SerializeField] Vector3 detectStartOffset;
    [SerializeField] Vector3 detectTargetOffset;
    [SerializeField] AnimationCurve detectOffsetAnimCurve;
    [SerializeField] float detectSpeed;

    [SerializeField] float detectStartSize;
    [SerializeField] float detectTargetSize;

    [SerializeField] LayerMask grabbable;


    // The lerp between a mouth open and closed 
    private float neckLerp;
    private float jawLerp;
    private float detectLerp;
    

    private Transform heldItem;

    private void Update()
    {
        AnimateJaw();
        GrabLogic();
    }

    private void GrabLogic()
    {
        if (Input.GetMouseButton(1))
        {
            // Grabbing an item 
            if (heldItem == null)
            {
                // Makes sure it's within range
                if (jawLerp >= jawLerpRange.x && jawLerp <= jawLerpRange.y)
                {
                    Collider[] grabbables = Physics.OverlapSphere
                        (
                            neckJoint.transform.position +
                            neckJoint.transform.TransformDirection(Vector3.Lerp(detectStartOffset, detectTargetOffset, detectOffsetAnimCurve.Evaluate(detectLerp))),
                            Maths.Lerp(detectStartSize, detectTargetSize, detectLerp),
                            grabbable
                        );

                    if (grabbables.Length > 0)
                    {


                        heldItem = grabbables[0].transform;

                        // Sets held objects params so physics don't mess with one another 
                        heldItem.GetComponent<Grabbable>().SetGrabActive();
                        heldItem.GetComponent<Rigidbody>().isKinematic = true;
                        heldItem.GetComponent<Buoyancy>().enabled = false;

                        heldItem.transform.parent = jawJoint.transform;
                    }
                }
            }
            else
            {
                // Grabbed hold postion
                jawLerp = closedJawLerp;
                jawJoint.targetRotation = Quaternion.Euler(Vector3.Lerp(jawDefaultRot, jawTargetRot, jawOpenAnimCurve.Evaluate(jawLerp)));
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            // Grabbing an item 
            if (heldItem != null)
            {
                // Throwing an object 
                heldItem.parent = null;

                // Reactivates parts of obj so can act as normal 
                heldItem.GetComponent<Grabbable>().SetNonGrabActive();
                heldItem.GetComponent<Rigidbody>().isKinematic = false;
                heldItem.GetComponent<Buoyancy>().enabled = true;

                heldItem.eulerAngles = new Vector3(0, heldItem.eulerAngles.y, 0);

                heldItem = null;
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
            if(detectLerp < 1)
            {
                detectLerp += detectSpeed * Time.deltaTime;
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
                jawLerp -= JawCloseSpeed * Time.deltaTime;
            }
            if (detectLerp > 0)
            {
                detectLerp -= detectSpeed * Time.deltaTime;
            }

            // Applys the rotation 
            neckJoint.targetRotation = Quaternion.Euler(Vector3.Lerp(neckDefaultRot, neckTargetRot, neckCloseAnimCurve.Evaluate(neckLerp)));
            jawJoint.targetRotation = Quaternion.Euler(Vector3.Lerp(jawDefaultRot, jawTargetRot, jawCloseAnimCurve.Evaluate(jawLerp)));
        }
    }

    private void OnDrawGizmos()
    {
        if (jawLerp >= jawLerpRange.x && jawLerp <= jawLerpRange.y)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }

        Gizmos.DrawWireSphere(neckJoint.transform.position + 
            neckJoint.transform.TransformDirection(Vector3.Lerp(detectStartOffset, detectTargetOffset, detectOffsetAnimCurve.Evaluate(detectLerp))),
            Maths.Lerp(detectStartSize, detectTargetSize, detectLerp)
            );
    }
}
