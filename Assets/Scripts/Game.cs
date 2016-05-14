using UnityEngine;
using System.Collections;
using System;
using Emgu.CV;
using Emgu.CV.CvEnum;
using System.Drawing;
using Emgu.CV.Util;

public class Game : MonoBehaviour {
	public String modelImageName;
	public String observedImageName;
	public static UnityEngine.UI.Image im2;
	public UnityEngine.UI.Image imToLookFor,imGiven;
	public UnityEngine.UI.Image[] Corners;
	// Use this for initialization
	void Start () {
		im2 = imToLookFor;
		modelImageName = Application.dataPath+"/Resources"+"/"+modelImageName+".png";
		observedImageName = Application.dataPath+"/Resources"+"/"+observedImageName+".png";

		SetPoints ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void Do2(){
		long matchTime;
		using(Mat modelImage = CvInvoke.Imread(modelImageName,LoadImageType.Grayscale))
		using(Mat observedImage = CvInvoke.Imread(observedImageName,LoadImageType.Grayscale))
		{
			Mat result = DrawMatches.getPoints(modelImage,observedImage,out matchTime);

			PointF[] resultPoints = DrawMatches.GetPerspectiveOfHomography(result,imToLookFor.rectTransform);

			foreach(PointF p in resultPoints){
				Debug.Log(p.X + "," + p.Y);	
			}
			for(int i = 0; i< resultPoints.Length; i++){

				Corners[i].rectTransform.position = SetPosition(resultPoints[i]);

			}

		}
	}
	Vector3 SetPosition(PointF pos){
		return new Vector3( imGiven.rectTransform.position.x - (imGiven.rectTransform.rect.width*0.5f) + pos.X, + imGiven.rectTransform.position.y - (imGiven.rectTransform.rect.height*0.5f) + invertY(pos.Y),0f);
	}
	float invertY(float y){
		return imGiven.rectTransform.rect.height-y;
		//return imGiven.rectTransform.position.y-y;

	}
	void SetPoints(){
		Do2 ();
	
	}

}
