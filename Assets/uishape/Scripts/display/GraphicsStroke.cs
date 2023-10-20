using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace display
{
	public class GraphicsStroke: IGraphicsStroke, IGraphicsData
	{
		public float thickness;

		//public var pixelHinting:Boolean;

		//private var _caps:String;

		//private var _joints:String;

		//public var miterLimit:Number;

		//private var _scaleMode:String;

		public IGraphicsFill fill;

		public GraphicsStroke(float thickness =float.NaN,IGraphicsFill fill = null)
		{
			this.thickness = thickness;
			//this.pixelHinting = pixelHinting;
			//this._caps = (caps == "none") ? "butt" : caps;
			//this._joints = joints;
			//this.miterLimit = miterLimit;
			//this._scaleMode = scaleMode;  // TODO implement scaleMode
			this.fill = fill;
		}

		public void gldraw(RenderingContext2D ctx, VertexHelper vh)
		{
			//Debug.Log("set linewidth,set line color");
			//throw new System.NotImplementedException();
			ctx.lineWidth = thickness;
			if (fill is GraphicsSolidFill)
			{
				var sf = fill as GraphicsSolidFill;
				ctx.strokeStyle = sf;//SpriteFlexjs.renderer.getCssColor(sf.color, sf.alpha, colorTransform, null) as String;
				//ctx.strokeStyle = sf._glcolor as String; //sf.getCssColor(colorTransform);
			}
		}
	} 
}