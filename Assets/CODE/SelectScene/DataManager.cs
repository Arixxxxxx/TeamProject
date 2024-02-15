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
    public int bossKillCount; // ����ų ī����
    public int totalKillEnemy; // ��Ż ���ʹ� ī����
    public int LevelUpCount; // ��Ż ������ ī����

    public UserGameInfo(int bossKillCount, int totalKillEnemy, int LevelUpCount) // ������
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
        string json = JsonUtility.ToJson(user); // Json���� ��ȯ
        UserEmail = UserEmail.Replace(".", ""); // .�� �������� �ν��� ����
        dataRef.Child("User_Email_List").Child(UserEmail).SetRawJsonValueAsync(json);
    }

    public void F_SaveGameAndServerUpload(UserGameInfo data)
    {
        string json = JsonUtility.ToJson(data); // ���� ��� Json���� ��ȯ
        
        dataRef.Child("User_Email_List").Child(userEmail).SetRawJsonValueAsync(json);

        print("Auto Save Complete");
    }
    public void F_UserLoginAndPoolServerData(string UserEmail)
    {
        UserEmail = UserEmail.Replace(".", "");
        userEmail = UserEmail; // ���ÿ� ����
        StartCoroutine(LoadData(userEmail));
    }

    IEnumerator LoadData(string UserEmail)
    {
        var severData = dataRef.Child("User_Email_List").Child(UserEmail).GetValueAsync(); // �������� ������ ������

        yield return new WaitUntil(predicate: () => severData.IsCompleted); // ���������͸� �����ö����� ��ٸ�

        print("Data Pull is complete");

        DataSnapshot snapshot = severData.Result; 
        string jsonData = snapshot.GetRawJsonValue();

        if(jsonData != null)
        {
            curruntPlayUserInfo =  JsonUtility.FromJson<UserGameInfo>(jsonData); //�����͸� �ȿ� �״�� �Է�����
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
