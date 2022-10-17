using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spearine : MonoBehaviour
{
    [Header("Ranges")]
    [SerializeField] float closeRange;
    [SerializeField] float midRange;
    [SerializeField] float farRange;

    private Transform player;

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
        // Gets player rather than Sizzle because Sizzle is a folder that doesn't represent the tru position 
        player = GameObject.FindWithTag("Player").transform;

    }

    // Update is called once per frame
    void Update()
    {
        print(GetZone(player.position));
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
