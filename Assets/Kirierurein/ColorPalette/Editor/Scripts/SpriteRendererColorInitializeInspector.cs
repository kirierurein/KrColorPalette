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
            targetSprite = serializedObject.FindProperty("target");
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
            EditorGUILayout.PropertyField(targetSprite);
            base.OnInspectorGUI();
            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// 色の更新
        /// </summary>
        protected override void UpdateColor(Color color)
        {
            if(targetSprite.objectReferenceValue != null)
            {
                SpriteRenderer graphic = (SpriteRenderer)targetSprite.objectReferenceValue;
                graphic.color = color;
            }
        }
    }
}
