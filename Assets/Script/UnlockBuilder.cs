using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnlockBuilder : MonoBehaviour
{
    public int id;

    public int quantityCashRequire;
    public int quantityCurrent;
    public TextMeshPro txtRequire;

    public GameObject Builder;
    public GameObject cashHolder;
    public void InitBuilder()
    {
        txtRequire.text = string.Format("x{0}/{1}", quantityCurrent, quantityCashRequire);

        int isUnlock = PlayerPrefs.GetInt("UNLOCK_BUILDER_" + id, 0);
        if(isUnlock == 0)
        {
            Builder.SetActive(false);
        }
        else
        {
            UnlockThisBuilder();
        }
    }
    public void AddCash(int quantity)
    {
        quantityCurrent += quantity;
        if(quantityCurrent >= quantityCashRequire)
        {
            UnlockThisBuilder();
        }
        txtRequire.text = string.Format("x{0}/{1}", quantityCurrent, quantityCashRequire);

    }
    public void UnlockThisBuilder()
    {
        PlayerPrefs.SetInt("UNLOCK_BUILDER_" + id, 1);
        TreeController tmp = Builder.GetComponent<TreeController>();
        Builder.SetActive(true);
        if (tmp != null)
        {
            tmp.InitTree();
        }
        this.gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {

        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            int quantity = quantityCashRequire - quantityCurrent;
            if (GameManager.Instance.QuantityCashUser < quantity)
            {
                quantity = GameManager.Instance.QuantityCashUser;
            }
           
            GameManager.Instance.effectManager.SpawnCash(player.transform.position, cashHolder.transform, quantity, () =>
            {
                AddCash(quantity);
            });
            GameManager.Instance.AddCash(-quantity);

        }
    }
}
