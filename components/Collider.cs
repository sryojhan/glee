using Glee.Behaviours;
using Glee.Physics;

namespace Glee.Components;


public class Collider : Component, IInitializable, IRemovableObserver
{
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

    public void Initialize()
    {
        bounds ??= new Rect();

        PhysicsWorld.RegisterCollider(this);
    }

    public void OnRemove()
    {
        PhysicsWorld.UnregisterCollider(this);
    }
}