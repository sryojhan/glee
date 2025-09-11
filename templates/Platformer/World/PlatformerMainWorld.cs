namespace Glee.Templates.Platformer;

using System.Collections;
using Glee.Behaviours;
using Glee.Components;
using Glee.Engine;
using Glee.Graphics;
using Glee.Input;
using Controller = PlatformerCharacterController;


public static class CollisionLayers
{
    public const string Player = "Player";
    public const string Ground = "Ground";
}

public class PlatformerMainWorld : World, IUpdatable, IInitializable
{
    public Controller Player { get; private set; }
    Texture groundTexture;

    public override void CreateWorld()
    {
        GleeError.Tolerance = GleeError.ToleranceMode.Permissive;

        physicsWorld.CollisionMatrix.RegisterLayer(CollisionLayers.Player);
        physicsWorld.CollisionMatrix.RegisterLayer(CollisionLayers.Ground);

        physicsWorld.CollisionMatrix.DoCollision(CollisionLayers.Player, CollisionLayers.Ground);

        InputBinding();

        groundTexture = Load<Texture>("Background");

        Player = new();
        Player.ImageComponent.texture = Load<Texture>("blue-circle");

        InitialiseScenery();
    }

    private GleeEntity CreatePlatform(Vector position, Vector size)
    {
        GleeEntity platform = CreateEntity("Platform", position, size);

        Collider collider = platform.CreateComponent<Collider>();
        collider.Friction = 2;
        collider.Layer = CollisionLayers.Ground;

        platform.CreateComponent<Image>().texture = groundTexture;
        return platform;
    }


    private void InputBinding()
    {
        Input.Bind("horizontal", new InputBinding(

            positive: [Keys.D, GenericButton.LeftThumbStick()],
            negative: [Keys.A]
        ));

        Input.Bind("jump", new InputBinding().Bind(Keys.Space).Bind(Gamepad.A));
    }

    private void InitialiseScenery()
    {
        CreatePlatform(position: new Vector(0, -3), size: new Vector(10, 1)).Name = "Ground";
    }


    public void Update()
    {
        bool isJumping = Input.IsJustDown("jump");
        float horizontal = Input.Value("horizontal");

        // Body playerBody = Player.BodyComponent;
        // playerBody.Accelerate(Utils.Right * horizontal * 10);


        if (isJumping)
        {
            //playerBody.SetVerticalVelocity(10);
            var a = worldObjects.Random();
            Print((a as GleeEntityRaw).Name);
            Destroy(a);
        }
    }

    public void Initialize()
    {
        Launch(MiCorrutina());
    }

    IEnumerator MiCorrutina()
    {
        //Print("Hola");
        yield return new WaitCondition(()=> Input.IsJustDown("jump"));
        //Print("Adios");
    }

}