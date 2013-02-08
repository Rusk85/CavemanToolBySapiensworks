using System;

namespace CavemanTools.Mvc.Controllers
{
    internal interface IPopulateModel
    {
        Type ModelType { get; }
    }
}