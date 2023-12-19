using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPointSc : MonoBehaviour
{
    [SerializeField] private bool noSpawn;
    [SerializeField] int areaNumber;
    public int AreaNumber {  get { return areaNumber; } }
    public bool NoSpawn {  get { return noSpawn; } set { noSpawn = value; } }
    void Start()
    {
        
    }
    
    public void F_Input_Area_Value(int value)
    {
        areaNumber = value;
    }


}
