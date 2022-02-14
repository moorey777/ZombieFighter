using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPumpkinMan : MonoBehaviour
{
    bool isAlive, isIdle, jumpAttack, isJumpUp, slideAttack, isHurt, canBeHurt;

    public int life;
    public float attackDistance, jumpHeight, jumpUpSpeed, jumpDownSpeed, slideSpeed, fallDownSpeed;


    GameObject player;
    Animator myAnim;
    Vector3 slideTargetPosition;
    BoxCollider2D myCollider;
    SpriteRenderer mySr;
    AudioSource myAudioSource;
    // Start is called before the first frame update
    private void Awake()
    {
        player = GameObject.Find("Player");
        myAnim = GetComponent<Animator>();
        myCollider = GetComponent<BoxCollider2D>();
        mySr = GetComponent<SpriteRenderer>();
        myAudioSource = GetComponent<AudioSource>();

        isAlive = true;
        isIdle = true;
        jumpAttack = false;
        isJumpUp = true;
        slideAttack = false;
        isHurt = false;
        canBeHurt = true;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isAlive)
        {
            if (isIdle)
            {
                LookAtPlayer();
                if(Vector3.Distance(player.transform.position, transform.position) <= attackDistance) 
                {
                    // slideAttack
                    isIdle = false;
                    StartCoroutine("IdleToSlideAttack");

                }
                else
                {
                    // jumpAttack
                    isIdle = false;
                    StartCoroutine("IdleToJumpAttack");
                }
            }
            else if(jumpAttack)             // 往上跳后往下攻击
            {
                LookAtPlayer();
                if (isJumpUp)
                {
                    Vector3 myTarget = new Vector3(player.transform.position.x, jumpHeight, transform.position.z);
                    transform.position = Vector3.MoveTowards(transform.position, myTarget, jumpUpSpeed * Time.deltaTime);
                    myAnim.SetBool("JumpUp", true);

                }
                else
                {
                    myAnim.SetBool("JumpUp", false);
                    myAnim.SetBool("JumpDown", true);

                    Vector3 myTarget = new Vector3(transform.position.x, -2.85f, player.transform.position.z);
                    transform.position = Vector3.MoveTowards(transform.position, myTarget, jumpDownSpeed * Time.deltaTime);
                }


                if(transform.position.y == jumpHeight)
                {
                    isJumpUp = false;
                }
                else if (transform.position.y == -2.85f)
                {
                    jumpAttack = false;
                    StartCoroutine("JumpDownToIdle");
                }
            }
            else if(slideAttack)
            {
                myAnim.SetBool("Slide", true);
                transform.position = Vector3.MoveTowards(transform.position, slideTargetPosition, slideSpeed * Time.deltaTime);
            
                if(transform.position == slideTargetPosition)
                {
                    // 改回去Collider原始大小
                    myCollider.offset = new Vector2(-0.2075321f, -0.128813f);
                    myCollider.size = new Vector2(1.200059f, 2.028081f);
                    myAnim.SetBool("Slide", false);
                    slideAttack = false;
                    isIdle = true;
                }
            
            }
            else if(isHurt)
            {
                Vector3 myTargetPosition = new Vector3(transform.position.x, -2.85f, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, myTargetPosition, fallDownSpeed * Time.deltaTime);
            }
        }
        else
        {
            Vector3 myTargetPosition = new Vector3(transform.position.x, -2.85f, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, myTargetPosition, fallDownSpeed * Time.deltaTime);
        }
    }

    void LookAtPlayer()
    {
        // 控制boss转向
        if (player.transform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(1f, 1.0f, 1.0f);
        }
        else
        {
            transform.localScale = new Vector3(-1f, 1.0f, 1.0f);
        }
    }

    IEnumerator IdleToSlideAttack()
    {
        yield return new WaitForSeconds(1.0f);
        // 在SlideAttack的时候 动态修改Collider大小
        myCollider.offset = new Vector2(-0.2075321f, -0.3766169f);
        myCollider.size = new Vector2(1.200059f, 1.532473f);
        slideTargetPosition = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
        LookAtPlayer();
        slideAttack = true;
    }

    IEnumerator IdleToJumpAttack()
    {
        yield return new WaitForSeconds(1.0f);
        jumpAttack = true;
    }

    IEnumerator JumpDownToIdle()
    {
        yield return new WaitForSeconds(0.5f);
        isIdle = true;
        isJumpUp = true;
        myAnim.SetBool("JumpUp", false);
        myAnim.SetBool("JumpDown", false);
    }

    IEnumerator SetAnimHurtToFalse()
    {
        yield return new WaitForSeconds(0.5f);
        myAnim.SetBool("Hurt", false);
        myAnim.SetBool("JumpUp", false);
        myAnim.SetBool("JumpDown", false);
        myAnim.SetBool("Slide", false);
        mySr.material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        isHurt = false;
        isIdle = true;

        // 受伤后两秒无敌 
        yield return new WaitForSeconds(2.0f);
        canBeHurt = true;
        mySr.material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerAttack")
        {
            if (canBeHurt)
            {
                myAudioSource.PlayOneShot(myAudioSource.clip);
                life--;
                if (life >= 1)
                {
                    isIdle = false;
                    jumpAttack = false;
                    slideAttack = false;
                    isHurt = true;

                    StopCoroutine("JumpDownToIdle");
                    StopCoroutine("IdleToJumpAttack");
                    StopCoroutine("IdleToSlideAttack");
                    myAnim.SetBool("Hurt", true);

                    StartCoroutine("SetAnimHurtToFalse");
                }
                else
                {
                    isAlive = false;
                    myCollider.enabled = false;
                    StopAllCoroutines();
                    myAnim.SetBool("Die", true);
                }

                canBeHurt = false;
            }
        }
    }
}
