using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamZone : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera cam;

    private void OnTriggerEnter(Collider other)
    {
        GameManager.SwapCamera(cam);
    }
}
