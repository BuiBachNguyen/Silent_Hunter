using System.Collections.Generic;
using System.Xml.XPath;
using UnityEngine;
using UnityEngine.UIElements;

public class PathFinder : MonoBehaviour
{
    #region Singleton
    public static PathFinder Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    #endregion

    [SerializeField] private GameObject player;
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    public void HandleObjectHovering(Vector3 pos)
    {
        Debug.Log("hovering position: " + pos);

        //FindPath(transform.position, pos);
    }

    public List<Vector3> FindPath(Vector3 start, Vector3 goal)
    {
        List<Vector3> path = new List<Vector3>();



        return path;
    }
}
