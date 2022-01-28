using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHuman : MonoBehaviour
{
    public Vector3 targetPosition;
    public float mySpeed;

    Animator myAnim;
    Vector3 orginalPosition, turnPoint;

    bool isFirstTimeIdle;
    // Start is called before the first frame update
    private void Awake()
    {
        myAnim = GetComponent<Animator>();
        orginalPosition = new Vector3(transform.position.x,transform.position.y,transform.position.z);
        isFirstTimeIdle = true;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x == targetPosition.x)
        {
            myAnim.SetTrigger("Idle");
            turnPoint = orginalPosition;
            StartCoroutine(TurnLeft(true));
            isFirstTimeIdle=false;
        }
        else if (transform.position.x == orginalPosition.x)
        {
            if (!isFirstTimeIdle)
            {
                myAnim.SetTrigger("Idle");
            }
            
            turnPoint = targetPosition;
            StartCoroutine(TurnLeft(false));
        }

        if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            transform.position = Vector3.MoveTowards(transform.position, turnPoint, mySpeed * Time.deltaTime);
        }
        

    }

    IEnumerator TurnLeft(bool turnLeft)
    {
        yield return new WaitForSeconds(1.0f);
        if (turnLeft)
        {
            transform.localScale = new Vector3(-1.0f,1.0f,1.0f); 
        }
        else
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        yield return new WaitForSeconds(1.0f);

    }

    
}
