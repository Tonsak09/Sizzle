using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamManager : MonoBehaviour
{
    [SerializeField] GameObject commonCam;
    [SerializeField] Transform pointOfInterest;

    private GameObject current;

    public GameObject Current { get { return current; } }

    // Start is called before the first frame update
    void Start()
    {
        current = commonCam;
    }


    /// <summary>
    /// Replaces the current camera with the ones cam 
    /// </summary>
    /// <param name="newCam"></param>
    public void ChangeCam(GameObject newCam)
    {
        current.SetActive(false);
        current = newCam;
        current.SetActive(true);
    }

    public void ReturnToCommon()
    {
        ChangeCam(commonCam);
    }

}
