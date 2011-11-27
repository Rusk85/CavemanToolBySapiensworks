//http://lostechies.com/jimmybogard/2011/06/22/cleaning-up-posts-in-asp-net-mvc/

using System;
using System.Web.Mvc;

namespace CavemanTools.Mvc
{
    public class FormActionResult<T> : ActionResult
    {
        public ViewResult Failure { get; private set; }
        public ActionResult Success { get; private set; }
        public T Form { get; private set; }

        public FormActionResult(T form,Action<T> handler, ActionResult success, ViewResult failure)
        {
            Form = form;
            Success = success;
            Failure = failure;
            Handler = handler;
        }

        protected Action<T> Handler { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            //pre handler check
            if (!context.Controller.ViewData.ModelState.IsValid)
            {
                Failure.ExecuteResult(context);

                return;
            }

            Handler(Form);

            //check to see if handling yielded errors
            if (!context.Controller.ViewData.ModelState.IsValid)
            {
                Failure.ExecuteResult(context);

                return;
            }

            Success.ExecuteResult(context);
        }
    }
}