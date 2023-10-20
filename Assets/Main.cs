using display;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
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
	}

    private void LateUpdate()
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
		float a = float.NaN;
		//https://www.desmos.com/calculator/czpj1kmsvf?lang=zh-CN
		//https://zh.wikipedia.org/wiki/%E6%82%AC%E9%93%BE%E7%BA%BF
		var p = canvasPosition/100;//new Vector2(6.37f*(Mathf.Sin(Time.time)), 1.77f * (Mathf.Sin(Time.time*3)));
		var sg = Mathf.Sign(p.x);
		if(p.x < 0)
		{
			p.x = -p.x;
		}
		//p.x = Mathf.Sign(p.x) * float.Epsilon;
		var s = slider.value;
		if (s < p.magnitude)
		{
			s = p.magnitude;
		}

		float fx(float x)
		{
			return a * MathF.Cosh(x / a);
		}

		float Ax(float x)
		{
			return 2 * x * MathF.Sinh(p.x / (2 * x)) - Mathf.Sqrt(s * s - p.y * p.y);
		}

		float Bx(float x)
		{
			return 2 * MathF.Sinh(p.x / (2 * x)) - p.x * MathF.Cosh(p.x / (2 * x)) / x;
		}

		float Nx(float x)
		{
			var ax = Ax(x);
			var bx= Bx(x);
			if (float.IsNaN(ax) || float.IsNaN(bx)) return x;
			return x - ax / bx;
		}

		var I = p.x * p.x / (2 * s);
		for (var j = 0; j < 9; j++)
		{
			I = Nx(I);
		}
		a = I;
		var dx = (p.x - a * Mathf.Log((s + p.y) / (s - p.y))) / 2;
		//Debug.Log("dx:" + dx);
		var dy = -a * MathF.Cosh(-dx / a);
		//Debug.Log("dy:" + dy);
		shape.graphics.clear();
		shape.graphics.lineStyle(0, Color.blue);
		//shape.graphics.beginFill(default);
		shape.graphics.moveTo(0, 0);
		for (var x = 0f; x <= p.x; x += 0.1f)
		{
			var fxv = fx(x - dx);
			var y = fxv + dy;
			if (float.IsNaN(y)) y = 0;
			if (y<(p.y-s))
			{
				y = p.y - s;
			}
			shape.graphics.lineTo(x * 100*sg, y * 100);
		}
		shape.graphics.lineTo(p.x*100, p.y*100);
	}
}
