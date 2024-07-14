using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

/// <summary>
/// Handles the management of mod packages, including loading, evaluation for compatibility and activation.
/// Providing methods for accessing the lists of mod packages.  
/// Performs the process of evaluation of a given path and return the proper one;
/// </summary>
public class ModManager : MonoBehaviour
{
    /// <summary> Singleton instance of this script. </summary>
    public static ModManager Instance;

    /// <summary> Path to the Mods folder. </summary>
    private string _ModsFolderPath;
    /// <summary> List to save all loaded mod packages. </summary>
    private List<ModPackage> _AllMods = new List<ModPackage>();
    /// <summary> List to save all loaded mods folder name. </summary>
    private List<string> _AllModsDirectories = new List<string>();
    /// <summary> List to save all compatible mod packages.</summary>
    private List<int> _CompatibleMods = new List<int>();
    /// <summary> List to save all incompatibles mod packages. </summary>
    private List<int> _IncompatibleMods = new List<int>();


    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(this); }
        Instance = this;

        // Set the mods folder path and create the directory if it doesn't exist
        _ModsFolderPath = Path.Combine(Application.dataPath, "AssetBundles");
        if (!Directory.Exists(_ModsFolderPath)) Directory.CreateDirectory(_ModsFolderPath);

        // Read and deserialize JSON files from the mods folder path into the _AllMods list
        ReadAndDeserializeJsonFiles(_ModsFolderPath, ref _AllMods);
        // Evaluate the mod packages and categorize them into compatible and incompatible mods
        LoadAndEvaluateModPackages(ref _AllMods, ref _IncompatibleMods, ref _CompatibleMods);
    }

    /// <summary>
    /// Loads and evaluates mod packages, categorizing them into compatible and incompatible lists based on version compatibility and validation procedure.
    /// </summary>
    /// <param name="_allMods"> A reference to the list of allMods we want to check and add into other lists. </param>
    /// <param name="_incombatibleMods"> A reference to the list of incompatible mod to fill. </param>
    /// <param name="_combatibleMods"> A reference to the list of compatible mod to fill. </param>
    private void LoadAndEvaluateModPackages(ref List<ModPackage> _allMods, ref List<int> _incombatibleMods, ref List<int> _combatibleMods)
    {
        for (int i = 0; i < _allMods.Count; i++)
        {
            if (ValidateDependency(_allMods[i].UnityVersion))
            { _combatibleMods.Add(i); }
            else
            { _incombatibleMods.Add(i); }
        }
    }

    /// <summary>
    /// Validates if the provided Unity version matches the application's Unity version.
    /// </summary>
    /// <param name="_supported"> The Unity version to check against the application's Unity version. </param>
    private bool ValidateDependency(string _supported)
    {
        if (_supported == Application.unityVersion) { return true; }
        return false;
    }

    /// <summary>
    /// Read and deserialize json files within the specified folder path, filling out the provided list with mod packages.
    /// Gets the directory name containing the JSON file and adds it to allModsdirectorieslist.
    /// </summary>
    /// <param name="_folderPath"> The path to the Mods folder that contains the subfolders that contains the JSON files. </param>
    /// <param name="_allMods"> A reference to the list of mod packages to fill. </param>
    /// <exception cref="Exception"> Thrown when a mod package contains more than one JSON file. </exception>
    private void ReadAndDeserializeJsonFiles(string _folderPath, ref List<ModPackage> _allMods)
    {
        if (!Directory.Exists(_folderPath)) return;

        string[] subdirectories = Directory.GetDirectories(_folderPath);

        for (int i = 0; i < subdirectories.Length; i++)
        {
            string[] jsonFile = Directory.GetFiles(subdirectories[i], "*.json");

            if (jsonFile.Length > 1)
            { throw new Exception("Each mod package should contain only one json file!"); }

            if (jsonFile.Length > 0)
            {
                string directoryPath = Path.GetFileName(Path.GetDirectoryName(jsonFile[0]));
                _AllModsDirectories.Add(directoryPath);

                string jsonContent = File.ReadAllText(jsonFile[0]);
                ModPackage modPackage = JsonConvert.DeserializeObject<ModPackage>(jsonContent);
                _allMods.Add(modPackage);
            }
        }
    }

    /// <summary>
    /// Retrieves a read-only collection of all available mod packages.
    /// </summary>
    /// <returns> A collection of all available mod packages. </returns>
    public IReadOnlyCollection<ModPackage> GetAllMods()
    {
        ModPackage[] allMod = new ModPackage[_AllMods.Count];

        for (int i = 0; i < allMod.Length; i++)
        {
            allMod[i] = _AllMods[i];
        }

        return allMod;
    }

    /// <summary>
    ///  Retrieves a read-only collection of mod packages that are compatible based on their index within the 'allMods' collection. 
    /// </summary>
    /// <returns>  A read-only collection of mod packages that are compatible. </returns>
    public IReadOnlyCollection<ModPackage> GetCompatibleMods()
    {
        ModPackage[] compatibleMod = new ModPackage[_CompatibleMods.Count];

        for (int i = 0; i < compatibleMod.Length; i++)
        {
            int compatblesModIndex = _CompatibleMods[i];
            compatibleMod[i] = _AllMods[compatblesModIndex];
        }

        return compatibleMod;
    }

    /// <summary>
    /// Retrieves a read-only collection of mod packages that are incompatible based on their index within the 'allMods' collection 
    /// </summary>
    /// <returns> A read-only collection of mod packages that are incompatible. </returns>
    public IReadOnlyCollection<ModPackage> GetIncompatibleMods()
    {
        ModPackage[] incompatibleMod = new ModPackage[_IncompatibleMods.Count];

        for (int i = 0; i < incompatibleMod.Length; i++)
        {
            int incompatblesModIndex = _IncompatibleMods[i];
            incompatibleMod[i] = _AllMods[incompatblesModIndex];
        }

        return incompatibleMod;
    }
}