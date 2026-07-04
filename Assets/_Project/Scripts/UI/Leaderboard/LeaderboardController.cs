using System.Linq;
using _Project.Scripts.MVC;
using UniRx;

public class LeaderboardController : BaseController<LeaderboardModel, LeaderboardView>
{
    public LeaderboardController(LeaderboardView view, LeaderboardModel model) : base(view, model)
    {
        // Subscribe to events here
        model.LeaderboardEntryDatas.ObserveAdd().Subscribe(_ =>
        {
             view.SetLeaderboard(model.LeaderboardEntryDatas.ToList());
        }).AddTo(view);
    }


    ~LeaderboardController()
    {
        // Unsubscribe to events here
    }
}                             