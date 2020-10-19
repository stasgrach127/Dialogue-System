using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    #region референсы худа
    [Header("Референсы HUDа")]
    [SerializeField] private Image imageHolder;
    [SerializeField] private Text nameHolder;
    [SerializeField] private Text textHolder;
    [SerializeField] private GameObject dialogueCanvas;
    #endregion

    #region референсы диалога
    [Header("Референсы Диалога")]
    [SerializeField] private AudioSource audioDialogue;
    [SerializeField] private string[] nameDialogue;
    [SerializeField] private Sprite[] spriteDialogue;
    [SerializeField] private AudioClip[] phrasesDialogue;
    int count;
    [TextArea(4,8)]
    [SerializeField] private string[] textDialogue;
    #endregion

    #region остальные референсы
    [Header("Остальные Референсы")]
    [SerializeField] private SpriteRenderer characterDialogue;
    [SerializeField] private GameObject playerDialogue;
    bool triggerCheck;
    bool canGo;
    #endregion

    void Start()
    {
        HideDialogue();
        canGo = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && triggerCheck && canGo)
        {
            NewPhrase();
            canGo = false;
            StartCoroutine("Proceed");
            //Debug.Log(textHolder.text);
        }
    }

    #region функции-хуюнкции
    private void HideDialogue()
    {
        dialogueCanvas.SetActive(false);
    }

    private void ShowDialogue()
    {
        dialogueCanvas.SetActive(true);
    }

    void OnTriggerEnter(Collider check)
    {
        if (check.gameObject == playerDialogue)
        {
            triggerCheck = true;
        }
    }

    void OnTriggerExit(Collider check)
    {
        if (check.gameObject == playerDialogue)
        {
            triggerCheck = false;
        }
    }

    private void NewPhrase()
    {
        if (!dialogueCanvas.activeSelf)
        {
            ShowDialogue();
        }
        if (count != textDialogue.Length)
        {
            textHolder.text = "";
            StartCoroutine("PlayText");
            //textHolder.text = textDialogue[count];
            nameHolder.text = nameDialogue[count];
            imageHolder.sprite = spriteDialogue[count];
            characterDialogue.sprite = spriteDialogue[count];
            audioDialogue.clip = phrasesDialogue[count];
            audioDialogue.Play();
            count++;
        }
        else
        {
            HideDialogue();
            textHolder.text = "tochange";
            nameHolder.text = "tochange";
            imageHolder.sprite = null;
            audioDialogue.Stop();
            count = 0;
        }
    }
    #endregion

    #region нумераторы
    IEnumerator PlayText() 
    {
        foreach (char c in textDialogue[count])
        {
            textHolder.text += c;
            yield return new WaitForSeconds(0.125f);
        }
    }

    IEnumerator Proceed()
    {
        yield return new WaitForSeconds(0.6f);
        canGo = true;
    }
    #endregion
}
