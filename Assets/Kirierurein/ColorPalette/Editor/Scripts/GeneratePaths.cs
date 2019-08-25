using UnityEngine;
using UnityEditor;

namespace KrColorPalette
{
    public class GeneratePaths : ScriptableObject
    {
        public const string configPath = "Assets/Kirierurein/ColorPalette/Editor/ScriptableObject/GenerateFilePathDefine.asset";

        /// <summary>
        /// colorsアセットの生成パス
        /// Editor直下にファイルを生成しないとColorPresetLibraryが認識しないので注意
        /// </summary>
        public string colorAssetPath    = "Kirierurein/ColorPalette/Editor/CustomPalette.colors";
        /// <summary>Palletクラスのファイルの生成パス</summary>
        public string defineScriptPath  = "Kirierurein/ColorPalette/Scripts/Palette.cs";

        [MenuItem ("Tools/Kirierurein/ColorPalette/CreatePathDefine")]
        static void CreateScriptableObject ()
        {
            var instance = CreateInstance<GeneratePaths> ();
            AssetDatabase.CreateAsset (instance, configPath);
            AssetDatabase.Refresh ();
        }
    }
}
