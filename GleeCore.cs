using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Glee.Audio;
using Glee.Input;


namespace Glee.Engine;


/*
    Glee: Graphic lightweight ECS engine
*/

public class GleeCore : Game
{
    internal static GleeCore s_instance;

    /// <summary>
    /// Gets a reference to the Core instance.
    /// </summary>
    public static GleeCore Instance => s_instance;

    public static WorldManager WorldManager { get; private set; }

    /// <summary>
    /// Gets the graphics device manager to control the presentation of graphics.
    /// </summary>
    public static GraphicsDeviceManager Graphics { get; private set; }

    /// <summary>
    /// Gets the graphics device used to create graphical resources and perform primitive rendering.
    /// </summary>
    public static new GraphicsDevice GraphicsDevice { get; private set; }

    /// <summary>
    /// Gets the sprite batch used for all 2D rendering.
    /// </summary>
    public static SpriteBatch SpriteBatch { get; private set; }

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


    public static GameTime Time { get; private set; }


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

        // Create a new graphics device manager.
        Graphics = new GraphicsDeviceManager(this);

        // Set the graphics defaults
        Graphics.PreferredBackBufferWidth = width;
        Graphics.PreferredBackBufferHeight = height;
        Graphics.IsFullScreen = fullScreen;


        //Target frame rate
        IsFixedTimeStep = true;
        TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 60.0f);
        Graphics.SynchronizeWithVerticalRetrace = true;


        // Apply the graphic presentation changes.
        Graphics.ApplyChanges();

        // Set the window title
        Window.Title = title;

        // Set the core's content manager to a reference of the base Game's
        // content manager.
        Content = base.Content;

        // Set the root directory for content.
        Content.RootDirectory = "content";

        // Mouse is visible by default.
        IsMouseVisible = true;

        // Exit on escape is true by default
        ExitOnEscape = true;
    }

    protected override void Initialize()
    {
        base.Initialize();

        // Set the core's graphics device to a reference of the base Game's
        // graphics device.
        GraphicsDevice = base.GraphicsDevice;

        // Create the sprite batch instance.
        SpriteBatch = new SpriteBatch(GraphicsDevice);

        // Create a new input manager.
        Input = new InputManager();

        // Create a new audio controller.
        Audio = new AudioController();


        WorldManager = new WorldManager();


        //TODO: Remove Debug behaviours

        WorldManager.StackWorld(new Debug.DebugWorld());
        WorldManager.UpdateStack();

    }

    protected override void UnloadContent()
    {
        // Dispose of the audio controller.
        Audio.Dispose();

        base.UnloadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        Time = gameTime;
        // Update the input manager.
        Input.Update(gameTime);

        // Update the audio controller.
        Audio.Update();

        if (ExitOnEscape && Input.Keyboard.WasKeyJustPressed(Keys.Escape))
        {
            Exit();
        }

        WorldManager.Udpate();

        //TODO: remove elements

        WorldManager.UpdateStack();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        WorldManager.Render();

        base.Draw(gameTime);
    }

}
