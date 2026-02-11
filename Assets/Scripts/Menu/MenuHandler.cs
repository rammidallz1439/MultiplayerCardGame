using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{
    [Header("UI")]
    public Button HostButton;
    public Button JoinButton;
    public Button TestButton;
    public TMP_Text IpText;
    public TMP_InputField IPInput;

    [Space(10)]
    [Header("Networking")]
    public CardNetworkManager NetworkManager;

    [Space(10)]
    [Header("Test")]
    public static bool IsTesting;
}
