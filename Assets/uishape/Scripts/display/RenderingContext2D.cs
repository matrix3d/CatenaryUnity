using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace display
{
	public class RenderingContext2D
	{
		internal GraphicsSolidFill fillStyle;
		internal float lineWidth;
		internal GraphicsSolidFill strokeStyle;
		GLPath2D currentPath;
		internal void fill(VertexHelper vh)
		{
			//throw new NotImplementedException();
			//Debug.Log("drawcurrentpath");
			currentPath.getDrawable(this,vh);
		}

		internal void drawPath(GraphicsPath graphicsPath)
		{
			currentPath = graphicsPath.glpath2d;
			if (currentPath==null)
			{
				currentPath= graphicsPath.glpath2d = new GLPath2D();
			}
			currentPath.path = graphicsPath;
			//throw new NotImplementedException();
		}
	}
}