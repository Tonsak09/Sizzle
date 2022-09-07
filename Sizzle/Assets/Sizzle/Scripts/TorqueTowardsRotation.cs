using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorqueTowardsRotation : MonoBehaviour
{

    [SerializeField] float xAim;
    [SerializeField] float yAim;
    [SerializeField] float zAim;

    [SerializeField] bool adjustX;
    [SerializeField] bool adjustY;
    [SerializeField] bool adjustZ;

    [SerializeField] float maxSpeed;
    [Tooltip("What is the maximum offset from the eulerAim that this object can reach to get maximum torque")]
    [SerializeField] float maxOffset;
    [SerializeField] AnimationCurve offsetFromEulerAimCurve;

    private Rigidbody rb;

    private Vector3 upAim 
    {
        get
        {
            if(!adjustX)
            {
                xAim = this.transform.eulerAngles.x;
            }
            if(!adjustY)
            {
                yAim = this.transform.eulerAngles.y;
            }
            if (!adjustZ)
            {
                zAim = this.transform.eulerAngles.z;
            }

            return new Vector3(xAim, yAim, zAim);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 offsetFromEulerAim = upAim - this.transform.eulerAngles;

        Quaternion deltaRotation = Quaternion.Euler(maxSpeed * offsetFromEulerAim.normalized * Time.fixedDeltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }
}
