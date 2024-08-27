#if UNITY_EDITOR
namespace CodeHelper.Editor
{
    using CodeHelper.Unity;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;
    using UnityEditor;
    using UnityEngine;

    public class SpawnAssets : EditorWindow
    {
        private string _folderToSpawn, _filename = string.Empty, _className;
        private List<string> _assetNames, _fileNames;
        private int _index, _scriptCount;
        private bool _show, _copyFilenameToClass, _multiSpawn, _customNames;
        private List<string> _names = new() { string.Empty};

        [MenuItem("Code Helper/Spawn Asset")]
        public static void Init()
        {
            Type inspectorType = Type.GetType("UnityEditor.InspectorWindow,UnityEditor.dll");
            var window = GetWindow<SpawnAssets>("Spawn assets", new Type[] { inspectorType });
            window.minSize = new Vector2(450, 600);
        }

        private void Refresh()
        {
            var path = "Assets/Code Helper/Script Assets";
            if (Directory.Exists(path)) _assetNames = new(Directory.GetFiles(path, "*.txt"));
            else _assetNames = new(Directory.GetFiles(Directory.CreateDirectory(path).FullName));
            _fileNames = new();
            foreach (var item in _assetNames) _fileNames.Add(Path.GetFileName(item));
        }

        private void OnEnable() => Refresh();

        private void OnGUI()
        {
            _folderToSpawn = EditorGUILayout.TextField(_folderToSpawn);
            Event evt = Event.current;
            Rect dropArea = GUILayoutUtility.GetRect(0.0f, 50.0f, GUILayout.ExpandWidth(true));
            GUI.Box(dropArea, "Drag folder here!");
            switch (evt.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (!dropArea.Contains(evt.mousePosition))
                        break;

                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                    if (evt.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();

                        foreach (string draggedObject in DragAndDrop.paths)
                        {
                            if (Directory.Exists(draggedObject))
                            {
                                _folderToSpawn = draggedObject;
                                Repaint();
                            }
                        }
                    }
                    break;
            }
            _index = EditorGUILayout.Popup("Select asset",_index, _fileNames.ToArray());
            GUILayout.Space(10);
            if (GUILayout.Button("Create script")) Spawn();
            if (GUILayout.Button("Refresh")) Refresh();
            _show = EditorGUILayout.Foldout(_show, "Optional settings");
            if (_show)
            {
                if (!_customNames)  _filename = EditorGUILayout.TextField("Enter filename", _filename);

                if(_filename != string.Empty || _customNames)_copyFilenameToClass = EditorGUILayout.Toggle("File name is class name", _copyFilenameToClass);
                _multiSpawn = EditorGUILayout.Toggle("Multi spawn",_multiSpawn);
                if (_multiSpawn)
                {
                    _scriptCount = EditorGUILayout.IntSlider("Script count", _scriptCount, 1, 10);
                    _customNames = EditorGUILayout.Toggle("Custom names", _customNames);
                    if(_customNames)
                    {
                        for (int i = 0; i < _scriptCount; i++)
                        {
                            if (_names.Count <= i) _names.Add(string.Empty);
                            _names[i] = EditorGUILayout.TextField(_names[i]);
                        }
                    }
                }
            }
        } 

        private void Spawn()
        {
            string fileName;
            if (_filename == string.Empty) fileName = _fileNames[_index].Replace("txt", "cs");
            else fileName = _filename + ".cs";
            fileName.Print();
            if (_multiSpawn)
            {
                for (int i = 0; i < _scriptCount; i++)
                {
                    if (!_customNames)
                    {
                        _className = fileName.Replace(".cs", "") + i;
                        CreateFolder(fileName.Replace(".cs", $"{i}.cs"));
                    }
                    else
                    {
                        if (_names[i] != string.Empty) _className = _names[i];
                        else _className = fileName.Replace(".cs", "");
                        CreateFolder(_className + ".cs");
                    }
                   
                }
            }
            else
            {
                _className = fileName.Replace(".cs", "");
                CreateFolder(fileName);
            }
            
            AssetDatabase.Refresh();
            Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(fileName);
            EditorGUIUtility.PingObject(Selection.activeObject); 
        }

        private void CreateFolder(string name)
        {
            var file = File.ReadAllText(_assetNames[_index]);
            if (_copyFilenameToClass)
            {
                string pattern = @"class\s+(\w+)";
                Match match = Regex.Match(file, pattern);
                if (match.Success)
                {
                    string currentClassName = match.Groups[1].Value;
                    string newContent = file.Replace(currentClassName, _className);
                    File.WriteAllText(Path.Combine(_folderToSpawn,name), newContent);
                }
            }
            else
            {
                File.WriteAllText(_folderToSpawn + "/" + name, file);
            }
        }
    }
}

#endif