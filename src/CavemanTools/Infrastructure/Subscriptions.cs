using System;
using System.Collections.Generic;

namespace CavemanTools.Infrastructure
{
    public class Subscriptions:List<IDisposable>
    {
         public void DisposeAll()
         {
             ForEach(d=>d.Dispose());
             Clear();
         }
    }
}