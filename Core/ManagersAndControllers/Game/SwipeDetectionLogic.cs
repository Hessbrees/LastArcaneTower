using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InputManager;

public class SwipeDetectionLogic
{
    private readonly SwipeDownEvent swipeDownDelegate;
    private readonly SwipeUpEvent swipeUpDelegate;
    private readonly ProjectileSwipeEvent projectileSwipeDelegate;

    private float minimumDistance;
    private float maximumTime;
    private float directionThreshold;

    private Vector2 startPosition;
    private float startTime;
    private Vector2 endPosition;
    private float endTime;

    public SwipeDetectionLogic(float directionThreshold, float maximumTime, float minimumDistance,
        SwipeDownEvent swipeDownDelegate, SwipeUpEvent swipeUpDelegate, ProjectileSwipeEvent projectileSwipeDelegate
        )
    {
        this.directionThreshold = directionThreshold;
        this.maximumTime = maximumTime;
        this.minimumDistance = minimumDistance;
        this.swipeDownDelegate = swipeDownDelegate;
        this.swipeUpDelegate = swipeUpDelegate;
        this.projectileSwipeDelegate = projectileSwipeDelegate;
    }

    public Vector2 GetSwipeDirection() => (endPosition - startPosition).normalized;

    public void SwipeStart(Vector2 position, float time)
    {
        startPosition = position;
        startTime = time;
    }

    public void SwipeEnd(Vector2 position, float time)
    {
        endPosition = position;
        endTime = time;
        DetectSwipe();
    }

    private void DetectSwipe()
    {
        if (Vector3.Distance(startPosition, endPosition) >= minimumDistance && (endTime - startTime) <= maximumTime)
        {
            Debug.DrawLine(startPosition, endPosition, Color.red, 5f);
            var position = endPosition - startPosition;
            SwipeDirection(position);
        }
    }

    private void SwipeDirection(Vector2 position)
    {
        if (Vector2.Dot(Vector2.down, position) > directionThreshold 
            && startPosition.y <= PlayerManager.Instance.RelativeInputPoint.position.y)
        {
            swipeDownDelegate(endPosition);
        }
        else if (Vector2.Dot(Vector2.up, position) > directionThreshold
            && startPosition.y <= PlayerManager.Instance.RelativeInputPoint.position.y)
        {
            swipeUpDelegate(endPosition);
        }
        else
        {
            projectileSwipeDelegate(GetSwipeDirection());
        }
    }
}
