using System;

namespace ObjectPrinting
{
    public static class ObjectExtensions
    {
        public static string PrintToString<T>(this T obj)
        {
            return ObjectPrinter.For<T>().PrintToString(obj);
        }
        
        public static string PrintToString<T>(this T obj, 
            Func<PrintingConfig<T>, PrintingConfig<T>> configFunc)
        {
            return configFunc(ObjectPrinter.For<T>()).PrintToString(obj);
        }
    }
}