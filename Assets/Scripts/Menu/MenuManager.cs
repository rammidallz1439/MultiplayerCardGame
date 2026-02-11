using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager 
{
    protected MenuHandler Handler;

    #region handlers

    #endregion

    #region methods
    protected void Init()
    {
        Handler.HostButton.onClick.AddListener(OnHostButton);
        Handler.JoinButton.onClick.AddListener(OnJoinButton);
        Handler.TestButton.onClick.AddListener(OnTestButton);
    }

    protected void DeInit()
    {
        Handler.HostButton.onClick.RemoveListener(OnHostButton);
        Handler.JoinButton.onClick.RemoveListener(OnJoinButton);
        Handler.TestButton.onClick.RemoveListener(OnTestButton);
    }

    private void OnHostButton()
    {
        Handler.NetworkManager.StartHost();
        string ip = NetworkUtils.GetLocalIPAddress();
        Handler.IpText.text = "Hosting on:\n" + ip;
        Handler.IPInput.gameObject.SetActive(false);
    }

    private void OnJoinButton()
    {
        string ip = Handler.IPInput.text;
        if (string.IsNullOrEmpty(ip))
        {
            Debug.LogWarning("IP address is empty");
            return;
        }
        Handler.NetworkManager.networkAddress = ip;
        Handler.NetworkManager.StartClient();

        Handler.JoinButton.interactable = false;
    }   


    private void OnTestButton()
    {
        MenuHandler.IsTesting = true;
        Handler.NetworkManager.StartHost();

    }

    #endregion
}
