using System;

namespace _Project.Scripts.PlayFab.Interfaces
{
    public interface ILeaderboardService
    {
        void GetLeaderboardEntries(int startPosition, int maxResultsCount, string leaderboardName);
        void UpdatePlayerStatics(string leaderboardName, int score);
        void GetPlayerLeaderboardPoints(string leaderboardName, Action<int> onResult);
        void SetModel(ILeaderboardModel leaderboardModel);
        void Dispose();
    }
}