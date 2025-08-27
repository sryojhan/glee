using System;
using System.Collections.Generic;
using Glee.Attributes;
using Glee.Behaviours;
using Glee.Components;
using Glee.Engine;
using Microsoft.Xna.Framework.Content;

namespace Glee.Graphics;


public class Animation :GleeResource
{
    public ITexture[] Frames { get; private set; }
    public int FrameCount => Frames.Length;

    /// <summary>
    /// Speed measured in Frames per seconds
    /// </summary>
    public float BaseSpeed { get; private set; }

    protected override IDisposable DisposableObj => null;


    public Animation(string name, ICollection<ITexture> frames, float speed)
    {
        Name = name;
        Frames = [.. frames];
        BaseSpeed = speed;
    }

}