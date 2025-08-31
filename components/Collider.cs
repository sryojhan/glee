using System.Numerics;
using Glee.Behaviours;
using Glee.Physics;

namespace Glee.Components;


public class Collider : ComponentRaw, IInitializable, ICleanable
{
    public float Friction { get; set; } = 0.5f;

    private Bounds _bounds = null;
    public Bounds bounds
    {
        get { return _bounds; }
        set
        {
            _bounds = value;
            _bounds.entity = entity;
        }
    }

    public bool Trigger { set; get; } = false;


    public void Initialize()
    {
        bounds ??= new Rect();

        PhysicsWorld.RegisterCollider(this);
    }

    public void CleanUp()
    {
        PhysicsWorld.UnregisterCollider(this);
    }


    public bool IsGrounded()
    {
        Vector bottom = Utils.Alignment.Bottom(entity);
        return Physics.Physics.Raycast(bottom, Utils.Down, distance: 0.01f, exclusionList: [entity]);
    }

}