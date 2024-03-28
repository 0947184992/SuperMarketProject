using DG.Tweening;
using PhongNH.LibTool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EffectManager : MonoBehaviour
{
    public GameObject pref_Tomato;
    public GameObject pref_Cash;

    [SerializeField] float _timeOutCoint = 0.55f;
    [SerializeField] float _speedMoveCoin = 10f;
    [SerializeField] AnimationCurve _animationCurveHoldCoinOut;
    [SerializeField] AnimationCurve _animationCurveHoldCoin;

    public void SpawnTomato(Vector3 posStart, Transform posEnd, int quantity, Action onFinish)
    {
        ShowEffectEarnCurrency(pref_Tomato, posStart, posEnd, quantity, null, onFinish);
    }
    
    public void SpawnCash(Vector3 posStart, Transform posEnd, int quantity, Action onFinish)
    {
        ShowEffectEarnCurrency(pref_Cash, posStart, posEnd, quantity, null, onFinish);
    }

    public void ShowEffectEarnCurrency(GameObject prefab, Vector3 from, Transform transDes, int quantityShow = 15, Action onStart = null, Action onFinish = null)
    {
        //from.z = transDes.position.z;
       
        int maxInstance = quantityShow;
        float offsetX = 0.6f;
        float offsetY = 0.6f;
        Vector3 desPos = transDes.position;
        Vector3 beforeScale = Vector3.one;

        for (int i = 0; i < maxInstance; i++)
        {
            int index = i;
            Vector3 randomPos = new Vector3(from.x + Random.Range(-offsetX, offsetX), from.y + Random.Range(-offsetY, offsetY), desPos.z);
            GameObject go = HelperTool.Spawn<Transform>(prefab.transform, from, Quaternion.identity, null).gameObject;
            go.transform.localScale = Vector3.one;
            go.transform.position = from;
            go.transform.DOScale(1.4f, 0.3f);
            go.transform.DOMove(randomPos, _timeOutCoint).SetEase(_animationCurveHoldCoinOut).OnComplete(() =>
            {
                desPos = transDes.position;
                if (onStart != null) onStart();
                float timeDelay = Random.Range(0, 7) * 0.05f;
                go.transform.DOScale(1f, 0.2f).SetDelay(timeDelay + 0.2f);
                go.transform.DOMove(desPos, _speedMoveCoin).SetEase(_animationCurveHoldCoin).SetSpeedBased().SetDelay(timeDelay).OnComplete(() =>
                {
                    if (onFinish != null) onFinish();

                    /*Tween tween = transDes?.DOScale(beforeScale * 1.2f, 0.1f).OnComplete(() => { tween = transDes?.DOScale(beforeScale, 0.05f); });
                    if (index + 1 == maxInstance)
                    {
                        if (onFinish != null) onFinish();
                        if (tween != null) tween.Kill();
                        transDes?.DOScale(beforeScale, 0.05f);
                    }*/
                    HelperTool.Despawn(go);
                });
            });
        }
    }
}
