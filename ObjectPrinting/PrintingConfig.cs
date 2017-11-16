using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ObjectPrinting
{
    public class PrintingConfig<TOwner>
    {

        public PropertyPrintingConfig<TOwner, TPropType> Printing<TPropType>()
        {
            return new PropertyPrintingConfig<TOwner,TPropType>(this);
        }
        
        public PropertyPrintingConfig<TOwner, TPropType> Printing<TPropType>(Expression<Func<TOwner, TPropType>> selector)
        {
            return new PropertyPrintingConfig<TOwner, TPropType>(this);
        }
        
        
        public PrintingConfig<TOwner> ExcludeType<TPropType>()
        {
            return this;
        }

        public PrintingConfig<TOwner> UseAltSerialization<TPropType>(Func<TPropType, string> altSerialize)
        {
            return this;
        }
        
        public string PrintToString(TOwner obj)
        {
            return PrintToString(obj, 0);
        }

        private string PrintToString(object obj, int nestingLevel)
        {
            //TODO apply configurations
            if (obj == null)
                return "null" + Environment.NewLine;

            var finalTypes = new[]
            {
                typeof(int), typeof(double), typeof(float), typeof(string),
                typeof(DateTime), typeof(TimeSpan)
            };
            if (finalTypes.Contains(obj.GetType()))
                return obj + Environment.NewLine;

            var identation = new string('\t', nestingLevel + 1);
            var sb = new StringBuilder();
            var type = obj.GetType();
            sb.AppendLine(type.Name);
            foreach (var propertyInfo in type.GetProperties())
            {
                sb.Append(identation + propertyInfo.Name + " = " +
                          PrintToString(propertyInfo.GetValue(obj),
                              nestingLevel + 1));
            }
            return sb.ToString();
        }
    }

    public class PropertyPrintingConfig<TOwner, TPropType> : IPropertyPrintingConfig<TOwner, TPropType>
    {
        private PrintingConfig<TOwner> printingConfig;

        PrintingConfig<TOwner> IPropertyPrintingConfig<TOwner, TPropType>
            .PrintingConfig => printingConfig;
        
        public PropertyPrintingConfig(PrintingConfig<TOwner> printingConfig)
        {
            this.printingConfig = printingConfig;
        }

        public PrintingConfig<TOwner> Using(Func<TPropType, string> serializeFunc)
        {
            return printingConfig;
        }
        

//        public PrintingConfig<TOwner> Using(CultureInfo cultureInfo)
//        {
//            return printingConfig;
//        }
    }

    public interface IPropertyPrintingConfig<TOwner, TPropType>
    {
        PrintingConfig<TOwner> PrintingConfig { get; }
    }

    public static class PropertyPrintingConfigExtensions
    {
        public static PrintingConfig<TOwner> Using<TOwner>(
            this PropertyPrintingConfig<TOwner, double> propertyPrintingConfig,
            CultureInfo cultureInfo)
        {
            return ((IPropertyPrintingConfig<TOwner, double>)propertyPrintingConfig)
                .PrintingConfig;
        }
    };
}