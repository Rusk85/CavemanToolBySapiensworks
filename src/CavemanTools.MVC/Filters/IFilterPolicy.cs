﻿using System.Reflection;

namespace CavemanTools.Mvc.Filters
{
    /// <summary>
    /// Policy to apply filters based on a convention
    /// </summary>
    public interface IFilterPolicy
    {
        int? Order { get; }
        /// <summary>
        /// Filter instance
        /// </summary>
        object Instance { get; }
        
        bool Match(MethodInfo action);
    }
}