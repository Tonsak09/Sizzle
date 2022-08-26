using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foot : MonoBehaviour
{

    [SerializeField] Transform root;
    [SerializeField] LayerMask terrain;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = root.position;

        // Match with floor normal 
        RaycastHit hit;
        Physics.Raycast(this.transform.position, Vector3.down, out hit, 10, terrain);

        this.transform.forward = -hit.normal;
        print(hit.normal);
    }
}
