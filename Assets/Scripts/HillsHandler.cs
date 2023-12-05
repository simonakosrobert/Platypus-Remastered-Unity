using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.OnScreen;

public class HillsMovement : MonoBehaviour
{
    public float speed;
    public int sortingOrder;
    public List<GameObject> PreFabs;
    public float offsetY;
    private List<GameObject> SpriteObjects = new List<GameObject>();

    public Camera cam;

    public static Vector3 objectSizeCalculator(Camera _cam ,GameObject _object)
    {
        Vector3 size = _cam.WorldToViewportPoint(_object.GetComponent<SpriteRenderer>().bounds.max) - _cam.WorldToViewportPoint(_object.GetComponent<SpriteRenderer>().bounds.min);
        return size;
    }

    Vector3 objectListSizeCalculator(Camera _cam, List<GameObject> _list)
    {   

        Vector3 size = new Vector3();

        if (_list.Count > 0)
        {
            foreach(GameObject _object in _list)
            {
                size += cam.WorldToViewportPoint(_object.GetComponent<SpriteRenderer>().bounds.max) - _cam.WorldToViewportPoint(_object.GetComponent<SpriteRenderer>().bounds.min);
            }
        }
        else
        {
            size = new Vector3(0f, 0f, 0f);
        }
        return size;

    }

    void addSprite()
    {   
        System.Random rnd = new System.Random();
        int randomSprite = rnd.Next(1, PreFabs.Count);
        GameObject clone = Instantiate(PreFabs[randomSprite], new Vector3(0, 0, 0), Quaternion.identity);
        Vector3 cloneSize = objectSizeCalculator(cam, clone);
        Vector3 listSize = objectListSizeCalculator(cam, SpriteObjects);
        Vector3 BottomLeft = cam.ViewportToWorldPoint(new Vector3(0f + cloneSize.x/2 + listSize.x, 0f + cloneSize.y/2 - offsetY, 1f));
        clone.transform.position = BottomLeft;
        clone.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;  
        
        SpriteObjects.Add(clone);
    }

    // Start is called before the first frame update
    void Start()
    {   
        while (objectListSizeCalculator(cam, SpriteObjects).x < 1.5f)
        {
            addSprite();
        }

    }

    List<GameObject> reSizedList(List<GameObject> oldList)
    {
        List<GameObject> newList = new List<GameObject>();

        for (int i = 0; i < oldList.Count; i++)
        {        
            Vector3 cloneSize = objectSizeCalculator(cam, oldList[i]);
            Vector3 listSize = objectListSizeCalculator(cam, newList);
            Vector3 BottomLeft = cam.ViewportToWorldPoint(new Vector3(0f + cloneSize.x/2 + listSize.x, 0f + cloneSize.y/2 - offsetY, 1f));
            oldList[i].transform.position = BottomLeft;
            oldList[i].GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;  
        
            newList.Add(oldList[i]);
        }

        return newList;
    }

    void sizeCalculator()
    {
        if (objectListSizeCalculator(cam, SpriteObjects).x < 1.5f)
        {
            addSprite();

            SpriteObjects = reSizedList(SpriteObjects);
        }
    }

    void HandleObjects()
    {   
        for (int i = SpriteObjects.Count - 1; i >= 0; i--)
        {        

            float newX = SpriteObjects[i].transform.position.x + speed;

            SpriteObjects[i].transform.position = new Vector3(newX, SpriteObjects[i].transform.position.y, 0);
            Vector3 RightX = cam.WorldToViewportPoint(SpriteObjects[i].GetComponent<Renderer>().bounds.max);
            if (RightX.x < 0.0f)
                {
                    Destroy(SpriteObjects[i]);
                    SpriteObjects.Remove(SpriteObjects[i]);
                }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleObjects();
        sizeCalculator();
    }
}
