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
        private class ColorParam
        {
            public SerializedProperty  color               = null;
            public SerializedProperty  selectPropertyName  = null;
            public int                 selectIndex         = -1;   // 選択しているプロパティ配列のindex(-1の場合は選択対象のプロパティが存在しない)

            public ColorParam(SerializedProperty serializedProperty, SerializedProperty selectPropertyName, PropertyInfo[] properties)
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
        private ColorParam          highlightedParam    = null;
        private ColorParam          pressedParam        = null;
        private ColorParam          disabledParam       = null;

        protected override void OnEnable()
        {
            base.OnEnable();
            targetSelectable = serializedObject.FindProperty("target");

            var highlightedColor = serializedObject.FindProperty("highlightedColor");
            var highlightedColorName = serializedObject.FindProperty("highlightedColorName");
            highlightedParam = new ColorParam(highlightedColor, highlightedColorName, properties);
            if(highlightedParam.selectIndex <= -1 && !string.IsNullOrEmpty(highlightedParam.selectPropertyName.stringValue))
            {
                 // すでに設定していたプロパティがなくなった場合はエラーログを表示
                Debug.LogError($"[{target.GetType().Name}]HighlightedColorに指定されている{highlightedParam.selectPropertyName.stringValue}は定義されていません : {target.name}");
            }

            var pressedColor = serializedObject.FindProperty("pressedColor");
            var pressedColorrName = serializedObject.FindProperty("pressedColorName");
            pressedParam = new ColorParam(pressedColor, pressedColorrName, properties);
            if(pressedParam.selectIndex <= -1 && !string.IsNullOrEmpty(pressedParam.selectPropertyName.stringValue))
            {
                // すでに設定していたプロパティがなくなった場合はエラーログを表示
                Debug.LogError($"[{target.GetType().Name}]PressedColorに指定されている{pressedParam.selectPropertyName.stringValue}は定義されていません : {target.name}");
            }

            var disabledColor = serializedObject.FindProperty("disabledColor");
            var disabledColorName = serializedObject.FindProperty("disabledColorName");
            disabledParam = new ColorParam(disabledColor, disabledColorName, properties);
            if(disabledParam.selectIndex <= -1 && !string.IsNullOrEmpty(disabledParam.selectPropertyName.stringValue))
            {
                // すでに設定していたプロパティがなくなった場合はエラーログを表示
                Debug.LogError($"[{target.GetType().Name}]DisabledColorに指定されている{disabledParam.selectPropertyName.stringValue}は定義されていません : {target.name}");
            }

            if(!EditorApplication.isPlaying)
            {
                // エディタがプレイ中でなければカラーを再設定しない
                UpdateColors(color.colorValue, highlightedParam.color.colorValue, pressedParam.color.colorValue, disabledParam.color.colorValue);
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(targetSelectable);
            base.OnInspectorGUI();

            highlightedParam.selectIndex = EditorGUILayout.Popup("HighlightedColor", highlightedParam.selectIndex, propertyNames);

            if(highlightedParam.selectIndex >= 0)
            {
                if(propertyNames[highlightedParam.selectIndex] != highlightedParam.selectPropertyName.stringValue)
                {
                    highlightedParam.selectPropertyName.stringValue = propertyNames[highlightedParam.selectIndex];
                    highlightedParam.color.colorValue = (Color)properties[highlightedParam.selectIndex].GetValue(null);
                }
            }

            pressedParam.selectIndex = EditorGUILayout.Popup("PressedColor", pressedParam.selectIndex, propertyNames);

            if(pressedParam.selectIndex >= 0)
            {
                if(propertyNames[pressedParam.selectIndex] != pressedParam.selectPropertyName.stringValue)
                {
                    pressedParam.selectPropertyName.stringValue = propertyNames[pressedParam.selectIndex];
                    pressedParam.color.colorValue = (Color)properties[pressedParam.selectIndex].GetValue(null);
                }
            }

            disabledParam.selectIndex = EditorGUILayout.Popup("DisabledColor", disabledParam.selectIndex, propertyNames);

            if(disabledParam.selectIndex >= 0)
            {
                if(propertyNames[disabledParam.selectIndex] != disabledParam.selectPropertyName.stringValue)
                {
                    disabledParam.selectPropertyName.stringValue = propertyNames[disabledParam.selectIndex];
                    disabledParam.color.colorValue = (Color)properties[disabledParam.selectIndex].GetValue(null);
                }
            }

            if(EditorGUI.EndChangeCheck())
            {
                UpdateColors(color.colorValue, highlightedParam.color.colorValue, pressedParam.color.colorValue, disabledParam.color.colorValue);
            }

            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// 色の更新
        /// </summary>
        private void UpdateColors(Color normalColor, Color highlightedColor, Color pressedColor, Color disabledColor)
        {
            if(targetSelectable.objectReferenceValue != null)
            {
                Selectable selectable = (Selectable)targetSelectable.objectReferenceValue;
                ColorBlock colors = selectable.colors;
                colors.normalColor = normalColor;
                colors.highlightedColor = highlightedColor;
                colors.pressedColor = pressedColor;
                colors.disabledColor = disabledColor;
                selectable.colors = colors;
            }
        }
    }
}
