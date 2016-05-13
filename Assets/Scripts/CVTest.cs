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

	// Use this for initialization
	void Start () {
		im2 = im;
		modelImageName = Application.dataPath+"/Resources"+"/"+modelImageName+".png";
		observedImageName = Application.dataPath+"/Resources"+"/"+observedImageName+".png";
		StartCoroutine(CameraCapture());


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
//			foreach(PointF p in result){
//				Debug.Log(p.X + "," + p.Y);	
//			}
			for(int i = 0; i< result.Length; i++){

				Corners[i].rectTransform.position = SetPosition(result[i]);
			
			}

		}
	}
	[ContextMenu ("Do3")]
	public void Do3(Mat frame){
		long matchTime;
		Bitmap bitmap = new Bitmap(WebCamera.Texture2Image());
		frame = new Mat()
		using(Mat modelImage = CvInvoke.Imread(modelImageName,LoadImageType.Grayscale))
		{
			PointF[] result = DrawMatches.Draw(modelImage,frame,out matchTime);
			//			foreach(PointF p in result){
			//				Debug.Log(p.X + "," + p.Y);	
			//			}
			if(result != null){
				for(int i = 0; i< result.Length; i++){

					Corners[i].rectTransform.position = SetPosition(result[i]);

				}	
			}


		}
	}
	Vector3 SetPosition(PointF pos){
		//im3.rectTransform.rect.x
		return new Vector3( im3.rectTransform.position.x - (im3.rectTransform.rect.width*0.5f) + pos.X,im3.rectTransform.position.y - (im3.rectTransform.rect.height*0.5f) + invertY(pos.Y),0f);
	}
	float invertY(float y){
		return im2.rectTransform.position.y-y;
		//return -y+CanvasRect.position.y;
		//return im2.rectTransform.rect.height-y+CanvasRect.position.y;
	}
	IEnumerator CameraCapture(){
		try{
			_cameraCapture = new Capture();

		}catch{
			Debug.LogError("Error!");
		}

		while(Capturing){
			
			Mat frame = _cameraCapture.QueryFrame();

			if(frame != null){
				Do3(frame);	
			}else{
				Debug.Log("is null");
			}

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
//	public static Image<Bgr, Byte> Draw(Image<Gray, Byte> modelImage, Image<Gray, byte> observedImage)
//	{
//		HomographyMatrix homography = null;
//
//		FastDetector fastCPU = new FastDetector(10, true);
//		VectorOfKeyPoint modelKeyPoints;
//		VectorOfKeyPoint observedKeyPoints;
//		Matrix<int> indices;
//
//		BriefDescriptorExtractor descriptor = new BriefDescriptorExtractor();
//
//		Matrix<byte> mask;
//		int k = 2;
//		double uniquenessThreshold = 0.8;
//
//		//extract features from the object image
//		modelKeyPoints = fastCPU.DetectKeyPointsRaw(modelImage, null);
//		Matrix<Byte> modelDescriptors = descriptor.ComputeDescriptorsRaw(modelImage, null, modelKeyPoints);
//
//		// extract features from the observed image
//		observedKeyPoints = fastCPU.DetectKeyPointsRaw(observedImage, null);
//		Matrix<Byte> observedDescriptors = descriptor.ComputeDescriptorsRaw(observedImage, null, observedKeyPoints);
//		BruteForceMatcher<Byte> matcher = new BruteForceMatcher<Byte>(DistanceType.L2);
//		matcher.Add(modelDescriptors);
//
//		indices = new Matrix<int>(observedDescriptors.Rows, k);
//		using (Matrix<float> dist = new Matrix<float>(observedDescriptors.Rows, k))
//		{
//			matcher.KnnMatch(observedDescriptors, indices, dist, k, null);
//			mask = new Matrix<byte>(dist.Rows, 1);
//			mask.SetValue(255);
//			Features2DToolbox.VoteForUniqueness(dist, uniquenessThreshold, mask);
//		}
//
//		int nonZeroCount = CvInvoke.cvCountNonZero(mask);
//		if (nonZeroCount >= 4)
//		{
//			nonZeroCount = Features2DToolbox.VoteForSizeAndOrientation(modelKeyPoints, observedKeyPoints, indices, mask, 1.5, 20);
//			if (nonZeroCount >= 4)
//				homography = Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(
//					modelKeyPoints, observedKeyPoints, indices, mask, 2);
//		}
//
//		//Draw the matched keypoints
//		Image<Bgr, Byte> result = Features2DToolbox.DrawMatches(modelImage, modelKeyPoints, observedImage, observedKeyPoints,
//			indices, new Bgr(255, 255, 255), new Bgr(255, 255, 255), mask, Features2DToolbox.KeypointDrawType.DEFAULT);
//
//		#region draw the projected region on the image
//		if (homography != null)
//		{  //draw a rectangle along the projected model
//			Rectangle rect = modelImage.ROI;
//			PointF[] pts = new PointF[] { 
//				new PointF(rect.Left, rect.Bottom),
//				new PointF(rect.Right, rect.Bottom),
//				new PointF(rect.Right, rect.Top),
//				new PointF(rect.Left, rect.Top)};
//			homography.ProjectPoints(pts);
//
//			result.DrawPolyline(Array.ConvertAll<PointF, Point>(pts, Point.Round), true, new Bgr(Color.Red), 5);
//		}
//		#endregion
//
//		return result;
//	}
}

