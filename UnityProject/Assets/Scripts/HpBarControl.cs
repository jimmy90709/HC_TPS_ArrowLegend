using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HpBarControl : MonoBehaviour
{
    private Image imgHp;
    private Text textHp;
    private Text textDamage;

    private void Start()
    {
        imgHp = transform.GetChild(1).GetComponent<Image>();
        textHp = transform.GetChild(2).GetComponent<Text>();
        textDamage = transform.GetChild(3).GetComponent<Text>();
    }
    private void Update()
    {
        AngleControl(); 
    }

    /// <summary>
    /// 角度控制 : 讓血條保持世界座標角度 35, -180, 0
    /// </summary>
    private void AngleControl()
    {
        // 變形元件.歐拉角度 = 新 三維向量() - 世界座標
        // transform.localEulerAnles         - 區域座標
        transform.eulerAngles = new Vector3(35, -180, 0);
    }

    public void UpdataHpBar(float hpMax, float hpCurrent)
    {
        imgHp.fillAmount = hpCurrent / hpMax;    // 圖片.填滿數值 = 目前 / 最大
        textHp.text = hpCurrent.ToString();      // 文字.文字內容 = 目前.轉字串()
    }
       
    public IEnumerator ShowDamage(float damage)                // 協程方法 : 顯示傷害值 - 接收傷害
    {
        Vector3 posOriginal = textDamage.transform.position;   // 取得原始位置
        textDamage.text = "-" + damage;                        // 更新傷害值 = 接收傷害
        // 迴圈
        for (int i = 0; i < 10; i++)
        {
            // 傷害值往上移動 (transform.position.y += 值)
            textDamage.transform.position += new Vector3(0, 0.1f, 0);

            // 等待
            yield return new WaitForSeconds(0.05f);
        }
        textDamage.transform.position = posOriginal;           // 位置 = 原始位置
        textDamage.text = "";                                  // 文字 = ""

    }

}
