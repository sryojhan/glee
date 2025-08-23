using System;

using Glee.Engine;

namespace Glee.Input;



public class GenericButton
{
    private GenericButton() { }

    //Binary inputs
    private Keys key = Keys.None;
    private MouseButton mouse = MouseButton.None;
    private Buttons gamepad = Buttons.None;

    private enum ContinousButton
    {
        None, LeftTrigger, RightTrigger, LeftThumbStick, RightThumbStick
    }
    private ContinousButton continousButton = ContinousButton.None;


    private enum ButtonType
    {
        None, Key, Mouse, Gamepad, Continous
    }

    private ButtonType buttonType = ButtonType.None;



    public static GenericButton LeftTrigger()
    {
        return new GenericButton()
        {
            buttonType = ButtonType.Continous,
            continousButton = ContinousButton.LeftTrigger
        };
    }
    public static GenericButton RightTrigger()
    {
        return new GenericButton()
        {
            buttonType = ButtonType.Continous,
            continousButton = ContinousButton.RightTrigger
        };
    }

    public static GenericButton LeftThumbStick()
    {
        return new GenericButton()
        {
            buttonType = ButtonType.Continous,
            continousButton = ContinousButton.LeftThumbStick
        };
    }

    public static GenericButton RightThumbStick()
    {
        return new GenericButton()
        {
            buttonType = ButtonType.Continous,
            continousButton = ContinousButton.RightThumbStick
        };
    }

    public static implicit operator GenericButton(Keys key)
    {
        return new GenericButton()
        {
            buttonType = ButtonType.Key,
            key = key
        };
    }

    public static implicit operator GenericButton(MouseButton mouse)
    {
        return new GenericButton()
        {
            buttonType = ButtonType.Key,
            mouse = mouse
        };
    }

    public static implicit operator GenericButton(Buttons gamepad)
    {
        return new GenericButton()
        {
            buttonType = ButtonType.Gamepad,
            gamepad = gamepad
        };
    }

    //TODO: functionality for anygamepad without specifying the index
    const int GAMEPAD_IDX = 0;

    public bool IsDown
    {
        get
        {
            return buttonType switch
            {
                ButtonType.None => throw new Exception("Using non-initialized generic buttons"),
                ButtonType.Key => GleeCore.Input.Keyboard.IsKeyDown(key),
                ButtonType.Mouse => GleeCore.Input.Mouse.IsButtonDown(mouse),
                ButtonType.Gamepad => GleeCore.Input.GamePads[GAMEPAD_IDX].IsButtonDown(gamepad),
                ButtonType.Continous => continousButton switch
                {
                    ContinousButton.None => throw new Exception("Using non-initialized continous buttons"),
                    ContinousButton.LeftTrigger => GleeCore.Input.GamePads[GAMEPAD_IDX].LeftTrigger > 0.5f,
                    ContinousButton.RightTrigger => GleeCore.Input.GamePads[GAMEPAD_IDX].RightTrigger > 0.5f,
                    ContinousButton.LeftThumbStick => GleeCore.Input.GamePads[GAMEPAD_IDX].IsButtonDown(Buttons.LeftThumbstickDown),
                    ContinousButton.RightThumbStick => GleeCore.Input.GamePads[GAMEPAD_IDX].IsButtonDown(Buttons.RightThumbstickDown),
                    _ => throw new ArgumentOutOfRangeException("Non existing continous button")
                }
                _ => throw new ArgumentOutOfRangeException("Non existing button type")
            };
        }
    }


    public bool IsJustDown
    {
        get
        {
            return buttonType switch
            {
                ButtonType.None => throw new Exception("Using non-initialized generic buttons"),
                ButtonType.Key => GleeCore.Input.Keyboard.WasKeyJustPressed(key),
                ButtonType.Mouse => GleeCore.Input.Mouse.WasButtonJustPressed(mouse),
                ButtonType.Gamepad => GleeCore.Input.GamePads[GAMEPAD_IDX].WasButtonJustPressed(gamepad),
                ButtonType.Continous => continousButton switch
                {
                    ContinousButton.None => throw new Exception("Using non-initialized continous buttons"),
                    ContinousButton.LeftTrigger => GleeCore.Input.GamePads[GAMEPAD_IDX].LeftTrigger > 0.5f,
                    ContinousButton.RightTrigger => GleeCore.Input.GamePads[GAMEPAD_IDX].RightTrigger > 0.5f,
                    ContinousButton.LeftThumbStick => GleeCore.Input.GamePads[GAMEPAD_IDX].IsButtonDown(Buttons.LeftThumbstickDown),
                    ContinousButton.RightThumbStick => GleeCore.Input.GamePads[GAMEPAD_IDX].IsButtonDown(Buttons.RightThumbstickDown),
                    _ => throw new ArgumentOutOfRangeException("Non existing continous button")
                }
                _ => throw new ArgumentOutOfRangeException("Non existing button type")
            };
        }
    }


    public float Value
    {
        get
        {

        }
    }

    public Vector2 Value2D
    {
        get
        {

        }
    }
}


