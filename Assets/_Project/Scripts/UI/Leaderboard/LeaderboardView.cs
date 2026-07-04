using System.Collections.Generic;
using _Project.Scripts.Auth;
using _Project.Scripts.MVC;
using _Project.Scripts.UI;
using UnityEngine;

public class LeaderboardView : GeneralView<LeaderboardModel, LeaderboardController>
{
    [SerializeField] private GameObject leaderboardEntryPrefab;
    [SerializeField] private Transform leaderboardEntryParent;
         protected override void Initialize()
         {
            if (controller == null)
            {
               controller = new LeaderboardController(this, GetComponent<LeaderboardModel>());
            }
            base.Initialize();
            PlayFabLeaderboard.Instance.GetLeaderboardEntries(0, 6, "Leaderboard");
        }

         public void SetLeaderboard(List<LeaderboardEntryData> leaderboardEntryDatas)
         {
             foreach (var entryData in leaderboardEntryDatas)
             {
                 var leaderboardEntry = Instantiate(leaderboardEntryPrefab, leaderboardEntryParent).GetComponent<LeaderboardEntry>();
                 leaderboardEntry.Init(entryData);
             }
         }
        
        protected override void ShowView()
        {
            base.ShowView();    
        }
        
        protected override void HideView()
        {
            base.HideView();    
        }
}