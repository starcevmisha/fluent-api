using System;

namespace ObjectPrinting
{
    public class PropertyPrintingConfig<TOwner, TPropType> : IPropertyPrintingConfig<TOwner, TPropType>
    {
        private PrintingConfig<TOwner> printingConfig;
        internal string propName;

        PrintingConfig<TOwner> IPropertyPrintingConfig<TOwner, TPropType>
            .PrintingConfig => printingConfig;
        
        public PropertyPrintingConfig(PrintingConfig<TOwner> printingConfig,
            string propName= null)
        {
            this.propName = propName;
            this.printingConfig = printingConfig;
        }

        public PrintingConfig<TOwner> Using(Func<TPropType, string> serializeFunc)
        {
            if (propName!= null)
                printingConfig.AddCustomPropertySerialization(propName, serializeFunc);
            else
                printingConfig.AddCustomTypeSerialize(typeof(TPropType), serializeFunc);
            return printingConfig;
        }
        
    }
    
}