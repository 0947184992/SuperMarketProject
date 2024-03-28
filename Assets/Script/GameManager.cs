using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Player player;
    public EffectManager effectManager;
    public TomatoTray tomatoTray;
    public TomatoTray tomatoTray2;
    public CashMachine cashMachine;
    public ClientSpawner spawnner;

    public static Action<Client> EV_REMOVE_CLIENT;
    public static Action EV_ADD_CASH;
    public int QuantityCashUser = 0;

    public List<UnlockBuilder> lsstBuilderUnlock;
    int indexTmp = 0;
    const string PREF_CASH_PLAYER = "PREF_CASH_PLAYER";
    public int LevelClient = 0;
    public TomatoTray GetTomatoTray()
    {
        if (tomatoTray2.gameObject.activeInHierarchy)
        {
            LevelClient = 1;
            indexTmp++;
            if(indexTmp % 2 == 0)
            {
                return tomatoTray2;
            }
        }

        return tomatoTray;
    }
    private void Awake()
    {
        Application.targetFrameRate = 60;
        Instance = this;
        QuantityCashUser = PlayerPrefs.GetInt(PREF_CASH_PLAYER, 0);
        player.InitPlayer();
        tomatoTray.InitTomato();
        tomatoTray2.InitTomato();
        cashMachine.InitCash();
        for(int i  = 0; i < lsstBuilderUnlock.Count; i++)
        {
            lsstBuilderUnlock[i].InitBuilder();
        }
        EV_ADD_CASH?.Invoke();
    }
    public void InitPlayer()
    {
        player.InitPlayer();
    }

    public void AddCash(int quantity)
    {
        QuantityCashUser += quantity;
        EV_ADD_CASH?.Invoke();
    }
    private void OnApplicationPause(bool pause)
    {
        PlayerPrefs.SetInt(PREF_CASH_PLAYER, QuantityCashUser);
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt(PREF_CASH_PLAYER, QuantityCashUser);
    }
}


public class GameConfigManager
{
    public static int MaxQuantityTomato = 5;
    public static int MaxQuantityTomatoInTray = 15;
    public static int MaxClient = 8;
}