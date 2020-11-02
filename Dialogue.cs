using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Dialogue : MonoBehaviour
{
    [System.Serializable]
    public class DialogueBase
    {
        public string nameDialogue;
        public Sprite spriteDialogue;
        public AudioClip phrasesDialogue;
        public Transform positionDialogue;
        public SpriteRenderer personTalkingDialogue;
        [TextArea(4, 8)]
        public string textDialogue;
    }

    #region референсы худа
    [Header("Референсы HUDа")]
    [SerializeField] private Image imageHolder;
    [SerializeField] private TMP_Text nameHolder;
    [SerializeField] private TMP_Text textHolder;
    [SerializeField] private GameObject dialogueCanvas;
    #endregion

    public DialogueBase[] dialogueBase;

    #region остальные референсы
    [Header("Остальные Референсы")]
    //[SerializeField] private SpriteRenderer characterDialogue;
    [SerializeField] private GameObject playerDialogue;
    [SerializeField] private GameObject cameraDialogue;
    public AudioSource audioDialogue;
    int count;
    bool triggerCheck;
    bool canGo;
    bool AutoMode;

    Vector3 startPosition;
    #endregion

    void Start()
    {
        HideDialogue();
        canGo = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && triggerCheck && canGo)
        {
            StopCoroutine("PlayText");
            NewPhrase();
            canGo = false;
            StartCoroutine("Proceed");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            AutoMode = true;
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
            startPosition = cameraDialogue.transform.position;
            playerDialogue.GetComponent<Renderer>().enabled = false;
            cameraDialogue.GetComponent<MouseLook>().enabled = false;
        }
        if (count != dialogueBase.Length)
        {
            textHolder.text = "";

            StartCoroutine("PlayText");
            StartCoroutine("SmoothCamera");
            nameHolder.text = dialogueBase[count].nameDialogue;
            imageHolder.sprite = dialogueBase[count].spriteDialogue;
            dialogueBase[count].personTalkingDialogue.sprite = dialogueBase[count].spriteDialogue;
            audioDialogue.clip = dialogueBase[count].phrasesDialogue;
            audioDialogue.Play();
            if (count > 0)
            {
                dialogueBase[count].personTalkingDialogue.enabled = false;
                dialogueBase[count - 1].personTalkingDialogue.enabled = true;
            }
            else
            {
                dialogueBase[count].personTalkingDialogue.enabled = false;
            }
            count += 1;
        }
        else
        {
            HideDialogue();
            StartCoroutine("SmoothCameraEnd");
            dialogueBase[count - 1].personTalkingDialogue.enabled = true;
            textHolder.text = "tochange";
            nameHolder.text = "tochange";
            imageHolder.sprite = null;
            audioDialogue.Stop();
            playerDialogue.GetComponent<Renderer>().enabled = true;
            cameraDialogue.GetComponent<MouseLook>().enabled = true;
            count = 0;
        }
    }
    #endregion

    #region нумераторы
    IEnumerator PlayText()
    {
        foreach (char c in dialogueBase[count].textDialogue)
        {
            textHolder.text += c;
            yield return new WaitForSeconds(0.02f);
        }
    }

    IEnumerator Proceed()
    {
        yield return new WaitForSeconds(0.6f);
        canGo = true;
    }

    IEnumerator AutoDialogue()
    {
        yield return new WaitForSeconds(2);
        NewPhrase();
    }

    IEnumerator SmoothCamera()
    {
        float timeToArrive = 0f;
        Vector3 startPos = cameraDialogue.transform.position;
        Vector3 endPos = dialogueBase[count].positionDialogue.position;
        while (timeToArrive < 1)
        {
            timeToArrive += Time.deltaTime / 0.2f;
            cameraDialogue.transform.position = Vector3.Lerp(startPos, endPos, timeToArrive);
            yield return null;
        }
    }

    IEnumerator SmoothCameraEnd() //костыль, позже исправлю
    {
        float timeToArrive = 0f;
        Vector3 startPos = cameraDialogue.transform.position;
        Vector3 endPos = startPosition;
        while (timeToArrive < 1)
        {
            timeToArrive += Time.deltaTime / 0.2f;
            cameraDialogue.transform.position = Vector3.Lerp(startPos, endPos, timeToArrive);
            yield return null;
        }
    }
    #endregion
}
