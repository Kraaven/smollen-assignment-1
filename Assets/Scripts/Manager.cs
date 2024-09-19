using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Firebase.Database;
using Firebase.Firestore;
using Firebase.Extensions;
using Google.MiniJSON;
using UnityEngine;
using Object = UnityEngine.Object;

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
            

            databaseReference.Child("Students").Child(data.RollNumber).Child("Name").SetValueAsync(data.Name);
            databaseReference.Child("Students").Child(data.RollNumber).Child("Age").SetValueAsync(data.Age);
            databaseReference.Child("Students").Child(data.RollNumber).Child("Gender").SetValueAsync(data.Gender);
            databaseReference.Child("Students").Child(data.RollNumber).Child("Department").SetValueAsync(data.Department);
            databaseReference.Child("Students").Child(data.RollNumber).Child("Email").SetValueAsync(data.Email);
            databaseReference.Child("Students").Child(data.RollNumber).Child("Roll Number").SetValueAsync(data.RollNumber);

        }
    }


 public void GetData(string ID, Action<string> callback)
    {
        if (UseFireStore)
        {
            Debug.Log("Retrieving data from Firestore Database");
            //StartCoroutine(GetDataFromFirestoreCoroutine(ID, callback));
            GetFireStoreData(ID, callback);
        }
        else
        {
            Debug.Log("Retrieving data from Realtime Database");
            //StartCoroutine(GetDataFromRealtimeDatabaseCoroutine(ID, callback));
            GetRealtimeData(ID,callback);
        }
    }
 
    public void GetRealtimeData(string ID, System.Action<string> callback)
    {
        databaseReference.Child("Students").Child(ID).GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                Debug.LogError("Firebase Realtime DB retrieval encountered an error: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                var test = snapshot.Value as Dictionary<string, object>;
                
                print(test["Name"]);
                
                callback(snapshot.GetRawJsonValue());
            }
        });
    }

    public void GetFireStoreData(string ID, Action<string> callback)
    {
        GetFirestoreData("Students", ID).ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Firebase Firestore DB retrieval encountered an error: " + task.Exception);
                callback(null);
            }
            else if (task.IsCompleted)
            {
                string jsonResult = task.Result;
                callback(jsonResult);
            }
        }, TaskScheduler.FromCurrentSynchronizationContext());
    }

    public async Task<string> GetFirestoreData(string collection, string document)
    {
        DocumentReference docRef = firestoreDatabase.Collection(collection).Document(document);
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

        if (snapshot.Exists)
        {
            Dictionary<string, object> docDictionary = snapshot.ToDictionary();
            return JsonUtility.ToJson(new RStudentData(
                docDictionary["Email"].ToString(),
                docDictionary["Name"].ToString(),
                docDictionary["Department"].ToString(),
                docDictionary["Gender"].ToString(),
                int.Parse(docDictionary["Age"].ToString()),
                docDictionary["RollNumber"].ToString()
                ));
        }
        else
        {
            Debug.Log("Document does not exist!");
            return null;
        }
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


public class RStudentData
{

    public string Email;

    public string Name;

    public int Age;

    public string Department;

    public string Gender;

    public string RollNumber;

    public RStudentData(string E, string N, string D, string G, int A, string R)
    {
        Email = E;
        Name = N;
        Age = A;
        Department = D;
        Gender = G;
        RollNumber = R;
    }

    public RStudentData()
    {
    }
}