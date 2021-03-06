﻿using System;
using System.Web.Mvc;

namespace CavemanTools.Mvc.Controllers
{
    public class ValidationFailedPolicyActivator : IValidationFailedPolicyFactory
    {
        private readonly IDependencyResolver _solver;

        public ValidationFailedPolicyActivator(IDependencyResolver solver)
        {
            //if (solver == null) solver = DependencyResolver.Current;
            _solver = solver;
        }

        public IResultForInvalidModel<T> GetInstance<T>(Type policy, T model) where T : class, new()
        {
            policy.MakeGenericType(typeof(T)).MustImplement<IResultForInvalidModel<T>>();
            var inst = _solver.GetService(policy.MakeGenericType(typeof (T)));

            if (inst==null) throw new InvalidOperationException("Can't instantiate type '{0}'. The type isn't available from the DI Container or it doesn't exist.".ToFormat(typeof(T)));

            var cast=  inst.Cast<IResultForInvalidModel<T>>();
            cast.Model = model;
            cast.ModelSetup = _solver.GetService<ISetupModel<T>>();                           
            return cast;
        }
    }
}