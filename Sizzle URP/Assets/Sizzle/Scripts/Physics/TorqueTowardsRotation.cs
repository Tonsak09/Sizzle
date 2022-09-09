using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorqueTowardsRotation : MonoBehaviour
{

    [SerializeField] Vector3 target;
    [SerializeField] float torque;


    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddTorque(Vector3.Cross(target, this.transform.up) * torque);
    }
}
