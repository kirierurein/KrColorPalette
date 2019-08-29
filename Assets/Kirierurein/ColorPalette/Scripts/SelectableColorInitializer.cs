using UnityEngine;
using UnityEngine.UI;

namespace KrColorPalette
{
    /// <summary>
    /// 指定されたColorをSelectableのcolorに設定するクラス
    /// SelectableColorInitializeInspectorによってInspectorが拡張されています
    /// </summary>
    public class SelectableColorInitializer : ColorInitializerBase
    {
        [SerializeField]
        private Selectable  target                  = null;
        [SerializeField]
        private Color       highlightedColor        = Color.white;
        [SerializeField]
        private     string  highlightedColorName    = string.Empty;
        [SerializeField]
        private Color       pressedColor            = Color.white;
        [SerializeField]
        private     string  pressedColorName        = string.Empty;
        [SerializeField]
        private Color       disabledColor           = Color.white;
        [SerializeField]
        private     string  disabledColorName       = string.Empty;

        private void Awake()
        {
            if(target != null)
            {
                target.colors = new ColorBlock
                {
                    normalColor = color,
                    highlightedColor = highlightedColor,
                    pressedColor = pressedColor,
                    disabledColor = disabledColor,
                };
            }
        }
    }
}
