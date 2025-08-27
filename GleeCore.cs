using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Glee.Audio;
using Glee.Input;
using Glee.Graphics;


namespace Glee.Engine;


/*
    Glee: Graphic lightweight extensible engine
*/

public abstract class GleeCore : Game
{
    public const string EngineVersion = "1.0";


    internal static GleeCore s_instance;

    /// <summary>
    /// Gets a reference to the Core instance.
    /// </summary>
    public static GleeCore Instance => s_instance;

    public static WorldManager WorldManager { get; private set; }

    /// <summary>
    /// Gets the sprite batch used for all 2D rendering.
    /// </summary>
    public static Renderer Renderer { get; private set; }

    public static new Services Services { get; private set; }

    /// <summary>
    /// Gets the content manager used to load global assets.
    /// </summary>
    public static new ContentManager Content { get; private set; }

    /// <summary>
    /// Gets a reference to to the input management system.
    /// </summary>
    public static InputManager Input { get; private set; }

    /// <summary>
    /// Gets or Sets a value that indicates if the game should exit when the esc key on the keyboard is pressed.
    /// </summary>
    public static bool ExitOnEscape { get; set; }

    /// <summary>
    /// Gets a reference to the audio control system.
    /// </summary>
    public static AudioController Audio { get; private set; }


    public static GameTime GameTime { get; private set; }


    public static float TargetFrameRate { get; } = 60.0f;


    /// <summary>
    /// Creates a new Core instance.
    /// </summary>
    /// <param name="title">The title to display in the title bar of the game window.</param>
    /// <param name="width">The initial width, in pixels, of the game window.</param>
    /// <param name="height">The initial height, in pixels, of the game window.</param>
    /// <param name="fullScreen">Indicates if the game should start in fullscreen mode.</param>
    public GleeCore(string title, int width, int height, bool fullScreen)
    {
        // Ensure that multiple cores are not created.
        if (s_instance != null)
        {
            throw new InvalidOperationException($"Only a single Core instance can be created");
        }


        // Store reference to engine for global member access.
        s_instance = this;

        // Set the window title
        Window.Title = title;

        Renderer = new Renderer(width, height, fullScreen, TargetFrameRate);

        // Set the core's content manager to a reference of the base Game's
        // content manager.
        Content = base.Content;

        // Set the root directory for content.
        Content.RootDirectory = "content";

        // Mouse is visible by default.
        IsMouseVisible = true;

        // Exit on escape is true by default
        ExitOnEscape = true;

        Services = new Services();
    }

    protected override void Initialize()
    {
        Renderer.Initialise();

        // Create a new input manager.
        Input = new InputManager();

        // Create a new audio controller.
        Audio = new AudioController();

        
        Services.Run<Log>();
        Services.Run<Events>();
        Services.Run<Resources>();

        WorldManager = new WorldManager();

        GameTime = new GameTime();

        WorldManager.StackWorld(LoadInitialWorld());
        WorldManager.UpdateStack();





        base.Initialize();

    }

    protected abstract World LoadInitialWorld();

    protected override void UnloadContent()
    {
        // Dispose of the audio controller.
        Audio.Dispose();

        base.UnloadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        GameTime = gameTime;

        Input.Update(gameTime);
        Audio.Update();

        if (ExitOnEscape && Input.Keyboard.WasKeyJustPressed(Keys.Escape))
        {
            Exit();
        }

        WorldManager.ProcessFrame();
        WorldManager.UpdateStack();


        Services.UpdateServices();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        Renderer.BeginFrame();

        WorldManager.Render();
        Services.RenderServices();


        Renderer.Present();

        base.Draw(gameTime);
    }


}
