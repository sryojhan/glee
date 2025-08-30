using Glee.Attributes;
using Glee.Behaviours;
using Glee.Components;
using Glee.Engine;
using Glee.Graphics;


namespace Glee.Components;


[DependsOn(typeof(Image))]
public class Flipbook : ComponentRaw, IUpdatable, IInitializable
{
    public Animation animation { get; set; }

    public enum Mode
    {
        Loop, Single, Reverse, ReverseLoop, PingPong
    }

    public Mode PlayMode { get; set; } = Mode.Loop;
    public int CurrentFrame { get; set; }
    public int TotalFrames => animation.FrameCount;
    public bool Playing { get; set; } = true;

    public float SpeedMultiplier { get; set; } = 1;

    private Image image;
    public Image Target
    {
        get => image;
        init { image = value; }
    }
    private float frameTimer = 0;
    private bool pingPongDirection = true;

    public void Initialize()
    {
        image ??= TryGetComponent<Image>();
    }

    public void Update()
    {
        if (animation == null) return;
        if (Target == null) return;

        if (!Playing) return;

        frameTimer += Time.deltaTime * SpeedMultiplier * animation.BaseSpeed;

        if (frameTimer > 1.0f)
        {
            frameTimer = 0;
            AdvanceFrame();
        }
    }

    public void AdvanceFrame()
    {
        if (animation == null) return;
        if (Target == null) return;


        if (TotalFrames == 1)
        {
            CurrentFrame = 0;
        }
        else
        {
            switch (PlayMode)
            {
                case Mode.Reverse:
                case Mode.Single:
                    {
                        if (CurrentFrame < TotalFrames - 1)
                            CurrentFrame++;

                        break;
                    }
                case Mode.ReverseLoop:
                case Mode.Loop:
                    {
                        CurrentFrame = (CurrentFrame + 1) % TotalFrames;
                        break;
                    }

                case Mode.PingPong:
                    {

                        if (pingPongDirection)
                        {
                            CurrentFrame++;

                            if (CurrentFrame == TotalFrames - 1)
                                pingPongDirection = false;
                        }
                        else
                        {

                            CurrentFrame--;

                            if (CurrentFrame == 0)
                                pingPongDirection = true;
                        }

                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }



        bool reversed = PlayMode == Mode.Reverse || PlayMode == Mode.ReverseLoop;

        int targetFrame = !reversed ? CurrentFrame : TotalFrames - CurrentFrame - 1;
        Target.texture = animation.Frames[targetFrame];
    }
}