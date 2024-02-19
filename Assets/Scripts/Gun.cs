using UnityEngine;

public class Gun : MonoBehaviour
{
    
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;

    public Camera fpsCam;
    public GameObject MuzzelFlash;
    public GameObject impactEffect;

    private float nextTimeToFire = 0f;

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            MuzzelFlash.SetActive(true);
            Shoot();
        }
    }

    void Shoot()
    {
        Invoke("MuzzelFlashOff", 0.1f);
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 1f);
        }
    }

    void MuzzelFlashOff()
    {
        MuzzelFlash.SetActive(false);
    }
}
