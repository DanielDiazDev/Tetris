using System.Collections.Generic;
using _Project.Scripts.Auth;
using _Project.Scripts.Core;
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
           ServiceLocator.Instance.GetService<ILeaderboardService>().GetLeaderboardEntries(0, 6, "Leaderboard");
        }

         public void SetLeaderboard(List<PlayerLeaderboardEntryData> leaderboardEntriesData)
         {
             foreach (Transform child in leaderboardEntryParent)
             {
                 Destroy(child.gameObject);
             }
             foreach (var entryData in leaderboardEntriesData)
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