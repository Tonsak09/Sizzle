using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Charges objects that it comes in contact with 
public class ChargeObj : MonoBehaviour
{
    
    [SerializeField] float chargeAmount;

    private void OnTriggerEnter(Collider other)
    {
        Chargeable objChargeable = other.GetComponent<Chargeable>();

        if (objChargeable != null)
        {
            objChargeable.AddCharge(chargeAmount);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Some chargeables exit by "destroy" so this is current solution TODO: Change to hold in local array 
        Chargeable objChargeable = other.GetComponent<Chargeable>();

        if (objChargeable != null)
        {
            // Staying in the charge filed continues to add charge 
            objChargeable.AddCharge(chargeAmount * Time.deltaTime);
        }
    }
}
