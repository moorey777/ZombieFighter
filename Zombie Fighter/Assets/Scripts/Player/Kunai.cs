using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunai : MonoBehaviour
{
    GameObject player;
    Rigidbody2D myRigi;

    public float kunaiSpeed;

    private void Awake()
    {
        player = GameObject.Find("Player");
        myRigi = GetComponent<Rigidbody2D>(); 

        if(player.transform.localScale.x == 1.0f)
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            myRigi.AddForce(Vector2.right * kunaiSpeed, ForceMode2D.Impulse);
        }
        else if(player.transform.localScale.x == -1.0f)
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            myRigi.AddForce(Vector2.left * kunaiSpeed, ForceMode2D.Impulse);
        }

        Destroy(this.gameObject, 5.0f); // 五秒钟后飞镖消失
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*        if(collision.tag == "Enemy")
                {
                    Destroy(this.gameObject); // 消除飞镖
                }

                if(collision.tag == "Ground")
                {
                    Destroy(this.gameObject);
                }*/

        //以下为视频代码，但是有问题
        if (collision == null)
        {
            Destroy(this.gameObject);
        }
    }
}
