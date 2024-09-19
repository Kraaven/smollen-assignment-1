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

        RollNumber = Root.Q<TextField>("ID-input");
        Name = Root.Q<Label>("StudentName");
        Age = Root.Q<Label>("StudentAge");
        Department = Root.Q<Label>("StudentDepartment");
        //Email = Root.Q<Label>("stu");
        Gender = Root.Q<Label>("StudentGender");
        IDNumber = Root.Q<Label>("StudentID");
        GetButton = Root.Q<Button>("submit-button");
        Submissiontext = Root.Q<Label>("Note");
        
        GetButton.clicked += GetAndPopulateData;

        Submissiontext.text = "Give the ID of a student";
        Submissiontext.style.color = new StyleColor(Color.black);
        
    }

    public void GetAndPopulateData()
    {
        print(RollNumber.text);
        Manager.Instance.GetData(RollNumber.text, (data =>
        {
            print(data);
            if (data == null)
            {
                Submissiontext.text = "Student ID does not exist";
                Submissiontext.style.color = new StyleColor(Color.red);
            }
            else
            {
                var DATA = JsonUtility.FromJson<RStudentData>(data);
                
                print($"RETRIEVED: {JsonUtility.ToJson(DATA)}");

                Name.text = DATA.Name;
                Age.text = DATA.Age.ToString();
                Gender.text = DATA.Gender;
                Department.text = DATA.Department;
                //Email.text = DATA.Email;
                IDNumber.text = DATA.RollNumber;
                
                Root.MarkDirtyRepaint();
            }
        }));
    }
}

