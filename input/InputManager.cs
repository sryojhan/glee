using System.Collections.Generic;
using Glee.Engine;
using Microsoft.Xna.Framework;

namespace Glee.Input;

public class InputManager
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
    /// <param name="gameTime">A snapshot of the timing values for the current frame.</param>
    public void Update(GameTime gameTime)
    {
        Keyboard.Update();
        Mouse.Update();

        for (int i = 0; i < 4; i++)
        {
            GamePads[i].Update(gameTime);
        }
    }



    readonly Dictionary<string, InputBinding> bindings = [];

    public static void Bind(string name, InputBinding bind)
    {
        GleeCore.Input.bindings.Add(name, bind);
    }

    public static bool IsDown(string name) => GleeCore.Input.bindings[name].IsDown;
    public static bool IsUp(string name) => GleeCore.Input.bindings[name].IsUp;
    public static bool IsJustDown(string name) => GleeCore.Input.bindings[name].IsJustDown;
    public static bool IsJustUp(string name) => GleeCore.Input.bindings[name].IsJustUp;
    public static float Value(string name) => GleeCore.Input.bindings[name].Value;
    public static Vector2 Value2D(string name) => GleeCore.Input.bindings[name].Value2D;
}