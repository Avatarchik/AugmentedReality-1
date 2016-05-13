﻿using UnityEngine;
using System.Collections;
using System.IO;
public class WebCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		WebCamTexture webcamTexture = new WebCamTexture();
		GetComponent<Renderer>().material.mainTexture = webcamTexture;
		webcamTexture.Play();

	}
	# region texture2image 
	public static System.Drawing.Image Texture2Image(Texture2D texture)
	{
		if (texture == null)
		{
			return null;
		}
		//Save the texture to the stream.
		byte[] bytes = texture.EncodeToPNG();

		//Memory stream to store the bitmap data.
		MemoryStream ms = new MemoryStream(bytes);

		//Seek the beginning of the stream.
		ms.Seek(0, SeekOrigin.Begin);

		//Create an image from a stream.
		System.Drawing.Image bmp2 = System.Drawing.Bitmap.FromStream(ms);

		//Close the stream, we nolonger need it.
		ms.Close();
		ms = null;

		return bmp2;
	}
	#region Texture2Image 
	public static System.Drawing.Image Texture2Image()
	{
		Texture2D  texture = GetComponent<Renderer>().material.mainTexture;
		if (texture == null)
		{
			return null;
		}
		//Save the texture to the stream.
		byte[] bytes = texture.EncodeToPNG();

		//Memory stream to store the bitmap data.
		MemoryStream ms = new MemoryStream(bytes);

		//Seek the beginning of the stream.
		ms.Seek(0, SeekOrigin.Begin);

		//Create an image from a stream.
		System.Drawing.Image bmp2 = System.Drawing.Bitmap.FromStream(ms);

		//Close the stream, we nolonger need it.
		ms.Close();
		ms = null;

		return bmp2;
	}
	#endregion
	#region image2texture 
	public static Texture2D Image2Texture(System.Drawing.Image im)
	{
		if (im == null)
		{
			return new Texture2D(4,4);
		}


		//Memory stream to store the bitmap data.
		MemoryStream ms = new MemoryStream();


		//Save to that memory stream.
		im.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

		//Go to the beginning of the memory stream.
		ms.Seek(0, SeekOrigin.Begin);
		//make a new Texture2D
		Texture2D tex = new Texture2D(im.Width, im.Height);

		tex.LoadImage(ms.ToArray());

		//Close the stream.
		ms.Close(); 
		ms = null;

		//
		return tex;
	}
	#endregion
	// Update is called once per frame
	void Update () {
		
	}
}
