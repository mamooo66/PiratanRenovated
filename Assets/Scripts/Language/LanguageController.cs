using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LanguageController : MonoBehaviour
{
    
    [SerializeField] private TextAsset currentJson;

    [SerializeField] private TextAsset[] languages;
    [SerializeField] private Dropdown languageDropDown;
    [SerializeField] private int SelectedLanguageIndex;
    
    [Header("Auth UI")]
    [SerializeField] private Text Discord;
    [SerializeField] private Text Support;
    [SerializeField] private Text ReportBug;
    [SerializeField] private Text Website;
    
    [Header("Register UI")] 
    [SerializeField] private Text UserName;
    [SerializeField] private Text Password;
    [SerializeField] private Text Email;
    [SerializeField] private Text CreateNewAccount;
    [SerializeField] private Text OrLogin;
    [SerializeField] private Text BecomeaPirateLegend;
    [SerializeField] private Text Terms;
    [SerializeField] private Text Tooltip;
    

    [Header("Login UI")]
    [SerializeField] private Text GetOnDeck;
    [SerializeField] private Text EmailLogin;
    [SerializeField] private Text PasswordLogin;
    
    [Header("Home UI")] 
    [SerializeField] private Text GoToBattle;
    [SerializeField] private Text Rank;
    [SerializeField] private Text Skills;
    [SerializeField] private Text Dock;
    [SerializeField] private Text Events;
    [SerializeField] private Text Market;


    void Start()
    {
        if (PlayerPrefs.HasKey("selectedLanguage"))
        {
            SelectedLanguageIndex = PlayerPrefs.GetInt("selectedLanguage");
            languageDropDown.value = SelectedLanguageIndex;
        }
        
        
    }

    private void FixedUpdate()
    {
        languageChangedDropdown();
    }
    
    public void ChangeLanguage(int _languageIndex)//0
    {
        currentJson = languages[_languageIndex];
        LanguageData data = new LanguageData();
        PlayerPrefs.SetInt("selectedLanguage", _languageIndex);
        data = JsonUtility.FromJson<LanguageData>(currentJson.text);
        UpdateDisplay(data);

    }

    private void UpdateDisplay(LanguageData data)
    {
        Scene scene = SceneManager.GetActiveScene();

        if (scene.name == "Home")
        {
            GoToBattle.text = data.GoToBattle;
            Rank.text = data.Rank;
            Skills.text = data.Skills;
            Dock.text = data.Dock;
            Events.text = data.Events;
            Market.text = data.Market;
        }
        else if (scene.name == "AuthMenu")
        {
            UserName.text = data.UserNameRegister;
            Password.text = data.PasswordRegister;
            Tooltip.text = data.Tooltip;
            Email.text = data.EmailRegister;
            BecomeaPirateLegend.text = data.BecomeaPirateLegendRegister;
            Terms.text = data.TermsRegister;
            CreateNewAccount.text = data.CreateNewAccountRegister;
            OrLogin.text = data.OrLoginRegister;
            EmailLogin.text = data.EmailLogin;
            PasswordLogin.text = data.PasswordLogin;
            GetOnDeck.text = data.GetondeckCaptain;
            Discord.text = data.Discord;
            Support.text = data.Support;
            ReportBug.text = data.ReportBug;
            Website.text = data.Website;
        }
        
    }

    public void languageChangedDropdown()
    {
        if (SelectedLanguageIndex == 0)
            ChangeLanguage(0);
 
        if (SelectedLanguageIndex == 1)
            ChangeLanguage(1);
    }
}
