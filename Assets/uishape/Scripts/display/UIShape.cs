using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace display
{
	[RequireComponent(typeof(CanvasRenderer))]
	public class UIShape : MaskableGraphic
	{
		GraphicsEndFill endFillInstance = new GraphicsEndFill();
		static RenderingContext2D ctx=new RenderingContext2D();
		public  Graphics graphics=new Graphics();

		// Update is called once per frame
		void Update()
		{
			if (graphics.dirtyGraphics)
			{
				SetVerticesDirty();
				SetMaterialDirty();
				graphics.dirtyGraphics = false;
			}
		}

		void renderGraphics(display.Graphics g, VertexHelper vh)
		{
			
			var len = g.graphicsData.Count;
			for (var i = 0; i<len;i++ )
			{
				var igd = g.graphicsData[i];
				igd.gldraw(ctx,vh);
			}
			if (g.lastFill!=null || g.lastStroke!=null)
			{
				endFillInstance.fill = g.lastFill;
				//endFillInstance._worldMatrix = g._worldMatrix;
				endFillInstance.gldraw(ctx,vh);
			}
		}

		protected override void OnRectTransformDimensionsChange()
		{
			base.OnRectTransformDimensionsChange();
			SetVerticesDirty();
			SetMaterialDirty();
		}

		// actually update our mesh
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			vh.Clear();
			renderGraphics(graphics,vh);
			//Debug.Log("Mesh was redrawn!");
		}
	}
}