using Glee.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Glee.Graphics;


public class Renderer
{
    private GraphicsDeviceManager graphics;
    private GraphicsDevice graphicsDevice;
    private SpriteBatch spriteBatch;


    private static Renderer instance => GleeCore.Renderer;


    public Renderer(int width, int height, bool fullScreen, float targetFrameRate)
    {
        graphics = new GraphicsDeviceManager(GleeCore.Instance);

        graphicsDevice = null;
        spriteBatch = null;


        graphics.PreferredBackBufferWidth = width;
        graphics.PreferredBackBufferHeight = height;

        graphics.IsFullScreen = fullScreen;

        GleeCore.Instance.IsFixedTimeStep = true;
        GleeCore.Instance.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / targetFrameRate);

        graphics.SynchronizeWithVerticalRetrace = true;

        graphics.ApplyChanges();
    }

    public void Initialise()
    {
        graphicsDevice = GleeCore.Instance.GraphicsDevice;
        spriteBatch = new SpriteBatch(graphicsDevice);
    }

    public static void BeginBatch()
    {
        instance.spriteBatch.Begin(samplerState: SamplerState.PointClamp);
    }

    public static void EndBatch()
    {
        instance.spriteBatch.End();
    }

    public static void Clear(Color color)
    {
        instance.graphicsDevice.Clear(color);
    }

    public static void Clear()
    {
        Clear(Color.Black);
    }


    public static void Render(Texture2D texture, Vector2 position, Vector2 size, Rectangle? sourceRectangle = null, Color? color = null, float rotation = 0)
    {
        if (!color.HasValue) color = Color.White;

        Vector2 centerPoint = new Vector2(texture.Width, texture.Height) * 0.5f;

        float targetSizeX = size.X / texture.Width;
        float targetSizeY = size.Y / texture.Height;

        instance.spriteBatch.Draw(

            texture, position, sourceRectangle, color.Value, rotation, centerPoint, new Vector2(targetSizeX, targetSizeY), SpriteEffects.None, 0
        );
    }
}