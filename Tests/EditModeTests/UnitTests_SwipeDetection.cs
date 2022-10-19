using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools.Utils;

public class UnitTests_SwipeDetection
{
    public class PointTestCase
    {
        public string TestName { get; private set; }
        public Vector2 P1 { get; private set; }
        public Vector2 P2 { get; private set; }
        public Vector2 Expected { get; set; }

        public PointTestCase(Vector2 p1, Vector2 p2, Vector2 expected)
        {
            P1 = p1;
            P2 = p2;
            Expected = expected;
        }

        public override string ToString()
        {
            return TestName;
        }
    }

    private static IEnumerable<PointTestCase> PointTestCases
    {
        get
        {
            yield return new PointTestCase(new Vector2(0, 0), new Vector2(0, 1), new Vector2(0, 1));
            yield return new PointTestCase(new Vector2(0, 0), new Vector2(0, -1), new Vector2(0, -1));
            yield return new PointTestCase(new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 0));
            yield return new PointTestCase(new Vector2(0, 0), new Vector2(-1, 0), new Vector2(-1, 0));
            yield return new PointTestCase(new Vector2(52.763f, 23.345f), new Vector2(-26.875f, 34.657f), new Vector2(-0.99f, 0.14f));
            yield return new PointTestCase(new Vector2(-52.763f, 23.345f), new Vector2(26.875f, 34.657f), new Vector2(0.99f, 0.14f));
            yield return new PointTestCase(new Vector2(8.763f, 23.345f), new Vector2(26.875f, -34.657f), new Vector2(0.30f, -0.95f));
            yield return new PointTestCase(new Vector2(52.763f, 23.345f), new Vector2(-26.875f, -34.657f), new Vector2(-0.81f, -0.59f));
        }
    }

    [Test]
    [TestCaseSource("PointTestCases")]
    public void GetSwipeDirection_ReturnsValidDirection(PointTestCase pointTestCase)
    {
        var mockSwipeDown = new Mock<InputManager.SwipeDownEvent>();
        var mockSwipeUp = new Mock<InputManager.SwipeUpEvent>();
        var mockProjectileSwipe = new Mock<InputManager.ProjectileSwipeEvent>();

        var swipeDetectionLogic = new SwipeDetectionLogic(0.9f, 1f, 0.2f,
            mockSwipeDown.Object, mockSwipeUp.Object, mockProjectileSwipe.Object);

        var comparer = new Vector2EqualityComparer(10e-3f);

        swipeDetectionLogic.SwipeStart(pointTestCase.P1, 0f);
        swipeDetectionLogic.SwipeEnd(pointTestCase.P2, 0f);

        var swipeDirection = swipeDetectionLogic.GetSwipeDirection();

        Assert.That(swipeDirection, Is.EqualTo(pointTestCase.Expected).Using(comparer));
    }
}
