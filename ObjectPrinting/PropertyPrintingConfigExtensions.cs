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
            PrintingConfig<TOwner>.CulturesDictAdd(printingConfig, typeof(double), cultureInfo);
            return printingConfig;
        }
        
        public static PrintingConfig<TOwner> Using<TOwner>(
            this PropertyPrintingConfig<TOwner, int> propertyPrintingConfig,
            CultureInfo cultureInfo)
        {
            var printingConfig = ((IPropertyPrintingConfig<TOwner, int>)propertyPrintingConfig)
                .PrintingConfig;
            PrintingConfig<TOwner>.CulturesDictAdd(printingConfig, typeof(int), cultureInfo);
            return printingConfig;
        }

        public static PrintingConfig<TOwner> TrimToLength<TOwner>(
            this PropertyPrintingConfig<TOwner, string> propertyPrintingConfig,
            int length)
        {
            var printingConfig = ((IPropertyPrintingConfig<TOwner, string>)propertyPrintingConfig)
                .PrintingConfig;
            PrintingConfig<TOwner>.TrimStringPropertyDictAdd(printingConfig,
                propertyPrintingConfig.propName, length);            
            return printingConfig;
        }
    };
}