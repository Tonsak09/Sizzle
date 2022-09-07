using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sparks : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] GameObject sparksFX;

    [Header("Neck")]
    [SerializeField] ConfigurableJoint neckJoint;

    [SerializeField] Vector3 neckDefaultRot;
    [SerializeField] Vector3 neckTargetRot;

    [SerializeField] AnimationCurve neckOpenAnimCurve;
    [SerializeField] AnimationCurve neckCloseAnimCurve;
    [SerializeField] float neckSpeed;



    [Header("Jaw")]
    [SerializeField] ConfigurableJoint jawJoint;

    [SerializeField] Vector3 jawDefaultRot;
    [SerializeField] Vector3 jawTargetRot;

    [SerializeField] AnimationCurve jawOpenAnimCurve;
    [SerializeField] AnimationCurve jawCloseAnimCurve;
    [SerializeField] float jawSpeed;

    private BodyAnimationManager animaManager; 

    private void Start()
    {
        animaManager = this.GetComponent<BodyAnimationManager>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            animaManager.TryAnimateHead(HeadAnimation(), "Sparks");
        }
    }

    private IEnumerator HeadAnimation()
    {

        float neckLerp = 0;
        float jawLerp = 0;

        // Opening
        while(neckLerp < 1 && jawLerp < 1)
        {
            neckLerp = Mathf.Clamp01(neckLerp + neckSpeed * Time.deltaTime);
            jawLerp = Mathf.Clamp01(jawLerp + jawSpeed * Time.deltaTime);

            // Sets lerp to animation curve
            neckJoint.targetRotation = Quaternion.Euler(Vector3.Lerp(neckDefaultRot, neckTargetRot, neckOpenAnimCurve.Evaluate(neckLerp)));
            jawJoint.targetRotation = Quaternion.Euler(Vector3.Lerp(jawDefaultRot, jawTargetRot, jawOpenAnimCurve.Evaluate(jawLerp)));

            yield return null;
        }

        // Closing 
        while(neckLerp > 0 && jawLerp > 0)
        {
            neckLerp = Mathf.Clamp01(neckLerp - neckSpeed * Time.deltaTime);
            jawLerp = Mathf.Clamp01(jawLerp - jawSpeed * Time.deltaTime);

            // Sets lerp to animation curve
            neckJoint.targetRotation = Quaternion.Euler(Vector3.Lerp(neckDefaultRot, neckTargetRot, neckCloseAnimCurve.Evaluate(neckLerp)));
            jawJoint.targetRotation = Quaternion.Euler(Vector3.Lerp(jawDefaultRot, jawTargetRot, jawCloseAnimCurve.Evaluate(jawLerp)));
            yield return null;
        }
        animaManager.EndAnimation("Sparks");
    }
}
