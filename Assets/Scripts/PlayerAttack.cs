using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;

    private Animator animator;
    private PlayerControl playerControl;
    private float timerCooldown = Mathf.Infinity;

    private void Awake()
    {
        //�������� ����������
        animator = GetComponent<Animator>();
        playerControl = GetComponent<PlayerControl>();

    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && timerCooldown > attackCooldown && playerControl.PlayerCanAttack())
            Attack();

        timerCooldown += Time.deltaTime;

    }

    private void Attack()
    {
        //������� ��� ���������
        animator.SetTrigger("PlayerAttack");
        timerCooldown = 0;
        
        //�� ������� � ���������� ������ �������
        fireballs[FindFireball()].transform.position = firePoint.position;
        fireballs[FindFireball()].GetComponent<SpriteFireball>().Set_Direction(Mathf.Sign(transform.localScale.x));

    }

    private int FindFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
                return i;
        }
        return 0;

    }
}
