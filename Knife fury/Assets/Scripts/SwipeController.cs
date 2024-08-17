using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeController : MonoBehaviour, IEndDragHandler
{
    [SerializeField] int maxPage;
    int currentPage;
    Vector3 TargetPos;
    [SerializeField] Vector3 pagestep;

    [SerializeField] RectTransform levelPageRect;
    [SerializeField] float tweenTime;
    [SerializeField] LeanTweenType tweenType;
    float dragThreshould;

    private void Awake()
    {
        currentPage = 1;
        TargetPos = levelPageRect.localPosition;
        dragThreshould = Screen.width / 15;
    }

    public void Next()
    {
        if (currentPage < maxPage)
        {
            currentPage++;
            TargetPos += pagestep;
            MovePage();
        }
    }

    public void Previous()
    {
        if (currentPage > 1)
        {
            currentPage--;
            TargetPos -= pagestep;
            MovePage();
        }
    }

    void MovePage()
    {
        levelPageRect.LeanMoveLocal(TargetPos, tweenTime).setEase(tweenType);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Mathf.Abs(eventData.position.x - eventData.pressPosition.x) > dragThreshould)
        {
            if (eventData.position.x > eventData.pressPosition.x) Previous();
            else Next();
        }
        else
        {
            MovePage();
        }
    }

    // Method to get the current page
    public int GetCurrentPage()
    {
        return currentPage;
    }
}
