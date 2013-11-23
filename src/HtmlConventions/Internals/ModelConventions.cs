﻿using System.Collections.Generic;

namespace MvcHtmlConventions.Internals
{
    class ModelConventions : IHaveModelConventions
    {
        public ModelConventions(IUseConventions registry)
        {
            Registry = registry;
            Modifiers=new IModifyElement[0];
        }
        /// <summary>
        /// Can be null
        /// </summary>
        public IBuildElement Builder { get; set; }
        public IEnumerable<IModifyElement> Modifiers { get; internal set; }
        
        public IGenerateHtml CreateGenerator()
        {
            return new GeneratorWrapper(Builder,Modifiers);
        }

        public IUseConventions Registry { get; private set; }
        
    }
}