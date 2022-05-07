using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    [Header("Product Details")]
    public string productName;
    public GameObject factoryProduct;
    public float productPerSecond;
    public List<GameObject> generatedProducts;
    public int productLimit;
    public Player player;
    
    [Space(2)]

    [Header("Product Position")]
    public Transform generatePos,stackPos;
    public float xOffset, yOffset, zOffset;

    private float timer;
    private void Start()
    {
        timer = 1 / productPerSecond;
        player = FindObjectOfType<Player>();
    }
    void Update()
    {
        if (generatedProducts.Count<productLimit)
        {
            if (timer<=0)
            {
                GameObject g = Instantiate(factoryProduct,generatePos.position,Quaternion.identity,stackPos);
                generatedProducts.Add(g);
                timer = 1 / productPerSecond;
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }
        Move();
    }

    private void Move()
    {

        for (int i = 0; i < generatedProducts.Count; i++)
        {
            Vector3 pos = new Vector3();
            int multiplier = 0;
            if (i<5)
            {
                 multiplier = i;
                pos = stackPos.position + Vector3.forward * zOffset * multiplier;
            }
            else if (i < 10)
            {
                multiplier = i - 5;
                pos = stackPos.position + new Vector3(xOffset , 0 , zOffset*multiplier);
            }
            else if (i < 15)
            {
                multiplier = i - 10;
                pos = stackPos.position + new Vector3(0, yOffset, zOffset*multiplier);
            }
            else if (i < 20)
            {
                multiplier = i - 15;
                pos = stackPos.position + new Vector3(xOffset,yOffset,zOffset*multiplier);
            }

            generatedProducts[i].transform.position = Vector3.Lerp(generatedProducts[i].transform.position, pos, 0.1f);
        }

    }
    public float timeToremove=0.5f;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            if (!player.PickedUpObjects.ContainsKey(productName))
            {
                player.PickedUpObjects.Add(productName, new List<GameObject>());
            }
            if (generatedProducts.Count > 0 && player.collectedObjects.Count < player.collectedObjectLimit)
            {
                if (timeToremove <= 0)
                {
                    GameObject g = generatedProducts[generatedProducts.Count - 1];
                    player.collectedObjects.Add(g);
                    player.PickedUpObjects[productName].Add(g);
                    g.transform.parent = player.StackPos;
                    g.transform.rotation =Quaternion.Euler(player.StackPos.transform.rotation.x, 0, 90);
                    generatedProducts.Remove(g);
                    timeToremove = 0.5f * player.collectionSpeedMultiplier;
                }
                else
                {
                    timeToremove -= Time.deltaTime;
                }

            }

        }
    }

}
