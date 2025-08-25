using Glee.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Glee.Graphics;


public class Renderer
{
    internal GraphicsDeviceManager graphics;
    internal GraphicsDevice graphicsDevice;
    internal SpriteBatch spriteBatch;


    private static Renderer instance => GleeCore.Renderer;

    private TargetTexture currentTargetTexture = null;

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
        instance.graphicsDevice.SetRenderTarget(
            instance.currentTargetTexture != null ?
            instance.currentTargetTexture.BaseTexture as RenderTarget2D :
            null
        );

        Camera current = GleeCore.WorldManager.Spotlight.Camera;
        instance.spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: current.ViewMatrix);
    }

    public static void BeginBatchWithCustomShader(Material material)
    {
        instance.graphicsDevice.SetRenderTarget(
            instance.currentTargetTexture != null ?
            instance.currentTargetTexture.BaseTexture as RenderTarget2D :
            null
        );

        Camera current = GleeCore.WorldManager.Spotlight.Camera;
        instance.spriteBatch.Begin(
            samplerState: SamplerState.PointClamp,
            effect: material.ShaderSource.effect,
            transformMatrix: current.ViewMatrix
        );
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
        position = AdjustPosition(position);

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
        position = AdjustPosition(position);

        float scale = 1.0f / GleeCore.WorldManager.Spotlight.Camera.ActiveCameraScale;

        Vector2 size = font.CalculateWidth(text);

        instance.spriteBatch.DrawString(
            spriteFont: font.font,
            text: text,
            position: position,
            color: Color.White,
            rotation: rotation,
            origin: size * 0.5f,
            scale: Vector2.One * scale,
            effects: SpriteEffects.None,
            layerDepth: 0
        );
    }

    public static void SetTargetTexture(TargetTexture texture)
    {
        EndBatch();

        instance.currentTargetTexture = texture;
        instance.graphicsDevice.SetRenderTarget(instance.currentTargetTexture.BaseTexture as RenderTarget2D);
        instance.graphicsDevice.Clear(Color.Yellow);

        BeginBatch();
    }


    public static void RemoveTargetTexture()
    {
        EndBatch();

        instance.currentTargetTexture = null;
        instance.graphicsDevice.SetRenderTarget(null);

        BeginBatch();
    }


    internal static void SetTargetTextureForScreenshot(TargetTexture texture)
    {
        instance.currentTargetTexture = texture;
        instance.graphicsDevice.SetRenderTarget(instance.currentTargetTexture.BaseTexture as RenderTarget2D);
    }


    internal static void RemoveTargetTextureForScreenshot()
    {
        instance.currentTargetTexture = null;
        instance.graphicsDevice.SetRenderTarget(null);
    }



    public static Vector2 AdjustPosition(Vector2 position)
    {
        position.Y *= -1;
        return position;
    }

    public Viewport Viewport => graphicsDevice.Viewport;
}