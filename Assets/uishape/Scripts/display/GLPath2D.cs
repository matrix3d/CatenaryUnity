using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace display
{
	public class GLPath2D 
	{
		private static bool useEarcut = false;
		public GraphicsPath path;
		public void getDrawable(RenderingContext2D gl,VertexHelper vh){
			//if (path.gpuPath2DDirty){
			//	path.gpuPath2DDirty = false;
				
			var isDrawline = gl.strokeStyle != null;
				
			var polys = path.polys;
				
			var nump = 0;
			var numi = 0;
			var len = path.polys.Count;
			for (var i = 0; i<len;i++ ){
				var plen = path.polys[i].Count;
				nump += plen;
				if(plen>=6){
					numi += (plen / 2 - 2) * 3;
				}
				var isClosePath = path.closePathIndex.ContainsKey(i);
				//isClosePath = false;
				var pathplen = plen / 2;
				//line
				if (isDrawline&&pathplen>=2){
					if (isClosePath){
						nump += pathplen* 8;
						numi += pathplen* 6;
					}else
					{
						nump += (pathplen - 1) * 8;
						numi += (pathplen - 1) * 6;
					}
				}
			}
			/*var tlen = path.tris.length;
			var diffuv:Boolean = false;
			for (i = 0; i < tlen; i++)
			{
				var tri:Array = path.tris[i];
				nump += tri[0].length as Number;
				numi += tri[1].length as Number;
				if (tri[2])
				{
					diffuv = true;
				}
			}*/

			Color colorv=default;
			var isDrawFill = gl.fillStyle != null;
			if (isDrawFill)
			{
				colorv = gl.fillStyle.color;
			}
			var offset = vh.currentVertCount;
			var pi = 0;
			var ii = 0;
			for (var i = 0; i < len; i++)
			{
				var poly = polys[i];
				if (useEarcut)
				{// todo : earcut
					//var ear:Earcut = new Earcut;
					//var ins:Array = ear.earcut(poly);
				}
				else
				{
					var plen = poly.Count;
					var plendiv2 = plen / 2;
					var j = 0;
					for (j = 0; j < plendiv2; j++ ){
						var x = poly[2 * j];
						var y = poly[2 * j + 1];
						//color[pi / 2] = colorv;
						//pos[pi++] = x;
						//pos[pi++] = y;
						UIVertex vert = new UIVertex();
						vert.position = new Vector3(x, y, 0);
						vert.color = colorv;
						vh.AddVert(vert);

						if (j >= 2&&isDrawFill)
						{
							//index[ii++] = offset;
							//index[ii++] = offset + j - 1;
							//index[ii++] = offset + j;
							vh.AddTriangle(offset, offset + j, offset + j-1);

						}
					}
					offset += j;
				}
			}
				
			/*for (i = 0; i < tlen; i++)
			{
				tri = path.tris[i];
				var vsdata:Vector.< Number > = tri[0];
				var idata:Vector.< int > = tri[1];
				var uvdata:Vector.< Number > = tri[2];
				var len2:int = vsdata.length as Number;
				for (j = 0; j < len2; j++)
				{
					pos[pi] = vsdata[j];
					if (uvdata)
						uv[pi] = uvdata[j];
					if (j % 2 == 0)
					{
						color[pi / 2] = colorv;
					}
					pi++;
				}
				len2 = idata.length as Number;
				for (j = 0; j < len2; j++)
				{
					index[ii++] = offset + idata[j];
				}
				offset += vsdata.length / 2;
			}*/

			if (isDrawline)
			{
				var hw = gl.lineWidth / 2;
				if (hw <= 0)
				{
					hw = .5f;
				}
				// todo : 1pixel width line no need JointStyle
				var lcolorv = gl.strokeStyle.color;
				for (var i = 0; i < len; i++)
				{
					var poly = polys[i];
					var isClosePath = path.closePathIndex.ContainsKey(i);
					//isClosePath = false;
					var plen = poly.Count;
					var plendiv2 = plen / 2;
					if (plendiv2 >= 2)
					{
						for (var j = 0; j < plendiv2; j++ )
						{
							if (j != 0 || isClosePath)
							{
								var x0 = poly[2 * j];
								var y0 = poly[2 * j + 1];
								float x1, y1;
								if (j == 0)
								{
									x1 = poly[plen - 2];
									y1 = poly[plen - 1];
								}
								else
								{
									x1 = poly[2 * j - 2];
									y1 = poly[2 * j - 1];
								}

								/////////////////////
								var dy = x1 - x0;
								var dx = y1 - y0;
								var distance = Mathf.Sqrt(dx * dx + dy * dy);
								dx *= hw / distance;
								dy *= -hw / distance;

								/////////////////////

								//color[pi / 2] = lcolorv;
								//pos[pi++] = x0 + dx;
								//pos[pi++] = y0 + dy;
								//color[pi / 2] = lcolorv;
								//pos[pi++] = x0 - dx;
								//pos[pi++] = y0 - dy;
								//color[pi / 2] = lcolorv;
								//pos[pi++] = x1 + dx;
								//pos[pi++] = y1 + dy;
								//color[pi / 2] = lcolorv;
								//pos[pi++] = x1 - dx;
								//pos[pi++] = y1 - dy;

								UIVertex vert = new UIVertex();
								if (float.IsNaN(dx)) dx = 0;
								if (float.IsNaN(dy)) dy = 0;
								vert.position = new Vector3(x0 + dx, y0 + dy, 0);
								vert.color = lcolorv;
								vh.AddVert(vert);

								 vert = new UIVertex();
								vert.position = new Vector3(x0 - dx, y0 - dy, 0);
								vert.color = lcolorv;
								vh.AddVert(vert);

								 vert = new UIVertex();
								vert.position = new Vector3(x1 + dx, y1 + dy, 0);
								vert.color = lcolorv;
								vh.AddVert(vert);

								 vert = new UIVertex();
								vert.position = new Vector3(x1 - dx, y1 - dy, 0);
								vert.color = lcolorv;
								vh.AddVert(vert);

								var a = offset;
								var b = offset + 1;
								var c = offset + 2;
								var d = offset + 3;


								//index[ii++] = a;
								//index[ii++] = b;
								//index[ii++] = c;
								//index[ii++] = b;
								//index[ii++] = d;
								//index[ii++] = c;
								vh.AddTriangle(a, b, c);
								vh.AddTriangle(b, d, c);
								offset += 4;
							}
						}
					}
				}
					
			}
		}
	}
}