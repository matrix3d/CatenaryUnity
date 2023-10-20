using display;
using System.Collections;
using UnityEngine;


public class Deom1 : UIShape
{

	// Use this for initialization
	void Start()
	{
		graphics.lineStyle(0, Color.black);
		graphics.beginFill(Color.red);
		graphics.drawRect(0, 0, 100, 50);
		graphics.endFill();
		graphics.beginFill(Color.green);
		graphics.drawRect(0, 100, 200, 100);
		graphics.endFill();
		graphics.beginFill(Color.yellow);
		graphics.drawCircle(0, -30, 30);
	}
}
