using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Rigidbody frontBody;

    [Header("Variables")]
    [SerializeField] float moveForce;
    [SerializeField] float torqueForce;

    public Vector3 forward;
    private Vector3 target;

    public Vector3 ForwardVec { get { return forward; } set { forward = value.normalized; } }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Sets forward as cam to target body and then projected 
        //forward = ForwardFromCamToTarget();
        float VInput = Input.GetAxis("Vertical");
        float hInput = Input.GetAxis("Horizontal");

        frontBody.AddTorque(torqueForce * frontBody.transform.up * hInput * Time.deltaTime, ForceMode.Acceleration);
        frontBody.AddForce(moveForce * frontBody.transform.forward * VInput * Time.deltaTime, ForceMode.Acceleration);

    }

    private void ForceControl()
    {
        // Get the input 
        float VInput = Input.GetAxis("Vertical");   // How much along the forward vector 
        float HInput = Input.GetAxis("Horizontal"); // How much to the perpendicular of the forward vector

        // Set the xz plane direction 
        //Vector3 vector = forward * VInput + 
        //transform.rotat
        // Set vector to the body's normal 
        
        //body.AddForce(vector);
    }

    /*private Vector3 ForwardFromCamToTarget()
    {
        // Get Vector from currentCam to target
        Vector3 camToTarget = body.transform.position - GameManager.CurrentCam.transform.position;

        // Project to target normal 
        Vector3 projCamToTarget = Vector3.ProjectOnPlane(camToTarget, body.transform.up);

        // return projection 
        return projCamToTarget.normalized;
    }
*/
    private void OnDrawGizmos()
    {
        /*Gizmos.color = Color.white;
        Gizmos.DrawLine(body.position, body.position + forward * 2);

        Gizmos.color = Color.red;
        Vector3 dirToTarget = target - body.transform.position;
        Gizmos.DrawLine(body.position, body.position + dirToTarget);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(body.position, Quaternion.Euler(body.transform.up * 90) * body.position + forward);*/
    }

    private void OnDrawGizmosSelected()
    {
        
    }
}
