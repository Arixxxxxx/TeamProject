using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPointSc : MonoBehaviour
{
    
    [SerializeField] int areaNumber;
    public int AreaNumber {  get { return areaNumber; } }
    
    void Start()
    {
        
    }
    
    public void F_Input_Area_Value(int value)
    {
        areaNumber = value;
    }


}
