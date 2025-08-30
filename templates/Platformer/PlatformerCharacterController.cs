using Glee.Engine;
using Glee.Components;
using Glee.Behaviours;
using System;


namespace Glee.Templates.Platformer;

//TODO: Create a EntityComplex?? Class that both manages components and supports custom engine callbacks (update, init, render, physics)
public class PlatformerCharacterController : EntityRaw, IInitializable, IUpdatable, IRenderizable, IPhysicsUpdatable
{
    public Image ImageComponent { get; }
    public Collider ColliderComponent { get; }
    public Body BodyComponent { get; }
    public Flipbook Flipbook { get; }

    public PlatformerCharacterController() : base("Player", GleeCore.WorldManager.Spotlight)
    {
        ImageComponent = new Image() {entity = this};
        ColliderComponent = new Collider() { entity = this };


        BodyComponent = new Body() {entity = this, collider = ColliderComponent};
        Flipbook = new Flipbook() { entity = this, Target = ImageComponent };



        world.AddEntity(this);
    }



    public void Initialize()
    {
        ColliderComponent.Initialize();
        BodyComponent.Initialize();
    }


    public void Update()
    {

    }


    public void Render()
    {
        ImageComponent.Render();
    }

    public void PhysicsUpdate()
    {

    }
}

