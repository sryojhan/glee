using System;

using Glee.Engine;

namespace Glee.Input;


//TODO: add mouse delta and scroll wheel
public class GenericButton : IEquatable<GenericButton>
{
    private GenericButton() { }

    private Keys key = Keys.None;
    private MouseButton mouse = MouseButton.None;
    private Gamepad gamepad = Gamepad.None;

    private InputManager i => Services.Fetch<InputManager>();

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
            buttonType = ButtonType.Mouse,
            mouse = mouse
        };
    }

    public static implicit operator GenericButton(Gamepad gamepad)
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
                ButtonType.Key => i.Keyboard.IsKeyDown(key),
                ButtonType.Mouse => i.Mouse.IsButtonDown(mouse),
                ButtonType.Gamepad => i.GamePads[GAMEPAD_IDX].IsButtonDown(gamepad),
                ButtonType.Continous => continousButton switch
                {
                    ContinousButton.None => throw new Exception("Using non-initialized continous buttons"),
                    ContinousButton.LeftTrigger => i.GamePads[GAMEPAD_IDX].LeftTrigger > GamePadInfo.Trigger.PRESSED_THRESHOLD,
                    ContinousButton.RightTrigger => i.GamePads[GAMEPAD_IDX].RightTrigger > GamePadInfo.Trigger.PRESSED_THRESHOLD,
                    ContinousButton.LeftThumbStick => i.GamePads[GAMEPAD_IDX].IsButtonDown(Gamepad.LeftThumbstickDown),
                    ContinousButton.RightThumbStick => i.GamePads[GAMEPAD_IDX].IsButtonDown(Gamepad.RightThumbstickDown),
                    _ => throw new ArgumentOutOfRangeException("Non existing continous button")
                },
                _ => throw new ArgumentOutOfRangeException("Non existing button type")
            };
        }
    }


    public bool IsUp
    {
        get
        {
            return !IsDown;
        }
    }


    public bool IsJustDown
    {
        get
        {
            return buttonType switch
            {
                ButtonType.None => throw new Exception("Using non-initialized generic buttons"),
                ButtonType.Key => i.Keyboard.WasKeyJustPressed(key),
                ButtonType.Mouse => i.Mouse.WasButtonJustPressed(mouse),
                ButtonType.Gamepad => i.GamePads[GAMEPAD_IDX].WasButtonJustPressed(gamepad),
                ButtonType.Continous => continousButton switch
                {
                    ContinousButton.None => throw new Exception("Using non-initialized continous buttons"),
                    ContinousButton.LeftTrigger => i.GamePads[GAMEPAD_IDX].WasLeftTriggerJustDown(),
                    ContinousButton.RightTrigger => i.GamePads[GAMEPAD_IDX].WasRightTriggerJustDown(),
                    ContinousButton.LeftThumbStick => i.GamePads[GAMEPAD_IDX].WasButtonJustPressed(Gamepad.LeftThumbstickDown),
                    ContinousButton.RightThumbStick => i.GamePads[GAMEPAD_IDX].WasButtonJustReleased(Gamepad.RightThumbstickDown),
                    _ => throw new ArgumentOutOfRangeException("Non existing continous button")
                },
                _ => throw new ArgumentOutOfRangeException("Non existing button type")
            };
        }
    }

    public bool IsJustUp
    {
        get
        {
            return buttonType switch
            {
                ButtonType.None => throw new Exception("Using non-initialized generic buttons"),
                ButtonType.Key => i.Keyboard.WasKeyJustReleased(key),
                ButtonType.Mouse => i.Mouse.WasButtonJustReleased(mouse),
                ButtonType.Gamepad => i.GamePads[GAMEPAD_IDX].WasButtonJustReleased(gamepad),
                ButtonType.Continous => continousButton switch
                {
                    ContinousButton.None => throw new Exception("Using non-initialized continous buttons"),
                    ContinousButton.LeftTrigger => i.GamePads[GAMEPAD_IDX].WasLeftTriggerJustUp(),
                    ContinousButton.RightTrigger => i.GamePads[GAMEPAD_IDX].WasRightTriggerJustUp(),
                    ContinousButton.LeftThumbStick => i.GamePads[GAMEPAD_IDX].WasButtonJustReleased(Gamepad.LeftThumbstickDown),
                    ContinousButton.RightThumbStick => i.GamePads[GAMEPAD_IDX].WasButtonJustReleased(Gamepad.RightThumbstickDown),
                    _ => throw new ArgumentOutOfRangeException("Non existing continous button")
                },
                _ => throw new ArgumentOutOfRangeException("Non existing button type")
            };
        }
    }


    public float Value
    {
        get
        {
            return buttonType switch
            {
                ButtonType.None => throw new Exception("Using non-initialized generic buttons"),
                ButtonType.Key => i.Keyboard.IsKeyDown(key) ? 1 : 0,
                ButtonType.Mouse => i.Mouse.IsButtonDown(mouse) ? 1 : 0,
                ButtonType.Gamepad => i.GamePads[GAMEPAD_IDX].IsButtonDown(gamepad) ? 1 : 0,
                ButtonType.Continous => continousButton switch
                {
                    ContinousButton.None => throw new Exception("Using non-initialized continous buttons"),
                    ContinousButton.LeftTrigger => i.GamePads[GAMEPAD_IDX].LeftTrigger,
                    ContinousButton.RightTrigger => i.GamePads[GAMEPAD_IDX].RightTrigger,
                    ContinousButton.LeftThumbStick => 0,
                    ContinousButton.RightThumbStick => 0,
                    _ => throw new ArgumentOutOfRangeException("Non existing continous button")
                },
                _ => throw new ArgumentOutOfRangeException("Non existing button type")
            };
        }
    }

    public Vector Value2D
    {
        get
        {
            return buttonType switch
            {
                ButtonType.None => throw new Exception("Using non-initialized generic buttons"),
                ButtonType.Key => Vector.Zero,
                ButtonType.Mouse => Vector.Zero,
                ButtonType.Gamepad => Vector.Zero,
                ButtonType.Continous => continousButton switch
                {
                    ContinousButton.None => throw new Exception("Using non-initialized continous buttons"),
                    ContinousButton.LeftTrigger => Vector.Zero,
                    ContinousButton.RightTrigger => Vector.Zero,
                    ContinousButton.LeftThumbStick => i.GamePads[GAMEPAD_IDX].LeftThumbStick,
                    ContinousButton.RightThumbStick => i.GamePads[GAMEPAD_IDX].RightThumbStick,
                    _ => throw new ArgumentOutOfRangeException("Non existing continous button")
                },
                _ => throw new ArgumentOutOfRangeException("Non existing button type")
            };
        }
    }

    
    public bool Equals(GenericButton other)
    {
        return buttonType == other.buttonType && key == other.key && mouse == other.mouse && gamepad == other.gamepad && continousButton == other.continousButton;
    }

    public override bool Equals(object obj)
    {
        if (this is not GenericButton a || obj is not GenericButton b) return false;

        return a.Equals(b);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}


