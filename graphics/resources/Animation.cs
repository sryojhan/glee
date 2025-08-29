using System;
using System.Collections.Generic;
using Glee.Attributes;
using Glee.Behaviours;
using Glee.Components;
using Glee.Engine;
using Microsoft.Xna.Framework.Content;

namespace Glee.Graphics;


public class Animation : GleeResource
{
    public ITexture[] Frames { get; private set; }
    public int FrameCount => Frames.Length;

    /// <summary>
    /// Speed measured in Frames per seconds
    /// </summary>
    public float BaseSpeed { get; private set; }

    protected override IDisposable DisposableObj => null;


    private Animation(string name, ICollection<ITexture> frames, float speed): base(name)
    {
        Frames = [.. frames];
        BaseSpeed = speed;
    }


    public static Animation Create(string name, ICollection<ITexture> frames, float speed)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            GleeError.InvalidInitialization($"Animation [{nameof(name)} is null or empty]");
        }

        if (frames == null)
        {
            GleeError.InvalidInitialization($"Animation [{nameof(frames)} is null]");
        }

        if (frames.Count == 0)
        {
            GleeError.InvalidInitialization($"Animation [{nameof(frames)} count is 0]");
        }

        if (speed < 0)
        {
            GleeError.InvalidInitialization($"Animation [invalid {nameof(speed)}]");
        }


        return new Animation(name, frames, speed);
    }
}