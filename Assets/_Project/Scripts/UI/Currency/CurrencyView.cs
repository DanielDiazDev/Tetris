using System;
using _Project.Scripts.MVC;
using TMPro;
using UnityEngine;

public class CurrencyView : GeneralView<CurrencyModel, CurrencyController>
{
    [SerializeField] private TextMeshProUGUI currencyText;
         protected override void Initialize()
         {
            if (controller == null)
            {
               controller = new CurrencyController(this, GetComponent<CurrencyModel>());
            }
            base.Initialize();
           
         }

        

         public void SetCurrency(int currency)
         {
             currencyText.text = currency.ToString();
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