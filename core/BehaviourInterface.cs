

using Glee.Components;
using Microsoft.Xna.Framework;

namespace Glee.Behaviours;


public interface IInitializable
{
    void Initialize();
}

public interface IUpdatable
{
    void Update();
}


public interface IRenderizable
{
    void Render();
}

//TODO: name idea. Last wish XD
public interface IRemovableObserver
{
    void OnRemove();
}


public interface IPhysicsUpdatable
{
    void PhysicsUpdate();
}

//TODO: collision info class? 
public interface ICollisionObserver
{
    void OnCollisionBegin(Collider other);
    void OnCollision(Collider other);
    void OnCollisionEnd(Collider other);
}

public interface ITriggerObserver
{
    void OnTriggerBegin(Collider other);
    void OnTrigger(Collider other);
    void OnTriggernd(Collider other);
}

public interface IWorldLifecycleOberserved
{
    void WorldLoaded();
    void WorldDestroyed();
    void WorldReloaded();
}

public interface ISpotlightObserver
{
    void SpotlightGained();
    void SpotlightLost();
}