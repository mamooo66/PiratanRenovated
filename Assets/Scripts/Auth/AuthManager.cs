using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using LootLocker.Requests;

public class AuthManager : MonoBehaviour
{
    public void Login()
    {
        string email = AuthUI.instance.emailLoginField.text;
        string password = AuthUI.instance.passwordLoginField.text;
        
        LootLockerSDKManager.WhiteLabelLogin(email, password,false, (response) =>
        {
            if (!response.success)
            {
                Debug.Log("Login Failed");
            }
            else
            {
                Debug.Log("Login Success");
            }

            if (response.VerifiedAt == null)
            {
                
            }
            
            LootLockerSDKManager.StartWhiteLabelSession((response) =>
            {
                if (!response.success)
                {
                    Debug.Log("Session Failed");
                }
                else
                {
                    Debug.Log("Session Success");
                    LoadScene();
                }
            });
        });
    }

    void LoadScene()
    {
        SceneManager.LoadScene("Home");
    }

    // This code should be placed in a handler when user clicks the sign up button.
    public void Register()
    {
        Debug.Log(1);
        string email = AuthUI.instance.emailRegisterField.text;
        string password = AuthUI.instance.passwordRegisterField.text;
        string username = AuthUI.instance.usernameRegisterField.text;
        
        LootLockerSDKManager.WhiteLabelSignUp(email, password, (response) =>
        {
            Debug.Log(2);
            if (!response.success)
            {
                Debug.Log(response.Error);
                return;
            }
            else
            {
                LootLockerSDKManager.WhiteLabelLogin(email,password, false, response =>
                {
                    if (!response.success)
                    {
                        Debug.Log(response.Error);
                        return;
                    }
                    LootLockerSDKManager.StartWhiteLabelSession((response) =>
                    {
                        if (!response.success)
                        {
                            Debug.Log(response.Error);
                            return;
                        }

                        //username = response.public_uid;
                        
                        username = username;
                        
                        LootLockerSDKManager.SetPlayerName(username, (response) =>
                        {
                            if (!response.success)
                            {
                                Debug.Log(response.Error);
                                return;
                            }

                            LootLockerSessionRequest sessionRequest = new LootLockerSessionRequest();
                            LootLocker.LootLockerAPIManager.EndSession(sessionRequest, (response) =>
                            {
                                if (!response.success)
                                {
                                    Debug.Log(response.Error);
                                    return;
                                }

                                Debug.Log("Account created successfully");
                                LoadScene();
                            });
                        });
                        
                    });
                });
            }
        });
    }
}
