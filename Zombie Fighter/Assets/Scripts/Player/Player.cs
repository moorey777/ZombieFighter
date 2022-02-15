using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class Player : MonoBehaviour
{
    // ��ɫ�ƶ��ٶ�
    public float mySpeed;
    public float jumpForce;
    public GameObject attackCollider, kunaiPrefab;
    public System.DateTime gameStartTime;


    float kunaiDistance;
    int playerLife;
    public Canvas myCanvas;
    [HideInInspector]
    public Animator myAnim;
    public Rigidbody2D myRigi;
    SpriteRenderer mySr;

    [HideInInspector]
    public bool isJumpPressed, canJump, isAttack, isHurt, canBeHurt;


    public AudioClip[] myAudioClip;
    AudioSource myAudioSource;


    // Start is called before the first frame update
    // Start֮���һ��ִ�еĺ���
    private void Awake()
    {
        gameStartTime = System.DateTime.Now;
        myAnim = GetComponent<Animator>();
        myRigi = GetComponent<Rigidbody2D>();
        mySr = GetComponent<SpriteRenderer>();
        myAudioSource = GetComponent<AudioSource>();
myCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        isJumpPressed = false;
        canJump = true;
        isAttack = false;
        isHurt = false;
        canBeHurt = true;

        playerLife = 10;
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && canJump == true && isHurt == false)
        {
            isJumpPressed =true;
            canJump = false;
        }

        if(Input.GetKeyDown(KeyCode.T) && isHurt == false)
        {
            myAnim.SetTrigger("Attack");
            isAttack = true;
            canJump = false;
        }

        // if(Input.GetKeyDown(KeyCode.G) && isHurt == false)
        // {
        //     myAnim.SetTrigger("AttackThrow");
        //     isAttack = true;
        //     canJump = false;
            
        // }
    }

    private void FixedUpdate()
    {
        // ������ش����������������Ҽ��ش�����������
        float a = Input.GetAxisRaw("Horizontal");

        if(isAttack || isHurt)
        {
            a = 0;
        }

        if (a > 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (a < 0)
        {
            // ���������ת��
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        myAnim.SetFloat("Run", Mathf.Abs(a));

        if(isJumpPressed)
        {
            myRigi.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isJumpPressed = false;

            myAnim.SetBool("Jump", true);
        }

        if(!isHurt)
        {
            myRigi.velocity = new Vector2(a * mySpeed, myRigi.velocity.y);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy" && isHurt == false && canBeHurt == true)
        {
            myAudioSource.PlayOneShot(myAudioClip[0]);
            playerLife--;
            if(playerLife >= 1)
            {
                StartCoroutine("StartTrack");
                isHurt = true;
                canBeHurt = false;
                mySr.color = new Color(mySr.color.r, mySr.color.g, mySr.color.b, 0.5f); // ����ʱ�ı�͸����
                myAnim.SetBool("Hurt", true);


                if (transform.localScale.x == 1.0f)
                {
                    myRigi.velocity = new Vector2(-2.5f, 10.0f);
                }
                else if (transform.localScale.x == -1.0f)
                {
                    myRigi.velocity = new Vector2(2.5f, 10.0f);
                }

                
                StartCoroutine("SetIsHurtFalse");
            }
            else if(playerLife < 1)
            {
                isHurt = true;
                isAttack = true;
                myRigi.velocity = new Vector2(0f, 0f);
                myAnim.SetBool("Die", true);
            }
        }


        if(collision.tag == "Item")
        {
            myAudioSource.PlayOneShot(myAudioClip[1]);
            Destroy(collision.gameObject);
        }
    }


    IEnumerator SetIsHurtFalse()
    {
        yield return new WaitForSeconds(1.0f);
        isHurt = false;
        myAnim.SetBool("Hurt", false);

        // �޵�״̬
        yield return new WaitForSeconds(1.0f);
        canBeHurt = true;
        mySr.color = new Color(mySr.color.r, mySr.color.g, mySr.color.b, 1.0f); // ����ʱ�ı�͸����
    }

    public void PlayScratchEffect()
    {
        myAudioSource.PlayOneShot(myAudioClip[3]);
    }

    public void PlayKunaiEffect()
    {
        myAudioSource.PlayOneShot(myAudioClip[2]);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // ��OnTriggerEnter2D һ����Ϊ�˷�ֹ���һֱվ�ڹ������治�յ��˺�
        if (collision.tag == "Enemy" && isHurt == false && canBeHurt == true)
        {
            myAudioSource.PlayOneShot(myAudioClip[0]);
            playerLife--;
            if (playerLife >= 1)
            {
                StartCoroutine("StartTrack");
                isHurt = true;
                canBeHurt = false;
                mySr.color = new Color(mySr.color.r, mySr.color.g, mySr.color.b, 0.5f); // ����ʱ�ı�͸����
                myAnim.SetBool("Hurt", true);


                if (transform.localScale.x == 1.0f)
                {
                    myRigi.velocity = new Vector2(-2.5f, 10.0f);
                }
                else if (transform.localScale.x == -1.0f)
                {
                    myRigi.velocity = new Vector2(2.5f, 10.0f);
                }


                StartCoroutine("SetIsHurtFalse");
            }
            else if (playerLife < 1)
            {
                isHurt = true;
                isAttack = true;
                myRigi.velocity = new Vector2(0f, 0f);
                myAnim.SetBool("Die", true);
            }
        }
    }

    // ���˵ĵ�һ��Frame���ú���
    public void SetIsAttackFalse()
    {
        isAttack = false;
        canJump = true;
        myAnim.ResetTrigger("Attack");
        myAnim.ResetTrigger("AttackThrow");
    }

    public void ForIsHurtSetting()
    {
        isAttack = false;
        myAnim.ResetTrigger("Attack");
        myAnim.ResetTrigger("AttackThrow");
        attackCollider.SetActive(false);
    }

    public void StartTrack() {
                System.TimeSpan duration = System.DateTime.Now - gameStartTime;
                // myCanvas.debug(System.Convert.ToInt64(duration.TotalSeconds).ToString());
                AnalyticsResult analyticsResult = Analytics.CustomEvent(
                    "Death",
                    new Dictionary<string, object>
                    {
                        { "Time", System.Convert.ToInt64(duration.TotalSeconds)}
                    }
                );
    }


    public void SetAttackColliderOn()
    {
        attackCollider.SetActive(true);
    }

    public void SetAttackColliderOff()
    {
        attackCollider.SetActive(false);
    }


    public void kunaiInstantiate()
    {
        if (transform.localScale.x == 1)
        {
            kunaiDistance = 1.0f;
        }
        else if (transform.localScale.x == -1.0f)
        {
            kunaiDistance = -1.0f;
        }

        Vector3 temp = new Vector3(transform.position.x + kunaiDistance, transform.position.y, transform.position.z);
        Instantiate(kunaiPrefab, temp, Quaternion.identity);
    }
}
