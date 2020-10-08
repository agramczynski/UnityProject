using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerBehav : CreatureBehav, IOrderableInLayer
{
    //position
    public Vector2 PosRightHand;
    public Vector2 PosLeftHand;
    public bool WeaponInRightHand = false;
    [SerializeField] SOWeapon[] Weapons;

    int currWeapon;

    public void AddWeapon(SOWeapon weapon)
    {
        Weapons.Append(weapon);
    }

    //IOrderableInLayer
    public void SetOrder(int order)
    {
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = order;
    }

    public void SetPlayerInFront()
    {
        SetOrder(1);
        Weapons[currWeapon].SetOrder(0);
    }

    public void SetWeaponInFront()
    {
        SetOrder(0);
        Weapons[currWeapon].SetOrder(1);
    }

    public void SwitchNextWeapon()
    {
        if (Weapons[currWeapon].LockRotation) return;
        Weapons[currWeapon].Deactivate();
        if (Weapons.Length == currWeapon + 1) currWeapon = 0;
        else currWeapon++;
        Weapons[currWeapon].Activate();
    }

    public void SwitchPreviousWeapon()
    {
        Weapons[currWeapon].Deactivate();
        if (Weapons.Length == 0) currWeapon = Weapons.Length - 1;
        else currWeapon--;
        Weapons[currWeapon].Activate();
    }

    //MonoBehav
    new protected void Start()
    {
        base.Start();
        foreach (SOWeapon weapon in Weapons) weapon.SetOwner(this);
        currWeapon = 0;
        CameraControls.SetTargetToFollow(transform);
        Weapons[currWeapon].Activate();
    }

    new protected void Update()
    {
        base.Update();
        if (InputReader.GetSwitchWeaponUp()) SwitchNextWeapon();
        else Weapons[currWeapon].OnUpdate();
    }

    protected override Vector2 SetMoveVector()
    {
        Vector2 vec = InputReader.FindDirections() * Time.deltaTime * MoveSpeed;
        return vec;
    }

    protected  override void Animate(Vector2 moveVec)
    {
        if (moveVec.magnitude > 0) animator.SetBool("Moving", true);
        else animator.SetBool("Moving", false);


        if (WeaponInRightHand) animator.SetBool("FacingRight", true);
        else animator.SetBool("FacingRight", false);
        //if (Input.mousePosition.x / Screen.width > 0.5f) animator.SetBool("FacingRight", true);
        //else animator.SetBool("FacingRight", false);
        //animator.Update(Time.deltaTime);
    }

}
