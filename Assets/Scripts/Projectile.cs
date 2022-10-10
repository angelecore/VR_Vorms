using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField]
    private GameObject explosionPrefab;
    public float explForce = 500;
    public float explRadius = 8;

    [Min(0f)]
    [SerializeField]
    private float velocity = 10f;

    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rb.velocity = transform.forward * velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
        CreateExplosionKnockback();
        CreateExplosionEffect();
    }

    private void CreateExplosionEffect()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    }

    private void CreateExplosionKnockback()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explRadius);

        foreach(Collider affectedObj in colliders)
        {
            Rigidbody rigg = affectedObj.GetComponent<Rigidbody>();
            if(rigg != null)
            {
                rigg.AddExplosionForce(explForce, transform.position, explRadius);
            }
        }
    }
}
