using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFemaleZombie : EnemyMaleZombie
{
    public float RunSpeed;

    protected override void MoveAndAttack()
    {
        if (isAlive)
        {
            // 攻击
            if (Vector3.Distance(myPlayer.transform.position, transform.position) < 4.0f)
            {
                if (myPlayer.transform.position.x <= transform.position.x)
                {
                    transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                }
                else
                {
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }

                Vector3 newTarget = new Vector3(myPlayer.transform.position.x, transform.position.y, transform.position.z);

                // 不要在静止状态去冲撞
                if(myAnim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                {
                    // 冲击玩家
                    transform.position = Vector3.MoveTowards(transform.position, newTarget, RunSpeed * Time.deltaTime);
                }

                isAfterBattleCheck = true;
                return;
            }
            else
            {
                if (isAfterBattleCheck)
                {
                    // 防止敌人倒退走
                    if(transform.position.x > turnPoint.x || transform.position.x < turnPoint.x)
                    {
                        if(transform.position.x > turnPoint.x)
                        {
                            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                        }
                        else if (transform.position.x < turnPoint.x)
                        {
                            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        }
                        else
                        {
                            // 攻击后转向
                            if (turnPoint == targetPosition)
                            {
                                StartCoroutine(TurnRight(false));
                            }
                            else if (turnPoint == originPosition)
                            {
                                StartCoroutine(TurnRight(true));
                            }
                        }
                    }

                    isAfterBattleCheck = false;
                }
            }

            if (transform.position.x == targetPosition.x)
            {
                myAnim.SetTrigger("Idle");
                turnPoint = originPosition;
                StartCoroutine(TurnRight(true));
                isFirstTimeIdle = false;
            }
            else if (transform.position.x == originPosition.x)
            {
                if (!isFirstTimeIdle)
                {
                    myAnim.SetTrigger("Idle");
                }

                turnPoint = targetPosition;

                StartCoroutine(TurnRight(false));
            }

            if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))  // 当走路动画为真的时候
            {
                transform.position = Vector3.MoveTowards(transform.position, turnPoint, mySpeed * Time.deltaTime); // 移动敌人 
            }
        }
    }
}
