using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbCopyManager : MonoBehaviour
{

    [SerializeField] List<LimbCopy> limbs;

    private Quaternion[] originalValues;

    // Start is called before the first frame update
    void Start()
    {
        originalValues = new Quaternion[limbs.Count];

        for (int i = 0; i < limbs.Count; i++)
        {
            originalValues[i] = limbs[i].targetValue;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < limbs.Count; i++)
        {
            limbs[i].UpdateTarget(originalValues[i]);
        }
    }
}
