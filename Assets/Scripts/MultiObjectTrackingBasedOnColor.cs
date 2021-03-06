﻿using UnityEngine;
using System.Collections;
using OpenCVForUnity;
using System.Collections.Generic;
using System.Threading;

namespace UrbanTag
{
    /// <summary>
    /// Multi object tracking based on color sample.
    /// referring to the https://www.youtube.com/watch?v=hQ-bpfdWQh8.
    /// </summary>
    public class MultiObjectTrackingBasedOnColor : MonoBehaviour
    {
        /// <summary>
        /// The web cam texture.
        /// </summary>
        WebCamTexture webCamTexture;

        /// <summary>
        /// The web cam device.
        /// </summary>
        WebCamDevice webCamDevice;

        /// <summary>
        /// The colors.
        /// </summary>
        Color32[] colors;

        /// <summary>
        /// Should use front facing.
        /// </summary>
        public bool shouldUseFrontFacing = false;

        /// <summary>
        /// The width.
        /// </summary>
        public int width = 640;

        /// <summary>
        /// The height.
        /// </summary>
        public int height = 480;

        /// <summary>
        /// The rgba mat.
        /// </summary>
        Mat rgbMat;

        /// <summary>
        /// The texture.
        /// </summary>
        Texture2D texture;

        /// <summary>
        /// The init done.
        /// </summary>
        bool initDone = false;

        /// <summary>
        /// The screenOrientation.
        /// </summary>
        ScreenOrientation screenOrientation = ScreenOrientation.Unknown;

        /// <summary>
        /// max number of objects to be detected in frame
        /// </summary>
        public int MAX_NUM_OBJECTS = 6;

        /// <summary>
        /// minimum and maximum object area
        /// </summary>
        public int MIN_OBJECT_AREA = 20 * 20;

        /// <summary>
        /// max object area
        /// </summary>
        int MAX_OBJECT_AREA;

        /// <summary>
        /// The threshold mat.
        /// </summary>
        Mat thresholdMat;

        /// <summary>
        /// The hsv mat.
        /// </summary>
        Mat hsvMat;

        /// <summary>
        /// Coroutine utilisé pour effectuer le tracking
        /// </summary>
        IEnumerator coroutine;

        /// <summary>
        /// Color to track
        /// </summary>
        List<ColorObject> colorsToTrack;

        /// <summary>
        /// Associate color should be track?
        /// </summary>
        bool whiteShouldBeTrack;
        bool blueShouldBeTrack;
        bool yellowShouldBeTrack;
        bool redShouldBeTrack;
        bool greenShouldBeTrack;
        bool orangeShouldBeTrack;

        /// <summary>
        /// Color objects which need to appear on screen when traking don't need to run
        /// </summary>
        private List<ColorObject> colorToDraw = new List<ColorObject>();

        /// <summary>
        /// use as frame count between two tracking
        /// </summary>
        bool isTracking;

        private Thread trackingThread;

        public List<ColorObject> GetTargets()
        {
            return colorToDraw;
        }

        // Use this for initialization
        void Start()
        {
            FindColorsToTrack();
            Screen.orientation = ScreenOrientation.Portrait;
            StartCoroutine(init());
            Renderer renderer = GetComponent<Renderer>();
        }

        private void FindColorsToTrack()
        {
            for (int i = 0; i < GameState.players.Count; i++)
            {
                if (GameState.players[i].playerColor == "red")
                {
                    redShouldBeTrack = true;
                }
                else if (GameState.players[i].playerColor == "blue")
                {
                    blueShouldBeTrack = true;
                }
                else if (GameState.players[i].playerColor == "green")
                {
                    greenShouldBeTrack = true;
                }
                else if (GameState.players[i].playerColor == "yellow")
                {
                    yellowShouldBeTrack = true;
                }
                else if (GameState.players[i].playerColor == "orange")
                {
                    orangeShouldBeTrack = true;
                }
                else if (GameState.players[i].playerColor == "white")
                {
                    whiteShouldBeTrack = true;
                }
            }
        }

