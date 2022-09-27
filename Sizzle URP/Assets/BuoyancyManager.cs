using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the speed at which the buoyancies bounce 
/// in relation to the speed of Sizzle 
/// </summary>
public class BuoyancyManager : MonoBehaviour
{
    [SerializeField] Buoyancy[] buoyancies;
    [SerializeField] Rigidbody rootRefernce;
    [Tooltip("The displacement of the buyancies from one another when the sine function is applied")]
    [SerializeField] float waveDisplacement;

    [Header("Walking")]
    // Walking animation curve goes between 0 and max 
    [SerializeField] float walkMaxLegSpeed;
    [SerializeField] float walkMinFreq;
    [SerializeField] float walkMaxFreq;
    [SerializeField] float walkMinMag;
    [SerializeField] float walkMaxMag;
    [Tooltip("The greatest sqrt velocity the root can be while still being within the walk")]
    [SerializeField] float walkMaxSqrMag;
    [SerializeField] AnimationCurve walkCurve;

    [Header("Running")]
    // Running animation curve goes between maxWalkSpeed and maxRunSpeed 
    [SerializeField] float runMaxLegSpeed;
    [SerializeField] float runMinFreq;
    [SerializeField] float runMaxFreq;
    [SerializeField] float runMinMag;
    [SerializeField] float runMaxMag;
    [Tooltip("The greatest sqrt velocity the root can be while still being within the run, though there are no more shifts after running so prepare for strangeness")]
    [SerializeField] float runMaxSqrMag;
    [SerializeField] AnimationCurve runCurve;

    private float[] buoyanciesDepthHold;
    private float speed { get { return rootRefernce.velocity.sqrMagnitude; } }

    private void Start()
    {
        // Set up hold array
        buoyanciesDepthHold = new float[buoyancies.Length];
        for (int i = 0; i < buoyancies.Length; i++)
        {
            buoyanciesDepthHold[i] = buoyancies[i].DepthBeforeSubmerged;
        }
    }

    // Update is called once per frame
    void Update()
    {
        print(speed);
        if(speed <= walkMaxSqrMag)
        {
            // Walking 
            float unitValue = walkCurve.Evaluate(speed / walkMaxSqrMag);
            WaveBuoyancies(unitValue * Mathf.Lerp(walkMinMag, walkMaxMag, unitValue), Mathf.Lerp(walkMinFreq, walkMaxFreq, unitValue));
        }
        else
        {
            // Running 
            // Use inverse lerp because minimum is not 0
            float unitValue = walkCurve.Evaluate( Mathf.InverseLerp(walkMaxSqrMag, runMaxSqrMag, speed));
            WaveBuoyancies(unitValue * Mathf.Lerp(runMinMag, runMaxMag, unitValue), Mathf.Lerp(walkMinFreq, walkMaxFreq, unitValue));
        }
    }

    private void WaveBuoyancies(float mag, float freq)
    {
        for (int i = 0; i < buoyancies.Length; i++)
        {
            buoyancies[i].DepthBeforeSubmerged = buoyanciesDepthHold[i] + Mathf.Sin((Time.time + waveDisplacement * i) / freq) * mag;
        }
    }
}
