#if UNITY_EDITOR
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;

namespace CodeHelper.Editor
{
    public class PlayerCreator
    {
        private static readonly string SavePath = "Assets/Helper Scripts";
        private static readonly string SideScroll = "\t\t_rigidbody.TransferControl2D(_speed,0);";

        private static readonly string TopDown = "\t\tvar horizontal = Input.GetAxis(\"Horizontal\") * _speed;\n\t\tvar vertical = Input.GetAxis(\"Vertical\") * _speed;\n\n\t\t" +
            "_rigidbody.velocity = new Vector3(horizontal,0, vertical).normalized;";

        public static void Create2D(string fileName, bool sideScroll, bool withJump)
        {
            string resultTxt = CodeData.Class(fileName);
            resultTxt = resultTxt.AddFirst("using CodeHelper.Unity;\n");
            resultTxt = resultTxt.AddTo("Update()", sideScroll == true ? SideScroll:TopDown);
            if(withJump)
            {
                resultTxt = resultTxt.Replace("0", "_jumpForce");
                resultTxt = resultTxt.InitFields("\t[SerializeField] private float _jumpForce;\n");
            }
            resultTxt = resultTxt.InitFields("\t[SerializeField] private Rigidbody2D _rigidbody;\n\t[SerializeField] private float _speed;\n");
            if (!Directory.Exists(SavePath)) Directory.CreateDirectory(SavePath);
            File.WriteAllText(SavePath+ "/"+fileName+".cs", resultTxt);
             
            AssetDatabase.Refresh();
            Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(fileName);
            EditorGUIUtility.PingObject(Selection.activeObject);
        }
    }
}
#endif