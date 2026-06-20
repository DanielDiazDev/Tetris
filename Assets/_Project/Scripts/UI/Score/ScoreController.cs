using _Project.Scripts.MVC;
using UniRx;

public class ScoreController : BaseController<ScoreModel, ScoreView>
{
    public ScoreController(ScoreView view, ScoreModel model) : base(view, model)
    {
        model.score.Subscribe(view.SetScore).AddTo(view);
    }


    ~ScoreController()
    {
        
    }
}