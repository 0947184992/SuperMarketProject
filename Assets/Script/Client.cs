using DG.Tweening;
using PhongNH.LibTool;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Client : MonoBehaviour
{
    public Animator anim;

    const string IDLE = "Idle";
    const string Run = "Run";

    public float speedMove = 2;
    public Transform CanvasRequire;
    public TextMeshPro txtRequire;
    int quantityRequire;
    int currentTomato;
    public GameObject TomatoHolder;
    public AnimationCurve animcurve;
    Tweener tweener;
    bool checkPay = false;
    public GameObject tomatoRequire;
    public GameObject cashRequire;
    public void InitClient()
    {
        tomatoRequire.SetActive(true);
        cashRequire.SetActive(false);

        checkPay = false;
        currentTomato = 0;
        quantityRequire = Random.Range(1, 5);
        txtRequire.text = string.Format("x{0}/{1}", currentTomato, quantityRequire);
        MoveToPos(GameManager.Instance.GetTomatoTray().GetPos().position);
    }
    public void GoToPay()
    {
        tomatoRequire.SetActive(false);
        cashRequire.SetActive(true);
    }
    public void MoveToPos(Vector3 pos)
    {
        pos.y = 0;
        OnMoving();
        Vector3 direction = (pos - transform.position).normalized;
       // direction.x = 0;
        transform.forward = direction;
        tweener = transform.DOMove(pos, speedMove)
            .SetEase(animcurve)
            .SetSpeedBased(true).OnComplete(() =>
        {
            OnIDLE();
            transform.DOKill();

        });
    }
    public void OnMoving()
    {
        anim.SetTrigger(Run);
    }
    public void OnIDLE()
    {
        anim.SetTrigger(IDLE);
    }
    private void Update()
    {
        CanvasRequire.rotation = Quaternion.identity;
    }
    public bool NeedTomato()
    {
        return currentTomato < quantityRequire;
    }
    public void AddTomato(int quantity)
    {
        currentTomato += quantity;
        if(currentTomato > quantityRequire)
        {
            currentTomato = quantityRequire;
        }
        txtRequire.text = string.Format("x{0}/{1}", currentTomato, quantityRequire);
    }
    public void CheckRequire()
    {
        txtRequire.text = string.Format("x{0}/{1}", currentTomato, quantityRequire);
        if(currentTomato >= quantityRequire && !checkPay)
        {
            checkPay = true;
            transform.DOKill();
            GameManager.Instance.cashMachine.ClientLineUpNextPos(this);
        }
    }
    public void PayAndGoOut(Transform pos, Action Done)
    {
        if (tweener != null)
        {
            tweener.Kill();
        }
        OnMoving();
        tweener = transform.DOMove(pos.position, speedMove)
           .SetEase(animcurve)
           .SetSpeedBased(true).OnComplete(() =>
           {
               transform.DOKill();

               OnIDLE();
               StartCoroutine(WaitAndGoOut(Done)); 
               
           });
    }
    IEnumerator WaitAndGoOut(Action Done)
    {
        yield return new WaitForSeconds(1f);
        Done?.Invoke();
        OnMoving();
        tweener = transform.DOMove(GameManager.Instance.spawnner.transform.position, speedMove)
           .SetEase(animcurve)
           .SetSpeedBased(true).OnComplete(() =>
           {
               transform.DOKill();
               GameManager.EV_REMOVE_CLIENT?.Invoke(this);
               HelperTool.Despawn(this.gameObject);
           });
    }
}
