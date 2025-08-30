using Glee.Engine;
using Glee.Components;
using Glee.Behaviours;


namespace Glee.Templates.Platformer;



public class PlatformerCharacterController : EntityRaw, IInitializable, IUpdatable, IRenderizable, IPhysicsUpdatable
{
    public Image ImageComponent { get; }
    public Collider ColliderComponent { get; }
    public Body BodyComponent { get; }
    public Flipbook Flipbook { get; }

    public PlatformerCharacterController() : base("Player", GleeCore.WorldManager.Spotlight)
    {
        ColliderComponent = new Collider() { entity = this };
        ImageComponent = new Image() {entity = this};
        BodyComponent = new Body() {entity = this};

        world.AddEntity(this);
    }


    public void Initialize()
    {

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

