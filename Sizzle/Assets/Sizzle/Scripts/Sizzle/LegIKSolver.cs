using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegIKSolver : MonoBehaviour
{
    public LayerMask mask;

    [Header("Leg IK parts")]
    //[SerializeField] Transform IKStart;
    [SerializeField] Transform IKHint;
    [SerializeField] Transform end;

    [Header("Bone transform references")]
    [SerializeField] Transform forwardBone;
    [SerializeField] Transform root;

    [Header("Step variables")]
    [SerializeField] float stepHeight;
    [SerializeField] float MaxDisFromFloor;
    [SerializeField] float stepDistance;
    [SerializeField] float footOffset;

    [Header("Offsets")]
    [Tooltip("The position where a raycast will check down to see where the foot should be put next")] 
    [SerializeField] Vector3 rayCheckStartOffset;
    [Tooltip("Where the leg tries to put the foot once initialized ")] 
    [SerializeField] Vector3 footStartOffset;
    [Tooltip("")] 
    [SerializeField] Vector3 IKHintOffset;
    

    /// <summary>
    /// The forward vector of the body section this leg is attached to 
    /// </summary>
    private Vector3 axisVectorForward { get { return forwardBone.transform.forward; } }
    private Vector3 axisVectorRight { get { return forwardBone.transform.right; } }
    private Vector3 axisVectorUp { get { return forwardBone.transform.up; } }

    /// <summary>
    /// The position where the the leg begins 
    /// </summary>
    private Vector3 Root 
    { get 
        { 
            return root.position; 
        } 
    }

    /// <summary>
    /// Where the leg tries to put the foot once initialized 
    /// </summary>
    private Vector3 startFootPosition 
    { 
        get 
        {
            return Root +
                axisVectorForward * footStartOffset.z +
                axisVectorRight * footStartOffset.x +
                axisVectorUp * footStartOffset.y;
        } 
    }

    /// <summary>
    /// Where the joint should tend to bend 
    /// </summary>
    private Vector3 offsetedIKHint 
    { get 
        { 
            return Root + 
                axisVectorForward* IKHintOffset.z +
                axisVectorRight * IKHintOffset.x +
                axisVectorUp * IKHintOffset.y;
        } 
    }

    /// <summary>
    /// The position where a raycast will check down to see where the foot should be put next 
    /// </summary>
    private Vector3 rayCheckStart 
    { 
        get 
        { 
            return Root +
                axisVectorForward* rayCheckStartOffset.z +
                axisVectorRight * rayCheckStartOffset.x +
                axisVectorUp * rayCheckStartOffset.y;
        } 
    }


    private Vector3 origin;
    private Vector3 target;
    private float lerp;
    private bool moving;

    public bool Moving { get { return moving; } }
    public bool showGizmos;

    // Start is called before the first frame update
    void Start()
    {
        //target = offsetedStart;
        lerp = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //IKStart.position = startFootPosition;
        IKHint.position = offsetedIKHint;
    }

    public void TryMove(float footSpeedMoving, float footSpeedNotMoving)
    {
        if(Moving)
        {
            return;
        }

        RaycastHit hit;
        // Checking for new position 
        if (Physics.Raycast(rayCheckStart, Vector3.down, out hit, MaxDisFromFloor, mask))
        {
            // New position found 
            if (Vector3.Distance(hit.point, target) > stepDistance)
            {
                lerp = 0;
                target = hit.point + hit.normal * footOffset;
                //print(hit.normal)
                StartCoroutine(Move(footSpeedMoving, footSpeedNotMoving));
            }
            else
            {
                end.position = origin;
            }
        }
        else
        {
            // Nothing to step down on 
        }
    }

    private IEnumerator Move(float footSpeedMoving, float footSpeedNotMoving)
    {
        moving = true;

        while(lerp <= 1)
        {
            // Moves smoothly to new point 
            Vector3 footPos = Vector3.Lerp(origin, target, lerp);
            footPos.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

            end.position = footPos;

            if (Input.GetKey(KeyCode.W)) // Moving forward 
            {
                lerp += Time.deltaTime * footSpeedMoving;
            }
            else
            {
                lerp += Time.deltaTime * footSpeedNotMoving;
            }
            yield return null;
        }

        // Once point is reached 
        end.transform.position = target;
        origin = target;
        moving = false;
    }

    private void OnDrawGizmos()
    {
        if(showGizmos)
        {
            //Vector3 difference = axisVector.normalized - Vector3.forward;
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(rayCheckStart, 0.05f);

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(startFootPosition, 0.05f);

            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(offsetedIKHint, 0.05f);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(target, 0.1f);

            RaycastHit hit;
            // Checking for new position 
            if (Physics.Raycast(rayCheckStart, Vector3.down, out hit, MaxDisFromFloor, mask))
            {
                Gizmos.DrawCube(hit.point, new Vector3(0.1f, 0.1f, 0.1f));
            }

        }    
    }
}
