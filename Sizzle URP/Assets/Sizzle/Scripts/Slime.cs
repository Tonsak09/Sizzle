using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Chargeable
{

    [SerializeField] Material mat;

    [ColorUsage(false, true)]
    public Color baseEmessiveColor;
    [ColorUsage(false, true)]
    public Color targetEmessiveColor;

    //[SerializeField] HD baseEmessiveColor;
    //[SerializeField] Color targetEmessiveColor;

    [SerializeField] float emessivityEmpty;
    [SerializeField] float emessivityFull; 

    private Rigidbody rb;
    private bool grounded;



    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();

    }

    public override void Desperse()
    {
        base.Desperse();

        print((Vector4)baseEmessiveColor);

        // Set emissivity from 0 - 10
        mat.SetColor("_EmissionColor", Color.Lerp(baseEmessiveColor, targetEmessiveColor, Mathf.InverseLerp(0, maxCharge, currentCharge)));

        // Base virtual code will make sure it is never below 0 
        if (currentCharge == 0)
        {
            return;
        }

        if(grounded)
        {
            // Add force oppoiste of direction that charge comes from 
            rb.AddForce(new Vector3(Random.Range(-5, 5), Random.Range(5, 10), Random.Range(-5, 5)), ForceMode.Impulse);
            grounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        grounded = true;
    }
}
