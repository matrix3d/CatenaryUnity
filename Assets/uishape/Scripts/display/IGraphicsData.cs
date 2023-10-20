using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace display
{
	public interface IGraphicsData
	{
		void gldraw(RenderingContext2D ctx, VertexHelper vh);
	}
}
