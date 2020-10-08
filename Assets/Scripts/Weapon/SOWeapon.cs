using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SOWeapon : ScriptableObject, IOrderableInLayer
{
    public enum ReloadType
    {
        AUTO,
        SEMI_AUTO,
        MANUAL
    }

    
    PlayerBehav _owner;
    GameObject _weaponObject;

    //General
    [SerializeField] string _objName;
    [SerializeField] Sprite _sprite;

    //animation
    [SerializeField] Vector2 _animationOffset;
    [SerializeField] string _mainAnimation;
    [SerializeField] string _secondaryAnimation;
    [SerializeField] float _animationTime;
    [SerializeField] float _rotationOffset;
    [SerializeField] bool _alwaysOnTop;
    [SerializeField] bool _reverseHand;
    [SerializeField] float _attackInitialRotation;
    [SerializeField] float _rotationSpeedAfter;
    [SerializeField] float _rotationSpeedDamp;
    //Attacking
    [SerializeField] ReloadType _reload;
    [SerializeField] int _damage;
    [SerializeField] float _timeBetweenAttacks;

    [SerializeField] GameObject attack;


    bool OnRight { get; set; }
    bool AfterAttack { get; set; }
    public bool LockRotation { get; private set; }

    List<Func<bool>> OnUpdateList { get; set; }


    float _nextAttackTime = 0;
    bool Ready => Time.time > _nextAttackTime;

    bool FlippedX {
        get { return _weaponObject.GetComponentInChildren<SpriteRenderer>().flipX; } 
        set { _weaponObject.GetComponentInChildren<SpriteRenderer>().flipX = value;
            _owner.WeaponInRightHand = !value;
        }
    }

    public void SetOwner(PlayerBehav player)
    {
        _owner = player;
    }
    public void Activate()
    {
        CreateWeaponObj();
        float rot = CalculateMouseRotation();
        SetRotation(rot);
        SetPosition(rot, true);
        SetPositionInLayer(rot);
    }
    public void Deactivate()
    {
        Destroy(_weaponObject);
    }

    public void CreateWeaponObj()
    {
        _nextAttackTime = Time.time;
        OnUpdateList = new List<Func<bool>>();
        GameObject spriteObj = new GameObject("Sprite");
        SpriteRenderer renderer = spriteObj.AddComponent<SpriteRenderer>();
        _weaponObject = new GameObject(_objName);
        
        spriteObj.transform.SetParent(_weaponObject.transform);
        renderer.sprite = _sprite;
        _weaponObject.transform.SetParent(_owner.transform);
    }

    public void OnUpdate()
    {
        SetPositionRotation();

        if (InputReader.GetAction1Down()) OnLeftClick();
        if (InputReader.GetAction2Down()) OnRightClick();

        List<Func<bool>> toRemove = new List<Func<bool>>();
        foreach (Func<bool> func in OnUpdateList) if (func()) toRemove.Add(func);
        foreach (Func<bool> func in toRemove) OnUpdateList.Remove(func);
    }

    private void SetPositionRotation()
    {
        float weaponRotation = CalculateWeaponRotation();
        SetPosition(CalculateMouseRotation());
        SetRotation(weaponRotation);
        SetPositionInLayer(weaponRotation);
    }

    protected float CalculateMouseRotation()
    {
        return Vector2.SignedAngle(Vector2.up, Camera.main.ScreenToWorldPoint(Input.mousePosition) - _weaponObject.transform.position);
    }

    protected float CalculateWeaponRotation()
    {
        return Vector2.SignedAngle(Vector2.up, Camera.main.ScreenToWorldPoint(Input.mousePosition) - _weaponObject.transform.position);
    }

    protected void SetHand(bool _onRight)
    {
        _weaponObject.transform.localPosition = _onRight ^ _reverseHand ? _owner.PosRightHand : _owner.PosLeftHand;
        FlippedX = !(_onRight);
    }
    
    private void SetPosition(float rot) => SetPosition(rot, false);
    protected void SetPosition(float rot, bool force)
    {
        if (force)
        {
            SetHand(rot < 0);
            return;
        }

        if (LockRotation) return;
        if (!OnRight && rot < 160 && rot > 20) SetHand(false);
        else if (rot < -20 && rot > -160) SetHand(true);
    }

    protected void SetPositionInLayer(float rot)
    {
        if (rot > 90 || rot < -90 || _alwaysOnTop) _owner.SetWeaponInFront();
        else _owner.SetPlayerInFront();
    }

    protected void SetRotation(float rot)
    {
        if (LockRotation) return;
        _weaponObject.transform.eulerAngles = new Vector3(
            _weaponObject.transform.eulerAngles.x,
            _weaponObject.transform.eulerAngles.y,
            rot
        );
        _weaponObject.transform.GetChild(0).transform.localRotation = Quaternion.Euler(Vector3.forward * _rotationOffset * (!FlippedX ? 1 : -1));
    }
    
    //IOrderableInLayer
    public void SetOrder(int order)
    {
        _weaponObject.GetComponentInChildren<SpriteRenderer>().sortingOrder = order;
    }

    public bool OnLeftClick()
    {
        if (!Ready) return false;
        Attack(_mainAnimation);
        return true;
    }

    public bool OnRightClick()
    {
        if (!Ready) return false;
        Attack(_secondaryAnimation);
        return true;
    }

    protected void AnimateWeapon()
    {
        _weaponObject.transform.GetChild(0).transform.Rotate(Vector3.forward, _attackInitialRotation * (FlippedX ? 1 : -1));
        LockRotation = true;
        bool oldOnTop = _alwaysOnTop;
        _alwaysOnTop = true;

        float animationEndTime = _nextAttackTime;
        float rotationSpeed = _rotationSpeedAfter;
        float rotDumpSpeed = _rotationSpeedDamp;
        bool sign = rotationSpeed > 0;
        OnUpdateList.Add(new Func<bool>(()=>{

            _weaponObject.transform.GetChild(0).transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime * (FlippedX ? 1 : -1));

            rotationSpeed -= rotDumpSpeed * Time.deltaTime;
            if(sign != rotationSpeed > 0)
            {
                rotDumpSpeed = 0;
                rotationSpeed = 0;
            }

            if (Time.time < animationEndTime) return false;
            LockRotation = false;
            return true;
        }));
    }

    protected void PlayAttackEffect(string animationName)
    {
        GameObject attackEffect = Instantiate(attack);
        attackEffect.transform.SetParent(_weaponObject.transform);
        attackEffect.transform.position = _weaponObject.transform.position;
        attackEffect.transform.localPosition += new Vector3(_animationOffset.x * (FlippedX ? -1 : 1), _animationOffset.y);
        attackEffect.transform.rotation = _weaponObject.transform.rotation * Quaternion.Euler(Vector3.forward * 90);
        attackEffect.transform.SetParent(null);
        attackEffect.GetComponent<AttackBehav>().PlayAndDestroy(animationName, _animationTime);
    }

    public void Attack(string animationName)
    {
        _nextAttackTime = Time.time + _timeBetweenAttacks;
        PlayAttackEffect(animationName);
        AnimateWeapon();
    }
}
