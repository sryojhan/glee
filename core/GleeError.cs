using System;
namespace Glee.Engine;



//TODO: try catch before each update call in components, entities and worlds
public class GleeError : Exception
{
    public static bool StrictMode { get; set; } = false;
    public static bool Verbose { get; set; } = true;

    public static ErrorType Last { get; private set; } = ErrorType.None;

    public enum ErrorType
    {
        None, Generic, AssetNotFount, ResourceAlreadyExists, ResourceTypeMismatch, InvalidInitialization, ResourceFactoryNotFound
    }

    public ErrorType Error { get; private set; }

    public GleeError(string errorMessage, ErrorType errorType = ErrorType.Generic) : base(errorMessage)
    {
        Error = errorType;
    }


    public static void Throw()
    {
        Throw("Unknown error.", ErrorType.Generic);
    }

    public static void Throw(string message, ErrorType type = ErrorType.Generic)
    {
        Last = type;

        if (StrictMode)
            throw new GleeError(message, type);

        if (Verbose)
            Services.Fetch<Log>().Error(message);
    }


    public static void AssetNotFound(string asset = "", string path = null)
    {
        string message = $"Failed to load asset '{asset}'.";

        if (!string.IsNullOrEmpty(path))
            message += $"Check if the asset exists in the '{path}' folder";

        Throw(message, ErrorType.AssetNotFount);
    }

    public static void ResourceAlreadyExists(string resource = "")
    {
        Throw($"Resource with name '{resource}' already exists.", ErrorType.ResourceAlreadyExists);
    }

    public static void ResourceTypeMismatch(string expected, string value)
    {
        Throw($"Tried to load a '{expected}' resource as a '{value}'");
    }

    public static void ResourceFactoryNotFound(string resourceType)
    {
        Throw($"Tried to create and instance of {resourceType} but no matching factory could be found", ErrorType.ResourceFactoryNotFound);
    }

    public static void InvalidInitialization(string className)
    {
        Throw($"Invalid initialization of '{className}'", ErrorType.InvalidInitialization);
    }
}
