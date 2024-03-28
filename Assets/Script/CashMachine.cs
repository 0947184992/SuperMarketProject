using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CashMachine : SerializedMonoBehaviour
{
    public List<GameObject> lstCash;
    int CurrentCash = 0;
    public GameObject cashHolder;
    public GameObject cashHolderClient;
    public List<Client> clientLineup;
    public Transform pos1;
    public Transform pos2;

    public int[] arr1;
    public int[] arr2;
    public int[] arr3;
    public Animator PackageWait;
    public Animator PackageTmp;
    private void Start()
    {
        clientLineup = new();
        PackageTmp.gameObject.SetActive(false);
    }

    public void InitCash()
    {
        for (int i = 0; i < CurrentCash; i++)
        {
            lstCash[i].SetActive(true);
        }
        for (int i = CurrentCash; i < lstCash.Count; i++)
        {
            lstCash[i].SetActive(false);
        }
    }
    public void AddCash(int quantity)
    {
        CurrentCash += quantity;

        if (CurrentCash < 0) CurrentCash = 0;
    }
    public void GetCash()
    {
        int cacheCash = CurrentCash;
        AddCash(-cacheCash);
        InitCash();
        GameManager.Instance.AddCash(cacheCash);
        GameManager.Instance.effectManager.SpawnCash(cashHolder.transform.position, GameManager.Instance.player.TomatoHolder.transform, cacheCash, () =>
        {
            
        });
    }
    public void ClientLineUpNextPos(Client client)
    {
        clientLineup.Add(client);
        LineUpClientRefesh();
    }
    
    public void LineUpClientRefesh()
    {
        int countClient = 0;
        for (int i = 0; i < clientLineup.Count; i++)
        {
            Vector3 pos = pos1.position + (pos2.position - pos1.position) * (i);
            Vector3 direction = (pos - clientLineup[i].transform.position);
            if(direction != Vector3.zero)
            {
                clientLineup[i].transform.forward = direction;
            }
            //clientLineup[i].DOKill();
            if (Vector3.Distance(clientLineup[i].transform.position, pos) > 0.1f) 
                clientLineup[i].OnMoving();
            clientLineup[i].GoToPay();
            clientLineup[i].transform.DOMove(pos, clientLineup[i].speedMove).SetSpeedBased(true).SetEase(clientLineup[i].animcurve)
                .OnComplete(() =>
                {
                    countClient++;
                    //clientLineup[i].OnIDLE();
                    //clientLineup[i].transform.forward = (pos1.position - pos2.position).normalized;
                    if(countClient == clientLineup.Count)
                    {
                        LineUpPay();
                    }
                });
        }
        if(clientLineup.Count > 0)
        {
            PackageWait.gameObject.SetActive(true);
        }
    }
    void LineUpPay()
    {
        for (int i = 0; i < clientLineup.Count; i++)
        {
            clientLineup[i].OnIDLE();
            clientLineup[i].transform.forward = (pos1.position - pos2.position).normalized;
        }

    }
    bool paying = false;
    private void OnTriggerStay(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            paying = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            paying = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            if (CurrentCash > 0)
            {
                GetCash();
            }
            // Packing
            Packing();
        }
    }
    public void Packing()
    {
        StopCoroutine(IEPacking());
        StartCoroutine(IEPacking());
    }
    IEnumerator IEPacking()
    {
        if (clientLineup.Count == 0) 
            yield return null;
        else
        {

            PackageWait.SetTrigger("PACKING");
            yield return new WaitForSeconds(1.5f);
            if(clientLineup.Count != 0)
            {
                var tmp = clientLineup[0];
                clientLineup.Remove(clientLineup[0]);
                PackageWait.gameObject.SetActive(false);
                PackageTmp.gameObject.SetActive(true);
                tmp.PayAndGoOut(cashHolderClient.transform, () =>
                {
                    LineUpClientRefesh();
                    PackageTmp.gameObject.SetActive(false);
                    AddCash(2);
                    InitCash();
                    PackageWait.gameObject.SetActive(true);
                    if (paying)
                    {
                        Packing();
                    }
                });
            }
        }
    }
}
