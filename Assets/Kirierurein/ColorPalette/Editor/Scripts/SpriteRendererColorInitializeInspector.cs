using UnityEngine;
using UnityEditor;

namespace KrColorPalette
{
    [CustomEditor(typeof(SpriteRendererColorInitializer))]
    [CanEditMultipleObjects]
    public class SpriteRendererColorInitializeInspector : ColorInitializeInspectorBase
    {
        private SerializedProperty  targetSprite        = null;

        protected override void OnEnable()
        {
            base.OnEnable();
            targetSprite = serializedObject.FindProperty("target");
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
            EditorGUILayout.PropertyField(targetSprite);
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
            if(targetSprite.objectReferenceValue != null)
            {
                SpriteRenderer graphic = (SpriteRenderer)targetSprite.objectReferenceValue;
                graphic.color = color;
            }
        }
    }
}
