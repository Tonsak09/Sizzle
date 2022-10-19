using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spearine : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform rotationBone;

    [Header("Ranges")]
    [SerializeField] float closeRange;
    [SerializeField] float midRange;
    [SerializeField] float farRange;

    [Header("Speeds")]
    [SerializeField] float curiousTurnSpeed;
    [SerializeField] float alertTurnSpeed;

    [Header("Alertness")]
    [SerializeField] float closeAlertRaise;
    [SerializeField] float midAlertRaise;
    [SerializeField] float farAlertRaise;

    private Transform player;
    // Target could be a player, but also any other charged entity 
    private Transform primaryTarget;

    // Used to calculate how aware spearine is to all
    // targets in its vicinity 
    private Dictionary<Transform, float> alertnessDictionary;
    private List<Transform> targets;

    // If reaches 100 then Spearine is alert to target 
    [Range(0, 100)]
    private float alertness;

    private enum DistanceZone
    {
        close,
        mid,
        far,
        NotWithinRange
    }

    // Start is called before the first frame update
    void Start()
    {
        alertnessDictionary = new Dictionary<Transform, float>();

        // Gets player rather than Sizzle because Sizzle is a folder that doesn't represent the tru position 
        player = GameObject.FindWithTag("Player").transform;
        alertnessDictionary.Add(player, 0);

        // TESTING ONLY 
        primaryTarget = player;
    }

    // Update is called once per frame
    void Update()
    {
        print(GetZone(player.position));

        if(primaryTarget != null)
        {
            Vector3 targetVec = Vector3.ProjectOnPlane(primaryTarget.position - this.transform.position, this.transform.up);
            this.transform.forward = Vector3.RotateTowards(this.transform.forward, targetVec, alertTurnSpeed * Time.deltaTime, 0.0f);
        }
        else
        {
            UpdateAlertness();

            // Get target with largest alert 
            // Curious rotate towards that 
        }
    }

    /// <summary>
    /// Gets the zone that the position is in 
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    private DistanceZone GetZone(Vector3 pos)
    {
        float dis = (this.transform.position - pos).sqrMagnitude;

        // Checks for each range 
        if (dis <= Mathf.Pow(closeRange, 2))
        {
            return DistanceZone.close;
        }
        else if (dis <= Mathf.Pow(midRange, 2))
        {
            return DistanceZone.mid;
        }
        else if (dis <= Mathf.Pow(farRange, 2))
        {
            return DistanceZone.far;
        }
        else
        {
            return DistanceZone.NotWithinRange;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void UpdateAlertness()
    {

    }

    /// <summary>
    /// Spearine attacks the player via animation 
    /// </summary>
    private void Attack()
    {
        // Lunges head towards target according to range 
    }

    /// <summary>
    /// Brings the player back and then resets the alertness
    /// </summary>
    private void ResetPosition()
    {

    }

    /// <summary>
    /// Whether or not the Spearine can see the target 
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    private bool CanDetect(Vector3 pos)
    {
        return true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, closeRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, midRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, farRange);
    }
}
