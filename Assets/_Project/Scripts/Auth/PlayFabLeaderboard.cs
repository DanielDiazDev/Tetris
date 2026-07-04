using System;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace _Project.Scripts.Auth
{
    public class PlayFabLeaderboard : MonoBehaviour
    {
        public static PlayFabLeaderboard Instance { get; private set; }
        public Action<List<LeaderboardEntryData>> OnLeaderboardEntryDatas;
        private LeaderboardModel model;

        // public PlayFabLeaderboard(LeaderboardModel model)
        // {
        //     this.model = model;
        // }
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SetModel(LeaderboardModel leaderboardModel)
        {
            model = leaderboardModel;
        }

        public void GetLeaderboardEntries(int startPosition, int maxResultsCount, string leaderboardName)
        {
            var request = new GetLeaderboardRequest()
            {
                StartPosition = startPosition,
                MaxResultsCount = maxResultsCount,
                StatisticName = leaderboardName
            };
            PlayFabClientAPI.GetLeaderboard(request, OnGetLeaderboardSuccess, OnGetLeaderboardError);
        }

        private void OnGetLeaderboardError(PlayFabError error)
        {
            Debug.LogError(error.GenerateErrorReport());

        }

        private void OnGetLeaderboardSuccess(GetLeaderboardResult response)
        {
            foreach (var playerLeaderboardEntry in response.Leaderboard)
            {
                var playerEntry = new LeaderboardEntryData();
                playerEntry.place = playerLeaderboardEntry.Position;
                playerEntry.leaderboardName = playerLeaderboardEntry.DisplayName;
                playerEntry.score =  playerLeaderboardEntry.StatValue;
                model.Add(playerEntry);
            }
        }

        public void UpdatePlayerStatics(string leaderboardName, int score)
        {
            var request = new UpdatePlayerStatisticsRequest()
            {
                Statistics = new List<StatisticUpdate>()
                {
                    new StatisticUpdate()
                    {
                        StatisticName = leaderboardName,
                        Value = score
                    }
                }
            };
            PlayFabClientAPI.UpdatePlayerStatistics(request, OnUpdatePlayerStaticsSuccess, OnUpdatePlayerStaticsError);
        }

        private void OnUpdatePlayerStaticsError(PlayFabError error)
        {
            Debug.LogError(error.GenerateErrorReport());
        }

        private void OnUpdatePlayerStaticsSuccess(UpdatePlayerStatisticsResult obj)
        {
            Debug.Log("Updated");
        }
    }
    public class LeaderboardEntryData
    {
        public int place;
        public string leaderboardName;
        public int score;
    }
}