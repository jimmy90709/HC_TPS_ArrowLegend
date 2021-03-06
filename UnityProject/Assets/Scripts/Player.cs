﻿using UnityEngine;

public class Player : MonoBehaviour
{
    #region 欄位
    [Header("移動速度"), Range(1, 200)]
    public float speed = 20;
    [Header("玩家資料")]
    public PlayerData data;

    private Joystick joy;
    private Transform target;
    private Rigidbody rig;
    private Animator ani;
    private LevelManager levelManager;  // 關卡管理器
    private HpBarControl hpControl;     // 血條控制器
    #endregion

    public float timer;

    #region 事件
    private void Start()
    {
        rig = GetComponent<Rigidbody>();                                 // 剛體欄位 = 取得元件<泛型>()
        ani = GetComponent<Animator>();
        // target = GameObject.Find("目標").GetComponent<Transform>();    // 寫法 1
        target = GameObject.Find("目標").transform;                       // 寫法 2
        joy = GameObject.Find("虛擬搖桿").GetComponent<Joystick>();
        levelManager = FindObjectOfType<LevelManager>();                  // 透過類型尋找物件
        hpControl = transform.Find("血條系統").GetComponent<HpBarControl>();           // 變形.尋找("子物件")
    }

    // 固定更新：固定一秒 50 次 - 物理行為
    private void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
        // 測試區域
        if (Input.GetKeyDown(KeyCode.Alpha1)) Attack();
        if (Input.GetKeyDown(KeyCode.Alpha2)) Dead();
    }

    // 觸發事件：碰到勾選 IsTrigger 物件執行一次
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "傳送區域")
        {
            levelManager.StartCoroutine("LoadLevel");
        }
    }
    #endregion

    #region 方法
    /// <summary>
    /// 移動玩家方法
    /// </summary>
    private void Move()
    {
        float h = joy.Horizontal;                       // 虛擬搖桿水平
        float v = joy.Vertical;                         // 虛擬搖桿垂直
        rig.AddForce(-h * speed, 0, -v * speed);        // 剛體.增加推力(水平，0，垂直)

        // 取得此物件變型元件
        // 原寫：GetComponent<Transform>() 
        // 簡寫：transform

        Vector3 posPlayer = transform.position;                                 // 玩家座標 = 取得玩家.座標
        Vector3 posTarget = new Vector3(posPlayer.x - h, 0, posPlayer.z - v);   // 目標座標 = 新 三維向量(玩家.X - 搖桿.X，Y，玩家.Z - 搖桿.Z)

        target.position = posTarget;                                            // 目標.座標 = 目標座標

        posTarget.y = posPlayer.y;          // 目標.Y = 玩家.Y (避免吃土)

        transform.LookAt(posTarget);        // 變形.看著(座標)
        // 水平 1、-1
        // 垂直 1、-1
        // 動畫控制器.設定布林值(參數名稱，布林值)
        ani.SetBool("跑步開關", h != 0 || v != 0);

        if (h == 0 && v == 0) Attack();     // 如果 水平與垂直 皆為 0 就 攻擊
    }

    private void Attack()
    {
        if (timer < data.cd)            // 如果 計時器 < 資料.冷卻
        {
            timer += Time.deltaTime;    // 累加時間
        }
        else
        {
            timer = 0;                  // 歸零
            ani.SetTrigger("攻擊觸發"); // 播放攻擊動畫 SetTrigger("參數名稱")
        }
        ani.SetTrigger("攻擊觸發");     // 播放攻擊動畫 SetTrigger("參數名稱")
    }

    public void Hit(float damage)
    {
        data.hp -= damage;                            // 血量 扣除 傷害值
        data.hp = Mathf.Clamp(data.hp, 0, 10000);     // 血量 夾在 0 - 10000
        hpControl.UpdataHpBar(data.hpMax, data.hp);   // 血量控制系統.更新血調(目前血量,最大血量)
        if (data.hp == 0) Dead();                     // 如果 血量 為 0 呼叫死亡方式
        StartCoroutine(hpControl.ShowDamage(damage)); // 血量控制氣.顯示傷害值
    }

    private void Dead()
    {
        if (ani.GetBool("死亡動畫")) return;               // 如果死亡動畫為勾選就跳出
        ani.SetBool("死亡動畫", true);                     // 播放死亡動畫 SetBool("參數名稱", 布林值)
        this.enabled = false;                              // this 此類別 - enabled 是否啟動
        StartCoroutine(levelManager.CountDownRevival());   // 啟動協程
    }

    public void Revival()
    {
        data.hp = data.hpMax;                         // 血量恢復最大值
        hpControl.UpdataHpBar(data.hpMax, data.hp);   // 更新血條
        ani.SetBool("死亡動畫", false);               // 動畫設為沒有死亡
        this.enabled = true;                          // 此腳本.啟動 = 開啟
        levelManager.CloseRevival();                  // 關卡管理器.關閉復活畫面
    } 
    #endregion
}
