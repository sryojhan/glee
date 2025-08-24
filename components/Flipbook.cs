using Glee.Attributes;
using Glee.Behaviours;
using Glee.Components;
using Glee.Graphics;


namespace Glee.Components;


[DependsOn(typeof(Image))]
public class Flipbook : Component, IUpdatable, IInitializable
{
    public Animation animation { get; set; }

    public enum Mode
    {
        Loop, Single, Reverse, ReverseLoop, PingPong
    }

    public Mode PlayMode { get; set; } = Mode.Loop;
    public int CurrentFrame { get; set; }
    public int TotalFrames => animation.FrameCount;
    public bool Playing { get; set; }

    public float SpeedMultiplier { get; set; } = 1;

    private Image target;
    private float frameTimer = 0;
    private bool pingPongDirection = true;

    public void Initialize()
    {
        target = GetComponent<Image>();
    }

    public void Update()
    {
        if (animation == null) return;
        if (target == null) return;

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
        if (target == null) return;


        if (TotalFrames == 1)
        {
            CurrentFrame = 0;
        }
        else
        {
            switch (PlayMode)
            {
                case Mode.Single:
                    {
                        if (CurrentFrame < TotalFrames - 1)
                            CurrentFrame++;

                        break;
                    }
                case Mode.Loop:
                    {
                        CurrentFrame = (CurrentFrame + 1) % TotalFrames;
                        break;
                    }
                case Mode.Reverse:
                    {

                        if (CurrentFrame > 0)
                            CurrentFrame--;

                        break;
                    }
                case Mode.ReverseLoop:
                    {

                        CurrentFrame--;

                        if (CurrentFrame == 0)
                            CurrentFrame = TotalFrames - 1;

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


        target.texture = animation.Frames[CurrentFrame];

    }
}