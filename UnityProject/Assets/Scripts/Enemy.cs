using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public EnemyData data;         // 敵人資料

    protected NavMeshAgent agent;     // 導覽代理器
    protected Transform player;       // 玩家變形
    protected Animator ani;           // 動畫遙控器

    public float timer;            // 計時器

    private void Start()
    {
        // 先取得元件
        ani = GetComponent<Animator>();             
        agent = GetComponent<NavMeshAgent>();
        agent.speed = data.speed;
        player = GameObject.Find("玩家").transform;
    }

    private void Update()
    {
        Move();
    }

    // protected 不允許外部類別存取，允許子類別存許
    protected virtual void Attack()
    {
        timer = 0;                                   // 計時器 歸零
        ani.SetTrigger("攻擊觸發");                  // 攻擊動畫
    }

    // virtual 虛擬 : 讓子類別可以複寫
    protected virtual void Move()
    {
        agent.SetDestination(player.position);  // 代理器.設定目的地(玩家.座標)
        Vector3 posTarget = player.position;    // 區域變數 目標做標 = 玩家.座標
        posTarget.y = transform.position.y;     // 目標做標.y = 本身.y
        transform.LookAt(posTarget);            // 看著(目標做標)

        //print(agent.remainingDistance);

        if (agent.remainingDistance <= data.stopDistance)  //如果 距離 <=
        {
            Wait();
        }
        else
        {
            agent.isStopped = false;                     // 代理器.是否停止 = 否
            ani.SetBool("跑步開關", true);               // 開起跑步開關
        }
    }

    protected virtual void Wait()
    {
        // base.Wait(); // 使用父類別方法內容

        agent.isStopped = true;                       // 代理器.是否停止 = 是
        agent.velocity = Vector3.zero;                // 代理器.加速度 = 零
        ani.SetBool("跑步開關", false);               // 關閉跑步開關

        if (timer <= data.cd)                          // 如果 計時器 <= 冷卻時間
        {
            timer += Time.deltaTime;                  // 時間累加
            //print("時間" + timer);
        }
        else
        {
            Attack();                                // 否則 計時器 > 冷卻時間 攻擊
        }
    }

    private void Hit()
    {

    }

    private void Dead()
    {

    }
}
