using System;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using Jint;
using Jint.CommonJS;

namespace gxclient.Util
{
    public static class Serializer
    {
        private static Engine JsContext;

        private static readonly object locker = new object();

        private static void LoadScript(string scriptName)
        {
            var assembly = typeof(Serializer).GetTypeInfo().Assembly;
            using (Stream stream = assembly.GetManifestResourceStream($"gxclient.Resources.{scriptName}.js"))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    try
                    {
                        string script = reader.ReadToEnd();
                        //JsContext.CommonJS().RunMain($"Resources/{scriptName}");
                        var result = JsContext.Execute(script).GetCompletionValue().ToString();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
        }

        private static Engine GetContext()
        {
            if (JsContext != null)
            {
                return JsContext;
            }
            lock (locker)
            {
                if (JsContext == null)
                {
                    JsContext = new Engine((obj) => { obj.DiscardGlobal(false); });
                    LoadScript("typedarray");
                    LoadScript("tx_serializer");
                }
            }
            return JsContext;
        }

        public static string SerializeActionData(string action, Object parameters,Object abi)
        {
            var e = GetContext();
            //string script = "serializer";
            string script = $"serializer.serializeCallData('{action}',{JsonConvert.SerializeObject(parameters)},{JsonConvert.SerializeObject(abi)}).toString('hex')";
            string result = e.Execute(script).GetCompletionValue().ToString();
            return result;
        }

        public static string SerializeTransaction(Object tx)
        {
            var e = GetContext();
            string script = $"serializer.serializeTransaction({JsonConvert.SerializeObject(tx)}).toString('hex')";
            string result = e.Execute(script).GetCompletionValue().ToString();
            return result;
        }
    }
}
