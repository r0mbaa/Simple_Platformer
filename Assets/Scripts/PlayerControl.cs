using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerControl : MonoBehaviour
{
    public AudioClip[] sounds;
    private Rigidbody2D rigidbody2d;
    private GameObject player;
    [SerializeField] private float playerspeed;
    [SerializeField] private float playerjumpforce;
    [SerializeField] private GameObject timer;
    [SerializeField] private GameObject detonation;
    [SerializeField] private GameObject Camera;
    private Animator animator;
    private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask groundlayer;
    [SerializeField] private LayerMask walllayer;
    private float wallJumpCooldown;
    private float horizontalInput;


    private AudioSource audioSource => GetComponent<AudioSource>();

    private void Awake()
    {
        //Получить компоненты
        rigidbody2d = GetComponent<Rigidbody2D>();
        player = GetComponent<GameObject>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();

    }
    
    void Update()
    {
        //ходьба
        horizontalInput = Input.GetAxis("Horizontal");

        rigidbody2d.velocity = new Vector2(horizontalInput * playerspeed, rigidbody2d.velocity.y);

        //Поворото игрока во время ходьбы
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1,1,1) ;

        //Прыжок и падение
        if (Input.GetKey(KeyCode.S))
            rigidbody2d.gravityScale = playerjumpforce;

        if (!Input.GetKey(KeyCode.S))
            rigidbody2d.gravityScale = 1;
        
        //Камикадзе взрыв
        if (Input.GetKeyDown(KeyCode.P))
        {
            audioSource.PlayOneShot(sounds[0]);
            Instantiate(timer);
            TMP_Text _TimerText = GameObject.Find("TimerText").GetComponent<TMP_Text>();
            StartCoroutine(ITimer(_TimerText));
        }

        if (wallJumpCooldown < 0.2f) 
        {
            
            if (playeronwall() && !playeronground())
            {
                rigidbody2d.gravityScale = 0.3f;
                rigidbody2d.velocity = Vector2.zero;
            }
            else
            {
                rigidbody2d.gravityScale = 1;
            }

            if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W))
                PlayerJump();
        }
        else
        {
            wallJumpCooldown += Time.deltaTime;
        }

        // Параметры аниматора
        animator.SetBool("PlayerIsRuning", horizontalInput != 0);
        animator.SetBool("PlayerOnGround", playeronground());

    }

    private void PlayerJump()
    {
        if (playeronground())
        {
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, playerjumpforce);
            animator.SetTrigger("PlayerJump");
        }
        else if (playeronwall() && !playeronground())
        {
            if (horizontalInput == 0)
            {
                rigidbody2d.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 25, 7);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                rigidbody2d.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 25, 6);
            }

            wallJumpCooldown = 0;
        }

    }
    public IEnumerator ITimer(TMP_Text _TimerText, int delta = 1, int sec = 8, int min = 0)
    {
        //Таймер для взрыва
        while (sec > 0)
        {
            sec -= delta;
            _TimerText.text = min.ToString("D2") + " : " + sec.ToString("D2");
            yield return new WaitForSeconds(1);
        }
        Destroy(GameObject.Find("Canvas(Clone)"));
        Instantiate(detonation, transform.position, Quaternion.identity);
        audioSource.PlayOneShot(sounds[1]);
        Vector3 nwVec = new Vector3(transform.position.x, transform.position.y, -1);

    }

    private bool playeronground()
    {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundlayer);
        return raycastHit2D.collider != null;

    }

    private bool playeronwall()
    {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0,new Vector2(transform.localScale.x, 0), 0.1f, walllayer);
        return raycastHit2D.collider != null;

    }

    public bool PlayerCanAttack()
    {
        return horizontalInput == 0 && playeronground() && !playeronwall();

    }
}
