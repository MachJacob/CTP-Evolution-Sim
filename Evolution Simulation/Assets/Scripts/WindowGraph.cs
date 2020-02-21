using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowGraph : MonoBehaviour
{
    [SerializeField] private Sprite circleSprite;
    private RectTransform graphContainer;
    private List<GameObject> graph;

    private void Awake()
    {
        graphContainer = transform.Find("GraphContainer").GetComponent<RectTransform>();
        graph = new List<GameObject>();
    }

    private GameObject CreateCircle(Vector2 anchoredPos)
    {
        GameObject circle = new GameObject("Circle", typeof(Image));
        circle.transform.SetParent(graphContainer, false);
        circle.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTrans = circle.GetComponent<RectTransform>();
        rectTrans.anchoredPosition = anchoredPos;
        rectTrans.sizeDelta = new Vector2(11, 11);
        rectTrans.anchorMin = new Vector2(0, 0);
        rectTrans.anchorMax = new Vector2(0, 0);

        return circle;
    }
    
    public void ShowGraph(List<float> valueList)
    {
        foreach (GameObject obj in graph)
        {
            Destroy(obj);
        }
        graph.Clear();
        float graphHeight = graphContainer.sizeDelta.y;
        float yMax = 100f;
        float xSize = graphContainer.sizeDelta.x / valueList.Count;

        GameObject lastCircle = null;

        for(int i = 0; i < valueList.Count; i++)
        {
            float xPos = i * xSize;
            float yPos = (valueList[i] / yMax) * graphHeight;
            GameObject circle = CreateCircle(new Vector2(xPos, yPos));
            graph.Add(circle);
            if (lastCircle != null)
            {
                graph.Add(CreateDotConnection(lastCircle.GetComponent<RectTransform>().anchoredPosition, 
                    circle.GetComponent<RectTransform>().anchoredPosition));
            }
            lastCircle = circle;
        }
    }

    private GameObject CreateDotConnection(Vector2 dotA, Vector2 dotB)
    {
        GameObject line = new GameObject("DotConnection", typeof(Image));
        line.transform.SetParent(graphContainer, false);
        line.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
        Vector2 dir = (dotB - dotA).normalized;
        float distance = Vector2.Distance(dotA, dotB);
        RectTransform rectTrans = line.GetComponent<RectTransform>();
        rectTrans.anchorMin = new Vector2(0, 0);
        rectTrans.anchorMax = new Vector2(0, 0);
        rectTrans.sizeDelta = new Vector2(distance, 3f);
        rectTrans.anchoredPosition = dotA + dir * distance * 0.5f;
        rectTrans.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);

        return line;
    }
}
