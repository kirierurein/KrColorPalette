using UnityEngine;

namespace KrColorPalette
{
    [System.Serializable]
    public sealed class ColorData
    {
        public string   details = string.Empty;
        public Color    color   = Color.white;

        public ColorData(){}

        public ColorData(string name, string hexColor)
        {
            this.details = name;
            ColorUtility.TryParseHtmlString( $"#{hexColor}", out color );
        }
    }
}
