using System.Collections.Generic;
using Glee.Behaviours;
using Glee.Engine;
using Glee.Graphics;
using Microsoft.Xna.Framework;

namespace Glee.Input; //TODO: change the namespace name because accessing Input.Input is ugly
//TODO: Move the static class to Glee namespace and change this namespace to something like InputManagement

public class InputManager: CoreService, IUpdatable
{

    /// <summary>
    /// Gets the state information of keyboard input.
    /// </summary>
    public KeyboardInfo Keyboard { get; private set; }

    /// <summary>
    /// Gets the state information of mouse input.
    /// </summary>
    public MouseInfo Mouse { get; private set; }

    /// <summary>
    /// Gets the state information of a gamepad.
    /// </summary>
    public GamePadInfo[] GamePads { get; private set; }

    /// <summary>
    /// Creates a new InputManager.
    /// </summary>
    public InputManager()
    {
        Keyboard = new KeyboardInfo();
        Mouse = new MouseInfo();

        GamePads = new GamePadInfo[4];
        for (int i = 0; i < 4; i++)
        {
            GamePads[i] = new GamePadInfo((PlayerIndex)i);
        }
    }

    /// <summary>
    /// Updates the state information for the keyboard, mouse, and gamepad inputs.
    /// </summary>
    public void Update()
    {
        Keyboard.Update();
        Mouse.Update();

        for (int i = 0; i < 4; i++)
        {
            GamePads[i].Update();
        }
    }


    internal readonly Dictionary<string, InputBinding> bindings = [];
}

public static class Input
{
    public static InputManager i => Services.Fetch<InputManager>();

    public static void Bind(string name, InputBinding bind)
    {
        //TODO: check if exists and combine with previous
        i.bindings.Add(name, bind);
    }

    public static bool IsDown(string name) => i.bindings[name].IsDown;
    public static bool IsUp(string name) => i.bindings[name].IsUp;
    public static bool IsJustDown(string name) => i.bindings[name].IsJustDown;
    public static bool IsJustUp(string name) => i.bindings[name].IsJustUp;
    public static float Value(string name) => i.bindings[name].Value;
    public static Vector2 Value2D(string name) => i.bindings[name].Value2D;



    //Mouse accessors
    public static Vector2 MousePosition => i.Mouse.Position.ToVector2();
    public static Vector2 MouseDelta => i.Mouse.PositionDelta.ToVector2();
    public static Point MousePositionInt => i.Mouse.Position;
    public static Point MouseDeltaInt => i.Mouse.PositionDelta;
    public static int ScrollWheel => i.Mouse.ScrollWheel;
    public static int ScrollWheelDelta => i.Mouse.ScrollWheelDelta;



    public static Vector2 MousePositionRelative
    {
        get
        {
            return MousePosition / GleeCore.Renderer.Viewport.Width;
        }
    }

    public static Vector2 MouseWorldPosition
    {
        get
        {
            Camera cam = Services.Fetch<WorldManager>().Spotlight.Camera;
            Matrix matrix = Matrix.Invert(cam.GetMatrixWithoutUpdating());

            return Renderer.AdjustPosition(Vector2.Transform(MousePosition, matrix));
        }
    }
}