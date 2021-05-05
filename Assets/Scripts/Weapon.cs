using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type { Range };
    public Type type;
    public int damage;
    public float rate; //∞¯∞›º”µµ
    public int maxAmmo; //√÷¥Î √—æÀ
    public int curAmmo; //«ˆ¿Á √—æÀ


    public TrailRenderer trailEffect;
    public Transform bulletPos;
    public GameObject bullet;

    public Transform bulletCasePos;
    public GameObject bulletCase;

    public void Use()
    {
        if (type == Type.Range && curAmmo>0)
        {
            curAmmo--;
            StartCoroutine("Shot");
        }
    }




    IEnumerator Shot()
    {   
        //√—æÀ πﬂªÁ
        GameObject instantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 50;
        yield return null;

        //≈∫«« πË√‚
        GameObject instantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        Rigidbody caseRigid = instantCase.GetComponent<Rigidbody>();

        Vector3 caseVec = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);

        caseRigid.AddForce(caseVec, ForceMode.Impulse);
        caseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);
        

    }

}

