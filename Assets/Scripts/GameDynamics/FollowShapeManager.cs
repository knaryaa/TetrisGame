using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowShapeManager : MonoBehaviour
{
    private ShapeManager followShape = null;

    private bool isTouchedGround = false;

    public Color color = new Color(1f, 1f, 1f, .2f);

    public void CreateFollowShapeFNC(ShapeManager realShape, BoardManager board)
    {
        if (!followShape)
        {
            followShape = Instantiate(realShape,realShape.transform.position,realShape.transform.rotation) as ShapeManager;

            followShape.name = "FollowShape";

            SpriteRenderer[] allSprite = followShape.GetComponentsInChildren<SpriteRenderer>();

            foreach (SpriteRenderer sr in allSprite)
            {
                sr.color = color;
            }
        }
        else
        {
            followShape.transform.position = realShape.transform.position;
            followShape.transform.rotation = realShape.transform.rotation;
        }

        isTouchedGround = false;

        while (!isTouchedGround)
        {
            followShape.DownMoveFNC();
            if (!board.IsInPosition(followShape))
            {
                followShape.UpMoveFNC();
                isTouchedGround = true;
            }
        }
    }

    public void ResetFNC()
    {
        Destroy(followShape.gameObject);
    }
}
