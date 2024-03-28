using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomatoTray : MonoBehaviour
{
    public int CurrentTomato;
    public List<GameObject> LstTomato;
    public GameObject TomatoHolder;
    public List<Transform> lstTransformPos;
    int index = 0;
    public Transform GetPos()
    {
        index ++;
        if (index >= lstTransformPos.Count)
        {
            index = 0;
        }
        return lstTransformPos[index];
    }
    public void AddTomato(int quantity)
    {
        CurrentTomato += quantity;
        if (CurrentTomato > LstTomato.Count)
        {
            CurrentTomato = LstTomato.Count;
        }
        if (CurrentTomato < 0)
        {
            CurrentTomato = 0;
        }
        //InitTomato();
    }
    public void InitTomato()
    {
        for (int i = 0; i < CurrentTomato; i++)
        {
            LstTomato[i].SetActive(true);
        }
        for (int i = CurrentTomato; i < LstTomato.Count; i++)
        {
            LstTomato[i].SetActive(false);
        }
    }
    void AddTomatoToTray()
    {

        if (CanAddTomato())
        {
            AddTomato(1);
            GameManager.Instance.player.AddTomato(-1);
            GameManager.Instance.player.InitTomato();
            GameManager.Instance.effectManager.SpawnTomato(GameManager.Instance.player.TomatoHolder.transform.position, TomatoHolder.transform, 1, () =>
            {
                InitTomato();
            });
            AddTomatoToTray();
        }
    }
    public bool CanAddTomato()
    {
        return CurrentTomato < GameConfigManager.MaxQuantityTomatoInTray && (GameManager.Instance.player.CurrentTomato > 0) ;
    }
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            AddTomatoToTray();
        }
        else
        {
            Client client = other.GetComponent<Client>();
            if (client != null)
            {
                if (client.NeedTomato() && CurrentTomato > 0)
                {
                    client.AddTomato(1);
                    AddTomato(-1);
                    InitTomato();
                    GameManager.Instance.effectManager.SpawnTomato(TomatoHolder.transform.position, client.TomatoHolder.transform, 1, () =>
                    {
                        client.CheckRequire();
                    });

                }
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        Client client = other.GetComponent<Client>();
        if (client != null)
        {
            if (client.NeedTomato() && CurrentTomato > 0)
            {
                client.AddTomato(1);
                AddTomato(-1);
                InitTomato();
                GameManager.Instance.effectManager.SpawnTomato(TomatoHolder.transform.position, client.TomatoHolder.transform, 1, () =>
                {
                    client.CheckRequire(); 

                });
            }
        }
    }
}
