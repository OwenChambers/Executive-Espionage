using EnemyAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    LineRenderer lineRenderer;
    Animator animator;
    AudioSource audioSource;
    Camera cam;
    Transform gunPoint;

    [SerializeField] float fireRate;
    [SerializeField] float fireRange;
    [SerializeField] GameObject hitEffect;
    float nextFire;
    // Start is called before the first frame update
    void Start()
    {
        animator =  GetComponent<Animator>();
        audioSource =  GetComponent<AudioSource>();
        cam = GetComponentInParent<Camera>();
        gunPoint = transform.Find("Gun Point");
    }

    public void Shoot()
    {
        if(Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            animator.SetTrigger("Shoot");
            audioSource.Play();
            Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
            RaycastHit hit;
            if (Physics.Raycast(rayOrigin, cam.transform.forward, out hit, fireRange))
            {
                if(hit.collider.gameObject.CompareTag("Enemy"))
                {
                    hit.collider.gameObject.GetComponent<EnemyHealth>().TakeDamage(hit.transform.position, cam.transform.forward,35, hit.collider);
                }
                GameObject currentHit =  Instantiate(hitEffect);
                currentHit.transform.position = hit.point;
                currentHit.transform.LookAt(rayOrigin);
                StartCoroutine(RemoveHitEffect(currentHit));
            }

        }
    }
    IEnumerator RemoveHitEffect(GameObject hit)
    {
        yield return new WaitForSeconds(2);
        GameObject.Destroy(hit);
    }
}
