using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegsController : MonoBehaviour
{
    [Header("Legs")]
    [SerializeField] LegIKSolver frontLeft;
    [SerializeField] LegIKSolver frontRight;
    [SerializeField] LegIKSolver backLeft;
    [SerializeField] LegIKSolver backRight;

    [Header("Speeds")]
    [SerializeField] float footSpeedMoving;
    [SerializeField] float footSpeedNotMoving;

    [Header("Values")]
    [SerializeField] float minlerpBeforePair;

    [Header("Dashing")]
    [SerializeField] Vector3 frontDashTarget;
    [SerializeField] Vector3 backDashTarget;

    [SerializeField] float minVel;
    [SerializeField] float maxVel;
    [SerializeField] AnimationCurve feetToTargetCurve;

    private BodyAnimationManager animManager;

    private LegIKSolver[] frontPair;
    private LegIKSolver[] backPair;
    private LegIKSolver[] allLegs;

    private const string KEY = "LEGS";


    public bool Active { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        animManager = GameObject.FindObjectOfType<BodyAnimationManager>();

        frontPair = new LegIKSolver[] { frontLeft, frontRight };
        backPair = new LegIKSolver[] { backLeft, backRight };

        animManager.TryAnimation(WalkCycleCo(frontPair, backPair), KEY);
    }

    public void Dash(Rigidbody body)
    {
        animManager.TryAnimation(DashCo(body), KEY, true);
    }

    private IEnumerator DashCo(Rigidbody body)
    {
        while(body.velocity.sqrMagnitude >= Mathf.Pow(minVel,2))
        {


            yield return null;
        }
    }


    private IEnumerator WalkCycleCo(LegIKSolver[] front, LegIKSolver[] back)
    {

        if (!Active)
        {
            yield return null;
        }

        // Index 
        while (true)
        {
            RunPair(front);
            RunPair(back);

            yield return null;
        }
    }

    private void RunPair(LegIKSolver[] pair)
    {
        // Find primary leg moving
        if (pair[0].Moving && !pair[1].Moving)
        {
            if (pair[0].Lerp >= minlerpBeforePair)
            {
                pair[1].TryMove(footSpeedMoving, footSpeedNotMoving);
            }
        }
        if (!pair[0].Moving && pair[1].Moving)
        {
            if (pair[1].Lerp >= minlerpBeforePair)
            {
                pair[0].TryMove(footSpeedMoving, footSpeedNotMoving);
            }
        }
        if (!pair[0].Moving && !pair[1].Moving)
        {
            // If neither are moving try to move one randomly 
            pair[Random.Range(0, 2)].TryMove(footSpeedMoving, footSpeedNotMoving);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        foreach (LegIKSolver leg in frontPair)
        {
            Vector3 pos = leg.transform.TransformDirection(frontDashTarget);
            Gizmos.DrawWireSphere(leg.transform.position + pos, 0.01f);

        }
    }
}
