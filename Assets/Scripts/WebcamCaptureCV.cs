using UnityEngine;
using System.Collections;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;
using System;

public class WebcamCaptureCV : MonoBehaviour {

	private int frameWidth;
	private int frameHeight;
	private Capture cvCapture;
	private Image<Bgr, byte> currentFrameBgr;
	private Image<Rgb, byte> currentFrameRgb;
	private Image<Bgra, byte> currentFrameBgra;
	private Texture2D tex;

	void Start () {

		cvCapture = new Capture();  

		cvCapture.FlipVertical = true;    //The image I am getting from webcam is flipped 
		cvCapture.ImageGrabbed += ProcessFrame;
		frameWidth = (int)cvCapture.GetCaptureProperty (Emgu.CV.CvEnum.CapProp.FrameWidth);
		//frameWidth = (int) cvCapture.GetCaptureProperty (Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FRAME_WIDTH);
		frameHeight = (int) cvCapture.GetCaptureProperty (Emgu.CV.CvEnum.CapProp.FrameHeight);
		//frameHeight = (int) cvCapture.GetCaptureProperty (Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FRAME_HEIGHT);


		tex = new Texture2D (frameWidth, frameHeight,  TextureFormat.BGRA32, false);
		gameObject.GetComponent<Renderer>().material.mainTexture = tex;

		currentFrameBgr = new Image<Bgr, byte> (frameWidth, frameHeight);
		currentFrameRgb = new Image<Rgb, byte> (frameWidth, frameHeight);
		currentFrameBgra = new Image<Bgra, byte> (frameWidth, frameHeight);

		cvCapture.Start();

	}

	private void ProcessFrame(object sender, EventArgs arg)
	{
		currentFrameBgr = ((cvCapture.QueryFrame ()).ToUMat (Emgu.CV.CvEnum.AccessType.Fast)).ToImage<Bgr, byte> ();
		currentFrameRgb.Bytes = currentFrameBgr.Bytes;
		currentFrameBgra = currentFrameRgb.Convert<Bgra, byte>(); 
		//    showImageDebug<Rgb, Byte> (currentFrameRgb);

	}


	void Update () {
		tex.LoadRawTextureData(currentFrameBgra.Bytes);
		tex.Apply();
		GetComponent<Renderer>().material.mainTexture = tex;
	}


	void OnDestroy(){

		cvCapture.Stop();
	}

//	private void showImageDebug<TColor, TDepth>(Image<TColor, TDepth> img, string windowName="Debug Window" ) where TColor : struct, IColor where TDepth : struct
//	{
//		
//		CvInvoke.cvShowImage(windowName, img);
//		CvInvoke.cvWaitKey(0);  
//		CvInvoke.cvDestroyWindow(windowName); 
//
//
//	}



}