#if UNITY_EDITOR
using System;
using System.Text.RegularExpressions;

namespace CodeHelper.Editor
{
    public static class CodeData
    {
        public static readonly string Update = "Update()", Start = "Start()", Awake = "Awake()";
        public static readonly string Core = "using UnityEngine;\n\n\r\npublic class Test : MonoBehaviour\r\n{\r\n\r\n}";

        public static string AddTo(this string content, string methodName, string newData)
        {
            if (content.Contains(methodName)) content = Regex.Replace(content, @$"({methodName}\(\)\s*{{)", "$1" + Environment.NewLine + newData); 
            else
            {
                int indexOfClass = content.LastIndexOf("class");
                int indexOfClassEnd = content.IndexOf("{", indexOfClass);
                content = content.Insert(indexOfClassEnd + 1, Environment.NewLine + $"\tpublic void {methodName}" + Environment.NewLine + "\t{" + Environment.NewLine + newData + Environment.NewLine + "\t}"
                    + Environment.NewLine);
            }
            return content;
        }

        public static string Class(string name) => Core.Replace("Test", name);

        public static string InitFields(this string code, string fields)
        {
            int indexOfClass = code.LastIndexOf("class");
            int indexOfClassEnd = code.IndexOf("{", indexOfClass) + 1;
            code = code.Insert(indexOfClassEnd, Environment.NewLine + fields);
            return code;
        }

        public static string AddFirst(this string code, string add) => add + code;
    }

}
#endif