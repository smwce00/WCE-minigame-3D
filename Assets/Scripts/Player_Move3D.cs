using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move3D : MonoBehaviour
{
    public float speed; // 직접 입력받는 스피드 변수

    public GameObject[] weapons;
    public bool[] hasWeapons;

    public int ammo; //탄수
    public int health; //체력
    public int coin; //동전

    public int maxAmmo;
    public int maxHealth;
    public int maxCoin;

    float hAxis;
    float vAxis;
    bool wDown; //걷기
    bool jDown; //점프
    bool idown; //아이템 집기
    bool fDown; //공격
    bool rDown; //원거리 무기 장전 

    bool sDown1;    //아이템 장비
   //bool sDown2;   //무기 추가하면 추가할 코드
   //bool sDown3;

    bool isJump; //무한점프 불가능하게 할 변수
    bool isSwap; //무기 교체
    bool isFireReady; //공격 준비
    bool isReload;

    Vector3 moveVec;
    Rigidbody rigid;
    Animator anim;

    GameObject nearObject;
    Weapon equipWeapon;
    int equipWeaponIndex=-1;
    float fireDelay;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }
    
    void Update()
    {
        GetInput();
        Move();
        Turn();
        Jump();
        Interaction();
        Swap();
        Attack();
        Reload();
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");            //shift 누른 상태=걷기
        jDown = Input.GetButtonDown("Jump");        //space바 한 번=점프
        idown = Input.GetButtonDown("Interaction"); //e키 누르면 아이템 입수
        sDown1= Input.GetButtonDown("Swap1"); //숫자 버튼으로 아이템 장비
        //sDown2~
        //
        fDown = Input.GetButton("Fire1");
        rDown = Input.GetButtonDown("Reload"); //재장전
        
    }


    void Move() //상하좌우 움직임 코드
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime; //걸으면 속도 느려짐

        if (isSwap)
            moveVec = Vector3.zero; //무기 교체시 움직임 x

        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);
    }


    void Turn() //회전 가능 코드
    {
        transform.LookAt(transform.position + moveVec);
    }

    void Jump() //점프 코드
    {
        if (jDown &&moveVec == Vector3.zero && !isJump && !isSwap)   //무한대 점프 불가
        {
            rigid.AddForce(Vector3.up * 20, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");

            isJump = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            anim.SetBool("isJump", false);
            isJump = false; //바닥에 닿으면 다시 점프 가능
        }
    }

    void Swap() //무기 교체
    {

        if (sDown1 && (!hasWeapons[0] || equipWeaponIndex == 0))
            return;

        int weaponIndex = -1;
        if (sDown1)
            weaponIndex = 0;
        //if (sDown2) weaponIndex = 1;
        //if (sDown3) weaponIndex = 2;

        if (sDown1 && !isJump)  //if((sDown1 || sDown2 || sDown3))&& !isJump)
        {
            if (equipWeapon != null)
                equipWeapon.gameObject.SetActive(false);

            
            equipWeaponIndex = weaponIndex;
            equipWeapon = weapons[weaponIndex].GetComponent<Weapon>();
            equipWeapon.gameObject.SetActive(true);

            anim.SetTrigger("doSwap");

            isSwap = true;

            Invoke("SwapOut", 0.4f);
        }
    }

    void SwapOut()
    {   
        isSwap=false;
    }

    void Interaction()
    {
        if(idown && nearObject !=null && !isJump){
            if (nearObject.tag == "Weapon")
            {
                Item item = nearObject.GetComponent<Item>();
                int weaponIndex = item.value;
                hasWeapons[weaponIndex] = true;

                Destroy(nearObject);
            }
        }
    }

    //무기 입수
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Weapon")
            nearObject = other.gameObject;

        Debug.Log(nearObject.name); 
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon")
            nearObject = null; 
    }

    //아이템 먹기
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();
            switch (item.type)
            {
                case Item.Type.Ammo:
                    ammo += item.value;
                    if (ammo > maxAmmo)
                        ammo = maxAmmo;
                    break;

                case Item.Type.Coin:
                    coin += item.value;
                    if (coin > maxCoin)
                        coin = maxCoin;
                    break;

                case Item.Type.Heart:
                    health += item.value;
                    if (health > maxHealth)
                        health = maxHealth;
                    break;
            }
  
            Destroy(other.gameObject);

        } 
    }

    //공격
    void Attack()
    {
        if (equipWeapon == null) return;

        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;

        if(fDown && isFireReady && !isSwap)
        {
            equipWeapon.Use();
            anim.SetTrigger("doShot");
            fireDelay = 0;
        }

    }

    //재장전
    void Reload()
    {
        if (equipWeapon == null) return;  //무기가 없을 때
        if (ammo==0) return;                //총알이 없을 때

        if (rDown && !isSwap && !isJump && isFireReady)
        {
            anim.SetTrigger("doReload");
            isReload = true;
            Invoke("ReloadOut", 3f);
        }
    }

    void ReloadOut()
    {   int reAmmo = ammo < equipWeapon.maxAmmo ? ammo : equipWeapon.maxAmmo;
        equipWeapon.curAmmo = reAmmo;
        ammo -= reAmmo;
        isReload = false;
    }
}
