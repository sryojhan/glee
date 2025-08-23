using System;

using Glee.Engine;

namespace Glee.Input;



public class GenericButton : IEquatable<GenericButton>
{
    private GenericButton() { }

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
                    ContinousButton.LeftTrigger => GleeCore.Input.GamePads[GAMEPAD_IDX].LeftTrigger > GamePadInfo.Trigger.PRESSED_THRESHOLD,
                    ContinousButton.RightTrigger => GleeCore.Input.GamePads[GAMEPAD_IDX].RightTrigger > GamePadInfo.Trigger.PRESSED_THRESHOLD,
                    ContinousButton.LeftThumbStick => GleeCore.Input.GamePads[GAMEPAD_IDX].IsButtonDown(Buttons.LeftThumbstickDown),
                    ContinousButton.RightThumbStick => GleeCore.Input.GamePads[GAMEPAD_IDX].IsButtonDown(Buttons.RightThumbstickDown),
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
                ButtonType.Key => GleeCore.Input.Keyboard.WasKeyJustPressed(key),
                ButtonType.Mouse => GleeCore.Input.Mouse.WasButtonJustPressed(mouse),
                ButtonType.Gamepad => GleeCore.Input.GamePads[GAMEPAD_IDX].WasButtonJustPressed(gamepad),
                ButtonType.Continous => continousButton switch
                {
                    ContinousButton.None => throw new Exception("Using non-initialized continous buttons"),
                    ContinousButton.LeftTrigger => GleeCore.Input.GamePads[GAMEPAD_IDX].WasLeftTriggerJustDown(),
                    ContinousButton.RightTrigger => GleeCore.Input.GamePads[GAMEPAD_IDX].WasRightTriggerJustDown(),
                    ContinousButton.LeftThumbStick => GleeCore.Input.GamePads[GAMEPAD_IDX].WasButtonJustPressed(Buttons.LeftThumbstickDown),
                    ContinousButton.RightThumbStick => GleeCore.Input.GamePads[GAMEPAD_IDX].WasButtonJustReleased(Buttons.RightThumbstickDown),
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
                ButtonType.Key => GleeCore.Input.Keyboard.WasKeyJustReleased(key),
                ButtonType.Mouse => GleeCore.Input.Mouse.WasButtonJustReleased(mouse),
                ButtonType.Gamepad => GleeCore.Input.GamePads[GAMEPAD_IDX].WasButtonJustReleased(gamepad),
                ButtonType.Continous => continousButton switch
                {
                    ContinousButton.None => throw new Exception("Using non-initialized continous buttons"),
                    ContinousButton.LeftTrigger => GleeCore.Input.GamePads[GAMEPAD_IDX].WasLeftTriggerJustUp(),
                    ContinousButton.RightTrigger => GleeCore.Input.GamePads[GAMEPAD_IDX].WasRightTriggerJustUp(),
                    ContinousButton.LeftThumbStick => GleeCore.Input.GamePads[GAMEPAD_IDX].WasButtonJustReleased(Buttons.LeftThumbstickDown),
                    ContinousButton.RightThumbStick => GleeCore.Input.GamePads[GAMEPAD_IDX].WasButtonJustReleased(Buttons.RightThumbstickDown),
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
                ButtonType.Key => GleeCore.Input.Keyboard.IsKeyDown(key) ? 1 : 0,
                ButtonType.Mouse => GleeCore.Input.Mouse.IsButtonDown(mouse) ? 1 : 0,
                ButtonType.Gamepad => GleeCore.Input.GamePads[GAMEPAD_IDX].IsButtonDown(gamepad) ? 1 : 0,
                ButtonType.Continous => continousButton switch
                {
                    ContinousButton.None => throw new Exception("Using non-initialized continous buttons"),
                    ContinousButton.LeftTrigger => GleeCore.Input.GamePads[GAMEPAD_IDX].LeftTrigger,
                    ContinousButton.RightTrigger => GleeCore.Input.GamePads[GAMEPAD_IDX].RightTrigger,
                    ContinousButton.LeftThumbStick => GleeCore.Input.GamePads[GAMEPAD_IDX].LeftThumbStick.X,
                    ContinousButton.RightThumbStick => GleeCore.Input.GamePads[GAMEPAD_IDX].RightThumbStick.X,
                    _ => throw new ArgumentOutOfRangeException("Non existing continous button")
                },
                _ => throw new ArgumentOutOfRangeException("Non existing button type")
            };
        }
    }

    public Vector2 Value2D
    {
        get
        {
            return buttonType switch
            {
                ButtonType.None => throw new Exception("Using non-initialized generic buttons"),
                ButtonType.Key => GleeCore.Input.Keyboard.IsKeyDown(key) ? Vector2.One : Vector2.Zero,
                ButtonType.Mouse => GleeCore.Input.Mouse.IsButtonDown(mouse) ? Vector2.One : Vector2.Zero,
                ButtonType.Gamepad => GleeCore.Input.GamePads[GAMEPAD_IDX].IsButtonDown(gamepad) ? Vector2.One : Vector2.Zero,
                ButtonType.Continous => continousButton switch
                {
                    ContinousButton.None => throw new Exception("Using non-initialized continous buttons"),
                    ContinousButton.LeftTrigger => GleeCore.Input.GamePads[GAMEPAD_IDX].LeftTrigger * Vector2.One,
                    ContinousButton.RightTrigger => GleeCore.Input.GamePads[GAMEPAD_IDX].RightTrigger * Vector2.One,
                    ContinousButton.LeftThumbStick => GleeCore.Input.GamePads[GAMEPAD_IDX].LeftThumbStick,
                    ContinousButton.RightThumbStick => GleeCore.Input.GamePads[GAMEPAD_IDX].RightThumbStick,
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


