using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 Todo:
    - Rect stays in one place and doesn't rotate when not foot doesn't move
    - Max length a foot can be from the root before it needs to move
    - Choose which part of the compass for next rayCheckStart
 */

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
    [SerializeField] Transform foot;

    [Header("Step variables")]
    [SerializeField] float stepHeight;
    [SerializeField] float MaxDisFromFloor;
    [SerializeField] float stepDistance;
    [SerializeField] float footOffset;
    [Tooltip("What are the dimensions of the cube that a new position can be in without causing the leg to be able to move")]
    [SerializeField] Vector2 rangeAreaBeforeMove;

    [Header("Offsets")]
    [Tooltip("The position where a raycast will check down to see where the foot should be put next")] 
    [SerializeField] Vector3 rayCheckStartOffset;
    [Tooltip("In what directionthe joint will bend towards")] 
    [SerializeField] Vector3 IKHintOffset;

    [Header("Leg Compass")]
    [SerializeField] Vector3[] compassDirections;

    [Header("GUI")]
    [SerializeField] bool showGizmos;
    public float rotValueTest;


    /// <summary>
    /// The forward vector of the body section this leg is attached to 
    /// </summary>
    private Vector3 axisVectorForward { get { return forwardBone.transform.forward; } }
    private Vector3 axisVectorRight { get { return forwardBone.transform.right; } }
    private Vector3 axisVectorUp { get { return forwardBone.transform.up; } }

    /// <summary>
    /// The position where the the leg begins 
    /// </summary>
    private Vector3 RootPos 
    { get 
        { 
            return root.position; 
        } 
    }


    /// <summary>
    /// Where the joint should tend to bend 
    /// </summary>
    private Vector3 offsetedIKHint 
    { get 
        { 
            return RootPos + 
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
            return RootPos +
                axisVectorForward* rayCheckStartOffset.z +
                axisVectorRight * rayCheckStartOffset.x +
                axisVectorUp * rayCheckStartOffset.y;
        } 
    }




    // Info for moving the limb from one spot to next 
    private Vector3[] processedRangePlane;
    private Vector3 origin;
    private Vector3 target;
    private float lerp;
    private bool moving;

    public bool Moving { get { return moving; } }

    // Start is called before the first frame update
    void Start()
    {
        //target = offsetedStart;
        lerp = 0;
        processedRangePlane = GetProcessedRangePlane();
    }

    // Update is called once per frame
    void Update()
    {
        //IKStart.position = startFootPosition;
        IKHint.position = offsetedIKHint;
        ChooseCompassDir();
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

            print(Maths.IsPointWithinRect(hit.point, processedRangePlane) + ": " + this.gameObject.name);
            // New position found 
            if (Vector3.Distance(hit.point, target) > stepDistance)
            {
                lerp = 0;
                target = hit.point + hit.normal * footOffset;
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

        // Sets new plane 
        processedRangePlane = GetProcessedRangePlane();
        // Makes sure position is where needed 
        end.transform.position = target;
        origin = target;
        moving = false;
    }

    private Vector3 ChooseCompassDir()
    {
        Vector3[] LocalizedCompass = GetLocalizedCompass();

        return new Vector3();
    }

    private Vector3[] GetLocalizedCompass()
    {
        if (compassDirections != null)
        {
            Vector3[] newCompass = new Vector3[compassDirections.Length];

            for (int i = 0; i < compassDirections.Length; i++)
            {
                // Get direction as if root is parent 
                Vector3 worldDir = forwardBone.TransformDirection(compassDirections[i]);

                newCompass[i] = worldDir;
            }
            return newCompass;
        }
        return null;
    }

    private Vector3[] GetProcessedRangePlane()
    {
        // Represents where a downward newPos can be and not update the foot to move
        Vector3[] areaPlane = Maths.FormPlaneFromSize(rangeAreaBeforeMove);
        Vector3[] proccessedAreaPlane = new Vector3[areaPlane.Length];

        for (int i = 0; i < areaPlane.Length; i++)
        {
            proccessedAreaPlane[i] = forwardBone.TransformDirection(areaPlane[i]);
            proccessedAreaPlane[i] = foot.position + Vector3.ProjectOnPlane(proccessedAreaPlane[i], Vector3.up);
        }

        return proccessedAreaPlane;
    }

    private void OnDrawGizmos()
    {
        if(showGizmos)
        {
            //Vector3 difference = axisVector.normalized - Vector3.forward;
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(rayCheckStart, 0.02f);

            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(offsetedIKHint, 0.02f);

            RaycastHit hit;
            // Checking for new position 
            if (Physics.Raycast(rayCheckStart, Vector3.down, out hit, MaxDisFromFloor, mask))
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(hit.point, 0.02f);

                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(new Vector3(hit.point.x, foot.position.y, hit.point.z), 0.01f);
            }

            // Visualizes compass directions
            if(compassDirections != null)
            {
                Gizmos.color = Color.yellow;
                // Draw each vector as if 
                foreach (Vector3 dir in compassDirections)
                {
                    // Get direction as if root is parent 
                    Vector3 worldDir = forwardBone.TransformDirection(dir);

                    // Change to world coords 

                    // Draw point 
                    Gizmos.DrawWireSphere(RootPos + worldDir * dir.magnitude, 0.01f);
                }
            }

            // Updates rangeAreaBeforeMoving even when not in game 
            if(!Application.isPlaying)
            {
                target = hit.point + hit.normal * footOffset;
                processedRangePlane = GetProcessedRangePlane();
            }

            for (int i = 0; i < processedRangePlane.Length; i++)
            {
                Gizmos.DrawSphere(processedRangePlane[i], 0.01f);

                // Draws line connceted to next in line 
                if(i < processedRangePlane.Length - 1)
                {
                    Gizmos.DrawLine(processedRangePlane[i], processedRangePlane[i + 1]);
                }
                else
                {
                    Gizmos.DrawLine(processedRangePlane[i], processedRangePlane[0]);
                }
            }
        }    
    }
}
