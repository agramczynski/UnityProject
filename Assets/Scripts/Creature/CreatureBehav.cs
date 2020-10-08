using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CreatureBehav : MonoBehaviour
{
    protected Animator animator;

    [SerializeField] protected float MoveSpeed;
    [SerializeField] protected int MaxHp;
    protected int CurrHp { get; set; }


    protected InputReader inputReader;

    protected abstract Vector2 SetMoveVector();

    protected abstract void Animate(Vector2 moveVec);

    protected Vector2 Move(Vector2 moveVec)
    {
        transform.position = transform.position + new Vector3(moveVec.x, moveVec.y);
        return moveVec;
    }
    protected void Awake()
    {
        CurrHp = MaxHp;
    }

    protected void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    protected void Update()
    {
        Vector2 moveVec = SetMoveVector();
        moveVec = Move(moveVec);
        Animate(moveVec);
    }
}
