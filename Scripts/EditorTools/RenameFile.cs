#if UNITY_EDITOR
namespace CodeHelper.Editor
{
    using CodeHelper.Unity;
    using System;
    using System.IO;
    using System.Text.RegularExpressions;
    using UnityEditor;
    using UnityEngine;

    public class RenameFile : EditorWindow
    {
        private static string _fileName, _newName;
        [MenuItem("Code Helper/Rename")]
        public static void Init()
        {
            Type inspectorType = Type.GetType("UnityEditor.InspectorWindow,UnityEditor.dll");
            GetWindow<RenameFile>("Rename", new Type[] {inspectorType});
        }

        private void OnGUI()
        {
            _fileName = EditorGUILayout.TextField("Enter the file name", _fileName);
            _newName = EditorGUILayout.TextField("Enter new name", _newName);
            if (GUILayout.Button("Apply"))
            {
                SearchForFile(Application.dataPath, _fileName);
            }
        }

        private static void SearchForFile(string folder, string fileName)
        {
            try
            {
                var name = fileName + ".cs";
                var path = Path.Combine(folder, name);
                if (File.Exists(path))
                {
                    Rename(folder, name, _newName);
                    return;
                }

                string[] subdirectories = Directory.GetDirectories(folder);
                foreach (var subdirectory in subdirectories) SearchForFile(subdirectory, fileName);
                
            }
            catch (Exception ex)
            {
                ($"Exception: {ex.Message}").PrintError();
            }
        }

        public static void Rename(string folderPath, string fileName, string newName)
        {           
            var combinedPath = Path.Combine(folderPath, fileName);
            var file = File.ReadAllText(combinedPath);
            string pattern = @"class\s+(\w+)";
            Match match = Regex.Match(file, pattern);
            if (match.Success)
            {
                string currentClassName = match.Groups[1].Value;
                string newContent = file.Replace(currentClassName, newName);
                File.WriteAllText(combinedPath, newContent);
                File.Move(Path.Combine(folderPath, fileName), Path.Combine(folderPath, newName) + ".cs");

                AssetDatabase.Refresh();
                Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(Path.Combine(folderPath, newName));
                EditorGUIUtility.PingObject(Selection.activeObject);
            }
        }
    }
}
#endif
