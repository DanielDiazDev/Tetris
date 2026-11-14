using System.Collections.Generic;

namespace _Project.Scripts.PlayFab.Interfaces
{
    public interface ILeaderboardModel
    {
        void Add(List<PlayerLeaderboardEntryData> entries);
    }
}
