using System;
using System.Collections.Generic;
using _Project.Scripts.Auth;
using _Project.Scripts.MVC;
using UniRx;
using UnityEngine;

public class LeaderboardModel : BaseModel
{
    public ReactiveCollection<LeaderboardEntryData> LeaderboardEntryDatas { get; } = new ReactiveCollection<LeaderboardEntryData>();

    private void Awake()
    {
        PlayFabLeaderboard.Instance.SetModel(this);
    }

    public void Add(LeaderboardEntryData entry)
    {
        LeaderboardEntryDatas.Add(entry);
    }
    
}