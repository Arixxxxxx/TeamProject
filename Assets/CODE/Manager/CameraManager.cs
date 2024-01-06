using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager inst;

    Camera mainCam;
    [SerializeField] CinemachineVirtualCamera playerCam;
    [SerializeField] float pos_Minusx;
    [SerializeField] float pos_PlusX;
    [SerializeField] float pos_MinusY;
    [SerializeField] float pos_PlusY;
    [Header("Camera Zoom value = 4 value")]
    [SerializeField] float[] camZoomValue;
    [SerializeField] float zoomOutSpeed;
    private void Awake()
    {
        mainCam = Camera.main;
        if(inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(this);
        }
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

    /// <summary>
    /// Ä«¸Þ¶ó ÁÜ¾Æ¿ôÇÔ¼ö
    /// </summary>
    /// <param name="value"></param>
    public void F_CameraZoomOut(int value)
    {
        float sumValue = playerCam.m_Lens.OrthographicSize + camZoomValue[value];

       StartCoroutine(ZoomOut(sumValue));
    }
    
    IEnumerator ZoomOut(float sumValue)
    {
        while (playerCam.m_Lens.OrthographicSize < sumValue)
        {
            playerCam.m_Lens.OrthographicSize += Time.deltaTime * zoomOutSpeed;

            yield return null;
        }
    }
}
