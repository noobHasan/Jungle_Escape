using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    #region variable

    public Transform StackPos;
    public Transform UnstackPos;

    public LayerMask axeUseLayer;
    public GameObject axe;
    public Animator anim;
    public bool isAxing;
    public float stackUpScale;
    public float collectionSpeedMultiplier=1;
    public Dictionary<string, List<GameObject>> PickedUpObjects = new Dictionary<string, List<GameObject>>();

    public int collectedObjectLimit=10;
    public List<GameObject> collectedObjects;

    [Header("UI Texts")]
    public TextMeshProUGUI pickedUpObjectText;
    #endregion


    void Start()
    {
       
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < collectedObjects.Count; i++)
        {

            if (collectedObjects[i].tag == "Log")
            {
                stackUpScale += 0.3f;

            }
            else if (collectedObjects[i].tag == "Oil")
            {
                stackUpScale += 0.5f;

            }
            else if (collectedObjects[i].tag == "Stone")
            {
                stackUpScale += 0.4f;
            }
            else
            {
                stackUpScale += 1;
            }
            if (i == 0)
            {
                stackUpScale = 0;
            }

            collectedObjects[i].transform.position = Vector3.Lerp(collectedObjects[i].transform.position, StackPos.transform.position + Vector3.up * stackUpScale, 0.2f);
            collectedObjects[i].transform.localRotation = Quaternion.Euler(collectedObjects[i].transform.localRotation.eulerAngles.x, 0, collectedObjects[i].transform.localRotation.eulerAngles.z);
        }
        if (!Input.GetMouseButton(0) && collectedObjects.Count<collectedObjectLimit)
        {
            Collider[] collidersArround = Physics.OverlapSphere(transform.position, 2f, axeUseLayer);
            if (collidersArround.Length > 0)
            {
                axe.SetActive(true);
                Collider[] attackable = Physics.OverlapSphere(transform.position, 0.8f, axeUseLayer);
                if (attackable.Length>0)
                {
                    GameObject toAttackObject = attackable[0].gameObject;
                    transform.LookAt(toAttackObject.transform);
                    anim.SetBool("Axing", true);
                    if (!isAxing)
                    {
                        StartCoroutine(CheckAnimationCompleted(toAttackObject));
                    }
                 
                }
                else
                {
                    anim.SetBool("Axing", false);
                }
              
            }
        }
        else
        {
            axe.SetActive(false);
            anim.SetBool("Axing", false);
        }



    }
    public IEnumerator CheckAnimationCompleted(GameObject g)
    {
        isAxing = true;
        yield return new WaitForSeconds(0.8f);

      
        g.transform.DOShakeScale(.5f, 10);
        yield return new WaitForSeconds(0.6f);
        g.transform.localScale *= 0.8f;
        if (g.GetComponent<Resource>())
        {
            Resource t = g.GetComponent<Resource>();
  
            t.hitPoint--;
            if (t.hitPoint <= 0)
            {
                for (int i = 0; i < t.resourceAmmount; i++)
                {
                   GameObject resource = Instantiate(t.resource, g.transform.position, Quaternion.identity, StackPos);
                   resource.transform.rotation = Quaternion.Euler(Vector3.zero);
                   collectedObjects.Add(resource);
                   AddObjectToDictionary(g.GetComponent<Resource>().resourceName, resource);
                }
                axe.SetActive(false);
                anim.SetBool("Axing", false);
                g.layer = 0;

                Destroy(g);
            }
        }

        isAxing = false;

    }
    private void AddObjectToDictionary(string name, GameObject g)
    {
        if (!PickedUpObjects.ContainsKey(name))
        {
            PickedUpObjects.Add(name, new List<GameObject>());
        }
        PickedUpObjects[name].Add(g);
    }

}
[System.Serializable]
public class UnlockMaterials
{

    public string name;
    public int requiredAmmount;
    public bool isFilledUp;
    public TextMeshProUGUI requiredAmmountText;
    public GameObject filledUpTick;
    public void UpdateText()
    {
        requiredAmmountText.text = requiredAmmount.ToString();
    }
    public void FilledUp()
    {
        isFilledUp = true;
        requiredAmmountText.enabled = false;
        if (filledUpTick)
        {
            filledUpTick.SetActive(true);
        }

    }
}
