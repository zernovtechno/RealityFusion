[README НА РУССКОМ](./READMERUS.md)

# Reality Fusion
Hey! This is MR app for any Android 9+ smartphone. It has hand tracking, 3DoF tracking, browser and more cool things.
It uses [mediapipe unity plugin 0.16](https://github.com/homuler/MediaPipeUnityPlugin) for hand tracking, [google cardboard 1.28](https://github.com/googlevr/cardboard-xr-plugin) for VR and stereo vision.

## If you want to just use it...
Select the "Releases" menu on the right side of your screen and download APK file from last release. Then install it on your phone and run. Interface is in russian now, so, maybe you need a translator ;)

## If you want to compile this project...

!!USE UNITY 2022.3.35f1, INSTALLED WITH ANDROID BUILDING TOOLS!!
Import project to Unity, by oppening it from Unity Hub, then connect your phone with USB cable, and turn USB debug mode on it. Allow installing apps from ADB.

Set Unity config like this (Basically, it sets automaticly):

### Project Settings
### Player
#### Resolution and Presentation
![image](https://github.com/ZernovTechno/AR/assets/90546939/a37b0eda-85c2-4c09-a83c-4e5bcf3da646)

#### Other Settings
![image](https://github.com/ZernovTechno/AR/assets/90546939/6ccac38f-c521-406d-8782-dbe65974547b)

#### Publishing Settings
![image](https://github.com/ZernovTechno/AR/assets/90546939/07f3d81a-a2b9-4af5-9bde-126a721199a9)

And then open scene in explorer in the bottom of Unity. Scene located in Assets->MediaPipeUnity->Samples->Scenes->Hand Landmark Detection->Hand Tracking.unity

Here you can do what you want.

Then click File->Build Settings->Add Open Scenes, check added scene.

Now you can build it and run on a phone. Check your phone connection, and click "Build And Run" in Build Settings/File menu.

## Congrats!

