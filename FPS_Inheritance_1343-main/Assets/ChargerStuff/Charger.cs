using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : Gun
{
    public float maxChargeTime = 3f;
    public float chargePowerMultiplier = 50f;
    private float currentChargeTime = 0f;
    private bool isCharging = false;

    public ParticleSystem chargeEffect;

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            isCharging = true;
            currentChargeTime = 0f;
            if (chargeEffect != null) chargeEffect.Play();
        }

        if (Input.GetButton("Fire1") && isCharging)
        {
            currentChargeTime += Time.deltaTime;
            currentChargeTime = Mathf.Min(currentChargeTime, maxChargeTime);
        }

        if (Input.GetButtonUp("Fire1") && isCharging)
        {
            isCharging = false;
            if (chargeEffect != null) chargeEffect.Stop();

            ChargeFire(); // ✅ custom logic goes here
        }
    }

    private void ChargeFire()
    {
        if (!AttemptFire()) return;

        float chargeMultiplier = Mathf.Clamp01(currentChargeTime / maxChargeTime);
        float finalDamage = 75 * chargeMultiplier;
        float finalSpeed = 12 + (15 * chargeMultiplier);
        float knockback = 3 + (2 * chargeMultiplier);

        GameObject b = Instantiate(bulletPrefab, gunBarrelEnd.transform.position, gunBarrelEnd.rotation);
        b.GetComponent<Projectile>().Initialize(finalSpeed, finalDamage, knockback, 25, null);

        if (anim != null)
            anim.SetTrigger("shoot");

        elapsed = 0f;
        ammo -= 1;
    }
}