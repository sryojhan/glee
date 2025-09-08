using System;
namespace Glee.Engine;



//TODO: try catch before each update call in components, entities and worlds
public class GleeError : Exception
{
    //TODO: 3 Modes: Strict, Normal, Permissive
    public enum ToleranceMode
    {
        Permissive, Normal, Strict
    }

    public static ToleranceMode Tolerance { get; set; }

    public static bool Verbose { get; set; } = true;


    public record class ErrorContent { public ErrorType Type { get; init; } public string Message { get; init; } }
    public static ErrorContent Last { get; private set; } = new(){Type = ErrorType.None, Message = ""};

    public enum ErrorType
    {
        None, Generic, AssetNotFount, ResourceAlreadyExists, ResourceTypeMismatch, InvalidInitialization, ResourceFactoryNotFound, InvalidGleeObject
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
    
    public static void Try(Callback func)
    {
        try
        {
            func?.Invoke();
        }
        catch (Exception exc)
        {
            Services.Fetch<Log>().Error(exc.Message);
        }
    }

    public static void Throw(string message, ErrorType type = ErrorType.Generic)
    {
        Last = new() { Message = message, Type = type };

        if (Tolerance != ToleranceMode.Permissive)
            throw new GleeError(message, type);

        if (Verbose)
            Services.Fetch<Log>().Error(message);
    }


    public static void InvalidGleeObject()
    {
        Throw("Tried to access a GleeObject that has already been cleared", ErrorType.InvalidGleeObject);
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
