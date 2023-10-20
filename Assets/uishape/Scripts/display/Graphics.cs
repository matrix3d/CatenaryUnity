using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace display
{
	public class Graphics 
	{
		public List<IGraphicsData> graphicsData;
		//public var gradientFills:Vector.<GraphicsGradientFill> = new Vector3D.<GraphicsGradientFill>;
		public GraphicsStroke lastStroke;
		public IGraphicsFill lastFill;
		private List<GraphicsPath> pathPool = new List<GraphicsPath>();
		private int pathPoolPos = 0;
		private GraphicsPath lastPath;
		//private Rect _bound;
		//private var _rect:Rectangle;
		//private var lockBound:Boolean = false;
		//public static var debug:Boolean = false;
		//public var _worldMatrix:Matrix = new Matrix;
		
		public bool dirtyGraphics = true;

		public Graphics()
		{
			clear();
		}

		public void clear()
		{
			dirtyGraphics = true;
			lastStroke = null;
			lastPath = null;
			pathPool.Clear();
			pathPoolPos = 0;
			graphicsData = new List<IGraphicsData>();
			//_bound = null;
		}

		public void lineStyle(float thickness = float.NaN,Color color = default)
		{
			endStrokAndFill();
			if (!float.IsNaN(thickness))
			{
				lastStroke = new GraphicsStroke(thickness == 0 ? 1 : thickness, new GraphicsSolidFill(color));
				graphicsData.Add(lastStroke);
			}
		}
		public void beginFill(Color color)
		{
			endStrokAndFill();
			lastFill = new GraphicsSolidFill(color);
			graphicsData.Add(lastFill as IGraphicsData);
		}
		public void endFill()
		{
			endStrokAndFill();
		}
		public void moveTo(float x,float y)
		{
			makePath();
			lastPath.moveTo(x, y);
			//inflateBound(x, y);
		}

		public void lineTo(float x,float y)
		{
			makePath();
			lastPath.lineTo(x, y);
			//inflateBound(x, y);
		}

		public void endStrokAndFill()
		{
			if (lastPath!=null)
			{
				if (lastFill!=null)
				{
					var efill = new GraphicsEndFill();
					efill.fill = lastFill;
					graphicsData.Add(efill);
					lastFill = null;
				}
				if (lastStroke!=null && !float.IsNaN(lastStroke.thickness))
				{
					lastStroke = new GraphicsStroke(float.NaN);
					graphicsData.Add(lastStroke);
				}
				lastPath = null;
			}
		}

		private void makePath()
		{
			if (lastPath == null)
			{
				var isInitial = false;
				if (pathPool.Count > pathPoolPos)
				{
					lastPath = pathPool[pathPoolPos];
				}
				else
				{
					lastPath = null;
				}
				if (lastPath==null){
					pathPool.Add(createPath());
					lastPath = pathPool[pathPoolPos];
					isInitial = true;
				}
				lastPath.clear();
				lastPath.gpuPath2DDirty = true;
				if (isInitial) lastPath.moveTo(0, 0);
				pathPoolPos++;
				graphicsData.Add(lastPath);
			}

			dirtyGraphics = true;
		}

		private GraphicsPath createPath()
		{
			return new GraphicsPath();
		}

		public void drawRect(float x, float y, float width, float height)
		{
			//lockBound = true;
				moveTo(x, y);
			lineTo(x + width, y);
			lineTo(x + width, y + height);
			lineTo(x, y + height);
			lastPath.closePath();
				//lineTo(x, y);
				//lockBound = false;
				//inflateBound(x, y);
			//inflateBound(x + width, y + height);
		}

		public void drawRoundRect(float x, float y, float width, float height, float ellipseWidth, float ellipseHeight = float.NaN)
			{
				//lockBound = true;
				if (float.IsNaN(ellipseHeight))
					ellipseHeight = ellipseWidth;
				moveTo(x + ellipseWidth, y);
		lineTo(x + width - ellipseWidth, y);
		curveTo(x + width, y, x + width, y + ellipseHeight);
		lineTo(x + width, y + height - ellipseHeight);
		curveTo(x + width, y + height, x + width - ellipseWidth, y + height);
		lineTo(x + ellipseWidth, y + height);
		curveTo(x, y + height, x, y + height - ellipseHeight);
		lineTo(x, y + ellipseHeight);
		curveTo(x, y, x + ellipseWidth, y);
		//lockBound = false;
				//inflateBound(x, y);
		//inflateBound(x + width, y + height);
	}

	public void drawRoundRectComplex(float x, float y, float width, float height, float topLeftRadius, float topRightRadius, float bottomLeftRadius, float bottomRightRadius)
	{
		//lockBound = true;
		moveTo(x + topLeftRadius, y);
		lineTo(x + width - topRightRadius, y);
		curveTo(x + width, y, x + width, y + topRightRadius);
		lineTo(x + width, y + height - bottomRightRadius);
		curveTo(x + width, y + height, x + width - bottomRightRadius, y + height);
		lineTo(x + bottomLeftRadius, y + height);
		curveTo(x, y + height, x, y + height - bottomLeftRadius);
		lineTo(x, y + topLeftRadius);
		curveTo(x, y, x + topLeftRadius, y);
		//lockBound = false;
		//inflateBound(x, y);
		//inflateBound(x + width, y + height);
	}

	public void drawCircle(float x, float y, float radius)
	{
		makePath();
		lastPath.moveTo(x + radius, y);
		lastPath.arc(x, y, radius, 0, Mathf.PI * 2);
		//inflateBound(x - radius, y - radius);
		//inflateBound(x + radius, y + radius);
	}

	//http://stackoverflow.com/questions/2172798/how-to-draw-an-oval-in-html5-canvas
	public void drawEllipse(float x, float y, float w, float h)
	{
		//lockBound = true;
		float kappa = .5522848f,
				ox = (w / 2) * kappa, // control point offset horizontal
				oy = (h / 2) * kappa, // control point offset vertical
				xe = x + w,           // x-end
				ye = y + h,           // y-end
				xm = x + w / 2,       // x-middle
				ym = y + h / 2;       // y-middle

		moveTo(x, ym);
		cubicCurveTo(x, ym - oy, xm - ox, y, xm, y);
		cubicCurveTo(xm + ox, y, xe, ym - oy, xe, ym);
		cubicCurveTo(xe, ym + oy, xm + ox, ye, xm, ye);
		cubicCurveTo(xm - ox, ye, x, ym + oy, x, ym);
		//lockBound = false;
		//inflateBound(x, y);
		//inflateBound(x + w, y + h);
	}
		public void curveTo(float controlX, float controlY, float anchorX, float anchorY)
		{
			makePath();
		lastPath.curveTo(controlX, controlY, anchorX, anchorY);
			//inflateBound(controlX, controlY);
		//inflateBound(anchorX, anchorY);
	}

	public void cubicCurveTo(float controlX1, float controlY1, float controlX2, float controlY2, float anchorX, float anchorY)
		{
			makePath();
	lastPath.cubicCurveTo(controlX1, controlY1, controlX2, controlY2, anchorX, anchorY);
			//inflateBound(controlX1, controlY1);
	//inflateBound(controlX2, controlY2);
	//inflateBound(anchorX, anchorY);
}
	}
}