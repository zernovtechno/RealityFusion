VR Cinema for Mobile(2019/7/11 - by Fatty War, id3644@gmail.com)

This document explains how to use the pack.

Introduction
If you are designing an VR app, try using this cinema interiors model.
The theater lights are ready and you can change the video clips.
You can see the theater where a short clip plays by playing the demo scene. ("assets\VRCinemaforMobile\Scenes\Example_Projector")
The images being screened are scattered on the screen, making it possible to illuminate the inside of the theater in real time.(It looks like a real time GI and helps performance).
Try to place assets in your VR apps!

1. You can edit "Example_Projector" scene if you want. (VideoClip, Lighting).

	1-1. VideoClip replacement
		You can replace the video you want to show.
		
		Replace the VideoClip of the VideoPlayer with your prepared one. ("SceneRoot/Projection/VideoPlayer")

	1-2. Lighting edit
		Theaters are set as key-light and fill-light.
	
		key-light - Set on screen to cover the entire theater
		You can change the brightness of the interior of the theater by modifying the emission of material-ScreenLight_20. ("assets\VRCinemaforMobile\Metrials\Light")
		*You can modify the brightness without bake the built-in lightmap
		
		fill-light-Located on the left and right walls of the theater, Add an additional mood.
		You can modify the brightness (color) of the light. ("assets\VRCinemaforMobile\Prefabs\WallLight00")
		*This light is applied exactly after baking the built-in lightmap.
		
2. projector light Setting
	
	It emits light synchronized with video in real time inside the theater.
	It simply works by replacing the VideoClip (in section 1-1). (You do not need to make any changes.)

	How the projector light works.
		1-Step. The scene has a camera that captures the image. ("SceneRoot/Projection/RenderTexture/RenderTextureCamera")
		2-Step. This camera will save the VideoClip in real time. ("assets\VRCinemaforMobile\Textures\Cookie\ScreenBounceRT.renderTexture")
		3-Step. Apply this RenderTexture to the light. ("SceneRoot/Projection/ScreenBounceLight/DynamicScreenLight")
		4-Step. The lights are synchronized with the video in real time and illuminate the room.
		
	2-1. Projector. (https://docs.unity3d.com/2018.3/Documentation/Manual/class-Projector.html)
		A Projector allows you to project a Material onto all objects that intersect its frustum.
		The material must use a special type of shader for the projection effect to work correctly. (see 2-2)
		- see the projector prefabs in Unity¡¯s standard assets for examples of how to use the supplied Projector/Light and Projector/Multiply shaders.
			
	2-2. Shader-ProjectorLight 
		You need projector shader - assets\VRCinemaforMobile\Shaders\ProjectorLight
		This shader comes from Unity Standard assets.
		
	2-3. RenderTexture-CookieTexture. (https://docs.unity3d.com/2018.1/Documentation/Manual/class-RenderTexture.html)
		Render Textures are special types of Textures that are created and updated at runtime.
		To use them, you first create a new Render Texture and designate one of your Cameras to render into it.
		Then you can use the Render Texture in a Material just like a regular Texture.
		The Water prefabs in Unity Standard Assets are an example of real-world use of Render Textures for making real-time reflections and refractions.
		
3. Folder structure
\assets
L_VRCinemaforMobile	//documentation and subfolders (readme.txt + Materials, Meshs, Prefabs, Scenes, Scripts, Shaders, Textures)
	L_Meshs	//FBX model.
	L_Materials	//contains subfolders(fx, light, screen, sky).
	L_Prefabs	//There are Prefabs for VRCinemaforMobile.
	L_Scenes	//demonstration scenes(demo, Example).
	L_Scripts	//There are two simple scripts that move objects.
	L_Shaders	// Include ProjectorLight Shader. (See "2-2")
	L_Textures	//Contains the texture used for the object.


Thank you for your purchase.