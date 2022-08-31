using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorqueTowardsRotation : MonoBehaviour
{

    [SerializeField] Vector3 upAim;
    [SerializeField] float maxSpeed;
    [Tooltip("What is the maximum offset from the eulerAim that this object can reach to get maximum torque")]
    [SerializeField] float maxOffset;
    [SerializeField] AnimationCurve offsetFromEulerAimCurve;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 offsetFromEulerAim = upAim - this.transform.eulerAngles;
        print(offsetFromEulerAim);

        float step = offsetFromEulerAimCurve.Evaluate(Mathf.Clamp01(offsetFromEulerAim.magnitude / maxOffset)) * Time.deltaTime;
        Vector3.RotateTowards(this.transform.up, upAim, step, 0.0f);
    }
}
