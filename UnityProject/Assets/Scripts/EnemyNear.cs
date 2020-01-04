using UnityEngine;
using System.Collections;

public class EnemyNear : Enemy
{
    public override void Move()
    {
        
        agent.SetDestination(player.position);  // 代理器.設定目的地(玩家.座標)
        

        print(agent.remainingDistance);

        if(agent.remainingDistance <= data.stopDistance)  //如果 距離 <=
        {
            Wait();
        }
        else
        {
            agent.isStopped = false;                     // 代理器.是否停止 = 否
            ani.SetBool("跑步開關", true);               // 開起跑步開關
        }
    }

    // overrice : 複寫父類別有 virtual 的方法
    public override void Wait()
    {
        // base.Wait(); // 使用父類別方法內容

        agent.isStopped = true;                       // 代理器.是否停止 = 是
        agent.velocity = Vector3.zero;                // 代理器.加速度 = 零
        ani.SetBool("跑步開關", false);               // 關閉跑步開關

        if(timer <= data.cd)                          // 如果 計時器 <= 冷卻時間
        {
            timer += Time.deltaTime;                  // 時間累加
            //print("時間" + timer);
        }
        else
        {
            Attack();                                // 否則 計時器 > 冷卻時間 攻擊
        }
    }

    public override void Attack()
    {
        timer = 0;                                   // 計時器 歸零
        ani.SetTrigger("攻擊觸發");                  // 攻擊動畫
        StartCoroutine(DelayAttack());               // 啟動協程
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(data.attackDelay);

        RaycastHit hit;                             // 射線碰撞資訊 - 存放射線碰到的內容

        // out 存放參數資訊
        // 物理.射線碰撞(中心點，方向，長度)
        if(Physics.Raycast(transform.position + new Vector3(0, 1, 0), transform.forward,out hit, data.attackRange))
        {
            print("打到東西!" + hit.collider.gameObject);
            // 取得玩家元件.受傷方法(怪物.攻擊力)
        }
    }

    // Ctrl + M O 摺疊
    // Ctrl + M L 展開

    // 事件 : 繪製圖示
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;      // 圖示.顏色 = 顏色

        // 前方 : transform.forward
        // 右方 : transform.right
        // 上方 : transform.up 

        // 圖示.繪製設限(中心點，方向)
        Gizmos.DrawRay(transform.position + new Vector3(0, 1, 0), transform.forward * data.attackRange);
    }
}
