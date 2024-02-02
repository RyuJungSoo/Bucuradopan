using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterComponent : MonoBehaviour
{
    // 스탯
    public float speed = 1f;
    public float Hp = 20;
    public float maxHp = 20;
    public float Damage = 2;

    // 변수
    public bool isDead = false;
    public bool isFreeze = false;
    public bool isAttack = false;
    public bool isAttacked = false;
    public bool isSingleton = false;

    // 게암오브젝트 및 컴포넌트
    private GameObject magicCircle;
    private Rigidbody2D monsterRig;
    private Animator animator;


    // Start is called before the first frame update
    void Awake()
    {
        monsterRig = GetComponent<Rigidbody2D>();
        magicCircle = GameObject.Find("magicCircle");
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // magicCircle이 비활성화되어있거나 할당받지 못했을 경우
        if (magicCircle == null || magicCircle.active == false)
            return;

        float distance = Vector2.Distance(transform.position, magicCircle.transform.position);
        if (isDead == false && isFreeze == false)
        {
            Move(distance);
        }
        else
            animator.SetBool("isWalk", false);

    }

    private void Object_OFF()
    {
        //Destroy(this.gameObject);
        gameObject.SetActive(false);
    }

    public void Object_ON()
    {
        gameObject.SetActive(true);
        gameObject.GetComponent<Collider2D>().enabled = true;
        Hp = maxHp;
        isDead = false;
        isAttacked = false;
        isFreeze = false;
    }

    private void Move(float distance)
    {

        if(distance > 0)
            animator.SetBool("isWalk", true);

        // 스프라이트 정면 바꾸기
        float Abs_x = Mathf.Abs(transform.localScale.x);
        float Abs_y = Mathf.Abs(transform.localScale.y);

        if (Mathf.Abs(transform.position.x - magicCircle.transform.position.x) >= 0.05) // 몬스터와 magicCircle의 거리 차가 클 때만 바꾸기
        {
            
            if (transform.position.x < magicCircle.transform.position.x)
                transform.localScale = new Vector2(1 * Abs_x, 1 * Abs_y);
            else if (transform.position.x > magicCircle.transform.position.x)
                transform.localScale = new Vector2(-1 * Abs_x, 1 * Abs_y);
        }

        // 이동
        Vector2 dirVec = magicCircle.GetComponent<Rigidbody2D>().position - monsterRig.position; // Rigidbody2D 중심 기준으로 방향 계산
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        monsterRig.MovePosition(monsterRig.position + nextVec);
        
    }

    public void TakeDamage(float damage)
    {
        isAttacked = true;
        Hp -= damage;
        if (Hp <= 0)
        {
            monsterRig.velocity = new Vector3(0, 0); // 관성 지우기
            isDead = true;
            GetComponent<Collider2D>().enabled = false;
            Invoke("Object_OFF", 0.1f);
                

        }
        isAttacked = false;

    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("magicCircle") && isFreeze == false)
        {
            collision.gameObject.GetComponent<magicCircleComponent>().TakeDamage(Time.deltaTime * Damage);
            UiManager.instance.HpBarUpdate();
        }
    }
}
