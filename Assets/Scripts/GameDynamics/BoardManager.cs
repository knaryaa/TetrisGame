using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private Transform tilePrefab;

    public int height = 22;
    public int width = 10;

    private Transform[,] grid;

    public int clearedLines = 0;

    public ParticleManager[] rowEffects = new ParticleManager[4];
    private void Awake()
    {
        grid = new Transform[width, height];
    }

    private void Start()
    {
        CreateEmptySquaresFNC();
    }

    bool InBoard(int x, int y)
    {
        return (x >= 0 && x < width && y >= 0);
    }

    bool FullSquare(int x, int y, ShapeManager shape)
    {
        return (grid[x, y] != null && grid[x, y].parent != shape.transform);
    }

    public bool IsInPosition(ShapeManager shape)
    {
        foreach (Transform child in shape.transform)
        {
            Vector2 pos = VectorToIntFNC(child.position);
            
            if(!InBoard((int)pos.x,(int)pos.y))
            {
                return false;
            }

            if (pos.y < height)
            {
                if(FullSquare((int)pos.x,(int)pos.y, shape))
                {
                    return false;
                }
            }
            
        }

        return true;
    }
    
    void CreateEmptySquaresFNC()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Transform tile=Instantiate(tilePrefab, new Vector3(x,y,0), Quaternion.identity);
                tile.name="x "+x.ToString()+" ,"+" y"+y.ToString();
            }
        }
    }

    public void PushObjectToGridFNC(ShapeManager shape)
    {
        if (shape == null)
        {
            return;
        }

        foreach (Transform child in shape.transform)
        {
            Vector2 pos = VectorToIntFNC(child.position);
            grid[(int)pos.x, (int)pos.y] = child;
        }
    }

    bool isFullRowFNC(int y)
    {
        for (int x = 0; x < width; ++x)
        {
            if (grid[x, y] == null)
            {
                return false;
            }
        }

        return true;
    }

    void ClearRowFNC(int y)
    {
        for (int x = 0; x < width; ++x)
        {
            if (grid[x, y] != null)
            {
                Destroy(grid[x,y].gameObject);
            }

            grid[x, y] = null;
        }
    }

    void LowerRowFNC(int y)
    {
        for (int x = 0; x < width; ++x)
        {
            if (grid[x, y] != null)
            {
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;
                grid[x,y-1].position+=Vector3.down;
            }
        }
    }

    void LowerAllRowsFNC(int startY)
    {
        for (int i = startY; i < height; ++i)
        {
            LowerRowFNC(i);
        }
    }

    public IEnumerator ClearAllRowsFNC()
    {
        clearedLines = 0;

        for (int y = 0; y < height; ++y)
        {
            if (isFullRowFNC(y))
            {
                StartRowEffectFNC(clearedLines,y);
                clearedLines++;
            }
        }

        yield return new WaitForSeconds(.5f);
        for (int y = 0; y < height; y++)
        {
            if (isFullRowFNC(y))
            {
                ClearRowFNC(y);
                LowerAllRowsFNC(y+1);
                yield return new WaitForSeconds(.2f);
                y--;
            }
        }
    }

    public bool IsOverflowFNC(ShapeManager shape)
    {
        foreach (Transform child in shape.transform)
        {
            if (child.transform.position.y >= height - 1)
            {
                return true;
            }
        }

        return false;
    }
    
    Vector2 VectorToIntFNC(Vector2 vector)
    {
        return new Vector2(Mathf.Round(vector.x), Mathf.Round(vector.y));
    }

    void StartRowEffectFNC(int rowCount, int y)
    {
        // if (rowsEffect)
        // {
        //     rowsEffect.transform.position = new Vector3(0, y, 0);
        //     rowsEffect.EffectPlayFNC();
        // }

        if (rowEffects[rowCount])
        {
            rowEffects[rowCount].transform.position = new Vector3(0, y, 0);
            rowEffects[rowCount].EffectPlayFNC();
        }
    }
}