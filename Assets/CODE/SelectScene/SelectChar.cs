using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SelectChar : MonoBehaviour
{
    public CharacterNum Character;
    [SerializeField] GameObject Btn;
    Button startBtn; 

    SpriteRenderer sr;
    int num;
    
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        startBtn = Btn.GetComponent<Button>();
        startBtn.onClick.AddListener(() => { SceneManager.LoadScene(2); });
    }
    private void Update()
    {
        num = DataManager.inst.curNum;

        switch (Character) 
        {
            case CharacterNum.Female:
                if(num == 0)
                {
                    sr.color = Color.white;
                }
                else if(num == 1) 
                {
                    sr.color = new Color(1, 1, 1, 0.2f);
                }
                break;

            case CharacterNum.Male:
                if (num == 1)
                {
                    sr.color = Color.white;
                }
                else if (num == 0)
                {
                    sr.color = new Color(1, 1, 1, 0.2f);
                }
                break;
        }
    }

    private void OnMouseUpAsButton()
    {
        DataManager.inst.CurrentCharacter = Character;
        Btn.SetActive(true);
    }
}
