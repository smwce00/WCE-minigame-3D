using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move3D : MonoBehaviour
{
    public float speed; // ���� �Է¹޴� ���ǵ� ����

    public GameObject[] weapons;
    public bool[] hasWeapons;

    public int ammo; //ź��
    public int health; //ü��
    public int coin; //����

    public int maxAmmo;
    public int maxHealth;
    public int maxCoin;

    float hAxis;
    float vAxis;
    bool wDown; //�ȱ�
    bool jDown; //����
    bool idown; //������ ����
    bool fDown; //����
    bool rDown; //���Ÿ� ���� ���� 

    bool sDown1;    //������ ���
   //bool sDown2;   //���� �߰��ϸ� �߰��� �ڵ�
   //bool sDown3;

    bool isJump; //�������� �Ұ����ϰ� �� ����
    bool isSwap; //���� ��ü
    bool isFireReady; //���� �غ�
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
        wDown = Input.GetButton("Walk");            //shift ���� ����=�ȱ�
        jDown = Input.GetButtonDown("Jump");        //space�� �� ��=����
        idown = Input.GetButtonDown("Interaction"); //eŰ ������ ������ �Լ�
        sDown1= Input.GetButtonDown("Swap1"); //���� ��ư���� ������ ���
        //sDown2~
        //
        fDown = Input.GetButton("Fire1");
        rDown = Input.GetButtonDown("Reload"); //������
        
    }


    void Move() //�����¿� ������ �ڵ�
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime; //������ �ӵ� ������

        if (isSwap)
            moveVec = Vector3.zero; //���� ��ü�� ������ x

        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);
    }


    void Turn() //ȸ�� ���� �ڵ�
    {
        transform.LookAt(transform.position + moveVec);
    }

    void Jump() //���� �ڵ�
    {
        if (jDown &&moveVec == Vector3.zero && !isJump && !isSwap)   //���Ѵ� ���� �Ұ�
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
            isJump = false; //�ٴڿ� ������ �ٽ� ���� ����
        }
    }

    void Swap() //���� ��ü
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

    //���� �Լ�
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

    //������ �Ա�
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

    //����
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

    //������
    void Reload()
    {
        if (equipWeapon == null) return;  //���Ⱑ ���� ��
        if (ammo==0) return;                //�Ѿ��� ���� ��

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
