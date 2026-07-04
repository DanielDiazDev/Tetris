

using System;
using _Project.Scripts.MVC;
using _Project.Scripts.Score;
using _Project.Scripts.SOAP;
using _Project.Scripts.SOAP.Events;
using UniRx;
using UnityEngine;

public class ScoreModel : BaseModel
{
    public ReactiveProperty<int> Score { get; } = new ReactiveProperty<int>();
    [SerializeField] private IntEvent addScoreEvent;

    public void AddScore(int amount)
    {
        Score.Value += amount;
    }
    public int GetScore() => Score.Value;
}