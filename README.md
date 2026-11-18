# Dragon-Wrapped Katana Showcase

An interactive real-time character and prop showcase built in Unity with the Universal Render Pipeline. The project presents an animated dragon-wrapped katana in a moonlit stone environment with a cinematic colour grade, responsive camera controls, and a scene-authored interface.

## Project Overview

The showcase focuses on material presentation, atmosphere, lighting, and interaction rather than gameplay. The scene includes:

- A rigged and animated dragon-wrapped katana
- Physically based dragon, katana, and ornament materials
- Interactive camera-relative model rotation, zoom, and roll
- Mouse, keyboard, and touch controls
- Moonlight, soft shadows, lanterns, rim lighting, and warm emissive accents
- A textured stone dais with animated runes
- Persistent scene-authored mountains, bamboo, rocks, and atmospheric particles
- URP post-processing with bloom, colour grading, depth of field, vignette, film grain, and chromatic aberration
- A responsive heads-up display using Cinzel

The showcase environment is stored directly in the Unity scene. No scene-generation script is required at runtime.

## Requirements

- Unity 6.0 or newer
- Tested with Unity 6000.3.8f1
- Universal Render Pipeline 17.3.0
- Unity UGUI 2.0
- A graphics-capable desktop device
- Git LFS when cloning the repository

## Getting Started

### 1. Clone the repository

```bash
git lfs install
git clone https://github.com/adishesha09/dragon-wrapped-katana-showcase.git
cd dragon-wrapped-katana-showcase
```

### 2. Open the project

1. Open Unity Hub.
2. Select **Add** > **Add project from disk**.
3. Select the repository folder.
4. Open the project with Unity 6000.3.8f1 or a compatible Unity 6 version.
5. Wait for package restoration and asset import to finish.

### 3. Run the showcase

Open:

```text
Assets/Scenes/MainScene.unity
```

Press **Play** to explore the model.

## Controls

| Input | Action |
| --- | --- |
| Left mouse drag | Rotate the model |
| Mouse wheel | Zoom in and out |
| Q / E | Roll the model left or right |
| R | Reset the rotation and camera |
| One-finger touch drag | Rotate the model |
| Two-finger pinch | Zoom in and out |

The rotation input is relative to the active showcase camera and includes damping for a smoother response.

## Scene Structure

The principal scene is `Assets/Scenes/MainScene.unity`.

Key scene elements include:

```text
MainScene
|-- DragonKatana
|-- Main Camera
|-- ShowcaseRig
|   |-- Environment
|   |-- Moon and mountain layers
|   |-- Stone dais and lanterns
|   |-- Bamboo, rocks, and particles
|   |-- Reflection probe
|   `-- Post-processing volume
`-- ControlsHUD
```

## Project Structure

```text
Assets/
|-- DragonKatana/          # Model, animation controller, materials, and textures
|-- Fonts/                 # Cinzel font and Open Font License
|-- Generated/             # Persistent showcase meshes, materials, and profiles
|-- Scenes/                # MainScene
|-- Scripts/               # Interaction and runtime visual effects
|-- Settings/              # URP renderer, pipeline, and volume assets
`-- TutorialInfo/          # Unity URP template support files
```

## Main Scripts

### ModelRotator

Handles camera-relative model rotation, touch input, rolling, zooming, and resetting the view.

Location:

```text
Assets/Scripts/ModelRotator.cs
```

### ShowcasePostProcessing

Manages the scene's post-processing volume and the subtle runtime bloom and exposure variation.

Location:

```text
Assets/Scripts/ShowcasePostProcessing.cs
```

### ShowcaseRuntimeEffects

Contains the flame flicker and animated rune-emission behaviours.

Location:

```text
Assets/Scripts/ShowcaseRuntimeEffects.cs
```

## Rendering Notes

- Render pipeline: Universal Render Pipeline
- HDR: enabled for the showcase camera
- Anti-aliasing: subpixel morphological anti-aliasing
- Colour management: post-processed ACES presentation
- Lighting: directional moonlight, warm key lighting, cool fill and rim lighting
- Particles: mesh-based to avoid camera-facing rectangular billboards
- UI: world-independent screen-space overlay Canvas with responsive scaling

The generated showcase assets under `Assets/Generated/ShowcaseScene` are persistent project assets and are intentionally retained in version control.

## GitHub and Git LFS

Large binary assets are tracked with Git LFS. This includes the rigged FBX, texture maps, and supported image/audio/video formats.

After cloning, confirm LFS files were downloaded correctly:

```bash
git lfs pull
```

If Unity reports a missing model or texture, run `git lfs pull` before reimporting the affected asset.

## Third-Party Resources

### Cinzel

The project uses the Cinzel typeface for the in-scene controls display. Cinzel is distributed under the SIL Open Font License. The license is included at:

```text
Assets/Fonts/OFL-Cinzel.txt
```

## Academic Context

Created by Adishesha Nandkoomar for GART6212 Part 3.

## Troubleshooting

### The scene appears unlit

Confirm that the project opened with the URP pipeline assets under `Assets/Settings` and wait for shader compilation to finish.

### The model or textures are missing

Run:

```bash
git lfs pull
```

Then reopen the project and allow Unity to reimport the assets.

### Controls do not respond

Make sure `MainScene` is the active scene and the `ModelRotator` component on the model is enabled.

### The HUD does not appear

Ensure `ControlsHUD` is enabled in the scene hierarchy and that the Cinzel font imported successfully.
