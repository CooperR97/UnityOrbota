using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

using MoreMountains.CorgiEngine;
public class RotatorWalk : AIWalk {

    /// <summary>
    /// Casts a ray (if needed) to see if a target is in sight. If yes, moves towards it.
    /// </summary>
    protected override void CheckForTarget()
    {
        if (WalkBehaviour != WalkBehaviours.MoveOnSight)
        {
            return;
        }

        bool hit = false;

        _distanceToTarget = 0;
        // we cast a ray to the left of the agent to check for a Player
        Vector2 raycastOrigin = transform.position + MoveOnSightRayOffset;

        // we cast it to the left	
        RaycastHit2D raycast = MMDebug.RayCast(raycastOrigin, Vector2.left, ViewDistance, MoveOnSightLayer, Color.yellow, true);
        RaycastHit2D raycast2 = MMDebug.RayCast(raycastOrigin, new Vector2(-2, .5f), ViewDistance, MoveOnSightLayer, Color.yellow, true);
        RaycastHit2D raycast3 = MMDebug.RayCast(raycastOrigin, new Vector2(-2, 1), ViewDistance, MoveOnSightLayer, Color.yellow, true);
        RaycastHit2D raycast4 = MMDebug.RayCast(raycastOrigin, new Vector2(-2, -2), ViewDistance, MoveOnSightLayer, Color.yellow, true);
        RaycastHit2D raycast5 = MMDebug.RayCast(raycastOrigin, new Vector2(-2, -.5f), ViewDistance, MoveOnSightLayer, Color.yellow, true);
        RaycastHit2D raycast6 = MMDebug.RayCast(raycastOrigin, new Vector2(-2, -1), ViewDistance, MoveOnSightLayer, Color.yellow, true);
        // if we see a player
        if (raycast || raycast2 || raycast3 || raycast4 || raycast5 || raycast6)
        {
            hit = true;
            // we change direction
            _direction = Vector2.left;
            if (raycast)
            {
                _distanceToTarget = raycast.distance;
            } else if (raycast2)
            {
                _distanceToTarget = raycast2.distance;
            }
            else if (raycast4)
            {
                _distanceToTarget = raycast4.distance;
            }
            else if (raycast5)
            {
                _distanceToTarget = raycast5.distance;
            }
            else if (raycast6)
            {
                _distanceToTarget = raycast6.distance;
            }
        }

        // we cast a ray to the right of the agent to check for a Player	
        raycast = MMDebug.RayCast(raycastOrigin, Vector2.right, ViewDistance, MoveOnSightLayer, Color.yellow, true);
        raycast2 = MMDebug.RayCast(raycastOrigin, new Vector2(2, .5f), ViewDistance, MoveOnSightLayer, Color.yellow, true);
        raycast3 = MMDebug.RayCast(raycastOrigin, new Vector2(2, 1), ViewDistance, MoveOnSightLayer, Color.yellow, true);
        raycast4 = MMDebug.RayCast(raycastOrigin, new Vector2(2, -2), ViewDistance, MoveOnSightLayer, Color.yellow, true);
        raycast5 = MMDebug.RayCast(raycastOrigin, new Vector2(2, -.5f), ViewDistance, MoveOnSightLayer, Color.yellow, true);
        raycast6 = MMDebug.RayCast(raycastOrigin, new Vector2(2, -1), ViewDistance, MoveOnSightLayer, Color.yellow, true);
        if (raycast || raycast2 || raycast3 || raycast4 || raycast5 || raycast6)
        {
            hit = true;
            _direction = Vector2.right;
            if(raycast)
            {
                _distanceToTarget = raycast.distance;
            }
            else if (raycast2)
            {
                _distanceToTarget = raycast2.distance;
            }
            else if (raycast3)
            {
                _distanceToTarget = raycast3.distance;
            }
            else if (raycast4)
            {
                _distanceToTarget = raycast4.distance;
            }
            else if (raycast5)
            {
                _distanceToTarget = raycast5.distance;
            }
            else if (raycast6)
            {
                _distanceToTarget = raycast6.distance;
            }
        }

        // if the ray has hit nothing, or if we've reached our target, we prevent our character from moving further.
        if ((!hit) || (_distanceToTarget <= StopDistance))
        {
            _direction = Vector2.zero;
        }
        else
        {
            // if we've hit something, we make sure there's no obstacle between us and our target
            RaycastHit2D raycastObstacle = MMDebug.RayCast(raycastOrigin, _direction, ViewDistance, MoveOnSightObstaclesLayer, Color.gray, true);
            if (raycastObstacle && _distanceToTarget > raycastObstacle.distance)
            {
                _direction = Vector2.zero;
            }
        }
    }
}
