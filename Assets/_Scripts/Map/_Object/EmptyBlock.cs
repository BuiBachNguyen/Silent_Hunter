using UnityEngine;
using UnityEngine.EventSystems;

public class EmptyBlock : Block, IPointerDownHandler, IPointerUpHandler
{
    private int x;
    private int v;
    private int z;

    public EmptyBlock(int x, int v, int z)
    {
        this.x = x;
        this.v = v;
        this.z = z;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
        Debug.Log("UP");
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
        Debug.Log("down");
    }
}
