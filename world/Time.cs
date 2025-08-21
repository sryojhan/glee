using System;

namespace Glee;



public class Time
{
    public float speed { get; set; } = 1;


    /*
        Enlapsed time since the last frame. When called from the physics loop, it takes the value of the fixedDeltaTime
    */
    public float deltaTime { get; internal set; } = 0;
    public float physicsDeltaTime { get; internal set; } = 0;
    public float activeTime { get; internal set; } = 0;


    public float realDeltaTime { get; internal set; } = 0;
    public float realPhysicsDeltaTime { get; internal set; } = 0;
    public float realActiveTime { get; internal set; } = 0;


    public uint frame { get; internal set; } = 0;
    public uint physicsFrame { get; internal set; } = 0;


    public float fps
    {
        get
        {
            if (realDeltaTime > 0) return (int)MathF.Round(1.0f / realDeltaTime);
            return (int)MathF.Round(Engine.GleeCore.TargetFrameRate);
        }
    }

}



