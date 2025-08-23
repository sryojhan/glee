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

    public static void BeginBatchWithCustomShader(Material material)
    {
        instance.spriteBatch.Begin(samplerState: SamplerState.PointClamp, effect: material.ShaderSource.effect);
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


    public static void Render(ITexture texture, Vector2 position, Vector2 size, Rectangle? sourceRectangle = null, float rotation = 0, Material material = null)
    {
        Color color = material != null ? material.MainColor : Color.White;

        Vector2 centerPoint = new Vector2(texture.Width, texture.Height) * 0.5f;

        float targetSizeX = size.X / texture.Width;
        float targetSizeY = size.Y / texture.Height;

        bool hasShader = material != null && material.HasCustomShader;

        if (hasShader)
        {
            EndBatch();
            BeginBatchWithCustomShader(material);
        }

        instance.spriteBatch.Draw(

            texture.BaseTexture, position, sourceRectangle, color, rotation, centerPoint, new Vector2(targetSizeX, targetSizeY), SpriteEffects.None, 0
        );

        if (hasShader)
        {
            EndBatch();
            BeginBatch();
        }
    }

    public static void RenderText(string text, Font font, Vector2 position, float rotation = 0)
    {
        Vector2 size = font.CalculateWidth(text);

        instance.spriteBatch.DrawString(
            spriteFont: font.font,
            text: text,
            position: position,
            color: Color.White,
            rotation: rotation,
            origin: size * 0.5f,
            scale: Vector2.One,
            effects: SpriteEffects.None,
            layerDepth: 0
        );
    }

}