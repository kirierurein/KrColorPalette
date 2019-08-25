using UnityEngine;
using UnityEditor;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace KrColorPalette
{
    public class ColorPaletteEditorWindow : EditorWindow
    {
        [System.Serializable]
        private class Data
        {
            public string       fieldName = string.Empty;
            public ColorData    colorData = new ColorData();
        }

        [SerializeField]
        private List<Data>      palette         = new List<Data>();
        private Vector2         scrollPosition  = Vector2.zero;
        private GeneratePaths   paths           = null;
        private bool            isValidated     = true;

        [MenuItem("Tools/Kirierurein/ColorPalette/ColorPaletteEditor")]
        private static void OpenWindow()
        {
            GetWindow<ColorPaletteEditorWindow>("Color Palette Editor");
        }

        private void OnEnable()
        {
            paths = AssetDatabase.LoadAssetAtPath<GeneratePaths>(GeneratePaths.configPath);

            System.Type type = typeof(Palette);
            palette.Clear();
            foreach(FieldInfo field in type.GetFields(BindingFlags.NonPublic | BindingFlags.Static))
            {
                // fieldNameは先頭に'_'がついているのでを取り除く
                Data data = new Data{ fieldName = field.Name.Substring(1), colorData = (ColorData)field.GetValue(null)};
                palette.Add(data);
            }
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            if(GUILayout.Button("保存"))
            {
                Save();
            }

            if(GUILayout.Button("追加"))
            {
                Undo.RecordObject(this, "Add ColorData");
                palette.Add(new Data{ fieldName = $"color{palette.Count + 1}" });
            }
            EditorGUILayout.EndHorizontal();

            if(!isValidated)
            {
                EditorGUILayout.LabelField("変数名が重複している箇所があります");
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("変数名", GUILayout.MinWidth(50));
            EditorGUILayout.LabelField("詳細", GUILayout.MinWidth(50));
            EditorGUILayout.LabelField("色", GUILayout.MinWidth(50));
            GUILayout.Label("");
            EditorGUILayout.EndHorizontal();

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            for(int i = 0; i < palette.Count; i++)
            {
                Data data = palette[i];
                EditorGUILayout.BeginHorizontal();
                string fieldName = EditorGUILayout.TextField(data.fieldName);
                if(fieldName != data.fieldName && !string.IsNullOrEmpty(fieldName))
                {
                    Undo.RecordObject(this, "Edit fieldName");
                    data.fieldName = fieldName;
                }
                string colorDataName = EditorGUILayout.TextField(data.colorData.details);
                if(colorDataName != data.colorData.details)
                {
                    Undo.RecordObject(this, "Edit colorDataName");
                    data.colorData.details = colorDataName;
                }
                Color color = EditorGUILayout.ColorField(data.colorData.color);
                if(color != data.colorData.color)
                {
                    Undo.RecordObject(this, "Edit color");
                    data.colorData.color = color;
                }
                if(GUILayout.Button("削除"))
                {
                    Undo.RecordObject(this, "Remove ColorData");
                    palette.RemoveAt(i);
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
        }

        private void Save()
        {
            isValidated = ValidateFieldName();
            if(isValidated)
            {
                CreateScript();
                CreateColorPaletteLibrary();
                AssetDatabase.Refresh();
            }
            else
            {
                Debug.LogError("[ColorPaletteEditor] 変数名が重複しているため保存できません");
            }
        }

        /// <summary>
        /// fieldNameに重複がないかを検証します
        /// </summary>
        /// <returns>重複がない場合Trueを返します</returns>
        private bool ValidateFieldName()
        {
            return palette.GroupBy(data => data.fieldName).Where(name => name.Count() > 1).Count() <= 0;
        }

        /// <summary>
        /// 定義ファイルを作成
        /// </summary>
        private void CreateScript()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("using UnityEngine;");
            builder.AppendLine();
            builder.AppendLine("namespace KrColorPalette");
            builder.AppendLine("{");
            builder.AppendLine("    /// <summary>");
            builder.AppendLine("    /// このファイルはColorPaletteEditorによって自動生成されるため変更しないでください");
            builder.AppendLine("    /// </summary>");
            builder.AppendLine("    public static class Palette");
            builder.AppendLine("    {");
            foreach(Data data in palette)
            {
                string hexColor = ColorUtility.ToHtmlStringRGBA(data.colorData.color);
                builder.AppendLine($"        private static ColorData _{data.fieldName} = new ColorData(\"{data.colorData.details}\", \"{hexColor}\" );");
                builder.AppendLine($"        /// <summary>{data.colorData.details}</summary>");
                builder.AppendLine($"        public static Color {data.fieldName} {{ get {{ return _{data.fieldName}.color; }}}}");
            }
            builder.AppendLine("    }");
            builder.AppendLine("}");

            using (StreamWriter writer = new StreamWriter(Path.Combine(Application.dataPath, paths.defineScriptPath), false, Encoding.UTF8))
            {
                writer.WriteLine(builder);
            }
        }

        /// <summary>
        /// Color編集の際のパレットを作成する
        /// </summary>
        private void CreateColorPaletteLibrary()
        {
            // UnityEditorのinlineなクラスを取得する
            System.Type type = System.Type.GetType( "UnityEditor.ColorPresetLibrary, UnityEditor" );
            ScriptableObject asset = CreateInstance( type );
            MethodInfo method = type.GetMethod( "Add" );

            foreach (Data data in palette )
            {
                method.Invoke( asset, new object[] { data.colorData.color, data.colorData.details } );
            }

            AssetDatabase.CreateAsset( asset, Path.Combine("Assets/", paths.colorAssetPath));
            AssetDatabase.SaveAssets();
        }
    }
}
