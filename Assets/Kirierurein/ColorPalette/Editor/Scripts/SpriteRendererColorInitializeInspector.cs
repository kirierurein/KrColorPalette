using UnityEngine;
using UnityEditor;

namespace KrColorPalette
{
    [CustomEditor(typeof(SpriteRendererColorInitializer))]
    [CanEditMultipleObjects]
    public class SpriteRendererColorInitializeInspector : ColorInitializeInspectorBase
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
                SpriteRenderer graphic = (SpriteRenderer)targetGraphic.objectReferenceValue;
                graphic.color = color;
            }
        }
    }
}
