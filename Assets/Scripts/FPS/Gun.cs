using UnityEngine;

public class Gun : MonoBehaviour
{
    
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;

    public Camera fpsCam;
    public ParticleSystem MuzzleFlash;
    public ParticleSystem PlayerMuzzleFlash;
    public GameObject BulletImpactEffect;

    private float nextTimeToFire = 0f;

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        MuzzleFlash.Play();
        PlayerMuzzleFlash.Play();
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            GameObject impactGO = Instantiate(BulletImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 1f);
        }
    }
}
