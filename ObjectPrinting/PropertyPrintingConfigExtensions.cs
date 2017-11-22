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
            printingConfig.CulturesDictAdd(typeof(double), cultureInfo);
            return printingConfig;
        }
        
        public static PrintingConfig<TOwner> Using<TOwner>(
            this PropertyPrintingConfig<TOwner, int> propertyPrintingConfig,
            CultureInfo cultureInfo)
        {
            var printingConfig = ((IPropertyPrintingConfig<TOwner, int>)propertyPrintingConfig)
                .PrintingConfig;
            printingConfig.CulturesDictAdd(typeof(int), cultureInfo);
            return printingConfig;
        }

        public static PrintingConfig<TOwner> TrimToLength<TOwner>(
            this PropertyPrintingConfig<TOwner, string> propertyPrintingConfig,
            int length)
        {
            var printingConfig = ((IPropertyPrintingConfig<TOwner, string>)propertyPrintingConfig)
                .PrintingConfig;
            printingConfig.TrimStringPropertyDictAdd(propertyPrintingConfig.propName,
                length);
            
            return printingConfig;
        }
    };
}