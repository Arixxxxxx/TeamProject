using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transform_Z_Rotaion : MonoBehaviour
{
    float RotationFloat;
    [SerializeField] float SpinSpeed;
    Vector3 rot;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RotationFloat += Time.deltaTime * SpinSpeed;
        RotationFloat = Mathf.Repeat(RotationFloat, 360);
        rot.z = RotationFloat;

        transform.eulerAngles = rot;
    }

    public void F_Set_SpinSpeed(float Value)
    {
        SpinSpeed = Value;
    }
}
