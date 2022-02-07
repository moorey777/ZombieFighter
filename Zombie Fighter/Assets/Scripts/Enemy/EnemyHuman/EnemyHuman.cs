using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHuman : EnemyMaleZombie
{

    
    private void Awake()
    {
        myAnim = GetComponent<Animator>();
        myCollider = GetComponent<BoxCollider2D>();
        mySward=transform.GetChild(0).GetComponent<BoxCollider2D>();
        mySr = GetComponent<SpriteRenderer>();
        myPlayer = GameObject.Find("Player");
        myAudioSource = GetComponent<AudioSource>();
        originPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        isFirstTimeIdle = true;
        isAfterBattleCheck = false;
        isAlive = true;
        isdieWalkLeft = true;
        canBeHurt = true;
        isFirstZombieWalk = true;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MoveAndAttack();
    }

    protected override void MoveAndAttack()
    {
        if (isAlive && canBeHurt)
        {
            // 攻击
            if (Vector3.Distance(myPlayer.transform.position, transform.position) < 1.3f)
            {
                if (myPlayer.transform.position.x <= transform.position.x)
                {
                    transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                }
                else
                {
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }

                if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack") || myAnim.GetCurrentAnimatorStateInfo(0).IsName("AttackWait"))
                {
                    return;
                }
                
                if(!myAudioSource.isPlaying) myAudioSource.PlayOneShot(myAudioClip[1]);
               
                myAnim.SetTrigger("Attack");
         
                isAfterBattleCheck = true;
                return;
            }
            else
            {
                if (isAfterBattleCheck)
                {
                    // 攻击后转向
                    if (turnPoint == targetPosition)
                    {
                        StartCoroutine(TurnRight(true));
                    }
                    else if (turnPoint == originPosition)
                    {
                        StartCoroutine(TurnRight(false));
                    }

                    isAfterBattleCheck = false;
                }
            }

            if (transform.position.x == targetPosition.x)
            {
                myAnim.SetTrigger("Idle");
                turnPoint = originPosition;
                StartCoroutine(TurnRight(false));
                isFirstTimeIdle = false;
            }
            else if (transform.position.x == originPosition.x)
            {
                if (!isFirstTimeIdle)
                {
                    myAnim.SetTrigger("Idle");
                }

                turnPoint = targetPosition;

                StartCoroutine(TurnRight(true));
            }

            if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))  // 当走路动画为真的时候
            {
                transform.position = Vector3.MoveTowards(transform.position, turnPoint, mySpeed * Time.deltaTime); // 移动敌人 
            }
        }
        else
        {
            if (isFirstZombieWalk)
            {
                
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                isFirstZombieWalk = false;
            }
            if (transform.position.x == dieLeftPosition.x)
            {
                myAnim.SetTrigger("AfterDieIdle");

                turnPoint = dieRightPosition;
                StartCoroutine(TurnRight(true));

            }
            else if (transform.position.x == dieRightPosition.x)
            {
                myAnim.SetTrigger("AfterDieIdle");

                turnPoint = dieLeftPosition;
                StartCoroutine(TurnRight(false));

            }

            if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("ZombieWalk"))  // 当僵尸走路动画为true时
            {
                transform.position = Vector3.MoveTowards(transform.position, turnPoint, mySpeed * Time.deltaTime); // 移动敌人 
            }

        }
    }


    




    private void OnTriggerEnter2D(Collider2D collision)
    {
      
        if (collision.tag == "PlayerAttack" && canBeHurt == true)
        {
            myAudioSource.PlayOneShot(myAudioClip[0]);
            canBeHurt = false;
            enemyLife--;
            if (enemyLife >= 1)
            {
                myAnim.SetTrigger("Hurt");
                StartCoroutine("SetIsHurtFalse");

            }
            else if (enemyLife < 1)
            {
                
                isAlive = false;
                myCollider.enabled = false;
                mySward.enabled = false;
                myAnim.SetTrigger("Die");
                diePosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                dieLeftPosition = new Vector3(Mathf.Max(transform.position.x - 3, -9.5f), transform.position.y, transform.position.z);
                dieRightPosition = new Vector3(Mathf.Min(transform.position.x + 3, 8.0f), transform.position.y, transform.position.z);
                turnPoint = dieRightPosition;
                StartCoroutine("AfterDie");
            }
        }
    }
    IEnumerator SetIsHurtFalse()
    {
        yield return new WaitForSeconds(2.0f);
        canBeHurt = true;



    }

    IEnumerator AfterDie()
    {
        yield return new WaitForSeconds(1.0f);
        mySr.material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        yield return new WaitForSeconds(1.0f);
        mySr.material.color = new Color(1.0f, 1.0f, 1.0f, 0.2f);
        yield return new WaitForSeconds(1.0f);
        PlayZombieBornEffect();
        myAnim.SetTrigger("AfterDie");
        mySr.material.color = new Color(1.0f, 1.0f, 1.0f, 1f);

        yield return new WaitForSeconds(2.0f);

    }

    public void PlayEnemyDeathEffect()
    {
        myAudioSource.PlayOneShot(myAudioClip[2]);
    }

    public void PlayZombieBornEffect()
    {
        myAudioSource.PlayOneShot(myAudioClip[3]);
    }

}
