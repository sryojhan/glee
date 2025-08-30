using Glee.Components;

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

//TODO: collision info class? know more information about the collision
public interface ICollisionObserver : ICollisionBeginObserver, ICollisionStayObserver, ICollisionEndObserver { }

public interface ICollisionBeginObserver
{
    void OnCollisionBegin(Collider other);
}

public interface ICollisionStayObserver
{
    void OnCollision(Collider other);
}

public interface ICollisionEndObserver
{
    void OnCollisionEnd(Collider other);
}


public interface ITriggerObserver: ITriggerBeginObserver, ITriggerStayObserver, ITriggerEndObserver{}

public interface ITriggerBeginObserver
{
    void OnTriggerBegin(Collider other);
}

public interface ITriggerStayObserver
{
    void OnTrigger(Collider other);
}

public interface ITriggerEndObserver
{
    void OnTriggernd(Collider other);
}

//TODO: world callbacks
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



public interface IBehaviour : IInitializable, IUpdatable { }
public interface IPhysicsObserver : ICollisionObserver, ITriggerObserver { }
public interface IEverything : IInitializable, IUpdatable, IPhysicsUpdatable, IPhysicsObserver, IRemovableObserver { }