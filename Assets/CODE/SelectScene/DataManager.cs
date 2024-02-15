using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using System;
using System.Runtime.InteropServices;


public enum CharacterNum
{
    Female, Male
}

[Serializable]
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
public class DataManager : MonoBehaviour
{

    public static DataManager inst;
    public CharacterNum CurrentCharacter;
    public int curNum { get { return (int)CurrentCharacter; } }

    DatabaseReference dataRef;
    
    [SerializeField] string userEmail;

    [SerializeField] private UserGameInfo curruntPlayUserInfo;

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

        dataRef = FirebaseDatabase.DefaultInstance.RootReference;
        
    }
    void Start()
    {
       
    }

 
    public void F_NewUserSet(string UserEmail)
    {
        UserGameInfo user = new UserGameInfo(0,0,0);
        string json = JsonUtility.ToJson(user); // Json파일 변환
        UserEmail = UserEmail.Replace(".", ""); // .을 서버에서 인식을 못함
        dataRef.Child("User_Email_List").Child(UserEmail).SetRawJsonValueAsync(json);
    }

    public void F_SaveGameAndServerUpload(UserGameInfo data)
    {
        string json = JsonUtility.ToJson(data); // 현재 기록 Json파일 변환
        
        dataRef.Child("User_Email_List").Child(userEmail).SetRawJsonValueAsync(json);

        print("Auto Save Complete");
    }
    public void F_UserLoginAndPoolServerData(string UserEmail)
    {
        UserEmail = UserEmail.Replace(".", "");
        userEmail = UserEmail; // 로컬에 저장
        StartCoroutine(LoadData(userEmail));
    }

    IEnumerator LoadData(string UserEmail)
    {
        var severData = dataRef.Child("User_Email_List").Child(UserEmail).GetValueAsync(); // 서버에서 데이터 가져옴

        yield return new WaitUntil(predicate: () => severData.IsCompleted); // 서버데이터를 가져올때까지 기다림

        print("Data Pull is complete");

        DataSnapshot snapshot = severData.Result; 
        string jsonData = snapshot.GetRawJsonValue();

        if(jsonData != null)
        {
            curruntPlayUserInfo =  JsonUtility.FromJson<UserGameInfo>(jsonData); //데이터를 안에 그대로 입력해줌
        }
        else
        {
            print("no Data");
        }
    }

    public UserGameInfo F_GetUserData()
    {
        return curruntPlayUserInfo;
    }
}
