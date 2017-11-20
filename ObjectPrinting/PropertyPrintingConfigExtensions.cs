using System.Globalization;

namespace ObjectPrinting
{
    public static class PropertyPrintingConfigExtensions
    {
        public static PrintingConfig<TOwner> Using<TOwner>(
            this PropertyPrintingConfig<TOwner, double> propertyPrintingConfig,
            CultureInfo cultureInfo)
        {
            var printingConfig = ((IPropertyPrintingConfig<TOwner, double>)propertyPrintingConfig)
                .PrintingConfig;
            printingConfig.Cultures.Add(typeof(double), cultureInfo);
            return printingConfig;
        }
        
        public static PrintingConfig<TOwner> Using<TOwner>(
            this PropertyPrintingConfig<TOwner, int> propertyPrintingConfig,
            CultureInfo cultureInfo)
        {
            var printingConfig = ((IPropertyPrintingConfig<TOwner, int>)propertyPrintingConfig)
                .PrintingConfig;
            printingConfig.Cultures.Add(typeof(int), cultureInfo);
            return printingConfig;
        }

        public static PrintingConfig<TOwner> TrimToLength<TOwner>(
            this PropertyPrintingConfig<TOwner, string> propertyPrintingConfig,
            int length)
        {
            var printingConfig = ((IPropertyPrintingConfig<TOwner, string>)propertyPrintingConfig)
                .PrintingConfig;
            printingConfig.MaxLength = length;
            return printingConfig;
        }
    };
}