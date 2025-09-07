using System;

using Glee.Assets.Text;

namespace Glee;


/// <summary>
/// Represents the data with the initial configuration of the game
/// </summary>
[Serializable]
public class GleeConfiguration : CoreService
{
    public string Title { get; init; } = "My Glee Game";

    public int Width { get; init; } = 500;
    public int Height { get; init; } = 500;
    public bool Fullscreen { get; init; } = false;

    public float TargetFrameRate { get; init; } = 10.0f;


    public bool ExitOnEscape { get; init; } = false;
    public bool IsMouseVisible { get; init; } = true;

    public GleeConfiguration() { }


    public static GleeConfiguration Create()
    {
        JSON data = JSON.Create("config");

        if (data == null) return new GleeConfiguration();

        return data.Cast<GleeConfiguration>();
    }

    //TODO: create a system to load and store custom data
}
