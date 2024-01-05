using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    Camera mainCam;
    [SerializeField] CinemachineVirtualCamera playerCam;
    [SerializeField] float pos_Minusx;
    [SerializeField] float pos_PlusX;
    [SerializeField] float pos_MinusY;
    [SerializeField] float pos_PlusY;

    private void Awake()
    {
        mainCam = Camera.main;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {
        playerCamsLimitPos(); 
    }
    private void playerCamsLimitPos()
    {

        Vector3 campos = playerCam.transform.position;
        campos.x = Mathf.Clamp(campos.x, pos_Minusx, pos_PlusX);
        campos.y = Mathf.Clamp(campos.y, pos_MinusY, pos_PlusY);

        mainCam.transform.position = campos;
        playerCam.transform.position = campos;
    }
}
