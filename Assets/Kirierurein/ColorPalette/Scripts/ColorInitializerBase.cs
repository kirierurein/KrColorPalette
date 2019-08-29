using UnityEngine;
using System.Collections;

namespace KrColorPalette
{
    public abstract class ColorInitializerBase : MonoBehaviour
    {
        [SerializeField]
        protected   Color   color               = Color.white;
        [SerializeField]
        private     string  selectPropertyName  = string.Empty;

        /// <summary>
        /// Memo : Enableのチェックボックスを表示するために実装
        /// </summary>
        private void Start()
        {
            
        }
    }
}
