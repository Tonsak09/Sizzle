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

    private void OnDrawGizmos()
    {
        if(showGizmos)
        {
            //Vector3 difference = axisVector.normalized - Vector3.forward;
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(rayCheckStart, 0.02f);

            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(offsetedIKHint, 0.02f);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(target, 0.05f);

            RaycastHit hit;
            // Checking for new position 
            if (Physics.Raycast(rayCheckStart, Vector3.down, out hit, MaxDisFromFloor, mask))
            {
                Gizmos.DrawWireSphere(hit.point, 0.02f);
            }


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
            }

            // Represents where a downward newPos can be and not update the foot to move
            Vector3[] areaPlane = Maths.FormPlaneFromSize(rangeAreaBeforeMove);
            Vector3[] proccessedAreaPlane = new Vector3[areaPlane.Length];

            for (int i = 0; i < areaPlane.Length; i++)
            {
                proccessedAreaPlane[i] = forwardBone.TransformDirection(areaPlane[i]);
                proccessedAreaPlane[i] = Vector3.ProjectOnPlane(proccessedAreaPlane[i], Vector3.up);
            }

            for (int i = 0; i < proccessedAreaPlane.Length; i++)
            {
                Gizmos.DrawSphere(foot.position + proccessedAreaPlane[i], 0.01f);

                // Draws line connceted to next in line 
                if(i < areaPlane.Length - 1)
                {
                    Gizmos.DrawLine(foot.position + proccessedAreaPlane[i], foot.position + proccessedAreaPlane[i + 1]);
                }
                else
                {
                    Gizmos.DrawLine(foot.position + proccessedAreaPlane[i], foot.position + proccessedAreaPlane[0]);
                }
            }
        }    
    }
}
