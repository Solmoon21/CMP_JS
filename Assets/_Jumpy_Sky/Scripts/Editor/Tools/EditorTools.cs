using UnityEngine;
using UnityEditor;

namespace ClawbearGames
{
    public class EditorTools : EditorWindow
    {
        [MenuItem("Tools/ClawbearGames/Reset PlayerPrefs")]
        public static void ResetPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("*************** PlayerPrefs Was Deleted ***************");
        }




        //[MenuItem("Tools/Capture Screenshot To Desktop")]
        //public static void CaptureScreenshot_Desktop()
        //{
        //    string path = "C:/Users/TIENNQ/Desktop/icon.png";
        //    ScreenCapture.CaptureScreenshot(path);
        //}
    }
}

