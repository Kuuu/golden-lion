using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Pathfinding.Serialization;


[JsonOptIn]
public class EnemyGridGraph : GridGraph
{
    public override void RecalculateCell(int x, int z, bool resetPenalties = true, bool resetTags = true)
    {
        var node = nodes[z * width + x];

        // Set the node's initial position with a y-offset of zero
        node.position = GraphPointToWorld(x, z, 0);

        RaycastHit hit;

        bool walkable;

        // Calculate the actual position using physics raycasting (if enabled)
        // walkable will be set to false if no ground was found (unless that setting has been disabled)
        Vector3 position = collision.CheckHeight((Vector3)node.position, out hit, out walkable);
        node.position = (Int3)position;

        if (resetPenalties)
        {
            node.Penalty = initialPenalty;

            // Calculate a penalty based on the y coordinate of the node
            if (penaltyPosition)
            {
                node.Penalty += (uint)Mathf.RoundToInt((node.position.y - penaltyPositionOffset) * penaltyPositionFactor);
            }
        }

        if (resetTags)
        {
            node.Tag = 0;
        }

        // Check if the node is on a slope steeper than permitted
        if (walkable && useRaycastNormal && collision.heightCheck)
        {
            if (hit.normal != Vector3.zero)
            {
                // Take the dot product to find out the cosinus of the angle it has (faster than Vector3.Angle)
                float angle = Vector3.Dot(hit.normal.normalized, collision.up);

                // Add penalty based on normal
                if (penaltyAngle && resetPenalties)
                {
                    node.Penalty += (uint)Mathf.RoundToInt((1F - Mathf.Pow(angle, penaltyAnglePower)) * penaltyAngleFactor);
                }

                // Cosinus of the max slope
                float cosAngle = Mathf.Cos(maxSlope * Mathf.Deg2Rad);

                // Check if the ground is flat enough to stand on
                if (angle < cosAngle)
                {
                    walkable = false;
                }
            }
        }

        // If the walkable flag has already been set to false, there is no point in checking for it again
        // Check for obstacles
        node.Walkable = !(walkable && collision.Check((Vector3)node.position));

        // Store walkability before erosion is applied. Used for graph updates
        node.WalkableErosion = node.Walkable;
    }
    
}
