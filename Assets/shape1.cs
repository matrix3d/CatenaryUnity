using display;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shape1 :UIShape
{
    // Start is called before the first frame update
    void Start()
    {
        graphics.beginFill(Color.red);
        graphics.lineStyle(0, Color.black);
        graphics.drawRect(0, 0, 100, 200);

        graphics.beginFill(Color.blue);
        graphics.drawCircle(120, 0, 50);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
