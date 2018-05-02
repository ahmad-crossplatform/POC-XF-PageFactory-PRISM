using System;

namespace PageFactoryPrism.Attributes
{
    public class TitleAttribute : Attribute
    {
        public string Label { get; }


        public TitleAttribute(string label)
        {
            Label = label;
        }
    }
}