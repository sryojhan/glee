using System;

namespace Glee;



public class Time
{
    public float speed { get; set; } = 1;
    public float deltaTime { get; internal set; } = 0;
    public float physicsDeltaTime { get; internal set; } = 0;
    public float activeTime { get; internal set; } = 0;


    public float realDeltaTime { get; internal set; } = 0;
    public float realPhysicsDeltaTime { get; internal set; } = 0;
    public float realActiveTime { get; internal set; } = 0;


    public uint frameCounter { get; internal set; } = 0;
    public uint physicsFrameCounter { get; internal set; } = 0;


    public float fps
    {
        get
        {
            if (realDeltaTime > 0) return (int)MathF.Round(1.0f / realDeltaTime);
            return (int)MathF.Round(Engine.GleeCore.TargetFrameRate);
        }
    }

}



