﻿using System;
using System.Collections.Generic;
using HtmlTags;

namespace MvcHtmlConventions.Internals
{
    class ConventionsRegistry:IDefinedConventions
    {
       
        List<IBuildElement> _builders=new List<IBuildElement>();
        List<IModifyElement> _modifiers=new List<IModifyElement>();
        private IBuildElement _defaultBuilder=NullBuilder.Instance;

        class ActionConfigurator:IConfigureAction
        {
            private readonly ConventionsRegistry _parent;
            private readonly Predicate<ModelInfo> _predicate;
            
            public ActionConfigurator(ConventionsRegistry parent,Predicate<ModelInfo> predicate)
            {
                _parent = parent;
                _predicate = predicate;
            }

            public IConfigureModifier Build(Func<ModelInfo, HtmlTag> action)
            {
                var lambda = new LambdaConventions(_predicate);
                lambda.Builder = action;
                _parent.Add((IBuildElement) lambda);
                return this;
            }

            public IConfigureConventions Modify(Func<HtmlTag, ModelInfo, HtmlTag> action)
            {
                var lambda = new LambdaConventions(_predicate);
                lambda.Modifier = action;
                _parent.Add((IModifyElement)lambda);
                return _parent;
            }
        }

        public IConfigureAction If(Predicate<ModelInfo> info)
        {
            return new ActionConfigurator(this,info);
        }

        public IConfigureModifier Always
        {
            get
            {
                return If(LambdaConventions.AppliesAlways);
            }
        }
        public IConfigureConventions Add(IBuildElement builder)
        {
            builder.MustNotBeNull();
            _builders.Add(builder);
            return this;
        }

        public IConfigureConventions Add(IModifyElement modifier)
        {
            modifier.MustNotBeNull();
            _modifiers.Add(modifier);
            return this;
        }



        public IConfigureConventions DefaultBuilder(Func<ModelInfo, HtmlTag> action)
        {
            action.MustNotBeNull();
            var lambda = new LambdaConventions(LambdaConventions.AppliesAlways);
            lambda.Builder = action;
            _defaultBuilder = lambda;
            return this;
        }

        public IHaveModelConventions GetConventions(ModelInfo info)
        {
            var conventions = new ModelConventions(this);
            conventions.Builder = _builders.Find(d => d.AppliesTo(info));
            conventions.Modifiers = _modifiers.FindAll(d => d.AppliesTo(info));
            return conventions;
        }

        public IBuildElement GetDefaultBuilder()
        {
            return _defaultBuilder;
        }
    }
}