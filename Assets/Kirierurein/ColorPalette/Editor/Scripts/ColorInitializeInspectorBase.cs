﻿using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Linq;

namespace KrColorPalette
{
    public abstract class ColorInitializeInspectorBase : Editor
    {
        protected PropertyInfo[]        properties          = null; // Palette色のクラスのプロパティ配列
        protected string[]              propertyNames       = null; // プルダウンに表示するプロパティ名配列
        protected SerializedProperty    color               = null;

        private SerializedProperty  selectPropertyName  = null;
        private int                 selectIndex         = -1;   // 選択しているプロパティ配列のindex(-1の場合は選択対象のプロパティが存在しない)

        protected virtual void OnEnable()
        {
            selectPropertyName = serializedObject.FindProperty("selectPropertyName");
            color = serializedObject.FindProperty("color");

            System.Type type = typeof(Palette);
            properties = type.GetProperties();
            propertyNames =  properties.Select(property => property.Name).ToArray();
            for(int i = 0; i < properties.Length; i ++)
            {
                if(selectPropertyName.stringValue == properties[i].Name)
                {
                    selectIndex = i;
                }
            }

            if(selectIndex <= -1 && !string.IsNullOrEmpty(selectPropertyName.stringValue))
            {
                // すでに設定していたプロパティがなくなった場合はエラーログを表示
                Debug.LogError($"[{target.GetType().Name}]指定されている{selectPropertyName.stringValue}は定義されていません : {target.name}");
            }
        }

        public override void OnInspectorGUI()
        {
            selectIndex = EditorGUILayout.Popup("Color", selectIndex, propertyNames);

            // -1の場合は色を反映できないのでリターンする
            if(selectIndex <= -1)
            {
                return;
            }

            if(propertyNames[selectIndex] != selectPropertyName.stringValue)
            {
                selectPropertyName.stringValue = propertyNames[selectIndex];
                color.colorValue = (Color)properties[selectIndex].GetValue(null);
            }
        }
    }
}

