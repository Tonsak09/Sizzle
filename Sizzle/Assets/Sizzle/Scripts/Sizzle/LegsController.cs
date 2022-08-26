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

    //[Range(0, 1)]
    //[SerializeField] float lerpToMoveOpposite;

    private LegIKSolver[] frontPair;
    private LegIKSolver[] backPair;

    // Start is called before the first frame update
    void Start()
    {
        /*frontPair = new LegIKSolver[] { frontLeft, frontRight };
        backPair = new LegIKSolver[] { backLeft, backRight };

        frontLeft.TryMove(footSpeedMoving, footSpeedNotMoving);
        backRight.TryMove(footSpeedMoving, footSpeedNotMoving);
        backLeft.TryMove(footSpeedMoving, footSpeedNotMoving);
        frontRight.TryMove(footSpeedMoving, footSpeedNotMoving);*/

        //StartCoroutine(LegUpdateCoroutine());
    }

    private void Update()
    {
        //RunPair(frontPair, frontCurrent);
        
        frontLeft.TryMove(footSpeedMoving, footSpeedNotMoving);
        backRight.TryMove(footSpeedMoving, footSpeedNotMoving);
        backLeft.TryMove(footSpeedMoving, footSpeedNotMoving);
        frontRight.TryMove(footSpeedMoving, footSpeedNotMoving);
        
    }

    /*private bool RunPair(LegIKSolver[] pair, bool current)
    {
        // Whether first or second in pair based on bool 
        int i = current ? 1 : 0;
        print(i);

        // If primary is over a certain lerp secondary can begin 
        if (pair[i].Lerp > lerpToMoveOpposite)
        {
            pair[(i + 1) % 2].TryMove(footSpeedMoving, footSpeedNotMoving);
        }

        return !pair[i].Moving;

    }*/

    public IEnumerator LegUpdateCoroutine()
    {
        while (true)
        {
            do
            {
                frontLeft.TryMove(footSpeedMoving, footSpeedNotMoving);
                backRight.TryMove(footSpeedMoving, footSpeedNotMoving);

                yield return null;
            } while (frontLeft.Moving || backRight.Moving);

            do
            {
                backLeft.TryMove(footSpeedMoving, footSpeedNotMoving);
                frontRight.TryMove(footSpeedMoving, footSpeedNotMoving);

                yield return null;
            } while (backLeft.Moving || frontRight.Moving);
        }
    }
}
