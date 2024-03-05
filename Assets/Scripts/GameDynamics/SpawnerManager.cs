using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField] ShapeManager[] allObjects;

    [SerializeField] private Image[] blockImages = new Image[2];

    private ShapeManager[] nextBlocks = new ShapeManager[2];

    

    public ShapeManager CreateObjectFNC()
    {
        ShapeManager block = null;

        block = GetNextShapeFNC();
        block.gameObject.SetActive(true);
        block.transform.position = transform.position;
        
        if (block != null)
        {
            return block;
        }
        else
        {
            return null;
        }
    }
    
    public void ChangeAllToNullFNC()
    {
        for (int i = 0; i < nextBlocks.Length; i++)
        {
            nextBlocks[i] = null;
        }
        
        CreateNextFNC();
    }

    void CreateNextFNC()
    {
        for (int i = 0; i < nextBlocks.Length; i++)
        {
            if (!nextBlocks[i])
            {
                nextBlocks[i] = Instantiate(CreateRandomBlockFNC(),transform.position, Quaternion.identity) as ShapeManager;
                nextBlocks[i].gameObject.SetActive(false);
                blockImages[i].sprite = nextBlocks[i].shapeBlock;
            }
        }

        StartCoroutine(BlockImageOnRoutine());
    }

    IEnumerator BlockImageOnRoutine()
    {
        for (int i = 0; i < blockImages.Length; i++)
        {
            blockImages[i].GetComponent<CanvasGroup>().alpha = 0f;
            blockImages[i].GetComponent<RectTransform>().localScale = Vector3.zero;
        }

        yield return new WaitForSeconds(.1f);

        int count = 0;

        while (count < blockImages.Length)
        {
            blockImages[count].GetComponent<CanvasGroup>().DOFade(1,.6f);
            blockImages[count].GetComponent<RectTransform>().DOScale(1, .6f).SetEase(Ease.OutBack);
            count++;

            yield return new WaitForSeconds(.4f);
        }
        
    }
    
    ShapeManager CreateRandomBlockFNC()
    {
        int randomBlock = Random.Range(0, allObjects.Length);

        if (allObjects[randomBlock])
        {
            return allObjects[randomBlock];
        }
        else
        {
            return null;
        }
    }

    ShapeManager GetNextShapeFNC()
    {
        ShapeManager nextBlock = null;

        if (nextBlocks[0])
        {
            nextBlock = nextBlocks[0];
        }

        for (int i = 1; i < nextBlocks.Length; i++)
        {
            nextBlocks[i - 1] = nextBlocks[i];
            blockImages[i - 1].sprite = nextBlocks[i - 1].shapeBlock;
        }

        nextBlocks[nextBlocks.Length - 1] = null;
        
        CreateNextFNC();

        return nextBlock;
    }

    public ShapeManager CreateHoldShapeFNC()
    {
        ShapeManager holdShape = null;
        holdShape = Instantiate(CreateRandomBlockFNC(),transform.position,Quaternion.identity) as ShapeManager;
        holdShape.transform.position = transform.position;

        return holdShape;
    }
}
