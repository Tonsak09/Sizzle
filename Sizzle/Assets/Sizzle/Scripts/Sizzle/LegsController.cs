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

    //[Range(0, 1)]
    //[SerializeField] float lerpToMoveOpposite;

    private LegIKSolver[] frontPair;
    private LegIKSolver[] backPair;

    // Start is called before the first frame update
    void Start()
    {
        frontPair = new LegIKSolver[] { frontLeft, frontRight };
        backPair = new LegIKSolver[] { backLeft, backRight };
        /*
        frontLeft.TryMove(footSpeedMoving, footSpeedNotMoving);
        backRight.TryMove(footSpeedMoving, footSpeedNotMoving);
        backLeft.TryMove(footSpeedMoving, footSpeedNotMoving);
        frontRight.TryMove(footSpeedMoving, footSpeedNotMoving);*/

        StartCoroutine(LegUpdateCoroutine(frontPair));
        StartCoroutine(LegUpdateCoroutine(backPair));
    }

    private void Update()
    {
        //RunPair(frontPair, frontCurrent);
        
        /*frontLeft.TryMove(footSpeedMoving, footSpeedNotMoving);
        backRight.TryMove(footSpeedMoving, footSpeedNotMoving);
        backLeft.TryMove(footSpeedMoving, footSpeedNotMoving);
        frontRight.TryMove(footSpeedMoving, footSpeedNotMoving);*/


        
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

    public IEnumerator LegUpdateCoroutine(LegIKSolver[] pair)
    {

        bool current = true;
        // Index 
        int index = 0;
        while (true)
        {
            /*do
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
            } while (backLeft.Moving || frontRight.Moving);*/


            // Find primary leg moving
            if(pair[0].Moving && !pair[1].Moving)
            {
                if(pair[0].Lerp >= minlerpBeforePair)
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
            if(!pair[0].Moving && !pair[1].Moving)
            {
                // If neither are moving try to move one randomly 
                pair[Random.Range(0, 2)].TryMove(footSpeedMoving, footSpeedNotMoving);
            }

            /*if(pair[index].Moving)
            {
                if(pair[index].Lerp >= minlerpBeforePair)
                {
                    pair[current ? 1 : 0].TryMove(footSpeedMoving, footSpeedNotMoving);
                }
            }*/



            yield return null;
        }
    }
}
