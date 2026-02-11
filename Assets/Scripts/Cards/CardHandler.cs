using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vault;

public class CardHandler : MonoBehaviour
{
    public int ID;
    public int Cost;
    public int Power;
    public string Name;

    public TMP_Text CardTitle;
    public TMP_Text CardCostText;
    public TMP_Text CardPowerText;

    public Button CardButton;

    public GameObject BackFace;

    /// <summary>
    /// set the card details using the json data and also update the card visuals
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cost"></param>
    /// <param name="power"></param>
    /// <param name="name"></param>
    public void SetCardDetails(int id, int cost, int power, string name)
    {
        ID = id;
        Cost = cost;
        Power = power;
        Name = name;

        CardTitle.text = Name;
        CardCostText.text = Cost.ToString();
        CardPowerText.text = Power.ToString();
    }

    public void OnCardClicked()
    {
        Debug.Log($"Card Clicked: {Name} (ID: {ID}, Cost: {Cost}, Power: {Power})");
        EventManager.Instance.TriggerEvent(new OnCardSelectedEvent(this));
    }


}
