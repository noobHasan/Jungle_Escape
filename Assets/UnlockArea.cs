using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UnlockArea : MonoBehaviour
{
    public bool carUnlock;
    public Player player;
    public UnlockMaterials[] unlockMaterials;
    public bool isUnlocked;
    private bool isMoving;

    private void Start()
    {
        for (int i = 0; i < unlockMaterials.Length; i++)
        {
            unlockMaterials[i].UpdateText();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer==6 && isUnlocked==false)
        {
            for (int i = 0; i < unlockMaterials.Length; i++)
            {
                if (player.PickedUpObjects.ContainsKey(unlockMaterials[i].name))
                {
                    if (!unlockMaterials[i].isFilledUp && player.PickedUpObjects[unlockMaterials[i].name].Count > 0)
                    {
                        Move(unlockMaterials[i], player.PickedUpObjects[unlockMaterials[i].name][player.PickedUpObjects[unlockMaterials[i].name].Count - 1]);
                    }
                }
              
            }
        }
    }

    private void Move(UnlockMaterials unlockMat, GameObject g)
    {
        if (!isMoving)
        {
            isMoving = true;
            g.transform.DOMoveY(4, 0.3f).OnComplete(() => {
                g.transform.DOMove(transform.position, 0.4f);
                g.transform.parent = null;
                Destroy(g, 5);
                player.PickedUpObjects[unlockMat.name].Remove(g);
                player.collectedObjects.Remove(g);
                unlockMat.requiredAmmount--;
                unlockMat.UpdateText();
                if (unlockMat.requiredAmmount<=0)
                {
                    unlockMat.FilledUp();
                    
                    if (CheckUnlocked())
                    {
                        Unlock();
                        unlockMat.requiredAmmountText.gameObject.transform.parent.parent.gameObject.SetActive(false);
                    }
                }
                isMoving = false;
            });
        }
      
    }

    private bool CheckUnlocked()
    {
        foreach (UnlockMaterials item in unlockMaterials)
        {
            if (item.isFilledUp == false)
            {
                return false;
            }
        }
        return true;
    }

    public void Unlock()
    {

        isUnlocked = true;    
        GetComponent<Collider>().enabled = false;
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().buildIndex + gameObject.name, 1);
        if (carUnlock)
        {
            transform.GetChild(0).gameObject.SetActive(false);

        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}

