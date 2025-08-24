using System.Collections.Generic;
using Glee.Attributes;
using Glee.Behaviours;
using Glee.Components;

namespace Glee.Graphics;


public class Animation
{
    public string Name { get; set; }
    public ITexture[] Frames { get; private set; }
    public int FrameCount => Frames.Length;

    /// <summary>
    /// Speed measured in Frames per seconds
    /// </summary>
    public float BaseSpeed { get; private set; }

    public Animation(string name, ICollection<ITexture> frames, float speed)
    {
        Name = name;
        Frames = [.. frames];
        BaseSpeed = speed;
    }
}