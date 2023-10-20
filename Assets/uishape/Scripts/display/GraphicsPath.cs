using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace display
{
	public class GraphicsPath : IGraphicsPath, IGraphicsData
	{
		public GLPath2D glpath2d;
		public bool gpuPath2DDirty = true;
		public List<List<float>> polys = new List<List<float>>();
		public Dictionary<int, bool> closePathIndex = new Dictionary<int, bool>();
		private List<float> poly;

		 public void clear(){
			polys.Clear();
			//tris.length = 0;
			closePathIndex = new Dictionary<int, bool>();
		}
		
		 public void moveTo(float x,float y)
{
	makePoly();
	//polys.push(makePoly());
	poly.Add(x);
	poly.Add(y);
	//poly.push(x, y);
}

 public void lineTo(float x,float y)
{
	if (poly == null)
	{
		makePoly();
	}
	//poly.push(x, y);
	poly.Add(x);
	poly.Add(y);
}

 public void curveTo(float controlX, float controlY, float anchorX, float anchorY)
{
	if (poly == null) makePoly();

	if (poly.Count >= 2)
	{
		var x0 = poly[poly.Count - 2];
		var y0 = poly[poly.Count - 1];
		var d = Mathf.Abs(x0 - anchorX) + Mathf.Abs(y0 - anchorY);
		var step = 5 / d;
		if (step > .5f) step = .5f;
		if (step < 0.01f) step = 0.01f;

		for (var t1 = step; t1 <= 1; t1 += step)
				{
			var t0 = 1 - t1;
			var q0x = t0 * x0 + t1 * controlX;
			var q0y = t0 * y0 + t1 * controlY;
			var q1x = t0 * controlX + t1 * anchorX;
			var q1y = t0 * controlY + t1 * anchorY;
			poly.Add(t0 * q0x + t1 * q1x);
			poly.Add(t0 * q0y + t1 * q1y);
		}
	}
}

/*private function getCurvePoint(t1:Number, p0:Point, p1:Point, p2:Point):Vector3D
{
	var t0:Number = 1 - t1;
	var q0x:Number = t0 * p0.x + t1 * p1.x;
	var q0y:Number = t0 * p0.y + t1 * p1.y;
	var q1x:Number = t0 * p1.x + t1 * p2.x;
	var q1y:Number = t0 * p1.y + t1 * p2.y;
	return new Vector3D(t0 * q0x + t1 * q1x, t0 * q0y + t1 * q1y, Math.atan2(q1y - q0y, q1x - q0x));
}*/

public void cubicCurveTo(float controlX1, float controlY1, float controlX2, float controlY2, float anchorX, float anchorY)
{
	// TODO implement GLGraphicsPath.cubicCurveTo() Needed for elipse drawing
	if (poly == null) makePoly();

	/*poly.push(controlX1);
	poly.push(controlY1);
	poly.push(controlX2);
	poly.push(controlY2);
	poly.push(anchorX);
	poly.push(anchorY);*/


	/*
	trace("controlX1: " + controlX1 + ", controlX2: " + controlX2 + ", controlY1: " + controlY1 + ", controlY2: " + controlY2);

	var x0:Number = poly.array[poly.length - 2];
	var y0:Number = poly.array[poly.length - 1];
	var left:Matrix3d = new Matrix3d(x0 * x0 * x0, x0 * x0, x0, 1,
									 controlX1 * controlX1 * controlX1, controlX1 * controlX1, 	controlX1, 1,
									 controlX2 * controlX2 * controlX2, controlX2 * controlX2, 	controlX2, 1,
									 anchorX * anchorX * anchorX, 	anchorX * anchorX, 	anchorX , 1);
	left.invert();
	var right:Matrix3d = new Matrix3d(		y0, 0, 0, 0,
									 controlY1, 0, 0, 0,
									 controlY2, 0, 0, 0,
									   anchorY, 0, 0, 0);

	right.append(left);

	trace("n11: " + right.n11 + ", n21: " + right.n21 + ", n31: " + right.n31 + ", n41: " + right.n41);
	trace("right: " + right.toString());

	for (var i:int = 0; i < 200; i++)
	{
		var x:Number = i * 2;
		var y:Number = right.n11 * x * x * x + 
					   right.n21 * x * x + 
					   right.n31 * x +
					   right.n41;

		poly.push(x);
		poly.push(y);

		trace("x: " + x + ", y: " + y);
	}
	*/
}

//override public function wideLineTo(x:Number, y:Number):void
//{
//	lineTo(x, y);
//}

//override public function wideMoveTo(x:Number, y:Number):void
//{
//	moveTo(x, y);
//}

 public void arc(float x, float y, float r, float a0, float a1)
{
	var da = Mathf.Abs(1 / r * 180 / Mathf.PI);// Math.PI * 3 / 4;
	if (da < 1)
	{
		da = 1;
	}
	if (da > 90)
	{
		da = 90;
	}
	da = da * Mathf.PI / 180;
	var x0 = r;
	var y0 = 0f;
	var sin = Mathf.Sin(da);
	var cos = Mathf.Cos(da);
	moveTo(x0 + x, y0 + y);
	for (var a = a0; a < a1; a += da ){
		var x_ = x0;
		x0 = x0 * cos - y0 * sin;
		y0 = x_ * sin + y0 * cos;
		lineTo(x0 + x, y0 + y);
	}
}

/*private function rect(x:Number, y: Number, w: Number, h: Number) : void
{
	moveTo(x, y);
	lineTo(x + w, y);
	lineTo(x + w, y + h);
	lineTo(x, y + h);
	closePath();
}*/

private void makePoly()
{
			poly = new List<float>();
			polys.Add(poly);
}

 public void closePath()
{
	
	closePathIndex[polys.Count - 1] = true;
}

		public void gldraw(RenderingContext2D ctx, VertexHelper vh)
		{
			//Debug.Log("drawpath");
			//throw new NotImplementedException();
			ctx.drawPath(this);
		}
	}
}
