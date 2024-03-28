using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : MonoBehaviour
{

    public GameObject tomato1;
    public GameObject tomato2;
    public GameObject tomato3;
    public float TimeWaitTomato;
    int quantityCurrent;
    private void OnEnable()
    {
        //InitTree();
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    public void InitTree()
    {
        StartCoroutine(SpawnTomato());
    }


    IEnumerator SpawnTomato()
    {
        while (true)
        {
            yield return new WaitForSeconds(TimeWaitTomato);
            if (!harvesting)
            {
                if (!tomato1.activeInHierarchy)
                {
                    tomato1.SetActive(true);
                }
                else if (!tomato2.activeInHierarchy)
                {
                    tomato2.SetActive(true);
                }
                else if (!tomato3.activeInHierarchy)
                {
                    tomato3.SetActive(true);
                }
            }
        }
    }
    bool harvesting = false;
    public void Harvest()
    {
        harvesting = true;
        if (tomato1.activeInHierarchy)
        {
            tomato1.SetActive(false);
        }
        else if (tomato2.activeInHierarchy)
        {
            tomato2.SetActive(false);
        }
        else if (tomato3.activeInHierarchy)
        {
            tomato3.SetActive(false);
        }
        GameManager.Instance.player.AddTomato(1); 
        GameManager.Instance.effectManager.SpawnTomato(tomato1.transform.position, GameManager.Instance.player.TomatoHolder.transform, 1, () =>
        {
            GameManager.Instance.player.InitTomato();
        });

        if (GameManager.Instance.player.CanHavest() && CanHavest())
        {
            Harvest();
        }
        //Debug.LogError("Harvest");
        // call thu hoach
    }
    
    private void OnTriggerExit(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null && harvesting)
        {
            harvesting = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if(player != null)
        {
            if (GameManager.Instance.player.CanHavest() && CanHavest())
            {
                Harvest();
            }
        }
    }
    bool CanHavest()
    {
        quantityCurrent = 0;
        if (tomato1.activeInHierarchy)
        {
            quantityCurrent++;
        }
        if (tomato2.activeInHierarchy)
        {
            quantityCurrent++;
        }
        if (tomato3.activeInHierarchy)
        {
            quantityCurrent++;
        }
        return quantityCurrent > 0;
    }
}
