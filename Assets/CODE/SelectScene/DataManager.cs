using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;


public enum CharacterNum
{
    Female, Male
}

public class DataManager : MonoBehaviour
{

    public static DataManager inst;
    public CharacterNum CurrentCharacter;
    public int curNum { get { return (int)CurrentCharacter; } }

    DatabaseReference dataRef;
    int dataCount = 1;
    public class UserGameInfo
    {
        public int bossKillCount; // 보스킬 카운터
        public int totalKillEnemy; // 토탈 에너미 카운터
        public int LevelUpCount; // 토탈 레벨업 카운터

        public UserGameInfo(int bossKillCount, int totalKillEnemy, int LevelUpCount) // 생성자
        {
            this.bossKillCount = bossKillCount;
            this.totalKillEnemy = totalKillEnemy;
            this.LevelUpCount = LevelUpCount;
        }
    }

    private void Awake()
    {
        if (inst == null)
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
        dataRef = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void F_NewUserSet(string UserEmail)
    {
        UserGameInfo user = new UserGameInfo(0,0,0);
        string json = JsonUtility.ToJson(user); // Json파일 변환

        dataRef.Child(UserEmail).SetRawJsonValueAsync(json);
    } 
    

}
