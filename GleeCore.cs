using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Glee.Audio;
using Glee.Input;
using Glee.Graphics;
using Glee.Assets.Internal;
using Glee.Assets.Text;


namespace Glee.Engine;


/*
    Glee: Graphic lightweight extensible engine
*/


//TODO: configuracion

/*
No entiendo a que te refieres con que haga lo del escape opcional? si la idea es que sea un bool para que la configuración decida

- Cerrar juego al pulsar escape
- Tamaño ventana
- Nombre de la ventana
- Fuerza gravedad
- Target frame rate
- Vsync activado 
- IsMouseVisible


Adicional
- Color de fondo por defecto: vale
- Physics settings adicionales, por ahora no tengo pensado poner nada de esto
- Audio: no tiene sentido poner esto aqui porque esto va en un xnb, luego ya haré otra forma de poder escribir en tiempo de ejecución en otro tipo de ficheros
- Opciones de debug:  vale. Haré configurable que se pinten los colliders, los raycast, se muestren los fps, y si el juego se ejecuta o no en modo estricto (el modo estricto lanza excepciones, el modo no estricto continua y hace log del error
- Escalado de ventana: pixeles por unidad por defecto y modo
- Logging: mostrar o no logs, con además con configuracion de verbose: all, warnings o errors, y con configuración para mostrar o no un timestamp al escribir el texto, y con opcion para volvar los datos en un log al terminar el juego
- Image error: configurar que hacer cuando se intente pintar una imagen sin textura, ignorarlo o pintar un cuadrado del color del material
- Material error: configurar que hacer cuando se intente pintar sin material, usar un color blanco y material por defecto o pintar la textura rosa
- Capas de render y capas de colisiones
- Default post processing


    Cerrar juego al pulsar Escape → bool

Tamaño ventana → ancho x alto

Nombre de ventana → string

Fuerza gravedad → float

Target frame rate → int

VSync activado → bool

IsMouseVisible → bool

Adicional

Color de fondo por defecto → Color

Opciones de debug → pintar colliders, raycast, FPS, modo estricto/logging de errores

Escalado de ventana → pixeles por unidad, modo (stretch, fit, etc.)

Logging → mostrar logs, verbose level (all/warnings/errors), timestamp, volcado al final del juego

Image error → ignorar / pintar cuadrado del color del material

Material error → usar color/material por defecto / pintar textura rosa




*/

public abstract class GleeCore : Game
{
    public const string EngineVersion = "1.0";


    internal static GleeCore s_instance;

    /// <summary>
    /// Gets a reference to the Core instance.
    /// </summary>
    public static GleeCore Instance => s_instance;

    //TODO: to cleanup maybe remove this property?
    private WorldManager worldManager;

    /// <summary>
    /// Gets the sprite batch used for all 2D rendering.
    /// </summary>

    public static new Services Services { get; private set; }

    /// <summary>
    /// Gets the content manager used to load global assets.
    /// </summary>
    public static new ContentManager Content { get; private set; }

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


        // Set the core's content manager to a reference of the base Game's
        // content manager.
        Content = base.Content;

        // Set the root directory for content.
        Content.RootDirectory = "Content";

        JSON config = JSON.Create("info");


        // Set the window title
        Window.Title = title;

        Services = new Services();
        Services.AppendInternal<Renderer>(new Renderer(width, height, fullScreen, TargetFrameRate));

        // Mouse is visible by default.
        IsMouseVisible = true;

        // Exit on escape is true by default
        ExitOnEscape = true;

    }

    protected override void Initialize()
    {
        Services.Fetch<Renderer>().Initialise();

        // Create a new audio controller.
        Audio = new AudioController();
        GameTime = new GameTime();

        Services.RunInternal<InputManager>();
        Services.RunInternal<Log>();
        Services.RunInternal<Events>();
        Services.RunInternal<Resources>();

        worldManager = Services.RunInternal<WorldManager>();

        worldManager.StackWorld(LoadInitialWorld());
        worldManager.UpdateStack();


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

        Audio.Update();

        Services.UpdateServices();

        if (ExitOnEscape && Services.Fetch<InputManager>().Keyboard.WasKeyJustPressed(Keys.Escape))
        {
            Exit();
        }

        worldManager.UpdateStack();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        Renderer.BeginFrame();

        Services.RenderServices();
        Renderer.Present();

        base.Draw(gameTime);
    }


}
