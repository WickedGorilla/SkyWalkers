using System.Runtime.InteropServices;

namespace SkyExtensions
{
    public static class WebGLExtensions
    {
        public static void CopyText(string text)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            OnCopyWebGLText(text);
#endif
        }
        
        [DllImport("__Internal")]
        private static extern void OnCopyWebGLText(string text);
    }
}