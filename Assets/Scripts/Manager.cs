using System;
using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using Firebase.Firestore;
using Firebase.Extensions;
using Google.MiniJSON;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static bool UseFireStore;
    public static Manager Instance;
    public GameObject MainMenu;
    public GameObject RegistrationMenu;
    public GameObject FetchMenu;
    
    private static DatabaseReference databaseReference;
    private static FirebaseFirestore firestoreDatabase;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
            firestoreDatabase = FirebaseFirestore.DefaultInstance;
        });
    }

    public static void UploadData(StudentData data)
    {
        if (UseFireStore)
        {
            Debug.Log("Uploading to FireStore Database");
            DocumentReference docRef = firestoreDatabase.Collection("Students").Document(data.RollNumber);
            docRef.SetAsync(data);
        }
        else
        {
            Debug.Log("Uploading to Realtime Database");
            // // databaseReference.Child("Students").Child(data.Email).SetRawJsonValueAsync(JsonUtility.ToJson(data))
            // //     .ContinueWithOnMainThread(task => {
            // //         if (task.IsCompleted)
            // //         {
            // //             Debug.Log("Data successfully written!");
            // //         }
            // //         else
            // //         {
            // //             Debug.LogError("Error writing data: " + task.Exception);
            // //         }
            // //     });
            //
            // Dictionary<string, object> legacydata = new Dictionary<string, object>
            // {
            //     { "Email", data.Email },
            //     { "Name", data.Name },
            //     { "Age", data.Age },
            //     { "Department", data.Department },
            //     { "Gender", data.Gender }
            // };
            //
            // databaseReference.Child("Students").Child(legacydata["Email"].ToString()).SetValueAsync(legacydata)
            //     .ContinueWithOnMainThread(task => {
            //         if (task.IsCompleted)
            //         {
            //             Debug.Log("Test Data successfully written!");
            //         }
            //         else
            //         {
            //             Debug.LogError("Error writing test data: " + task.Exception);
            //         }
            //     });

            databaseReference.Child("Students").Child(data.RollNumber).Child("Name").SetValueAsync(data.Name);
            databaseReference.Child("Students").Child(data.RollNumber).Child("Age").SetValueAsync(data.Age);
            databaseReference.Child("Students").Child(data.RollNumber).Child("Gender").SetValueAsync(data.Gender);
            databaseReference.Child("Students").Child(data.RollNumber).Child("Department").SetValueAsync(data.Department);
            databaseReference.Child("Students").Child(data.RollNumber).Child("Email").SetValueAsync(data.Email);
            databaseReference.Child("Students").Child(data.RollNumber).Child("Roll Number").SetValueAsync(data.RollNumber);

        }
    }


    public static StudentData GetData(string ID)
{
    if (UseFireStore)
    {
        Debug.Log("Retrieving data from Firestore Database");
        return GetDataFromFirestore(ID);
    }
    else
    {
        Debug.Log("Retrieving data from Realtime Database");
        return GetDataFromRealtimeDatabase(ID);
    }
}

private static StudentData GetDataFromFirestore(string ID)
{
    var docRef = firestoreDatabase.Collection("Students").Document(ID);
    var task = docRef.GetSnapshotAsync();
    task.ContinueWithOnMainThread(t => {
        if (t.IsCompleted && !t.IsFaulted && !t.IsCanceled)
        {
            DocumentSnapshot snapshot = t.Result;
            if (snapshot.Exists)
            {
                StudentData data = snapshot.ConvertTo<StudentData>();
                Debug.Log($"Data retrieved from Firestore: {JsonUtility.ToJson(data)}");
                return data;
            }
            else
            {
                Debug.LogWarning($"No data found in Firestore for ID: {ID}");
                return null;
            }
        }
        else
        {
            Debug.LogError($"Error retrieving data from Firestore: {t.Exception}");
            return null;
        }
    });
    return null;
}

private static StudentData GetDataFromRealtimeDatabase(string ID)
{
    var task = databaseReference.Child("Students").Child(ID).GetValueAsync();
    task.ContinueWithOnMainThread(t => {
        if (t.IsCompleted && !t.IsFaulted && !t.IsCanceled)
        {
            DataSnapshot snapshot = t.Result;
            if (snapshot.Exists)
            {
                StudentData data = snapshot.Value as StudentData;
                if (data != null)
                {
                    Debug.Log($"Data retrieved from Realtime Database: {JsonUtility.ToJson(data)}");
                    return data;
                }
                else
                {
                    Debug.LogWarning($"Data format incorrect for ID: {ID}");
                    return null;
                }
            }
            else
            {
                Debug.LogWarning($"No data found in Realtime Database for ID: {ID}");
                return null;
            }
        }
        else
        {
            Debug.LogError($"Error retrieving data from Realtime Database: {t.Exception}");
            return null;
        }
    });
    return null;
}

    private void Awake()
    {
        Instance = this;
        MainMenu.SetActive(true);
        RegistrationMenu.SetActive(false);
        FetchMenu.SetActive(false);
    }

    public void ViewRegistration()
    {
        MainMenu.SetActive(false);
        RegistrationMenu.SetActive(true);
    }
    
    public void ViewFetch()
    {
        MainMenu.SetActive(false);
        FetchMenu.SetActive(true);
    }
}

[FirestoreData]
public class StudentData
{
    [FirestoreProperty]
    public string Email { get; set; }

    [FirestoreProperty]
    public string Name { get; set; }

    [FirestoreProperty]
    public int Age { get; set; }

    [FirestoreProperty]
    public string Department { get; set; }

    [FirestoreProperty]
    public string Gender { get; set; }
    
    [FirestoreProperty]
    public string RollNumber { get; set; }

    public StudentData(string E, string N, string D, string G, int A, string R)
    {
        Email = E;
        Name = N;
        Age = A;
        Department = D;
        Gender = G;
        RollNumber = R;
    }

    public StudentData()
    {
    }
}