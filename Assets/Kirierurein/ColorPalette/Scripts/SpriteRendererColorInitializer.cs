using UnityEngine;

namespace KrColorPalette
{
    /// <summary>
    /// 指定されたColorをSpriteRendererのcolorに設定するクラス
    /// SpriteRendererColorInitializeInspectorによってInspectorが拡張されています
    /// </summary>
    public class SpriteRendererColorInitializer : ColorInitializerBase
    {
        [SerializeField]
        private SpriteRenderer target   = null;

        private void Awake()
        {
            if(target != null)
            {
                target.color = color;
            }
        }
    }
}
