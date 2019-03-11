using System;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using Jurassic;

namespace gxclient.Util
{
    public static class Serializer2
    {
        private static ScriptEngine JsContext;

        private static readonly object locker = new object();

        private static void LoadScript(string scriptName)
        {
            var assembly = typeof(Serializer2).GetTypeInfo().Assembly;
            using (Stream stream = assembly.GetManifestResourceStream($"gxclient.Resources.{scriptName}.js"))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    try
                    {
                        string script = reader.ReadToEnd();
                        //JsContext.CommonJS().RunMain($"Resources/{scriptName}");
                        var result = JsContext.Evaluate(script).ToString();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
        }

        private static ScriptEngine GetContext()
        {
            if (JsContext != null)
            {
                return JsContext;
            }
            lock (locker)
            {
                if (JsContext == null)
                {
                    JsContext = new ScriptEngine();
                    LoadScript("typedarray");
                    LoadScript("tx_serializer");
                }
            }
            return JsContext;
        }

        public static string SerializeActionData(string action, Object parameters,Object abi)
        {

            //string script = "serializer";
            try
            {
                var e = GetContext();
                string script = $"serializer.serializeCallData('{action}',{JsonConvert.SerializeObject(parameters)},{JsonConvert.SerializeObject(abi)}).toString('hex')";
                string result = e.Evaluate(script).ToString();
                return result;
            }
            catch(Exception ex)
            {
                Console.Write(ex);
                return null;
            }

        }

        public static string SerializeTransaction(Object tx)
        {
            try
            {
                var e = GetContext();
                string script = $"serializer.serializeTransaction({JsonConvert.SerializeObject(tx)}).toString('hex')";
                string result = e.Evaluate(script).ToString();
                return result;
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                return null;
            }

        }
    }
}
