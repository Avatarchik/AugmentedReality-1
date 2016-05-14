using UnityEngine;
using System.Collections;
using System;
using Emgu.CV;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.Geodetic;
using Emgu.Util;
using Emgu.CV.BgSegm;
using Emgu.CV.Cuda;
using Emgu.CV.Flann;
using Emgu.CV.Cvb;
using Emgu.CV.CvEnum;
using Emgu.CV.Face;
using Emgu.CV.ML;
using Emgu.CV.OCR;
using Emgu.CV.Reflection;
using Emgu.CV.Shape;
using Emgu.CV.Stitching;
using Emgu.CV.Superres;
using Emgu.CV.Text;
using Emgu.CV.Tiff;
using Emgu.CV.UI;
using Emgu.CV.VideoStab;
using Emgu.CV.VideoSurveillance;
using Emgu.CV.XFeatures2D;
using Emgu.CV.XImgproc;
using Emgu.CV.XPhoto;
using System.Drawing;

public class CVTest : MonoBehaviour {
	public String modelImageName;
	public String observedImageName;
	public UnityEngine.UI.Image[] Corners;
	public UnityEngine.UI.Image im,im3;
	public static UnityEngine.UI.Image im2;
	public RectTransform CanvasRect;

	//
	public bool Capturing;
	Capture _cameraCapture;
	//
	public Material material;
    //
    public WebCamera WC;
	// Use this for initialization
	void Start () {
		im2 = im;
		modelImageName = Application.dataPath+"/Resources"+"/"+modelImageName+".png";
		observedImageName = Application.dataPath+"/Resources"+"/"+observedImageName+".png";
		//StartCoroutine(CameraCapture());


	}
	
	// Update is called once per frame
	void Update () {
		//material.mainTexture = 
	}
	[ContextMenu ("Do2")]
	public void Do2(){
		long matchTime;
		using(Mat modelImage = CvInvoke.Imread(modelImageName,LoadImageType.Grayscale))
		using(Mat observedImage = CvInvoke.Imread(observedImageName,LoadImageType.Grayscale))
		{
			PointF[] result = DrawMatches.Draw(modelImage,observedImage,out matchTime);
			foreach(PointF p in result){
				Debug.Log(p.X + "," + p.Y);	
			}
			for(int i = 0; i< result.Length; i++){

				Corners[i].rectTransform.position = SetPosition(result[i]);
			
			}

		}
	}
	[ContextMenu ("Do3")]
	public void Do3(){
	//public void Do3(Mat frame){
		long matchTime;
		if(WC.Texture2Image() != null){
			Bitmap bitmap = new Bitmap(WC.Texture2Image());
			Mat frame = new Mat();
			frame = CvInvoke.CvArrToMat(bitmap.GetHbitmap());
			using(Mat modelImage = CvInvoke.Imread(modelImageName,LoadImageType.Grayscale))
			{
				PointF[] result = DrawMatches.Draw(modelImage,frame,out matchTime);
				if(result != null){
					for(int i = 0; i< result.Length; i++){

						Corners[i].rectTransform.position = SetPosition(result[i]);

					}	
				}


			}	
		}

	}
	Vector3 SetPosition(PointF pos){
		
		//return new Vector3( pos.X,invertY(pos.Y),0f);
		return new Vector3( im3.rectTransform.position.x - (im3.rectTransform.rect.width*0.5f) + pos.X,im3.rectTransform.position.y - (im3.rectTransform.rect.height*0.5f) + invertY(pos.Y),0f);
	}
	float invertY(float y){
		return im2.rectTransform.position.y-y;
		//return -y+CanvasRect.position.y;
		//return im2.rectTransform.rect.height-y+CanvasRect.position.y;
	}
	IEnumerator CameraCapture(){
		while(Capturing){
			Do3();	
			yield return null;
		}
		yield return null;
	}

	[ContextMenu ("Do")]
	public void Do(){
		Debug.Log("-"+Application.dataPath+"-");
		Image<Bgr, byte> picture = new Image<Bgr, byte>(Application.dataPath+"/Resources"+"/"+"box.png"); 
		Bgr myWhiteColor = new Bgr(255, 255, 255);
		for (int i=0; i<200; i++)
		{
			picture[i,i] = myWhiteColor;
		}
		picture.Save(Application.dataPath +"/"+"box2.png");

	}

}

