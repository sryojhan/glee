using Glee.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Glee.Graphics;


public class Renderer
{
    //TODO: support to render in a texture
    private GraphicsDeviceManager graphics;
    private GraphicsDevice graphicsDevice;
    private SpriteBatch spriteBatch;


    private static Renderer instance => GleeCore.Renderer;


    //TODO: read all this from a file
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


    public static void Present()
    {

    }

    public static void Render(Texture2D texture, Vector2 position, float rotation = 0)
    {
        Vector2 centerPoint = new Vector2(texture.Width, texture.Height) * 0.5f;

        instance.spriteBatch.Draw(

            texture, position, null, Color.White, rotation, centerPoint, Vector2.One, SpriteEffects.None, 0
        );
    }

    public static void RenderAbsolute()
    {

    }

}