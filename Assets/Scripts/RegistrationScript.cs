using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RegistrationScript : MonoBehaviour
{
    public UIDocument Document;
    private VisualElement Root;

    private TextField Name;
    private IntegerField Age;
    private TextField Department;
    private TextField Email;
    private DropdownField Gender;
    private Button SubmitButton;
    private TextField RollNumber;
    
    private Label Submissiontext;
    // Start is called before the first frame update
    void OnEnable()
    {
        Root = Document.rootVisualElement;

        Name = Root.Q<TextField>("name-input");
        Age = Root.Q<IntegerField>("age-input");
        Department = Root.Q<TextField>("department-input");
        Email = Root.Q<TextField>("email-input");
        Gender = Root.Q<DropdownField>("gender-dropdown");
        RollNumber = Root.Q<TextField>("Roll-input");
        SubmitButton = Root.Q<Button>("submit-button");
        Submissiontext = Root.Q<Label>("SubmissionStatus");

        if (SubmitButton == null)
        {
            print("SubmitButton is null, check the UXML element name");
        }
        else
        {
            SubmitButton.clicked += SubmitDetails;
        }

        if (Submissiontext == null)
        {
            print("Submissiontext is null, check the UXML element name");
        }
        else
        {
            Submissiontext.text = "Input your details";
        }
    }


    public void SubmitDetails()
    {
        print("Fire Submission");

        string stdname = Name.value;
        int age = Age.value;
        string dept = Department.value;
        string email = Email.value;
        string gender = Gender.value;
        string rollnumber = RollNumber.value;
        
        if (string.IsNullOrEmpty(stdname) || string.IsNullOrEmpty(dept) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(gender) || age == 0)
        {
            Submissiontext.text = "Error: Fill all the fields";
            Submissiontext.style.color = new StyleColor(Color.red);
        }
        else
        {
            Submissiontext.text = $"Submited";
            Submissiontext.style.color = new StyleColor(new Color(0, 60, 8));
            
            Manager.UploadData(new StudentData(
                E : email,
                N : stdname,
                D : dept,
                G : gender,
                A : age,
                R : rollnumber
                ));
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
