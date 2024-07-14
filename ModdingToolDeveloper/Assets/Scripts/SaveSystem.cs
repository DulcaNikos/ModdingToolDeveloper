using System;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Represents the data of a GameObject for serialization purposes.
/// </summary>
[Serializable]
public class GameObjectData
{
    public string Name { get; set; }
    public Vector3 Position { get; set; }
    public Quaternion Rotation { get; set; }
    public Vector3 Scale { get; set; }


    /// <summary>
    /// Creates a GameObjectData instance from a GameObject.
    /// </summary>
    public static GameObjectData FromGameObject(GameObject gameObject)
    {
        return new GameObjectData
        {
            Name = gameObject.name,
            Position = gameObject.transform.position,
            Rotation = gameObject.transform.rotation,
            Scale = gameObject.transform.localScale
        };
    }
}

/// <summary>
/// Helper class for serialization and deserialization of data.
/// </summary>
public static class SerializationHelper
{
    /// <summary> The directory path where mod data is stored. </summary>
    private static readonly string directoryPath = Path.Combine(Application.dataPath, "ModDataFolder");
    /// <summary> The file path where the serialized mod data is saved. </summary>
    private static readonly string filePath = Path.Combine(directoryPath, "mod_data.json");

    /// <summary>
    /// JSON serialization settings to handle reference loops and format the output.
    /// </summary>
    private static readonly JsonSerializerSettings jsonSettings = new JsonSerializerSettings
    {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        Formatting = Formatting.Indented
    };

    /// <summary>
    /// Saves a dictionary of mod packages and game objects to a JSON file.
    /// </summary>
    public static void SaveDictionary(Dictionary<string, (ModPackage, GameObject)> dict)
    {
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        // Convert the original dictionary to a serializable dictionary
        SerializableDictionary serializableDict = new SerializableDictionary();
        serializableDict.FromOriginalDictionary(dict);

        // Serialize the dictionary to JSON
        string json = JsonConvert.SerializeObject(serializableDict, jsonSettings);
        File.WriteAllText(filePath, json);
    }

    /// <summary>
    /// Loads a dictionary of mod packages and game objects from a JSON file.
    /// </summary>
    /// <returns>The loaded dictionary.</returns>
    public static Dictionary<string, (ModPackage, GameObject)> LoadDictionary()
    {
        if (!File.Exists(filePath))
        {
            return new Dictionary<string, (ModPackage, GameObject)>();
        }

        string json = File.ReadAllText(filePath);

        // Deserialize the JSON to a serializable dictionary
        SerializableDictionary serializedDict = JsonConvert.DeserializeObject<SerializableDictionary>(json, jsonSettings);
        return serializedDict.ToOriginalDictionary();
    }
}

/// <summary>
/// A serializable version of the dictionary to facilitate JSON serialization.
/// </summary>
[Serializable]
public class SerializableDictionary
{
    /// <summary> A list of keys from the original dictionary. </summary>
    public List<string> Keys = new List<string>();
    /// <summary> A list of JSON strings representing serialized ModPackage objects. </summary>
    public List<string> ModPackages = new List<string>();
    /// <summary> A list of GameObjectData objects representing the game objects. </summary>
    public List<GameObjectData> GameObjects = new List<GameObjectData>();

    /// <summary>
    /// Populates the serializable dictionary from the original dictionary.
    /// </summary>
    /// <param name="originalDict">The original dictionary to convert.</param>
    public void FromOriginalDictionary(Dictionary<string, (ModPackage, GameObject)> originalDict)
    {
        foreach (KeyValuePair<string, (ModPackage, GameObject)> obj in originalDict)
        {
            Keys.Add(obj.Key);
            ModPackages.Add(JsonConvert.SerializeObject(obj.Value.Item1));
            GameObjects.Add(GameObjectData.FromGameObject(obj.Value.Item2));
        }
    }

    /// <summary>
    /// Converts the serializable dictionary back to the original dictionary format.
    /// </summary>
    /// <returns>The original dictionary.</returns>
    public Dictionary<string, (ModPackage, GameObject)> ToOriginalDictionary()
    {
        Dictionary<string, (ModPackage, GameObject)> originalDict = new Dictionary<string, (ModPackage, GameObject)>();

        for (int i = 0; i < Keys.Count; i++)
        {
            originalDict[Keys[i]] = (JsonConvert.DeserializeObject<ModPackage>(ModPackages[i]), GameObject.Find(Keys[i]));
        }

        return originalDict;
    }
}