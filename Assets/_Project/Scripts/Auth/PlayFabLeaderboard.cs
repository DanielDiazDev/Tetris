using System;
using System.Collections.Generic;
using _Project.Scripts.Core;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace _Project.Scripts.Auth
{
    public interface ILeaderboardService
    {
        void GetLeaderboardEntries(int startPosition, int maxResultsCount, string leaderboardName);
        void UpdatePlayerStatics(string leaderboardName, int score);
        void GetPlayerLeaderboardPoints(string leaderboardName, Action<int> onResult);
        void SetModel(LeaderboardModel leaderboardModel);
        void Dispose();
    }

    public class PlayFabLeaderboardService : ILeaderboardService, IDisposable
    {
        private LeaderboardModel model;
        

        public void SetModel(LeaderboardModel leaderboardModel)
        {
            model = leaderboardModel;
        }

        public void GetPlayerLeaderboardPoints(string leaderboardName, Action<int> onResult)
        {
            var request = new GetLeaderboardAroundPlayerRequest()
            {
                StatisticName = leaderboardName,
                MaxResultsCount = 1
            };
            PlayFabClientAPI.GetLeaderboardAroundPlayer(
                request,
                result =>
                {
                    if (result.Leaderboard.Count > 0)
                    {
                        int points = result.Leaderboard[0].StatValue;
                        onResult?.Invoke(points);
                    }
                    else
                    {
                        onResult?.Invoke(0);
                    }
                },
                error =>
                {
                    Debug.LogError($"Failed to get leaderboard: {error.GenerateErrorReport()}");
                    onResult?.Invoke(0);
                });
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
            List<PlayerLeaderboardEntryData> playerLeaderboardEntriesData = new List<PlayerLeaderboardEntryData>();
            foreach (var playerLeaderboardEntry in response.Leaderboard)
            {
                var playerLeaderboardEntryData = new PlayerLeaderboardEntryData();
                playerLeaderboardEntryData.place = playerLeaderboardEntry.Position;
                playerLeaderboardEntryData.leaderboardName = playerLeaderboardEntry.DisplayName;
                playerLeaderboardEntryData.score =  playerLeaderboardEntry.StatValue;
                playerLeaderboardEntriesData.Add(playerLeaderboardEntryData);
            }
            model.Add(playerLeaderboardEntriesData);
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

        public void Dispose()
        {
            ServiceLocator.Instance.UnregisterService<ILeaderboardService>();
        }
    }
    public class PlayerLeaderboardEntryData
    {
        public int place;
        public string leaderboardName;
        public int score;
    }
}