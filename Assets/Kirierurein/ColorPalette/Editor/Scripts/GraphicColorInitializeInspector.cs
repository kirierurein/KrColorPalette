using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace KrColorPalette
{
    [CustomEditor(typeof(GraphicColorInitializer))]
    [CanEditMultipleObjects]
    public class GraphicColorInitializeInspector : ColorInitializeInspectorBase
    {
        private SerializedProperty  targetGraphic       = null;

        protected override void OnEnable()
        {
            targetGraphic = serializedObject.FindProperty("target");
            base.OnEnable();
            if(!EditorApplication.isPlaying)
            {
                // エディタがプレイ中でなければカラーを再設定しない
                UpdateColor(color.colorValue);
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(targetGraphic);
            base.OnInspectorGUI();
            if(EditorGUI.EndChangeCheck())
            {
                UpdateColor(color.colorValue);
            }
            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// 色の更新
        /// </summary>
        private void UpdateColor(Color color)
        {
            if(targetGraphic.objectReferenceValue != null)
            {
                Graphic graphic = (Graphic)targetGraphic.objectReferenceValue;
                graphic.color = color;
            }
        }
    }
}
