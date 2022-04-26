using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Player : MonoBehaviour
{
    #region variable

    public Transform StackPos;
    public Transform UnstackPos;
    public List<GameObject> PickedUpObjects;


    public float pickUpSpeed;

    public float axeSwingSpeed;
    public LayerMask axeUseLayer;
    public GameObject axe;
    public Animator anim;
    public bool isAxing;
    public float stackUpScale;
    #endregion


    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < PickedUpObjects.Count; i++)
        {
            PickedUpObjects[i].transform.position = Vector3.Lerp(PickedUpObjects[i].transform.position, StackPos.transform.position + Vector3.up * stackUpScale * i, 0.2f);
            PickedUpObjects[i].transform.rotation = StackPos.rotation;
        }
        if (!Input.GetMouseButton(0) && PickedUpObjects.Count<10)
        {
            Collider[] collidersArround = Physics.OverlapSphere(transform.position, 1.5f, axeUseLayer);
            if (collidersArround.Length > 0)
            {
                axe.SetActive(true);
                Collider[] attackable = Physics.OverlapSphere(transform.position, 0.5f, axeUseLayer);
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
        yield return new WaitForSeconds(1.4f);
        if (g.GetComponent<Tree>())
        {
            Tree t = g.GetComponent<Tree>();
            g.transform.localScale *= 0.8f;
            t.hitPoint--;
            if (t.hitPoint <= 0)
            {
                for (int i = 0; i < t.logCount; i++)
                {
                   GameObject log = Instantiate(t.log, g.transform.position, Quaternion.identity, StackPos);
                    PickedUpObjects.Add(log);
                }
                axe.SetActive(false);
                anim.SetBool("Axing", false);
                g.layer = 0;
                Destroy(g);
            }
        }

        isAxing = false;

    }

}
