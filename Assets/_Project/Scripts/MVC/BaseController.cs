
     namespace _Project.Scripts.MVC
     {
         public abstract class BaseController<TModel, TView> 
             where TModel : BaseModel
             where TView : BaseView
         {
             public TModel model;
             protected TView view;

             public BaseController(TView view, TModel model)
             {
                 this.view = view;
                 this.model = model;
             }
         }
     }
