using System.Collections;
using UnityEngine;

public class MonsterScript : MonoBehaviour
{
    public GameObject sword1Prefab;
   // public GameObject sword2Prefab;
    private Animator animator;
    public float throwForceUp=-70f;
    public float throwForceLeft=50f;
    private bool isAttacking = false; 
    void start()
    {
        animator = GetComponent<Animator>();
    }
    IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f); 
            Attack();
        }
    }

    void Attack()
    {
        //animator.SetInteger("state", 2);
        isAttacking = true;

        Vector3 position = transform.position;
        InstantiateSword(position + Vector3.left * 10); 
        InstantiateSword(position);
        InstantiateSword(position + Vector3.right * 10); 
    }

    void InstantiateSword(Vector3 position)
    {
        GameObject sword = Instantiate(sword1Prefab, position, sword1Prefab.transform.rotation);
        sword.SetActive(true);
        Rigidbody2D rb = sword.GetComponent<Rigidbody2D>();
        StartCoroutine(ThrowAfterDelay(rb, sword.transform)); 
    }

    IEnumerator ThrowAfterDelay(Rigidbody2D rb, Transform swordTransform)
    {
        Vector3 initialPosition = swordTransform.position;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f;
        yield return new WaitForSeconds(1f);

        rb.gravityScale = 1f;

        swordTransform.position = initialPosition; 

        Vector2 throwDirection = new Vector2(-1, 1).normalized;
        throwDirection.x *= throwForceLeft;
        throwDirection.y *= throwForceUp;
        rb.AddForce(throwDirection, ForceMode2D.Impulse);

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(other.gameObject);
        }
    }
}