using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeObj : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] float detectRadius;
    [SerializeField] LayerMask chargeLayer;

    [Header("Charging")]
    [SerializeField] float chargeRate;
    [SerializeField] float desperseRate;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, detectRadius);
    }
}
