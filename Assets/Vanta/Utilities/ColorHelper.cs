using UnityEngine;



namespace Vanta.Utilities
{
    
    public class ColorHelper
    {
        
        public static Color ColorFromHex(uint hex)
        {
            Color c;
            ColorUtility.TryParseHtmlString("#" + hex.ToString("X"), out c);
            return c;
        }
        
    }

}