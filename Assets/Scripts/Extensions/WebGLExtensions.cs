using System.Runtime.InteropServices;

namespace SkyExtensions
{
    public static class WebGLExtensions
    {
        [DllImport("__Internal")]
        private static extern void OnCopyWebGLText(string text);

        public static void CopyWebGLText(string text)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            OnCopyWebGLText(text);
#endif
        }
    }
}