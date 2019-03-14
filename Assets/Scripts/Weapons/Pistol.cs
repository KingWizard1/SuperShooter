using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperShooter;

public class Pistol : Weapon
{
    #region Old
    //public GameObject bullet;
    //public Transform spawnPoint;


    //void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        GameObject clone = Instantiate(bullet, spawnPoint.position, spawnPoint.rotation);
    //        Bullet newBullet = clone.GetComponent<Bullet>();

    //        newBullet.Fire(transform.forward);
    //    }
    //}
    #endregion
    public float spread;
    public int magSize;

    public override void Attack()
    {
        GameObject clone = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);
        Bullet newBullet = clone.GetComponent<Bullet>();

        newBullet.Fire(transform.forward);
    }

}
