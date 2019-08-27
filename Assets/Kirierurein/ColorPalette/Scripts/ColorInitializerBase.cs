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
    }
}
