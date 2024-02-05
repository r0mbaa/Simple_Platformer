using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFireball : MonoBehaviour
{
    [SerializeField]private float fireballspeed;
    private float direction;
    private bool hit;
    private float timelifefireball;

    private BoxCollider2D boxCollider2D;
    private Animator animator;

    private void Awake() 
    {
        //Получить компоненты
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();

    }

    private void Update()
    {
        if (hit)
            return;
        float movementspeed = fireballspeed * Time.deltaTime * direction;
        transform.Translate(movementspeed, 0, 0);

        timelifefireball += Time.deltaTime;
        if (timelifefireball > 5)
        {
            gameObject.SetActive(false);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        boxCollider2D.enabled = false;
        animator.SetTrigger("FireballExplode");

    }

    public void Set_Direction(float _direction)
    {
        timelifefireball = 0;
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        boxCollider2D.enabled = true;

        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction)
            localScaleX = -localScaleX;

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);

    }

    private void Deactivate()
    {
        gameObject.SetActive(false);

    }
}
