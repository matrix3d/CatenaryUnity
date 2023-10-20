using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace display
{
	public class GraphicsSolidFill : IGraphicsFill, IGraphicsData
	{
		public Color color = Color.black;
		
		//public float alpha = 1.0f;
		//private var cssColor:String;
		//public var _glcolor:Array = [];
		public GraphicsSolidFill(Color color/*,float alpha = 1.0f*/)
		{
			this.color = color;
			//this.alpha = alpha;
			//cssColor = "rgba(" + (color >> 16 & 0xff) + "," + (color >> 8 & 0xff) + "," + (color & 0xff) + "," + this.alpha + ")";
		}

		public void gldraw(RenderingContext2D ctx, VertexHelper vh)
		{
			//Debug.Log("set color");
			ctx.fillStyle = this;


		}
	}
}