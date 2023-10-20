using display;
using System.Collections;
using UnityEngine;

public class Deom2 : UIShape
{

	// Use this for initialization
	void Start()
	{
		for (var i=0;i<10000;i++)
		{
			graphics.beginFill(Random.ColorHSV());
			graphics.drawRect(Random.Range(-200,200), Random.Range(-200, 200), 10,10);
		}
	}
}
