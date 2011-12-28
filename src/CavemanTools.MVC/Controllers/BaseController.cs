using System;
using System.Collections.Generic;
using System.Web.Mvc;


namespace CavemanTools.Mvc.Controllers
{
    /// <summary>
    /// Base controller with methods to help you keep the actions slim. 
    /// </summary>
    public abstract class BaseController : Controller
    {
        public BaseController()
        {
            ActionSuccessResult=()=>{ throw new InvalidOperationException("Action success result is not set!"); };
        }
        /// <summary>
        /// Gets or sets the default result handler when a model update is successful
        /// </summary>
        protected Func<ActionResult> ActionSuccessResult { get; set; }

        /// <summary>
        /// Returns a handler with the action result to use when a model update has errors.
        /// </summary>
        protected virtual Func<ActionResult> ActionFailResult<T>(T model) where T : class ,new()
        {
            return ViewResultError(model);
        }


        #region ActionResult handlers
        /// <summary>
        /// Returns a ViewResult handler
        /// Default ActionFailResult
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model">view model</param>
        /// <returns></returns>
        protected Func<ActionResult> ViewResultError<T>(T model) where T : class,new()
        {
            return () => View(GetModel(model));
        }

        /// <summary>
        /// Returns a JsonResult handler
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model">view model</param>
        /// <returns></returns>
        protected Func<ActionResult> JsonResultError<T>(T model) where T : class,new()
        {
            return () => Json(GetModel(model));
        } 
        #endregion
        

        /// <summary>
        /// Update model and returns a result depending on model state
        /// </summary>
        /// <typeparam name="T">model type</typeparam>
        /// <param name="model">view model</param>
        /// <param name="action">action which processes the model</param>
        /// <param name="failure">result if model state is invalid</param>
        /// <param name="success">result if action is succesful</param>
        /// <returns></returns>
        protected ActionResult UpdateModelResult<T>(T model, Action<T> action, Func<ActionResult> failure = null,Func<ActionResult> success = null) where T : class,new()
        {
            if (action == null) throw new ArgumentNullException("action");
            if (success == null) success = ActionSuccessResult;
            if (failure == null) failure = ActionFailResult(model);
            
            if (!ModelState.IsValid)
            {
                return failure();
            }

            action(model);

            //check to see if handling yielded errors
            if (!ModelState.IsValid)
            {
                return failure();
            }

            return success();
        }

        List<IPopulateModel> _pop = new List<IPopulateModel>();
       
        /// <summary>
        /// Sets the action which populates a view model
        /// </summary>
        /// <typeparam name="T">TModel</typeparam>
        /// <param name="action">action</param>
        protected void Populate<T>(Action<T> action) where T : class
        {
            var vm = new PopulateModel<T>(action);
            _pop.Add(vm);
        }

        /// <summary>
        /// Get view model with populated data
        /// </summary>
        /// <typeparam name="T">View model</typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        protected T GetModel<T>(T model = null) where T : class ,new()
        {
            if (model == null) model= new T();

            var filler = _pop.Find(d => d.ModelType.Equals(typeof (T))) as PopulateModel<T>;
             if (filler!=null) filler.Map(model);
                        
            return model;
        }

      
    }   
}