        private IEnumerator init()
        {
            if (webCamTexture != null)
            {
                webCamTexture.Stop();
                initDone = false;

                rgbMat.Dispose();
                thresholdMat.Dispose();
                hsvMat.Dispose();
            }

            // Checks how many and which cameras are available on the device
            for (int cameraIndex = 0; cameraIndex < WebCamTexture.devices.Length; cameraIndex++)
            {
                if (WebCamTexture.devices[cameraIndex].isFrontFacing == shouldUseFrontFacing)
                {
                    Debug.Log(cameraIndex + " name " + WebCamTexture.devices[cameraIndex].name + " isFrontFacing " + WebCamTexture.devices[cameraIndex].isFrontFacing);
                    webCamDevice = WebCamTexture.devices[cameraIndex];
                    webCamTexture = new WebCamTexture(webCamDevice.name, width, height);
                    break;
                }
            }

            if (webCamTexture == null)
            {
                webCamDevice = WebCamTexture.devices[0];
                webCamTexture = new WebCamTexture(webCamDevice.name, width, height);
            }

            Debug.Log("width " + webCamTexture.width + " height " + webCamTexture.height + " fps " +
                      webCamTexture.requestedFPS);


            // Starts the camera
            webCamTexture.Play();


            while (true)
            {
                //If you want to use webcamTexture.width and webcamTexture.height on iOS, you have to wait until webcamTexture.didUpdateThisFrame == 1, otherwise these two values will be equal to 16. (http://forum.unity3d.com/threads/webcamtexture-and-error-0x0502.123922/)
#if UNITY_IOS && !UNITY_EDITOR && (UNITY_4_6_3 || UNITY_4_6_4 || UNITY_5_0_0 || UNITY_5_0_1)
				                if (webCamTexture.width > 16 && webCamTexture.height > 16) {
								#else
                if (webCamTexture.didUpdateThisFrame)
                {
#if UNITY_IOS && !UNITY_EDITOR && UNITY_5_2
										while (webCamTexture.width <= 16) {
												webCamTexture.GetPixels32 ();
												yield return new WaitForEndOfFrame ();
										}
										#endif
#endif

                    Debug.Log("width " + webCamTexture.width + " height " + webCamTexture.height + " fps " +
                              webCamTexture.requestedFPS);
                    Debug.Log("videoRotationAngle " + webCamTexture.videoRotationAngle + " videoVerticallyMirrored " +
                              webCamTexture.videoVerticallyMirrored + " isFrongFacing " + webCamDevice.isFrontFacing);

                    colors = new Color32[webCamTexture.width * webCamTexture.height];
                    rgbMat = new Mat(webCamTexture.height, webCamTexture.width, CvType.CV_8UC3);
                    texture = new Texture2D(webCamTexture.width, webCamTexture.height, TextureFormat.RGBA32, false);


                    thresholdMat = new Mat();
                    hsvMat = new Mat();

                    MAX_OBJECT_AREA = (int) (webCamTexture.height * webCamTexture.width / 1.5);

                    gameObject.GetComponent<Renderer>().material.mainTexture = texture;

                    updateLayout();

                    screenOrientation = Screen.orientation;
                    initDone = true;

                    break;
                }
                else
                {
                    yield return 0;
                }
            }
            isTracking = false;
            colorsToTrack = new List<ColorObject>();
            //first blue

            if (blueShouldBeTrack)
            {
                colorsToTrack.Add(new ColorObject("blue"));
            }
            //then yellows
            if (yellowShouldBeTrack)
            {
                colorsToTrack.Add(new ColorObject("yellow"));
            }
            //then reds
            if (redShouldBeTrack)
            {
                colorsToTrack.Add(new ColorObject("red"));
            }
            //then greens
            if (greenShouldBeTrack)
            {
                colorsToTrack.Add(new ColorObject("green"));
            }
            //then orange
            if (orangeShouldBeTrack)
            {
                colorsToTrack.Add(new ColorObject("orange"));
            }
            if (whiteShouldBeTrack)
            {
                colorsToTrack.Add(new ColorObject("white"));
            }
        }

        private void updateLayout()
        {
            gameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);
            gameObject.transform.localScale = new Vector3(webCamTexture.width, webCamTexture.height, 1);
            if (webCamTexture.videoRotationAngle == 90 || webCamTexture.videoRotationAngle == 270)
            {
                gameObject.transform.eulerAngles = new Vector3(0, 0, -90);
            }
            float width = 0;
            float height = 0;
            if (webCamTexture.videoRotationAngle == 90 || webCamTexture.videoRotationAngle == 270)
            {
                width = gameObject.transform.localScale.y;
                height = gameObject.transform.localScale.x;
            }
            else if (webCamTexture.videoRotationAngle == 0 || webCamTexture.videoRotationAngle == 180)
            {
                width = gameObject.transform.localScale.x;
                height = gameObject.transform.localScale.y;
            }
            float widthScale = (float) Screen.width / width;
            float heightScale = (float) Screen.height / height;
            if (widthScale < heightScale)
            {
                Camera.main.orthographicSize = (width * (float) Screen.height / (float) Screen.width) / 2;
            }
            else
            {
                Camera.main.orthographicSize = height / 2;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (!initDone)
                return;

#if UNITY_IOS && !UNITY_EDITOR && (UNITY_4_6_3 || UNITY_4_6_4 || UNITY_5_0_0 || UNITY_5_0_1)
				        if (webCamTexture.width > 16 && webCamTexture.height > 16) {
						#else
            if (webCamTexture.didUpdateThisFrame)
            {
#endif
                Utils.webCamTextureToMat(webCamTexture, rgbMat, colors);
                if (webCamDevice.isFrontFacing)
                {
                    if (webCamTexture.videoRotationAngle == 0)
                    {
                        Core.flip(rgbMat, rgbMat, 1);
                    }
                    else if (webCamTexture.videoRotationAngle == 90)
                    {
                        Core.flip(rgbMat, rgbMat, 0);
                    }
                    if (webCamTexture.videoRotationAngle == 180)
                    {
                        Core.flip(rgbMat, rgbMat, 0);
                    }
                    else if (webCamTexture.videoRotationAngle == 270)
                    {
                        Core.flip(rgbMat, rgbMat, 1);
                    }
                }
                else if (webCamTexture.videoRotationAngle == 180 || webCamTexture.videoRotationAngle == 270)
                {
                    Core.flip(rgbMat, rgbMat, -1);
                }

                if (!isTracking)
                {
                    trackingThread = new Thread(trackingFunc);
                    trackingThread.Start();
                }
                Imgproc.cvtColor(rgbMat, hsvMat, Imgproc.COLOR_RGB2HSV);
                morphOps(thresholdMat);
                drawObjectsInMemory(thresholdMat, hsvMat, rgbMat);
                
                Utils.matToTexture2D(rgbMat, texture, colors);
            }
        }

        void OnDisable()
        {
            webCamTexture.Stop();
        }

        Size erodeSizeParam = new Size(3, 3);
        Size dilateSizeParam = new Size(20, 20);

        /// <summary>
        /// Morphs the ops.
        /// </summary>
        /// <param name="thresh">Thresh.</param>
        void morphOps(Mat thresh)
        {
            //create structuring element that will be used to "dilate" and "erode" image.
            //the element chosen here is a 3px by 3px rectangle
            Mat erodeElement = Imgproc.getStructuringElement(Imgproc.MORPH_RECT, erodeSizeParam);
            //dilate with larger element so make sure object is nicely visible
            Mat dilateElement = Imgproc.getStructuringElement(Imgproc.MORPH_RECT, dilateSizeParam);

            Imgproc.erode(thresh, thresh, erodeElement);
            //Imgproc.erode(thresh, thresh, erodeElement);

            Imgproc.dilate(thresh, thresh, dilateElement);
            //Imgproc.dilate(thresh, thresh, dilateElement);
        }

        /// <summary>
        /// Tracks the filtered object.
        /// </summary>
        /// <param name="theColorObject">The color object.</param>
        /// <param name="threshold">Threshold.</param>
        /// <param name="HSV">HS.</param>
        /// <param name="cameraFeed">Camera feed.</param>
        ColorObject trackFilteredObject(ColorObject theColorObject, Mat threshold, Mat HSV, Mat cameraFeed)
        {
            ColorObject tmpForMemory = new ColorObject();
            tmpForMemory.setType("null");
            Mat temp = new Mat();
            threshold.copyTo(temp);
            //these two vectors needed for output of findContours
            List<MatOfPoint> contours = new List<MatOfPoint>();
            Mat hierarchy = new Mat();
            //find contours of filtered image using openCV findContours function
            Imgproc.findContours(temp, contours, hierarchy, Imgproc.RETR_CCOMP, Imgproc.CHAIN_APPROX_SIMPLE);
            //use moments method to find our filtered object
            if (hierarchy.rows() > 0)
            {
                int numObjects = hierarchy.rows();
                //if number of objects greater than MAX_NUM_OBJECTS we have a noisy filter
                if (numObjects < MAX_NUM_OBJECTS)
                {
                    double areaMemory = 0;
                    for (int index = 0; index >= 0; index = (int) hierarchy.get(0, index)[0])
                    {
                        Moments moment = Imgproc.moments(contours[index]);
                        double area = moment.get_m00();
                        //if the area is less than 20 px by 20px then it is probably just noise
                        //if the area is the same as the 3/2 of the image size, probably just a bad filter
                        //we only want the object with the largest area so we safe a reference area each
                        //iteration and compare it to the area in the next iteration.
                        if (area > MIN_OBJECT_AREA)
                        {
                            ColorObject colorObject = new ColorObject();
                            colorObject.setXPos((int) (moment.get_m10() / area));
                            colorObject.setYPos((int) (moment.get_m01() / area));
                            colorObject.setType(theColorObject.getType());
                            colorObject.setColor(theColorObject.getColor());
                            if (area > areaMemory)
                            {
                                areaMemory = area;
                                tmpForMemory = colorObject;
                            }
                        }
                    }
                }
                else
                {
                    Core.putText(cameraFeed, "TOO MUCH NOISE!", new Point(5, cameraFeed.rows() - 10),
                        Core.FONT_HERSHEY_SIMPLEX, 1.0, new Scalar(255, 255, 255, 255), 2, Core.LINE_AA, false);
                }
            }
            return tmpForMemory;
        }

        private Point tmpDraw = new Point();

        /// <summary>
        /// draw the list of objects kept in memory
        /// </summary>
        /// <param name="threshold"></param>
        /// <param name="HSV"></param>
        /// <param name="cameraFeed"></param>
        void drawObjectsInMemory(Mat threshold, Mat HSV, Mat cameraFeed)
        {
            for (int i = 0; i < colorToDraw.Count; i++)
            {
                tmpDraw.x = colorToDraw[i].getXPos();
                tmpDraw.y = colorToDraw[i].getYPos();
                Core.circle(cameraFeed, tmpDraw, 50, colorToDraw[i].getColor());
            }
        }

        /// <summary>
        /// Coroutine de gestion du tracking
        /// </summary>
        /// <param name="threshold"></param>
        /// <param name="HSV"></param>
        /// <param name="cameraFeed"></param>
        /// <returns></returns>
        void trackingFunc()
        {
            isTracking = true;
            List<ColorObject> myMemory = new List<ColorObject>();
            ColorObject test;
            int i = 0;
            //first find blue objects
            while (i<colorsToTrack.Count)
            {
                Imgproc.cvtColor(rgbMat, hsvMat, Imgproc.COLOR_RGB2HSV);
                Core.inRange(hsvMat, colorsToTrack[i].getHSVmin(), colorsToTrack[i].getHSVmax(), thresholdMat);
                morphOps(thresholdMat);
                test = trackFilteredObject(colorsToTrack[i], thresholdMat, hsvMat, rgbMat);
                if(test != null && test.getType() != "null")
                {
                    myMemory.Add(test);
                }
                i++;
            }
            colorToDraw = myMemory;
            isTracking = false;
        }
    }
}