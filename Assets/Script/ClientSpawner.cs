using PhongNH.LibTool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSpawner : MonoBehaviour
{
    public Client prebClient;
    public float timeSpawn;
    public List<Client> allClient;
    private void OnEnable()
    {
        GameManager.EV_REMOVE_CLIENT += UpdateList;
    }
    private void OnDisable()
    {
        GameManager.EV_REMOVE_CLIENT -= UpdateList;
    }
    private void Start()
    {
        InitSpawner();
    }
    public void UpdateList(Client objCl)
    {
        if (allClient.Contains(objCl))
        {
            allClient.Remove(objCl);
        }
    }
    public void InitSpawner()
    {
        StartCoroutine(WaitAndSpawn());
    }
    IEnumerator WaitAndSpawn()
    {
        while(true)
        {
            yield return new WaitForSeconds(timeSpawn);
            if(allClient.Count < GameConfigManager.MaxClient * (GameManager.Instance.LevelClient + 1))
                SpawnClient();
        }
    }

    public void SpawnClient()
    {
        Client tmp = HelperTool.Spawn<Client>(prebClient.transform, transform.position, Quaternion.identity, null);
        tmp.InitClient();
        allClient.Add(tmp);

    }
}
