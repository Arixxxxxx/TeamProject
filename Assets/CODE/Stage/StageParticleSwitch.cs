using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageParticleSwitch : MonoBehaviour
{
    [Header("#Insert StageParticle Object")]
    [Space]
    [SerializeField] ParticleSystem[] stagePs;
    [SerializeField] int PlayerPlaceNum;
    SpawnManager sm;
    void Start()
    {
        sm = SpawnManager.inst;
    }

    bool once;
    void Update()
    {
        
        PlayerPlaceNum = sm.PlayerAreaNum;
        
        if (PlayerPlaceNum == 0)
        {
            stagePs[0].gameObject.SetActive(true);
            stagePs[1].gameObject.SetActive(false);
            stagePs[2].gameObject.SetActive(false);
        }
        else if(PlayerPlaceNum == 1) 
        {
            stagePs[0].gameObject.SetActive(false);
            stagePs[1].gameObject.SetActive(true);
            stagePs[2].gameObject.SetActive(false);
        }
        else if( PlayerPlaceNum == 2) 
        {
            stagePs[0].gameObject.SetActive(false);
            stagePs[1].gameObject.SetActive(false);
            stagePs[2].gameObject.SetActive(true);
        }
        
    }
}
