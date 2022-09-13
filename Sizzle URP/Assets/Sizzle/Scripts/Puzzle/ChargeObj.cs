using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeObj : MonoBehaviour
{
    /*[Header("Detection")]
    [SerializeField] float detectRadius;
    [SerializeField] LayerMask chargeLayer;

    [Header("Charging")]
    [SerializeField] float chargeRate;
    [SerializeField] float desperseRate;*/

    private void OnTriggerEnter(Collider other)
    {
        print(other.gameObject.name);

        if (other.gameObject.tag == "Slime")
        {
            other.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-5, 5), Random.Range(5, 10), Random.Range(-5, 5)), ForceMode.Impulse);
        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(this.transform.position, detectRadius);
    }
}
