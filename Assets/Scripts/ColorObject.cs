using UnityEngine;
using System.Collections;
using OpenCVForUnity;

public class ColorObject
{
    int xPos, yPos;
    string type;
    Scalar HSVmin, HSVmax;
    Scalar Color;

    public ColorObject()
    {
        //set values for default constructor
        setType("Object");
        setColor(new Scalar(0, 0, 0));
    }

    public ColorObject(string name)
    {
        /*
        HUE VALUE
        Orange  0 - 22
        Yellow 22 - 38
        Green 38 - 75
        Blue 75 - 130
        Violet 130 - 160
        Red 160 - 179
        */
        setType(name);
        if (name == "blue")
        {
            //and HSV max values
            setHSVmin(new Scalar(75, 150, 50));
            setHSVmax(new Scalar(130, 255, 255));
            //BGR value for Green:
            setColor(new Scalar(0, 0, 255));
        }
        if (name == "green")
        {
            //and HSV max values
            setHSVmin(new Scalar(38, 150, 50));
            setHSVmax(new Scalar(75, 255, 255));
            //BGR value for Yellow:
            setColor(new Scalar(0, 255, 0));
        }
        if (name == "yellow")
        {
            //and HSV max values
            setHSVmin(new Scalar(22, 150, 50));
            setHSVmax(new Scalar(38, 255, 255));
            //BGR value for Red:
            setColor(new Scalar(255, 255, 0));
        }
        if (name == "red")
        {
            //and HSV max values
            setHSVmin(new Scalar(160, 150, 50));
            setHSVmax(new Scalar(179, 255, 255));
            //BGR value for Red:
            setColor(new Scalar(255, 0, 0));
        }
        if (name == "orange")
        {
            //and HSV max values
            setHSVmin(new Scalar(0, 150, 50));
            setHSVmax(new Scalar(22, 255, 255));
            //BGR value for Yellow:
            setColor(new Scalar(255, 153, 0));
        }
    }

    public int getXPos()
    {
        return xPos;
    }

    public void setXPos(int x)
    {
        xPos = x;
    }

    public int getYPos()
    {
        return yPos;
    }

    public void setYPos(int y)
    {
        yPos = y;
    }

    public Scalar getHSVmin()
    {
        return HSVmin;
    }

    public Scalar getHSVmax()
    {
        return HSVmax;
    }

    public void setHSVmin(Scalar min)
    {
        HSVmin = min;
    }

    public void setHSVmax(Scalar max)
    {
        HSVmax = max;
    }

    public string getType()
    {
        return type;
    }

    public void setType(string t)
    {
        type = t;
    }

    public Scalar getColor()
    {
        return Color;
    }

    public void setColor(Scalar c)
    {
        Color = c;
    }

    public void setBound(Scalar hsvColor)
    {
        if (hsvColor.val[0] < HSVmin.val[0])
        {
            HSVmin.val[0] = hsvColor.val[0];
        }
        if (hsvColor.val[1] < HSVmin.val[1])
        {
            HSVmin.val[1] = hsvColor.val[1];
        }
        if (hsvColor.val[2] < HSVmin.val[2])
        {
            HSVmin.val[2] = hsvColor.val[2];
        }


        if (hsvColor.val[0] > HSVmax.val[0])
        {
            HSVmax.val[0] = hsvColor.val[0];
        }
        if (hsvColor.val[1] > HSVmax.val[1])
        {
            HSVmax.val[1] = hsvColor.val[1];
        }
        if (hsvColor.val[2] > HSVmax.val[2])
        {
            HSVmax.val[2] = hsvColor.val[2];
        }
    }
}
