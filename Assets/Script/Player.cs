using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Animator anim;

    const string IDLE = "Idle";
    const string Run = "Run";
    const string PREF_LEVEL_TOMATO = "PREF_LEVEL_TOMATO";

    bool isMoving;
    public int CurrentTomato;
    public int LevelTomato;
    public List<GameObject> LstTomato;
    public GameObject TomatoHolder;
    public void InitPlayer()
    {
        LevelTomato = PlayerPrefs.GetInt(PREF_LEVEL_TOMATO);
        InitTomato();
    }
    public void OnMoing()
    {
        if (!isMoving)
        {
            isMoving = true;
            anim.SetTrigger(Run);
            //Debug.LogError("Moving");
        }
       
    }
    public void OnIdle()
    {
        if (isMoving)
        {
            isMoving = false;
            anim.SetTrigger(IDLE);
            //Debug.LogError("Idle");
        }
    }
    public void AddTomato(int quantity)
    {
        CurrentTomato += quantity;
        if(CurrentTomato > LstTomato.Count)
        {
            CurrentTomato = LstTomato.Count;
        }
        if(CurrentTomato < 0)
        {
            CurrentTomato = 0;
        }
        //InitTomato();
    }
    public void InitTomato()
    {
        for(int i = 0; i < CurrentTomato; i++)
        {
            LstTomato[i].SetActive(true);
        }
        for(int i = CurrentTomato; i < LstTomato.Count; i++)
        {
            LstTomato[i].SetActive(false);
        }

    }
    public void HarvestTomato(Action CanHavest)
    {

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddTomato(-100);
            InitTomato();
        }
    }
    public bool CanHavest()
    {
        return CurrentTomato < GameConfigManager.MaxQuantityTomato * (LevelTomato + 1);
    }
}
