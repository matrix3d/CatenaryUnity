using display;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Main2 : MonoBehaviour
{
    public UIShape shape;
	public Slider slider;
    // Start is called before the first frame update
    void Start()
    {
		/*shape.graphics.clear();
		shape.graphics.lineStyle(0, Color.blue);
		shape.graphics.moveTo(0, 0);
		shape.graphics.lineTo(0, 0);*/
		LateUpdate2();
	}

	float  catenary(float a,float x) { return a * MathF.Cosh(x / a); }

	float getA (Vector2 vec0,Vector2 vec1,float ropeLen)
	{
		//Solving A comes from : http://rhin.crai.archi.fr/rld/plugin_details.php?id=990
		var yDelta = vec1.y - vec0.x;
		var	vecLen = Vector2.Distance(vec1,vec0);

		if (yDelta > ropeLen || vecLen > ropeLen) { Debug.Log("not enough rope"); return 0; }
		if (yDelta < 0)
		{   //Swop verts, low end needs to be on the left side
			var tmp = vec0;
			vec0 = vec1;
			vec1 = vec0;
			yDelta *= -1;
		}

		//....................................
		var max_tries = 100;
		var vec3 = new Vector2(vec1.x, vec0.y);
		var e = float.MaxValue;
		var a = 100f;
		var aTmp = 0.0f;
		var yRopeDelta = 0.5f * Mathf.Sqrt(ropeLen * ropeLen - yDelta * yDelta);  //Optimize the loop
		var vecLenHalf = 0.5f * vecLen;                                                      //Optimize the loop
		int	i;

		for (i = 0; i < max_tries; i++)
		{
			//aTmp	= 0.5 * vecLen / ( Math.asinh( 0.5 * Math.sqrt(ropeLen**2 - yDelta**2) / a ) );
			aTmp = vecLenHalf / (MathF.Asinh(yRopeDelta / a));
			e = Mathf.Abs((aTmp - a) / a);
			a = aTmp;
			if (e < 0.001) break;
		}
		//console.log("tries", i);
		return a;
	}

	private void LateUpdate2()
    {

		// 获取鼠标在屏幕上的位置
		Vector2 mousePosition = Input.mousePosition;

		// 将鼠标位置转换为画布上的坐标
		RectTransform canvasRectTransform = GetComponent<Canvas>().transform as RectTransform;
		Vector2 canvasPosition;

		if (RectTransformUtility.ScreenPointToLocalPointInRectangle(shape.rectTransform, mousePosition, null, out canvasPosition))
		{
			// canvasPosition 现在包含了鼠标在画布上的坐标
			Debug.Log("Mouse position on canvas: " + canvasPosition);
		}

		//https://codepen.io/sketchpunk/pen/ajGrqZ
		var segCnt = 10;                   // How many divisions (segments) to make
		var ropeLen = 370;                  // Rope/Chain Length.
		var pntA =// canvasPosition; //
								   new Vector2(-170, 80);  // Starting Point of the Rope
		var pntB =// Vector2.zero; //
								 new Vector2(100, 0);     // Ending Point of the Rope

		//...........................
		//Draw Main Points
		//circle(pntA, 5);
		//circle(pntB, 5);

		//...........................
		// This determines the sagging factor
		var A = getA(pntA, pntB, ropeLen);

		//...........................
		// Draw in between points of the curve
		var dist = Vector2.Distance(pntB, pntA);     // Length between Two Points
		var distHalf = dist * 0.5f;                    // ... Half of that
		var segInc = dist / segCnt;             // Size of Each Segment
		var offset = catenary(A, -distHalf); // First C on curve, use it as an Offset to align everything.
		var pnt = Vector2.zero;
		float xpos;
		float c;
			int i;

		float y; //todo not need, only for testing inverting the sag



		shape.graphics.clear();
		shape.graphics.lineStyle(0, Color.blue);
		//shape.graphics.beginFill(default);
		shape.graphics.moveTo(pntA.x, pntA.y);

		for (i = 1; i < segCnt; i++)
		{
			pnt= Vector2.Lerp(pntA, pntB, (float)i / segCnt);
			y = pnt.y;                          // only for inverting testing, throw away if only want downward sag
			xpos = i * segInc - distHalf;   // x position between two points but using half as zero center

			c = catenary(A, xpos);                // get a y value, but needs to be changed to work with coord system.
			pnt.y -= (offset - c);             // Current lerped Y minus C of starting point minus current C
											   //circle(pnt, 4);
			//shape.graphics.drawCircle(pnt.x, pnt.y, 4);
			//pnt.y = y + (offset - c);          // Add Offset C to Lerp Y, inverts the sag upwards
			//circle(pnt, 1);
			shape.graphics.lineTo(pnt.x,pnt.y);
			//shape.graphics.drawCircle(pnt.x, pnt.y, 1);
			
		}
		shape.graphics.lineTo(pntB.x, pntB.y);
	}
}
