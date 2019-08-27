using UnityEngine;
using UnityEngine.UI;

namespace KrColorPalette
{
    /// <summary>
    /// 指定されたColorをGraphicのcolorに設定するクラス
    /// GraphicColorInitializeInspectorによってInspectorが拡張されています
    /// </summary>
    public class GraphicColorInitializer : ColorInitializerBase
    {
        [SerializeField]
        private Graphic target              = null;

        private void Awake()
        {
            if(target != null)
            {
                target.color = color;
            }
        }
    }
}
