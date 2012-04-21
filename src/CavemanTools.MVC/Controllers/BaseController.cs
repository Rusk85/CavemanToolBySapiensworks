using System;
using System.Collections.Generic;
using System.Web.Mvc;


namespace CavemanTools.Mvc.Controllers
{
    ////todo review base controller for update model
    ///// <summary>
    ///// Base controller with methods to help you keep the actions slim. 
    ///// </summary>
    //public abstract class BaseController : Controller
    //{
    //    internal protected BaseController()
    //    {
    //       ActionSuccessResult=()=>{ throw new InvalidOperationException("Action success result is not set!"); };            
    //    }
        
    //      /// <summary>
    //    /// Gets or sets the default result handler when a model update is successful
    //    /// </summary>
    //    protected Func<ActionResult> ActionSuccessResult { get; set; }

    //    /// <summary>
    //    /// Update model and returns a result depending on model state
    //    /// </summary>
    //    /// <typeparam name="T">model type</typeparam>
    //    /// <param name="model">view model</param>
    //    /// <param name="action">action which processes the model</param>
    //    /// <param name="failure">result if model state is invalid</param>
    //    /// <param name="success">result if action is succesful</param>
    //    /// <returns></returns>
    //    internal protected ActionResult UpdateModel<T>(T model, Action<T> action, Func<ActionResult> success = null, Func<ActionResult> failure = null)
    //    {
    //        if (action == null) throw new ArgumentNullException("action");
    //        if (success == null) success = ActionSuccessResult;
    //        if (failure == null) failure = ActionFailResult(model);
            
    //        if (!ModelState.IsValid)
    //        {
    //            return failure();
    //        }

    //        action(model);

    //        //check to see if handling yielded errors
    //        if (!ModelState.IsValid)
    //        {
    //            return failure();
    //        }

    //        return success();
    //    }


    //    #region ActionResult handlers

    //    #endregion
    //}   
}