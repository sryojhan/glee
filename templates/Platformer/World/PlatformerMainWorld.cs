namespace Glee.Templates.Platformer;

using Glee.Behaviours;
using Glee.Components;
using Glee.Graphics;
using Glee.Input;
using Controller = PlatformerCharacterController;


public static class CollisionLayers
{
    public const string Player = "Player";
    public const string Ground = "Ground";
}

public class PlatformerMainWorld : World, IUpdatable
{
    public Controller Player { get; private set; }
    Texture groundTexture;

    public override void CreateWorld()
    {
        physicsWorld.CollisionMatrix.RegisterLayer(CollisionLayers.Player);
        physicsWorld.CollisionMatrix.RegisterLayer(CollisionLayers.Ground);

        physicsWorld.CollisionMatrix.DoCollision(CollisionLayers.Player, CollisionLayers.Ground);

        InputBinding();

        groundTexture = Load<Texture>("Background");

        Player = new();
        Player.ImageComponent.texture = Load<Texture>("blue-circle");

        InitialiseScenery();
    }

    private Entity CreatePlatform(Vector position, Vector size)
    {
        Entity platform = CreateEntity("Platform", position, size);

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

        Body playerBody = Player.BodyComponent;
        //playerBody.SetHorizontalVelocity(horizontal * 10);

        playerBody.Accelerate(Utils.Right * horizontal * 10);


        if (isJumping)
        {
            playerBody.SetVerticalVelocity(10);
        }
    }

}