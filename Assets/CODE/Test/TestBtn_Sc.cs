using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestBtn_Sc : MonoBehaviour
{
    [SerializeField] GameObject TestBtn;
    Button startBtn;
    void Start()
    {
        startBtn = TestBtn.transform.Find("StartBtn").GetComponent<Button>();
        startBtn.onClick.AddListener(() => GameManager.Inst.MainGameStart = true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
