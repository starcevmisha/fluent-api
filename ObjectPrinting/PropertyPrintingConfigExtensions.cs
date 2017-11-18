using System.Globalization;

namespace ObjectPrinting
{
    public static class PropertyPrintingConfigExtensions
    {
        public static PrintingConfig<TOwner> Using<TOwner>(
            this PropertyPrintingConfig<TOwner, double> propertyPrintingConfig,
            CultureInfo cultureInfo)
        {
            return ((IPropertyPrintingConfig<TOwner, double>)propertyPrintingConfig)
                .PrintingConfig;
        }
        
        public static PrintingConfig<TOwner> Using<TOwner>(
            this PropertyPrintingConfig<TOwner, int> propertyPrintingConfig,
            CultureInfo cultureInfo)
        {
            return ((IPropertyPrintingConfig<TOwner, int>)propertyPrintingConfig)
                .PrintingConfig;
        }

        public static PrintingConfig<TOwner> TrimToLength<TOwner>(
            this PropertyPrintingConfig<TOwner, string> propertyPrintingConfig,
            int length)
        {
            return ((IPropertyPrintingConfig<TOwner, string>)propertyPrintingConfig)
                .PrintingConfig;
        }
    };
}