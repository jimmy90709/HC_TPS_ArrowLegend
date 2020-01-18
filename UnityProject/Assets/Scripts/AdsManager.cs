using UnityEngine;
using UnityEngine.Advertisements;          // 引用 廣告 API

public class AdsManager : MonoBehaviour
{
    private string googleID = "3436906";              // Google 專案 ID
    private bool testMode = true;                     // 測試模式 : 允許在 Unity 內測試

    private string placemnetRevival = "revival";      // 廣告類型 : 復活
    private void Start()
    {
        Advertisement.Initialize(googleID, testMode); // 廣告.初始化(ID，測試模式);
    }
}
