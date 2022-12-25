using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AuthUI : AuthManager
{
    public static AuthUI instance;
    [Space]
    [Header("Ekran objeleri değişkenleri")]
    public GameObject loginUI;
    public GameObject registerUI;
    public GameObject linkPanel;
    public GameObject screenText; //kaldırılabilir
    [Space]
    [Header("Giriş ekranı değişkenleri")]
    public InputField emailLoginField;
    public InputField passwordLoginField;
    public Text warningLoginText; //kaldırılabilir
    public Text confirmLoginText; //kaldırılabilir
    [Space]
    [Header("Kayıt ekranı değişkenleri")]
    public InputField usernameRegisterField;
    public InputField emailRegisterField;
    public InputField passwordRegisterField;
    public GameObject userNameInfoPanel;
    public Text warningRegisterText;    //kaldırılabilir
    public Toggle verifyToogle;
    
    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Debug.Log("Örnek zaten var, nesneyi yok ediyor!");
            Destroy(this);
        }
    }


    public void userNameInfoButton()
    {
        userNameInfoPanel.SetActive(!userNameInfoPanel.activeSelf);
    }

    public void linkButton()
    {
        linkPanel.SetActive(!linkPanel.activeSelf);
    }

    public void loginScreenButton() 
    {
        loginUI.SetActive(true);
        registerUI.SetActive(false);
    }

    public void registerScreenButton()
    {
        loginUI.SetActive(false);
        registerUI.SetActive(true);
    }

    public void onWriteScreenText(string screenSTR)
    {
        // Burası Kaldırılabilir
        
        screenText.transform.GetChild(0).GetComponent<Text>().text = screenSTR;
        screenText.GetComponent<Animation>().Stop();
        screenText.GetComponent<Animation>().Play();
    }
    

    public void showHidePass(InputField passField)
    {
        // Şifre giriş alanında göster/gizle butonu
        if (passField.contentType == InputField.ContentType.Password)
        {
            passField.contentType = InputField.ContentType.Standard;
        }
        else
        {
            passField.contentType = InputField.ContentType.Password;
        }   
        passwordRegisterField.ForceLabelUpdate();

    }
    
}