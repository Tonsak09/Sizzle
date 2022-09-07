using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sparks : MonoBehaviour
{

    [SerializeField] Transform jawHinge;
    [SerializeField] Transform jawTip;

    [SerializeField] Transform sparksStart;
    [SerializeField] GameObject sparksFX;
    [SerializeField] GameObject triggerSphere;

    [SerializeField] Grab grabber;

    [SerializeField] float coolDown;
    [SerializeField] float triggerTime;
    [SerializeField] float triggerSphereSpeed;
    [SerializeField] float triggerSphereDistance;


    [SerializeField] Vector3 neckOffset;
    [SerializeField] float neckSpeed;
    [SerializeField] float jawRotation;
    [SerializeField] float jawRotSpeed;

    [SerializeField] AudioClip[] mouthCloseSounds;
    [SerializeField] AudioClip sparkStartHiss;

    private float timer;
    private HeadTargeting targeting;
    private SoundManager sm;

    private void Awake()
    {
        targeting = this.GetComponent<HeadTargeting>();
        sm = GameObject.FindObjectOfType<SoundManager>();
        timer = coolDown;
    }

    // Update is called once per frame
    void Update()
    {
        // On spark click 
        if(Input.GetMouseButton(0)) //&& !grabber.candleHeld)
        {
            RaycastHit initialHit;
            if (Physics.Raycast(jawHinge.position, (triggerSphere.transform.position - jawHinge.position).normalized, out initialHit, (triggerSphere.transform.position - jawHinge.position).magnitude))
            {
                // Does not go past if there is some obstruction between head and spark start 
                return;
            }

            // If cooldown is over 
            if(timer >= coolDown)
            {
                sm.PlaySoundFX(sparkStartHiss, sparksStart.position, "Hiss");

                // Moves head up and down 
                StartCoroutine(AnimatedHead());
                StartCoroutine(AnimatedJaw());

                // Creates fx 
                Instantiate(sparksFX, sparksStart.position, this.transform.rotation);

                // Will be used to hold new trigger sphere for informing slimes 
                GameObject triggerSphereTemp = null;


                // Makes sure that there is nothing in way betteen trigger sphere and sparsk start
                Vector3 vector = triggerSphere.transform.position - sparksStart.position;
                RaycastHit hit;
                if(Physics.Raycast(sparksStart.position, vector, out hit, vector.magnitude))
                {
                    // Creates temporary trigger box for slime detection but at hit point instead of full distance
                    triggerSphereTemp = Instantiate(triggerSphere, triggerSphere.transform.position, Quaternion.identity);
                    triggerSphereTemp.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                }
                else
                {
                    // Creates temporary trigger box for slime detection 
                    triggerSphereTemp = Instantiate(triggerSphere, triggerSphere.transform.position, Quaternion.identity);
                    StartCoroutine(MoveSphereForwardForward(triggerSphereTemp.transform, transform.forward));
                }

                triggerSphereTemp.GetComponent<Collider>().enabled = true;

                DestroyTimer dt = triggerSphereTemp.AddComponent<DestroyTimer>();
                dt.time = 1;

                // Rests timer 
                timer = 0;
            }
        }

        timer += Time.deltaTime;
    }

    private IEnumerator AnimatedHead()
    {
        float verticalLerp = 0;

        while(verticalLerp <= 1)
        {
            verticalLerp += neckSpeed * Time.deltaTime;
            targeting.HeadOffset = Vector3.Lerp(Vector3.zero, neckOffset, verticalLerp);
            yield return null;
        }

        while(verticalLerp >= 0)
        {
            verticalLerp -= neckSpeed * Time.deltaTime;
            targeting.HeadOffset = Vector3.Lerp(Vector3.zero, neckOffset, verticalLerp);
            yield return null;
        }
    }

    private IEnumerator AnimatedJaw()
    {
        float angleLerp = 0;
        float rotOffset;
        float holdXRot = jawHinge.localEulerAngles.x;

        while (angleLerp <= 1)
        {
            angleLerp += jawRotSpeed * Time.deltaTime;
            rotOffset = Mathf.Lerp(0, jawRotation, angleLerp);
            jawHinge.localEulerAngles = new Vector3(holdXRot + rotOffset, 0, 0);
            yield return null;
        }

        while (angleLerp >= 0)
        {
            angleLerp -= jawRotSpeed * Time.deltaTime;
            rotOffset = Mathf.Lerp(0, jawRotation, angleLerp);
            jawHinge.localEulerAngles = new Vector3(holdXRot + rotOffset, 0, 0);
            yield return null;
        }

        sm.PlaySoundFX(mouthCloseSounds[Random.Range(0, mouthCloseSounds.Length)], this.transform.position, "mouthClose");
        jawHinge.localEulerAngles = new Vector3(holdXRot, 0, 0);
    }

    private IEnumerator MoveSphereForwardForward(Transform sphere, Vector3 dir)
    {
        Vector3 start = sphere.transform.position;
        float lerp = 0;

        while(lerp <= 1 && sphere != null)
        {
            sphere.position =  Vector3.Lerp(start, start + dir * triggerSphereDistance, lerp);

            lerp += Time.deltaTime * triggerSphereSpeed;
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        RaycastHit initialHit;
        if (Physics.Raycast(jawTip.position, (sparksStart.position - jawTip.position).normalized, out initialHit, (sparksStart.position - jawTip.position).magnitude))
        {
            
        }

        Gizmos.color = Color.white;
        Gizmos.DrawLine(jawHinge.position, jawHinge.position + (triggerSphere.transform.position - jawHinge.position));
    }
}
