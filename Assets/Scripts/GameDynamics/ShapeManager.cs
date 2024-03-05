using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShapeManager : MonoBehaviour
{
    [SerializeField]
    private bool canTurn=true;

    public Sprite shapeBlock;

    GameObject[] placedEffects;

    //public string effectName="PlacedEffect";

    private void Start()
    {
        placedEffects = GameObject.FindGameObjectsWithTag("PlacedEffect");
    }

    public void CreatePlacedEffectFNC()
    {
        int count = 0;
        foreach (Transform child in gameObject.transform)
        {
            if (placedEffects[count])
            {
                placedEffects[count].transform.position = new Vector3(child.position.x, child.position.y, 0f);

                ParticleManager particleManager = placedEffects[count].GetComponent<ParticleManager>();

                if (particleManager)
                {
                    particleManager.EffectPlayFNC();
                }
            }

            count++;
        }
    }

    public void LeftMoveFNC()
    {
        transform.Translate(Vector3.left,Space.World);
    }
    public void RightMoveFNC()
    {
        transform.Translate(Vector3.right,Space.World);
    }
    public void DownMoveFNC()
    {
        transform.Translate(Vector3.down,Space.World);
    }
    public void UpMoveFNC()
    {
        transform.Translate(Vector3.up,Space.World);
    }

    public void RightTurnFNC()
    {
        if (canTurn)
        {
            transform.Rotate(0, 0, -90);
        }
    }
    public void LeftTurnFNC()
    {
        if (canTurn)
        {
            transform.Rotate(0, 0, 90);
        }
    }

    IEnumerator MoveRoutine()
    {
        while (true)
        {
            DownMoveFNC();
            yield return new WaitForSeconds(.25f);
        }
    }

    public void clockwiseTurnFNC(bool clockwise)
    {
        if (clockwise)
        {
            RightTurnFNC();
        }
        else
        {
            LeftTurnFNC();
        }
    }
}
