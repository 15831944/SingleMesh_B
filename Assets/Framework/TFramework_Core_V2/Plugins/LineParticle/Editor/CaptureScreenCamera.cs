using UnityEngine;
using UnityEditor;
using System;

public class CaptureScreenCamera : EditorWindow
{

	[MenuItem ("Camera/CaptureScreenShot")]
	static void InitCaptureScreenShot ()
	{
		CaptureScreenShot ();
	}

	[MenuItem ("Camera/CameraCaptureScreenShot")]
	static void InitCameraCaptureScreenShot ()
	{
		CameraCaptureScreenShot ();
	}

	private static void CaptureScreenShot ()
	{
		var camera = Camera.main;
		var resWidth = camera.pixelWidth;
		var resHeight = camera.pixelHeight;
		var name = ScreenShotName (resWidth, resHeight);
		ScreenCapture.CaptureScreenshot (name);
	}

	private static void CameraCaptureScreenShot ()
	{
		var camera = Camera.main;
		var resWidth = camera.pixelWidth;
		var resHeight = camera.pixelHeight;
		var culltingMask = camera.cullingMask;
		camera.cullingMask = 1 << 0;
		RenderTexture rt = new RenderTexture (resWidth, resHeight, 24);
		camera.targetTexture = rt;
		Texture2D screenShot = new Texture2D (resWidth, resHeight, TextureFormat.RGB24, false);
		camera.Render ();
		RenderTexture.active = rt;
		screenShot.ReadPixels (new Rect (0, 0, resWidth, resHeight), 0, 0);
		camera.targetTexture = null;
		RenderTexture.active = null; // JC: added to avoid errors
		if (Application.isPlaying) {
			Destroy (rt);
		}
		byte[] bytes = screenShot.EncodeToPNG ();
		string filename = ScreenShotName (resWidth, resHeight);
		System.IO.File.WriteAllBytes (filename, bytes);
		Debug.Log (string.Format ("Took screenshot to: {0}", filename));
		camera.cullingMask = culltingMask;
	}

	private static string ScreenShotName (int width, int height)
	{
		return string.Format ("{0}/screen_{1}x{2}_{3}.png", 
			Application.dataPath, 
			width, height, 
			System.DateTime.Now.ToString ("yyyy-MM-dd_HH-mm-ss"));
	}

}
