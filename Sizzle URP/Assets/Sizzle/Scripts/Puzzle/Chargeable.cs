using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chargeable : MonoBehaviour
{
    [SerializeField] protected float desperseChargeSpeed;
    [SerializeField] protected float maxCharge;
    protected private float currentCharge;

    // Only meant for debug to read 
    public float CurrentCharge;

    /// <summary>
    /// Called when trying to charge this object 
    /// </summary>
    public virtual void AddCharge(float chargeAmount)
    {
        if((currentCharge + chargeAmount) > maxCharge)
        {
            currentCharge = maxCharge;
        }
        else
        {
            currentCharge += chargeAmount;
        }
    }

    /// <summary>
    /// Looses charge every update at speed 
    /// </summary>
    public virtual void Desperse()
    {
        if(currentCharge <= 0)
        {
            currentCharge = 0;
            return;
        }

        currentCharge -= Time.deltaTime * desperseChargeSpeed;
    }

    private void Update()
    {
        Desperse();

        CurrentCharge = currentCharge;
    }
}
