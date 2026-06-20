using _Project.Scripts.MVC;
using TMPro;
using UnityEngine;

public class ScoreView : GeneralView<ScoreModel, ScoreController>
{
    [SerializeField] private TextMeshProUGUI scoreText;
         protected override void Initialize()
         {
            if (controller == null)
            {
               controller = new ScoreController(this, GetComponent<ScoreModel>());
            }
            base.Initialize();
        }

         public void SetScore(int score)
         {
             scoreText.text = score.ToString();
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