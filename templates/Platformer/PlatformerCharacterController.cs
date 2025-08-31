using Glee.Engine;
using Glee.Components;
using Glee.Behaviours;
using System;
using Glee.Graphics;


namespace Glee.Templates.Platformer;

//TODO: Create a EntityComplex?? Class that both manages components and supports custom engine callbacks (update, init, render, physics)
public class PlatformerCharacterController : EntityRaw, IEverything
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

        ImageComponent.material = Material.Default;
    }


    public void Update()
    {
        ImageComponent.material.MainColor = ColliderComponent.IsGrounded() ? Color.SeaGreen : Color.White;
    }

    public void PhysicsUpdate()
    {
    }


    public void Render()
    {
        ImageComponent.Render();
    }

    public void OnCollisionBegin(Collider other)
    {
        Print("Me choco");
    }

    public void OnCollision(Collider other)
    {
    }

    public void OnCollisionEnd(Collider other)
    {
        Print("Ya no");
    }

    public void OnTriggerBegin(Collider other)
    {
    }

    public void OnTrigger(Collider other)
    {
    }

    public void OnTriggernd(Collider other)
    {
    }

    public void CleanUp()
    {

    }
}

