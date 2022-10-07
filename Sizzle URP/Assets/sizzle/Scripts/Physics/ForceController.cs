using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Rigidbody frontBody;

    [Header("Walking")]
    [SerializeField] float moveForce;
    [SerializeField] float torqueForce;

    [Header("Crouching")]
    [SerializeField] KeyCode crouchkey;
    [SerializeField] float moveForceCrouch;
    [SerializeField] float torqueForceCrouch;
    [SerializeField] float crouchSpeed;
    [SerializeField] float unCrouchSpeed;
    [SerializeField] float minLerp;

    [SerializeField] BuoyancyManager bManager;

    private float crouchLerp;

    [Header("Dash")]
    [SerializeField] KeyCode dashKey;
    [SerializeField] float dashForceImpulse;
    [SerializeField] float dashForceContinuous;
    [SerializeField] float dashTime;
    [Tooltip("The minimum speed that Sizzle must maintain to stay in dash")]
    [SerializeField] float minSqrtSpeedForDash;

    [SerializeField] AnimationCurve dashForceoverLerp;

    private Coroutine DashCo;


    private Vector3 target;
    [SerializeField] private states SizzleState;
    private enum states
    {
        movement,
        crouch,
        dash
    };

    // Start is called before the first frame update
    void Start()
    {
        SizzleState = states.movement;
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


        AdjustOrientation();

        switch (SizzleState)
        {
            case states.movement:
                ForceControl(moveForce, torqueForce);

                if(Input.GetKey(crouchkey))
                {
                    // Change to crouch state 
                    SizzleState = states.crouch;
                }
                else
                {
                    crouchLerp = Mathf.Clamp(crouchLerp + Time.deltaTime * unCrouchSpeed, minLerp, 1);
                }

                break;
            case states.crouch:

                ForceControl(moveForceCrouch, torqueForceCrouch); // Slower Movement 
                CrouchLogic();
                TryDash(); // Checks whether the dash state should begin 

                break;
            case states.dash:
                break;

        }
        bManager.AdjustHeights(crouchLerp);

    }

    /// <summary>
    /// The basic controls of Sizzle that allow directional movement
    /// and turning
    /// </summary>
    /// <param name="moveForce"></param>
    /// <param name="torqueForce"></param>
    private void ForceControl(float moveForce, float torqueForce)
    {
        // Get the input 
        float VInput = Input.GetAxis("Vertical");
        float hInput = Input.GetAxis("Horizontal");

        frontBody.AddTorque(torqueForce * frontBody.transform.up * hInput * Time.deltaTime, ForceMode.Acceleration);
        frontBody.AddForce(moveForce * frontBody.transform.forward * VInput * Time.deltaTime, ForceMode.Acceleration);
    }

    private void AdjustOrientation()
    {
        RaycastHit hit;
        if (Physics.Raycast(frontBody.transform.position, Vector3.down, out hit, 5))
        {
            print(hit.point);
            //frontBody.transform.up = hit.normal;
        }
    }

    private void CrouchLogic()
    {
        if(Input.GetKey(crouchkey))
        {
            crouchLerp = Mathf.Clamp(crouchLerp - Time.deltaTime * crouchSpeed, minLerp, 1);
        }
        else if (Input.GetKeyUp(crouchkey))
        {
            SizzleState = states.movement;
        }
    }

    private void TryDash()
    {
        
        // Activates the dash state 
        if (Input.GetKeyDown(dashKey) && DashCo == null)
        {
            SizzleState = states.dash;
            frontBody.AddForce(dashForceImpulse * frontBody.transform.forward, ForceMode.Impulse);

            DashCo = StartCoroutine(DashSubroutine());
        }
    }

    private IEnumerator DashSubroutine()
    {
        float timer = dashTime;
        while (timer >= 0)
        {
            /*if (frontBody.velocity.sqrMagnitude < minSqrtSpeedForDash)
            {
                // No longer fast enough
                break;
            }*/

            frontBody.AddForce(dashForceContinuous * dashForceoverLerp.Evaluate((timer / dashTime)) * frontBody.transform.forward, ForceMode.Acceleration);

            timer -= Time.deltaTime;
            yield return null;
        }

        SizzleState = states.movement;
        StopCoroutine(DashCo);
        DashCo = null;
    }

}
