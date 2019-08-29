using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Reflection;

namespace KrColorPalette
{
    [CustomEditor(typeof(SelectableColorInitializer))]
    [CanEditMultipleObjects]
    public class SelectableColorInitializeInspector : ColorInitializeInspectorBase
    {
        private class SelectableColor
        {
            public SerializedProperty  color               = null;
            public SerializedProperty  selectPropertyName  = null;
            public int                 selectIndex         = -1;   // 選択しているプロパティ配列のindex(-1の場合は選択対象のプロパティが存在しない)

            public SelectableColor(SerializedProperty serializedProperty, SerializedProperty selectPropertyName, PropertyInfo[] properties)
            {
                color = serializedProperty;
                this.selectPropertyName = selectPropertyName;
                for(int i = 0; i < properties.Length; i ++)
                {
                    if(selectPropertyName.stringValue == properties[i].Name)
                    {
                        selectIndex = i;
                    }
                }
            }
        }

        private SerializedProperty  targetSelectable    = null;
        private SelectableColor     highlighted         = null;
        private SelectableColor     pressed             = null;
        private SelectableColor     disabled            = null;

        protected override void OnEnable()
        {
            base.OnEnable();
            targetSelectable = serializedObject.FindProperty("target");

            var highlightedColor = serializedObject.FindProperty("highlightedColor");
            var highlightedColorName = serializedObject.FindProperty("highlightedColorName");
            highlighted = new SelectableColor(highlightedColor, highlightedColorName, properties);
            if(highlighted.selectIndex <= -1 && !string.IsNullOrEmpty(highlighted.selectPropertyName.stringValue))
            {
                 // すでに設定していたプロパティがなくなった場合はエラーログを表示
                Debug.LogError($"[{target.GetType().Name}]HighlightedColorに指定されている{highlighted.selectPropertyName.stringValue}は定義されていません : {target.name}");
            }

            var pressedColor = serializedObject.FindProperty("pressedColor");
            var pressedColorrName = serializedObject.FindProperty("pressedColorName");
            pressed = new SelectableColor(pressedColor, pressedColorrName, properties);
            if(pressed.selectIndex <= -1 && !string.IsNullOrEmpty(pressed.selectPropertyName.stringValue))
            {
                // すでに設定していたプロパティがなくなった場合はエラーログを表示
                Debug.LogError($"[{target.GetType().Name}]PressedColorに指定されている{pressed.selectPropertyName.stringValue}は定義されていません : {target.name}");
            }

            var disabledColor = serializedObject.FindProperty("disabledColor");
            var disabledColorName = serializedObject.FindProperty("disabledColorName");
            disabled = new SelectableColor(disabledColor, disabledColorName, properties);
            if(disabled.selectIndex <= -1 && !string.IsNullOrEmpty(disabled.selectPropertyName.stringValue))
            {
                // すでに設定していたプロパティがなくなった場合はエラーログを表示
                Debug.LogError($"[{target.GetType().Name}]DisabledColorに指定されている{disabled.selectPropertyName.stringValue}は定義されていません : {target.name}");
            }

            if(!EditorApplication.isPlaying)
            {
                // エディタがプレイ中でなければカラーを再設定しない
                UpdateColors(color.colorValue, highlighted.color.colorValue, pressed.color.colorValue, disabled.color.colorValue);
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(targetSelectable);
            base.OnInspectorGUI();

            highlighted.selectIndex = EditorGUILayout.Popup("HighlightedColor", highlighted.selectIndex, propertyNames);

            if(highlighted.selectIndex >= 0)
            {
                if(propertyNames[highlighted.selectIndex] != highlighted.selectPropertyName.stringValue)
                {
                    highlighted.selectPropertyName.stringValue = propertyNames[highlighted.selectIndex];
                    highlighted.color.colorValue = (Color)properties[highlighted.selectIndex].GetValue(null);
                }
            }

            pressed.selectIndex = EditorGUILayout.Popup("PressedColor", pressed.selectIndex, propertyNames);

            if(pressed.selectIndex >= 0)
            {
                if(propertyNames[pressed.selectIndex] != pressed.selectPropertyName.stringValue)
                {
                    pressed.selectPropertyName.stringValue = propertyNames[pressed.selectIndex];
                    pressed.color.colorValue = (Color)properties[pressed.selectIndex].GetValue(null);
                }
            }

            disabled.selectIndex = EditorGUILayout.Popup("DisabledColor", disabled.selectIndex, propertyNames);

            if(disabled.selectIndex >= 0)
            {
                if(propertyNames[disabled.selectIndex] != disabled.selectPropertyName.stringValue)
                {
                    disabled.selectPropertyName.stringValue = propertyNames[disabled.selectIndex];
                    disabled.color.colorValue = (Color)properties[disabled.selectIndex].GetValue(null);
                }
            }

            if(EditorGUI.EndChangeCheck())
            {
                UpdateColors(color.colorValue, highlighted.color.colorValue, pressed.color.colorValue, disabled.color.colorValue);
            }

            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// 通常色の更新
        /// </summary>
        protected override void UpdateColor(Color color)
        {
            UpdateColors(color, highlighted.color.colorValue, pressed.color.colorValue, disabled.color.colorValue);
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateColors(Color normalColor, Color highlightedColor, Color pressedColor, Color disabledColor)
        {
            if(targetSelectable.objectReferenceValue != null)
            {
                Selectable graphic = (Selectable)targetSelectable.objectReferenceValue;
                ColorBlock colors = graphic.colors;
                colors.normalColor = normalColor;
                colors.highlightedColor = highlightedColor;
                colors.pressedColor = pressedColor;
                colors.disabledColor = disabledColor;
                graphic.colors = colors;
            }
        }
    }
}
