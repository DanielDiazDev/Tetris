using System;
using System.Collections.Generic;
using _Project.Scripts.MVC;
using _Project.Scripts.PlayFab;
using _Project.Scripts.PlayFab.Interfaces;
using UniRx;
using UnityEngine;
using UnityServiceLocator;

public class LeaderboardModel : BaseModel, ILeaderboardModel
{
    public ReactiveCollection<PlayerLeaderboardEntryData> LeaderboardEntryDatas { get; } = new ReactiveCollection<PlayerLeaderboardEntryData>();

    private void Awake()
    {
        ServiceLocator.Global.Get<ILeaderboardService>().SetModel(this);
    }

    public void Add(List<PlayerLeaderboardEntryData> leaderboardEntriesData)
    {
        foreach (var entry in leaderboardEntriesData)
        {
            LeaderboardEntryDatas.Add(entry);
        }
    }
    
}