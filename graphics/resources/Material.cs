using System;
using Glee.Engine;

namespace Glee.Graphics;



public class Material : GleeResource
{
    const string DefaultMaterialSuffix = "_mat";

    public Shader ShaderSource { get; private set; }
    public Color MainColor { get; set; }

    protected override IDisposable DisposableObj => null;

    private Material(string name, Shader shader) : base(name)
    {
        Name = name;
        ShaderSource = shader;
        MainColor = Color.White;
    }

    public static Material Create()
    {
        return Default();
    }

    /// <summary>
    /// Loads the shader with the given name and returns a new Material that references it. The material name will have "_mat" suffix
    /// </summary>
    /// <param name="shaderName">Name of the shader to load</param>
    public static Material Create(string shaderName)
    {
        if (string.IsNullOrWhiteSpace(shaderName))
        {
            GleeError.InvalidInitialization($"Material [{shaderName} is null of empty]");
            return null;
        }

        Shader shader = Load<Shader>(shaderName);

        if (!shader) return null;

        return new Material(shaderName + DefaultMaterialSuffix, shader);
    }


    /// <summary>
    /// Creates a material from the given shader. The material name will have "_mat" suffix
    /// </summary>
    /// <param name="shader">Name of the shader to load</param>
    public static Material Create(Shader shader)
    {
        if (shader == null)
        {
            GleeError.InvalidInitialization($"Material [{shader} is null]");
            return null;
        }

        return new Material(shader.Name + DefaultMaterialSuffix, shader);
    }

    /// <summary>
    /// Creates a material from the given shader.
    /// </summary>
    /// <param name="shader">Name of the shader to load</param>
    public static Material Create(string name, Shader shader)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            GleeError.InvalidInitialization($"Material [{name} is null of empty]");
            return null;
        }

        if (shader == null)
        {
            GleeError.InvalidInitialization($"Material [{shader} is null]");
            return null;
        }

        return new Material(name, shader);
    }


    /// <summary>
    /// Returns a material with no assigned shader
    /// </summary>
    /// <returns></returns>
    public static Material Default(string name = "default")
    {
        return new Material(name, null);
    }


    public bool HasCustomShader
    {
        get
        {
            return ShaderSource != null;
        }
    }

}