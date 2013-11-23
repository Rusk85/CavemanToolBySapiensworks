using System;
using CavemanTools.Model;

namespace HtmlConventionsSample.Models
{
    public class MyModel
    {
        public int Number { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsTrue { get; set; }
        public string[] Items { get; set; }
        public IdName Model { get; set; }

        public MyModel()
        {
            
        }
    }
}