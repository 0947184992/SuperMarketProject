using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CashControllerUI : MonoBehaviour
{
    public Text quantityCash;
    private void OnEnable()
    {
        GameManager.EV_ADD_CASH += UpdateUI;
    }
    void UpdateUI()
    {
        quantityCash.text = GameManager.Instance.QuantityCashUser.ToString();
    }
    private void OnDisable()
    {
        GameManager.EV_ADD_CASH -= UpdateUI;
    }
}
