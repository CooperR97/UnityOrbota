using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.CorgiEngine;

public class AimControl : MonoBehaviour {


    public Vector2 RaycastOriginOffset = new Vector2(0, -1);

    protected Vector2 _raycastOrigin;

    protected RaycastHit2D _raycast;
    protected RaycastHit2D _raycast2;

    protected Vector2 _direction;

    protected ProjectileWeapon _weapon;

    /// The layers the agent will try to shoot at
    public LayerMask TargetLayerMask;

    public float ShootDistance = 10f;

    private WeaponAim _aimSystem;

    // Use this for initialization
    void Start () {
        Initialization();
	}

    protected virtual void Initialization()
    {
        _aimSystem = GetComponent<WeaponAim>();
        _weapon = GetComponent<ProjectileWeapon>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        rollerBossAim();
    }

    public void rollerBossAim()
    {
        //Cast a bunch of rays to find the character and aim at it
        for (int i = 0; i < 10; i++)
        {
            _raycastOrigin.x = _weapon.transform.position.x;
            _raycastOrigin.y = _weapon.transform.position.y + RaycastOriginOffset.y + i * .7f;
            _raycast = MMDebug.RayCast(_raycastOrigin, new Vector2(-1,0), ShootDistance, TargetLayerMask, Color.cyan, true);
            _raycast2 = MMDebug.RayCast(_raycastOrigin, new Vector2(1, 0), ShootDistance, TargetLayerMask, Color.cyan, true);
            if (_raycast)
            {
                _aimSystem.SetCurrentAim(new Vector3(-RaycastOriginOffset.x, RaycastOriginOffset.y + i, 0));
            }
            else if(_raycast2)
            {
                _aimSystem.SetCurrentAim(new Vector3(RaycastOriginOffset.x, RaycastOriginOffset.y + i, 0));
            }
        }
    }
}
