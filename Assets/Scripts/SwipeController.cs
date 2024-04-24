using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeController : MonoBehaviour, IEndDragHandler
{
    [SerializeField] int maxPage;
    int currentPage;
    Vector3 targetPos;
    [SerializeField] Vector3 pageStep;
    [SerializeField] RectTransform levelPagesRect;
    [SerializeField] float tweenTime;
    [SerializeField] LeanTweenType tweenType;
    float dragThreshold;
    bool isSwiping = false;

    private void Awake()
    {
        currentPage = 1;
        targetPos = levelPagesRect.localPosition;
        dragThreshold = Screen.width / 15;
    }

    public void Next()
    {
        if (currentPage < maxPage)
        {
            currentPage++;
            targetPos += pageStep;
            MovePage();
        }
    }

    public void Previous()
    {
        if (currentPage > 1)
        {
            currentPage--;
            targetPos -= pageStep;
            MovePage();
        }
    }

    void MovePage()
    {
        isSwiping = true;
        levelPagesRect.LeanMoveLocal(targetPos, tweenTime).setEase(tweenType).setOnComplete(() => isSwiping = false);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isSwiping)
        {
            float dragDistance = Mathf.Abs(eventData.position.x - eventData.pressPosition.x);

            if (dragDistance > dragThreshold)
            {
                if (eventData.position.x > eventData.pressPosition.x)
                    Previous();
                else
                    Next();
            }
        }
    }
}


