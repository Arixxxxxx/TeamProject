using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterNum
{
    Female, Male
}

public class DataManager : MonoBehaviour
{

    public static DataManager inst;
    public CharacterNum CurrentCharacter;
    public int curNum { get {  return (int)CurrentCharacter; } }
    
    private void Awake()
    {
        if(inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
    }
    void Start()
    {
        
    }

    
}
