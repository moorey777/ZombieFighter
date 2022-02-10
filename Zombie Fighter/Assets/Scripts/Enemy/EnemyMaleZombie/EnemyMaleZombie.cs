using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMaleZombie : MonoBehaviour
{
    public Vector3 targetPosition;
    public float mySpeed;
    public GameObject attackCollider;
    public int enemyLife;
    public Rigidbody2D myRigi;

    public Canvas myCanvas;

    protected Animator myAnim;
    protected Vector3 originPosition, turnPoint;
    protected Vector3 diePosition, dieLeftPosition, dieRightPosition;
    protected bool isFirstTimeIdle, isAfterBattleCheck, isAlive, isdieWalkLeft, canBeHurt, isFirstZombieWalk;


    protected GameObject myPlayer;

    protected BoxCollider2D myCollider, mySward;
    protected SpriteRenderer mySr;
    [SerializeField]
    protected AudioClip[] myAudioClip;
    protected AudioSource myAudioSource;

    private void Awake()
    {
        myAnim = GetComponent<Animator>();
        myCollider = GetComponent<BoxCollider2D>();
        mySward = transform.GetChild(0).GetComponent<BoxCollider2D>();
        mySr = GetComponent<SpriteRenderer>();
        myPlayer = GameObject.Find("Player");
        myAudioSource = GetComponent<AudioSource>();
        
        originPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        
        isFirstTimeIdle = true;
        isAfterBattleCheck = false;
        isAlive = true;
        isdieWalkLeft = true;
        canBeHurt=true;
        isFirstZombieWalk=true;
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

    protected virtual void MoveAndAttack()
    {
        if (isAlive && canBeHurt)
        {
            // ����
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

                if (!myAudioSource.isPlaying) myAudioSource.PlayOneShot(myAudioClip[1]);
                myAnim.SetTrigger("Attack");
                isAfterBattleCheck = true;
                return;
            }
            else
            {
                if (isAfterBattleCheck)
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
                // transform.position = Vector3.MoveTowards(transform.position, turnPoint, mySpeed * Time.deltaTime); // �ƶ����� 
                transform.position = Vector3.MoveTowards(transform.position, turnPoint, mySpeed * Time.deltaTime);
            }
        }
        else
        {
            if (isFirstZombieWalk)
            {
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
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

            if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("ZombieWalk"))  // ����ʬ��·����Ϊtrueʱ
            {
                transform.position = Vector3.MoveTowards(transform.position, turnPoint, mySpeed * Time.deltaTime); // �ƶ����� 
            }
            
        }
    }


    protected IEnumerator TurnRight(bool turnRight)
    {
        yield return new WaitForSeconds(2.0f);
        if(turnRight)
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        else
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
    }


    public void SetAttackColliderOn()
    {
        attackCollider.SetActive(true);
    }

    public void SetAttackColliderOff()
    {
        attackCollider.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerAttack" && canBeHurt == true)
        {
            myAudioSource.PlayOneShot(myAudioClip[0]);
            canBeHurt = false;
            enemyLife--;
            if(enemyLife >= 1)
            {
                myAnim.SetTrigger("Hurt");
                StartCoroutine("SetIsHurtFalse");

            }
            else if(enemyLife < 1)
            {
                isAlive = false;
                myCollider.enabled = false;
                mySward.enabled = false;
                myAnim.SetTrigger("Die");
                diePosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                dieLeftPosition = new Vector3(Mathf.Max(transform.position.x-3,-9.5f), transform.position.y, transform.position.z);
                dieRightPosition = new Vector3(Mathf.Min(transform.position.x + 3,8.0f), transform.position.y, transform.position.z);
                turnPoint = dieLeftPosition;
                StartCoroutine("AfterDie");
            }
        }
    }
    IEnumerator SetIsHurtFalse()
    {
        yield return new WaitForSeconds(2.0f);
        canBeHurt = true;
        

       
    }

    IEnumerator AfterGone()
    {
        yield return new WaitForSeconds(1.0f);
        mySr.material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        yield return new WaitForSeconds(1.0f);
        mySr.material.color = new Color(1.0f, 1.0f, 1.0f, 0.2f);
        yield return new WaitForSeconds(1.0f);
        // myAnim.SetTrigger("Gone");
        mySr.material.color = new Color(1.0f, 1.0f, 1.0f, 1f);
        yield return new WaitForSeconds(2.0f);
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
