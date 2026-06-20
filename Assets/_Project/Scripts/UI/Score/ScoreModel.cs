

using System;
using _Project.Scripts.MVC;
using _Project.Scripts.Score;
using _Project.Scripts.SOAP;
using _Project.Scripts.SOAP.Events;
using UniRx;
using UnityEngine;

public class ScoreModel : BaseModel
{
    [HideInInspector] public ReactiveProperty<int> score = new();
    [SerializeField] private IntEvent addScoreEvent;

    private void Awake()
    {
        ScoreSystem.Instance.SetScoreModel(this);
    }
}