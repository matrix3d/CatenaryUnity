using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace display
{
	public class GraphicsEndFill : IGraphicsFill, IGraphicsData
	{
		public IGraphicsFill fill;
		public Matrix4x4 _worldMatrix = Matrix4x4.identity;

		public void gldraw(RenderingContext2D ctx, VertexHelper vh)
		{
			ctx.fill(vh);
		}
	}
}