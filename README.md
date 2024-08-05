# ModdingToolDeveloper

## Table of Contents
- [Overview](#overview)
- [Folder Structure](#folder-structure)
  - [AddressableAssetsData and TextMesh Pro](#addressableassetsdata-and-textmesh-pro)
  - [Materials](#materials)
  - [Prefabs](#prefabs)
  - [Scenes](#scenes)
  - [Scripts](#scripts)
- [Scripts and Scenes Setup](#scripts-and-scenes-setup)
  - [Start Menu Scene](#start-menu-scene)
    - [Canvas](#canvas)
    - [ModManager](#modmanager)
    - [LoadModsToTheList](#loadmodstothelist)
    - [PreviewManager](#previewmanager)
    - [LoadAssetsFromBundle](#loadassetsfrombundle)
    - [DontDestroyOnLoadScript](#dontdestroyonloadscript)
  - [Preview Scene](#preview-scene)
  - [Game Scene](#game-scene)

## Overview

ModdingToolDeveloper is a Unity-based toolkit designed to facilitate the creation, management, and integration of mods into your Unity projects. This tool provides a structured approach to handling modifiable game assets making it easier for developers to support user-generated content and modifications.

## Folder Structure

Upon setup, you will notice several folders in your Assets directory:

![Screenshot 2024-07-17 182823](https://github.com/user-attachments/assets/ea1adbf2-8dd4-41cc-99e0-28c3cd8c21aa)

### AddressableAssetsData And TextMesh Pro

These two folders are Unity packages. If you don't have them, you can download them from the package manager. AddressableAssetsData is used for managing game assets in a flexible way, allowing for easier loading and unloading of assets. TextMesh Pro provides advanced text rendering capabilities.

![Screenshot 2024-07-17 182759](https://github.com/user-attachments/assets/5f799d2b-9e88-4dff-a8b7-2f3f721aecb6)

### Materials

This folder contains materials prepared for objects used in the scene for demonstration purposes. These materials can be customized or replaced based on the specific needs of your modding project.

![Screenshot 2024-07-17 193426](https://github.com/user-attachments/assets/a83ea283-b1f0-4f7d-8748-2d4246ca63f5)

### Prefabs

- **Capsule And Sphere**: Contains prefabs used in the scene for demonstration purposes.
- **Render Texture**: Used by the camera in the "Target Texture" field on the preview scene and by the "Preview Manager" script to display the selected object in the scene.
- **Assets Panel Prefab**: An object prefab with different UI elements used to create panel instances for each compatible mod.
- **Button Asset Prefab**: A button prefab used to create corresponding buttons for each game object that can be modded. 
- **Mod Button Prefab**:  A button prefab used to create corresponding buttons for each mod.
  
![Screenshot 2024-07-17 194545](https://github.com/user-attachments/assets/760d3ef8-8573-4145-98d6-1638ff8fd3d3)

### Scenes

- **Game Scene**: The main game scene that will play when you press start.
- **Preview Scene**: Used to display objects additively.
- **Start Menu Scene**: The game's start menu, where you can enable the mods.

![Screenshot 2024-07-17 224812](https://github.com/user-attachments/assets/ed27c7f2-e40b-4c8b-98ec-8f428fce148d)

### Scripts

This folder contains all the necessary scripts for the modding functionality. These scripts manage loading mods, integrating them into the game, and displaying them in the appropriate scenes.

![Screenshot 2024-07-17 224822](https://github.com/user-attachments/assets/068eba0d-bf95-4015-8a0f-957991de34a9)

## Scripts And Scenes Setup

### Start Menu Scene

Υour hierarchy should be something like this:

![Screenshot 2024-07-19 115328](https://github.com/user-attachments/assets/419e1c00-b572-419a-8987-334372d8c9cb)

#### Canvas

The Canvas object serves as the root for all UI elements in the start menu.

![Screenshot 2024-07-19 121304](https://github.com/user-attachments/assets/211bb655-afa7-4978-81a8-545dd188734a)

#### ModManager

This script manages the list of mods available in the game. It handles loading mod data, enabling or disabling mods, and integrating them into the game.

#### LoadModsToTheList

This script populates the UI with the list of available mods. It ensures that each mod is represented by a corresponding button in the UI.

![Screenshot 2024-07-19 121344](https://github.com/user-attachments/assets/1959f5d1-966d-4710-a0ba-f6bb73e759ed)

#### PreviewManager

This script handles the display of objects in the preview scene. It uses the Render Texture prefab to show selected objects from the mod list.

![Screenshot 2024-07-19 121432](https://github.com/user-attachments/assets/c9f48448-7aa9-4d7b-8b09-02024bdf5a9e)

#### LoadAssetsFromBundle

This script is responsible for managing which objects in your game can be replaced by mods and creates corresponding buttons for each game object. 

![Screenshot 2024-07-19 121607](https://github.com/user-attachments/assets/3c21a0c9-23bd-40f2-8fde-ff2eed171580)

#### DontDestroyOnLoadScript

This script ensures that specific objects persist across scene loads. It is essential for maintaining the state of modded content.

### Preview Scene

The Preview Scene is designed to show selected objects from the mod list. It uses the Render Texture prefab to display these objects in a controlled environment.

### Game Scene

The Game Scene is the primary gameplay scene. When mods are enabled, this scene will load the necessary assets and integrate them into the gameplay.

![Screenshot 2024-07-19 120546](https://github.com/user-attachments/assets/e2d815f4-00fb-4c1c-a6e4-837a383e60e7)
 
