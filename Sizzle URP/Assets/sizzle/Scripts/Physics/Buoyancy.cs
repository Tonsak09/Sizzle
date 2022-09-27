using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buoyancy : MonoBehaviour
{

    [SerializeField] float maxBuoyancyForce;
    [SerializeField] float fallForce;
    [SerializeField] float depthBeforeSubmerged;
    //[SerializeField] float fallForce;
    [SerializeField] LayerMask layer;
    [SerializeField] AnimationCurve buoyancyForceCurve;

    private Rigidbody rb;

    public float DepthBeforeSubmerged { get { return depthBeforeSubmerged; } set { depthBeforeSubmerged = value; } }

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        

        //UpdateRayCheck();
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, Vector3.down, out hit, depthBeforeSubmerged, layer))
        {
            float disFromGround = Vector3.Distance(this.transform.position, hit.point);
            float forceModifier = buoyancyForceCurve.Evaluate(1 - Mathf.Clamp01(disFromGround / depthBeforeSubmerged)) * maxBuoyancyForce;
            rb.AddForce(Vector3.up * forceModifier, ForceMode.Acceleration);
        }    
        else
        {
            rb.AddForce(Vector3.down * fallForce, ForceMode.Acceleration);
        }

    }

    /*private void UpdateRayCheck()
    {
        if(Physics.Raycast(this.transform.position, Vector3.down, out hit, 100, layer))
        {
            float disFromGround = Vector3.Distance(this.transform.position, hit.point);
            float forceModifier = (1 - Mathf.Clamp01(disFromGround / depthBeforeSubmerged)) * maxBuoyancyForce;
            
            
            // Not falling
            *//*if (disFromGround <= maxDisFromGround)
            {
                float xValue = disFromGround / maxBuoyancyForce;

                float force = buoyancyForceCurve.Evaluate(xValue) * maxBuoyancyForce;
                print(force);
                rb.AddForce(force * Vector3.up, ForceMode.Acceleration);
            }*//*
        }
    }*/

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(this.transform.position, this.transform.position - Vector3.up * depthBeforeSubmerged);
    }
}
