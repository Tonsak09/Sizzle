using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseCopyManager : MonoBehaviour
{

    [SerializeField] List<PoseCopy> body;
    [SerializeField] List<PoseCopy> legs;

    private Quaternion[] originalValuesBody;


    private enum SizzleSections
    {
        neck,
        jaw,
        mid,
        tail,
        frontLegs,
        backLegs
    };

    // Start is called before the first frame update
    void Start()
    {
        originalValuesBody = new Quaternion[body.Count];

        for (int i = 0; i < body.Count; i++)
        {
            originalValuesBody[i] = body[i].TargetValue;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < body.Count; i++)
        {
            body[i].UpdateTarget(originalValuesBody[i]);
        }
    }
}
