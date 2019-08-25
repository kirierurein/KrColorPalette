using UnityEngine;

namespace KrColorPalette
{
    /// <summary>
    /// このファイルはColorPaletteEditorによって自動生成されるため変更しないでください
    /// </summary>
    public static class Palette
    {
        private static ColorData _Red = new ColorData("赤", "FF0000FF" );
        /// <summary>赤</summary>
        public static Color Red { get { return _Red.color; }}
        private static ColorData _Blue = new ColorData("青", "0000FFFF" );
        /// <summary>青</summary>
        public static Color Blue { get { return _Blue.color; }}
        private static ColorData _Green = new ColorData("緑", "00FF00FF" );
        /// <summary>緑</summary>
        public static Color Green { get { return _Green.color; }}
    }
}

