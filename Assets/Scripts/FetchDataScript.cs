using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FetchDataScript : MonoBehaviour
{
    public UIDocument Document;
    private VisualElement Root;

    private TextField RollNumber;

    private Label Name;
    private Label Age;
    private Label Department;
    private Label Email;
    private Label Gender;
    private Label IDNumber;

    private Button GetButton;
    private Label Submissiontext;

    // Start is called before the first frame update
    void OnEnable()
    {
        Root = Document.rootVisualElement;

        Name = Root.Q<Label>("StudentName");
        Age = Root.Q<Label>("StudentAge");
        Department = Root.Q<Label>("StudentDepartment");
        Email = Root.Q<Label>("StudentDepartment");
        Gender = Root.Q<Label>("StudentGender");
        IDNumber = Root.Q<Label>("StudentID");
        GetButton = Root.Q<Button>("submit-button");

        GetButton.clicked += GetAndPopulateData;


    }

    public void GetAndPopulateData()
    {
        StudentData data = Manager.GetData(IDNumber.text);
    }
}

