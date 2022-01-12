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
            // ����
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

                // ��Ҫ�ھ�ֹ״̬ȥ��ײ
                if(myAnim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                {
                    // ������
                    transform.position = Vector3.MoveTowards(transform.position, newTarget, RunSpeed * Time.deltaTime);
                }

                isAfterBattleCheck = true;
                return;
            }
            else
            {
                if (isAfterBattleCheck)
                {
                    // ��ֹ���˵�����
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
                            // ������ת��
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

            if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))  // ����·����Ϊ���ʱ��
            {
                transform.position = Vector3.MoveTowards(transform.position, turnPoint, mySpeed * Time.deltaTime); // �ƶ����� 
            }
        }
    }
}
