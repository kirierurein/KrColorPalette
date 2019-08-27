using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Reflection;
using System.Linq;

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
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(targetGraphic);
            base.OnInspectorGUI();
            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// 色の更新
        /// </summary>
        protected override void UpdateColor(Color color)
        {
            if(targetGraphic.objectReferenceValue != null)
            {
                Graphic graphic = (Graphic)targetGraphic.objectReferenceValue;
                graphic.color = color;
            }
        }
    }
}
