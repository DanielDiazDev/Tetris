using System;
using System.Collections.Generic;
using _Project.Scripts.Auth;
using _Project.Scripts.Core;
using _Project.Scripts.MVC;
using UniRx;
using UnityEngine;

public class LeaderboardModel : BaseModel
{
    public ReactiveCollection<PlayerLeaderboardEntryData> LeaderboardEntryDatas { get; } = new ReactiveCollection<PlayerLeaderboardEntryData>();

    private void Awake()
    {
        ServiceLocator.Instance.GetService<ILeaderboardService>().SetModel(this);
    }

    public void Add(List<PlayerLeaderboardEntryData> leaderboardEntriesData)
    {
        foreach (var entry in leaderboardEntriesData)
        {
            LeaderboardEntryDatas.Add(entry);
        }
    }
    
}