using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuScript : MonoBehaviour
{
    public UIDocument Document;
    private VisualElement Root;


    private Button RegisterButton;
    private Button FetchButton;
    private Toggle FireToggle;
    private Label FireToggleLabel;
    
    
    //private 
    
    // Start is called before the first frame update
    void Start()
    {
        Root = Document.rootVisualElement;
        RegisterButton = Root.Q<Button>("Register");
        FetchButton = Root.Q<Button>("Fetch");
        FireToggle = Root.Q<Toggle>("DataBaseType");
        FireToggleLabel = FireToggle.Q<Label>();

        RegisterButton.clicked += OnRegisterClick;
        FetchButton.clicked += OnFetchClick;
        
        FireToggle.RegisterValueChangedCallback(FireToggled);
        Manager.UseFireStore = false;
        FireToggle.style.color = new StyleColor(Color.white);
    }

    public void OnRegisterClick()
    {
        print("Register");
        Manager.Instance.ViewRegistration();
    }
    
    public void OnFetchClick()
    {
        print("Fetch");
        Manager.Instance.ViewFetch();
    }


    public void FireToggled(ChangeEvent<bool> evt)
    {
        print($"Toggled {evt.newValue}");
        if (evt.newValue)
        {
            Manager.UseFireStore = true;
            FireToggleLabel.style.color = new StyleColor(Color.red);
        }
        else
        {
            Manager.UseFireStore = false;
            FireToggleLabel.style.color = new StyleColor(Color.white);
        }
    }

}
