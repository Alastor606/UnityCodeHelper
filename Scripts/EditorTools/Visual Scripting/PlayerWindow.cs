#if UNITY_EDITOR
namespace CodeHelper.Editor
{
    using UnityEngine;
    using UnityEditor;
    using System;

    public class PlayerWindow : EditorWindow
    {
        private string _scriptName;
        private bool _2D = true;

        private bool _sideScrollView = true, _withJump = true, _hp, _slider;
        private bool _dash, _rotateToMouse;
        public static void Init()
        {
            Type inspectorType = Type.GetType("UnityEditor.InspectorWindow,UnityEditor.dll");
            var window = GetWindow<PlayerWindow>("Create player scripts", new Type[] { inspectorType });
            window.minSize = new Vector2(450, 600);
        }

        private void OnGUI()
        {
            _2D = GUILayout.Toggle(_2D, "2D", "Button");
            _scriptName = EditorGUILayout.TextField("Script name", _scriptName) ;
            GUILayout.Space(20);
            if (_2D) Draw2D();
            else Draw3D();
            if (GUILayout.Button("Create"))
            {
                if (_2D)
                {
                    PlayerCreator.Create2D(_scriptName, _sideScrollView, _withJump);
                }
            }
        }

        private void Draw2D()
        {
            GUILayout.Label("2D character settings");
            _sideScrollView = GUILayout.Toggle(_sideScrollView, "Side scroll controller");
            if (_sideScrollView)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(40);
                _withJump = GUILayout.Toggle(_withJump, "Jump");
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Space(40);
                _dash = GUILayout.Toggle(_dash, "Dash");
                GUILayout.EndHorizontal();
            }
            else
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(40);
                _rotateToMouse = GUILayout.Toggle(_rotateToMouse, "Rotete to mouse positon");
                GUILayout.EndHorizontal();
            }

            _hp = GUILayout.Toggle(_hp,"Health");

            if (_hp)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(40); 
                _slider = GUILayout.Toggle(_slider, "0-100 hp");
                GUILayout.EndHorizontal();
            }
        }

        private void Draw3D()
        {

        }
    }
}
#endif