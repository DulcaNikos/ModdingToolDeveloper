# ModdingToolDeveloper

## Overview

## Folder Structure

Upon setup, you will notice several folders in your Assets directory:

![Screenshot 2024-07-17 182823](https://github.com/user-attachments/assets/ea1adbf2-8dd4-41cc-99e0-28c3cd8c21aa)

### AddressableAssetsData And TextMesh Pro

Τhese two folders are unity packages and if you don't have them you can download them from the package manager.

![Screenshot 2024-07-17 182759](https://github.com/user-attachments/assets/5f799d2b-9e88-4dff-a8b7-2f3f721aecb6)

### Materials

This folder contains some materials i prepared for the objects that im using in the scene for demonstation.

![Screenshot 2024-07-17 193426](https://github.com/user-attachments/assets/a83ea283-b1f0-4f7d-8748-2d4246ca63f5)

### Prefabs

- **Capsule And Sphere**: Contains some prefabs that im using in the scene for demonstation.
- **Render Texture**: Is used by the Camera at "Target Texture" field on the preview scene and from "Preview Manager" script to display the selected object in the scene. 
- **Assets Panel Prefab**: An object prefab with different ui elements that is used to create panel instances for each compatible mod.
- **Button Asset Prefab**: A button prefab that is used to create the corresponding button for each gameobject that you can mod. 
- **Mod Button Prefab**: A button prefab that is used to create the corresponding button for each mod. 
  
![Screenshot 2024-07-17 194545](https://github.com/user-attachments/assets/760d3ef8-8573-4145-98d6-1638ff8fd3d3)

### Scenes

- **Game Scene**: The game scene that will play when you press start.
- **Preview Scene**: Is used to display the objects additively
- **Start Menu Scene**: The game start menu, is also where you can enable the mods. 

![Screenshot 2024-07-17 224812](https://github.com/user-attachments/assets/ed27c7f2-e40b-4c8b-98ec-8f428fce148d)

### Scripts

All the Scripts is needed the modding to work , then we will see what to set for each script.

![Screenshot 2024-07-17 224822](https://github.com/user-attachments/assets/068eba0d-bf95-4015-8a0f-957991de34a9)

## Scripts And Scenes Set Up

### Start Menu Scene

Υour hierarchy should be something like this:

![Screenshot 2024-07-19 115328](https://github.com/user-attachments/assets/419e1c00-b572-419a-8987-334372d8c9cb)

#### Canvas

![Screenshot 2024-07-19 121304](https://github.com/user-attachments/assets/211bb655-afa7-4978-81a8-545dd188734a)

#### ModManager

#### LoadModsToTheList

![Screenshot 2024-07-19 121344](https://github.com/user-attachments/assets/1959f5d1-966d-4710-a0ba-f6bb73e759ed)

#### PreviewManager

![Screenshot 2024-07-19 121432](https://github.com/user-attachments/assets/c9f48448-7aa9-4d7b-8b09-02024bdf5a9e)

#### LoadAssetsFromBundle

![Screenshot 2024-07-19 121607](https://github.com/user-attachments/assets/3c21a0c9-23bd-40f2-8fde-ff2eed171580)

#### DontDestroyOnLoadScript

### Preview Scene

### Game Scene

![Screenshot 2024-07-19 120546](https://github.com/user-attachments/assets/e2d815f4-00fb-4c1c-a6e4-837a383e60e7)
 
