using Recognizer.Dollar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;

[DefaultExecutionOrder(2)]
[RequireComponent(typeof(GestureController))]
public class GestureRecognizer : MonoBehaviour
{
    [SerializeField] private float PointMultiplier_X = 1f;
    [SerializeField] private float PointMultiplier_Y = 1f;
    [SerializeField] private float minimumScore = 0.8f;

    private bool touchingScreen;
    private List<TimePointF> points;
    private Recognizer.Dollar.Recognizer gestureRecognizer;
    private bool protractor = false;
    private string[] gestureNames;

    private GestureController gestureController;

    private bool spellRecognized;

    public string RecognizedSpellName { get; set; }

    void Start()
    {
        gestureController = GetComponent<GestureController>();
        gestureRecognizer = new Recognizer.Dollar.Recognizer();
        points = new List<TimePointF>();
        gestureNames = new string[100];

        gestureRecognizer.LoadGesturesFilesNames();
        gestureRecognizer.LoadGestures();

        LoadGestureNames();
    }

    void Update()
    {
        ReadTouchInput();
    }

    private void ReadTouchInput()
    {
        if (touchingScreen)
        {
            OnTouchMove();
        }
    }

    public void OnStartTouch(Vector2 position, float time)
    {
        if (!gestureController.SpecialSpellModeActive)
            return;

        var pos = Camera.main.WorldToViewportPoint(new Vector2(position.x * PointMultiplier_X, position.y * PointMultiplier_Y));
        touchingScreen = true;
        spellRecognized = false;
        points.Clear();
    }

    void OnTouchMove()
    {
        if (!gestureController.SpecialSpellModeActive)
            return;

        var pos = Camera.main.ScreenToViewportPoint(Touchscreen.current.primaryTouch.position.ReadValue());
        pos.x *= PointMultiplier_X;
        pos.y *= PointMultiplier_Y;
        points.Add(new TimePointF(pos.x, pos.y, TimeEx.NowMs));
    }

    public void OnEndTouch(Vector2 position, float time)
    {
        if (!gestureController.SpecialSpellModeActive)
            return;

        if (touchingScreen)
        {
            touchingScreen = false;

            NBestList result = gestureRecognizer.Recognize(points, protractor);

            if (result == null || result[0].Score < minimumScore)
            {
                RecognizedSpellName = Settings.GestureErrorMessage;
                points.Clear();
            }
            else
                spellRecognized = true;

            if (spellRecognized)
                RecognizedSpellName = result[0].Name;

            gestureController.OnGestureRead(RecognizedSpellName);
        }
    }

    private void LoadGestureNames()
    {
        var gestureTemplateNames = Resources.LoadAll("Gesture Templates");

        int counter = 0;

        foreach (var item in gestureTemplateNames)
        {
            TextAsset textAsset = (TextAsset)item;
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(textAsset.text);

            XmlElement root = xmldoc.DocumentElement;
            string name = root.Attributes["Name"].Value;

            gestureNames[counter] = name;
            counter++;
        }
    }
}

