using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using Glee.Engine;
using Microsoft.Xna.Framework.Content;

namespace Glee.Assets.Text;


public class JSON : TextAsset
{
    private JsonNode jsonNode;
    public JsonNode Query => jsonNode;

    private JSON(string name, string value, JsonNode json) : base(name, value)
    {
        jsonNode = json;
    }

    public new static JSON Create(string name)
    {
        TextAsset textAsset = TextAsset.Create(name);

        if (!textAsset) return null;

        try
        {
            JsonNode json = JsonNode.Parse(textAsset.Content);
            return new JSON(textAsset.Name, textAsset.Content, json);
        }
        catch (ArgumentNullException)
        {
            //TODO: custom error types
            GleeError.InvalidInitialization("JSON [text content is null]");
            return null;
        }
        catch (JsonException)
        {
            GleeError.InvalidInitialization("JSON [not a valid JSON]");
            return null;
        }

    }



    public TargetClass Cast<TargetClass>() where TargetClass : class
    {
        try
        {
            return JsonSerializer.Deserialize<TargetClass>(jsonNode);
        }
        catch (JsonException)
        {
            GleeError.InvalidInitialization($"JSON [cannot cast to class {typeof(TargetClass).Name}]");
            return null;
        }
    }

}