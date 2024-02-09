using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CloudFader : MonoBehaviour
{
    public MeshRenderer M;
    void Start()
    {
        M.material.DOFade(0, 20f).SetEase(Ease.Linear);
    }

    // Update is called once per frame
    private void OnDestroy()
    {
        DOTween.Kill(transform);
    }
}
