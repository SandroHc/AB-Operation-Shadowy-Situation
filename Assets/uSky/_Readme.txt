uSky : Skybox-Based Lighting Shader

REQUIREMENTS
uSky requires Unity Free or Pro, version 4.3.4 or newer.
Skybox Shaders require Shader Model 3.0 or equivalent.


INSTALLATION 
Import the uSky package file into your project. It will contain two version of unity sub-folders:
uSky/Unity 4
uSky/Unity 5

In each folder contents necessary files of this package: Material, Shader, Prefab and startup Scene file.
If you are using Unity 4 only, then you can feel free to remove Unity 5 folder and vice versa.
NOTE: The Core folder must be present in the project with any version of Unity.


LANUCH USKY SCENE
Depend on which version of Unity Editor your are currently using, you will need to launch uSky Scene in the correct version folder.


SETUP USKY FROM NEW SCENE:
Method 1 - load the uSky Manager prefab
1) Choose the uSky Manager Prefab from the correct version folder.
2) Drag the Prefab to the Scene or Hierarchy window. 
(In Unity 5, you need to apply manually Directional Light to uSkyManager as Sun Light)

Method 2 - apply uSky Manager Component
1) Create a new Gameobject. 
2) Apply the uSky Manager Component to that new Gameobject.
Component locate at: click on the Component menu at the top of your screen and find "uSky / uSky Manager".
3) Create a Directional light. (In Unity 4)
4) Attach the Directional light to uSky Manager Component's Sun Light field.
By Default the uSky Manager Component has the Skybox material (uSkybox U4) attached for Unity 4.
if your using Unity 5, then apply the uSkybox U5 material to the uSky Manager Component's skybox material field.


After the you launch uSky Scene or setup the uSky Manager in the new scene, then you can change the different "Time of Day" slider value to see the day/night sky cycle.

TIPS:
If you import the project from Unity 4 to Unity 5, Default Ambient and Reflection from Scene Render Settings may not be skybox, so you need to manual set both of them to skybox.

Enjoy uSky! If you experience any problems, feel free to contact me at michaellam430@gmail.com