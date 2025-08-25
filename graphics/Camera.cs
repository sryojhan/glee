using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Glee.Graphics;


public class Camera
{
    public Vector2 Position { get; set; }
    public float Rotation { get; set; } = 0;
    public float Zoom { get; set; } = 1;


    public enum CameraScalingMode
    {
        ConstantPixelsPerUnit, ViewportRelative
    }

    public CameraScalingMode ScalingMode { get; set; }

    public float PixelsPerUnit { get; set; } = 64;
    public float Size { get; set; } = 10;

    public float ActiveCameraScale
    {
        get
        {
            float ppu = PixelsPerUnit;

            if (ScalingMode == CameraScalingMode.ViewportRelative)
                ppu = ScreenWidth / Size;

            return ppu;
        }
    }

    public float ScreenWidth => viewport.Width;
    public float ScreenHeight => viewport.Height;
    public float AspectRatio => viewport.AspectRatio;


    public Matrix ViewMatrix { get; private set; }


    private Viewport viewport;

    public Camera(Viewport viewport)
    {
        this.viewport = viewport;
    }

    public void UpdateMatrix()
    {

        float ppu = ActiveCameraScale;


        ViewMatrix =
        Matrix.CreateTranslation(new Vector3(-Position, 0f))
        * Matrix.CreateRotationZ(Rotation)
        * Matrix.CreateScale(ppu * Zoom, ppu * Zoom, 1)
        * Matrix.CreateTranslation(ScreenWidth * 0.5f, ScreenHeight * 0.5f, 0.0f)
        ;
    }
}