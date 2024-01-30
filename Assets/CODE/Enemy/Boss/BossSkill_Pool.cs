using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkill_Pool : MonoBehaviour
{
    public static BossSkill_Pool inst;

    [Header("# Insert Skill Prefabs")]
    [Space] // 0 벼락
    [SerializeField] int AwakeIntanti_Ea;
    [SerializeField] GameObject[] skillEffect;
    [SerializeField] Transform[] SkillTrs;

    // Queue
    Queue<GameObject>[] skillQueue = new Queue<GameObject>[4];

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

        for(int i = 0; i < skillQueue.Length; i++)
        {
            skillQueue[i] = new Queue<GameObject>();
        }

        if (AwakeIntanti_Ea == 0)
        {
            Debug.Log("초기화하는 인스펙터값이 비어있습니다. 기본값 20으로 설정합니다.");
            AwakeIntanti_Ea = 20;
        }

        for(int i = 0; i < SkillTrs.Length;  i++)
        {
            for(int j = 0; j < AwakeIntanti_Ea; j++)
            {
                GameObject obj = Instantiate(skillEffect[i], SkillTrs[i]);
                obj.transform.position = Vector3.zero;
                obj.gameObject.SetActive(false);
                skillQueue[i].Enqueue(obj);
            }
        }
    }
    void Start()
    {

    }
    /// <summary>
    ///  Get Obj In Pool
    /// </summary>
    /// <param name="value"> 0 = Lightning </param>
    /// <returns></returns>
    public GameObject F_GetSkillObj(int value)
    {

        GameObject obj;

        if (skillQueue[value].Count == 0)
        {
            obj = Instantiate(skillEffect[value], SkillTrs[value]);
            obj.transform.position = Vector3.zero;
            obj.gameObject.SetActive(false);
            skillQueue[value].Enqueue(obj);
        }

        obj = skillQueue[value].Dequeue();

        return obj;
    }


    /// <summary>
    /// Return SkillEffect
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="roomNum"></param>
    public void F_ReturnSkillEfeect(GameObject obj, int roomNum)
    {
        if(obj.activeSelf == true)
        {
            obj.gameObject.SetActive (false);
        }

        obj.transform.position = Vector3.zero;
        skillQueue[roomNum].Enqueue(obj);
    }
}
