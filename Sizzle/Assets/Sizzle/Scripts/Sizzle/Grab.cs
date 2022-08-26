using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    /*
    public KeyCode grabKey;
    public float range;

    public float grabSpeed;
    public float throwSpeed;
    public Vector3 throwOffset;

    public Transform grabPoint;
    [SerializeField] Transform boneReference;

    private Candle[] candles;
    private Candle heldCandle;

    public bool candleHeld { get { return heldCandle != null; } }


    // Start is called before the first frame update
    void Start()
    {
        candles = FindObjectsOfType<Candle>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(grabKey))
        {
            if(heldCandle == null)
            {
                GrabCandle();
            }
            else
            {
                heldCandle.SetCandle(throwSpeed, (grabPoint.forward + throwOffset).normalized);
                heldCandle.transform.eulerAngles = new Vector3(0, heldCandle.transform.eulerAngles.y, 0);
                heldCandle = null;
            }
        }

        if(heldCandle != null)
        {
            heldCandle.transform.LookAt(boneReference);
        }
    }

    private void GrabCandle()
    {
        List<Candle> candlesInRange = new List<Candle>();

        // Get all candles within range 
        foreach (Candle candle in candles)
        {
            if( (candle.distance = Vector3.Distance(candle.transform.position, grabPoint.position)) < range)
            {
                candlesInRange.Add(candle);
            }
        }

        // Makes sure there is actually a candles within range 
        if(candlesInRange.Count > 0)
        {
            // Find closest candle 
            Candle closest = null;
            foreach (Candle item in candlesInRange)
            {
                if ((closest == null || item.distance < closest.distance) && item.canBeGrabbed)
                {
                    closest = item;
                }
            }

            heldCandle = closest;

            if(heldCandle != null)
            {
                // At least one candle 
                StartCoroutine(closest.GrabCandle(grabSpeed, grabPoint));
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(grabPoint.position, grabPoint.position + (grabPoint.forward + throwOffset).normalized * 4);
    }
    */
}
