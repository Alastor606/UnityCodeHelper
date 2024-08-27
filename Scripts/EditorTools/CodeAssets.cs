#if UNITY_EDITOR
namespace CodeHelper.Editor
{
    using System;
    using System.IO;
    using System.Text.RegularExpressions;
    using UnityEditor;
    using UnityEngine;

    public class CodeAssets : EditorWindow
    {
        public static readonly string CodeAssetsPath = "Assets/Code Helper/Script Assets/";
        private string _fileName, _code, _message, _savedFileName, _savedCode;
        private bool _containsSaved = false;
        private Vector2 _pos;

        [MenuItem("Code Helper/Create Asset")]
        public static void Init()
        {
            Type inspectorType = Type.GetType("UnityEditor.InspectorWindow,UnityEditor.dll");
            var window = GetWindow<CodeAssets>("Create code asset", new Type[] { inspectorType });
            window.minSize = new Vector2(450, 600);
        }

        private void OnEnable()
        {
            Selection.selectionChanged += CheckSelection;
        }

        private void OnGUI()
        {
            _fileName = EditorGUILayout.TextField("Enter filename", _fileName);
            GUILayout.Label("Code asset");
            _pos = EditorGUILayout.BeginScrollView(_pos, GUILayout.Height(Screen.height / 2));
            _code = EditorGUILayout.TextArea(_code, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true), GUILayout.Width(EditorGUIUtility.currentViewWidth));
            EditorGUILayout.EndScrollView();
            if (GUILayout.Button("Save asset")) SaveFile();
            if(GUILayout.Button("Clear"))
            {
                _fileName = string.Empty;
                _code = string.Empty;
            }
            if (_containsSaved)
            {
                EditorGUILayout.LabelField($"Now {_savedFileName} in buffer!");
                if(GUILayout.Button("Apply buffer to asset"))
                {
                    _fileName = _savedFileName;
                    _code = _savedCode;
                }
            }
            GUILayout.Label(_message);
        }

        [MenuItem("Assets/Create/Asset script", false, -1)]
        public static void SaveAsAsset()
        {
            var filename = CodeAssetsPath + Selection.activeObject.name + ".txt";
            if (Selection.activeObject == null) return;
            if (!Directory.Exists("Assets/HelperPrefabs")) Directory.CreateDirectory(CodeAssetsPath);
            var file = File.ReadAllText(AssetDatabase.GetAssetPath(Selection.activeObject));
            string pattern = @"class\s+(\w+)";
            Match match = Regex.Match(file, pattern);
            if (match.Success)
            {
                string currentClassName = match.Groups[1].Value;
                string newContent = file.Replace(currentClassName, currentClassName + "Asset");
                File.WriteAllText(filename, newContent);
                AssetDatabase.Refresh();
                Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(filename);
                EditorGUIUtility.PingObject(Selection.activeObject);
            }
        }

        private void CheckSelection()
        {
            string assetPath = string.Empty;
            if (Selection.assetGUIDs.Length > 0)
            {
                foreach (string guid in Selection.assetGUIDs)
                {
                    assetPath = AssetDatabase.GUIDToAssetPath(guid);
                    break;
                }
            }
            else return;
            if (!assetPath.Contains(".txt") && !assetPath.Contains(".cs")) return;
            _savedCode = File.ReadAllText(assetPath);
            var name = Path.GetFileName(assetPath);
            _savedFileName = name.Replace(name.Contains(".txt") == true ?".txt" : ".cs", "");
            _containsSaved = _savedCode != string.Empty && _savedFileName != string.Empty;
        }

        private void SaveFile()
        {
            if (_code == string.Empty || _fileName == string.Empty) return;
            var filename = CodeAssetsPath + _fileName + ".txt";
            if (!Directory.Exists("Assets/HelperPrefabs")) Directory.CreateDirectory(CodeAssetsPath);
            _message = "File creating";
            if (!File.Exists(filename))
            {
                File.WriteAllText(filename, _code);
                _message = $"File created in `{filename}`!";
                AssetDatabase.Refresh();
                Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(filename);
                EditorGUIUtility.PingObject(Selection.activeObject);
            }
            else
            {
                File.WriteAllText(filename, _code);
                _message = "File edited";
            }
        }
    }
}

#endif