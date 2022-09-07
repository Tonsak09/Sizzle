using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterestZone : MonoBehaviour
{
    [SerializeField] LayerMask interestLayer;
    [SerializeField] Vector3 areaCenter;
    [SerializeField] float areaSize;

    [SerializeField] HeadTargeting targeting;

    private void Update()
    {
        List<PointOfInterest> points = new List<PointOfInterest>();
        Collider[] colliders = Physics.OverlapSphere(this.transform.position + areaCenter, areaSize, interestLayer);
        foreach (Collider  item in colliders)
        {
            PointOfInterest poi = item.GetComponent<PointOfInterest>();
            if (poi != null)
            {
                points.Add(poi);
            }
        }

        if(points.Count > 0)
        {
            PointOfInterest closestPoint = points[0];
            for (int i = 0; i < points.Count; i++)
            {
                if(Vector3.Distance(this.transform.position, points[0].transform.position) < Vector3.Distance(this.transform.position, closestPoint.Target.position))
                {
                    closestPoint = points[i];
                }
            }
            targeting.PointOfInterest = closestPoint.Target;
        }
        else
        {
            targeting.PointOfInterest = null;
        }
    }

    private void AddCollider(Collider other)
    {
        PointOfInterest poi = other.GetComponent<PointOfInterest>();
        print(other.gameObject.name);
        if (poi != null)
        {
            //points.Add(poi);
            print("Added " + other.gameObject.name);
        }
    }

    private void RemoveCollider(Collider other)
    {
        PointOfInterest poi = other.GetComponent<PointOfInterest>();

        if (poi != null)
        {
            //points.Remove(poi);
            print("Removed " + other.gameObject.name);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position + areaCenter, areaSize);
    }
}
