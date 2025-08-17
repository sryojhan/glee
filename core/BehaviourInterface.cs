

using Microsoft.Xna.Framework;

namespace Glee.ECS.Behaviour;


public interface IUpdatable
{
    void Update();
}


public interface IRenderizable
{
    void Render();
}

public interface IPhysicsUpdatable
{
    void PhysicsUpdate();
}

public interface ICollisionObserver
{
    void CollisionBegin();
    void CollisionPersist();
    void CollisionEnd();
}

public interface ITriggerObserver
{
    void TriggerBegin();
    void TriggerPersist();
    void Triggernd();

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