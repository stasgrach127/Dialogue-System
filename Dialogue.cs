using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialogue : MonoBehaviour
{
    #region референсы худа
    [Header("Референсы HUDа")]
    [SerializeField] private Image imageHolder;
    [SerializeField] private Text nameHolder;
    [SerializeField] private TMP_Text textHolder;
    [SerializeField] private GameObject dialogueCanvas;
    #endregion

    #region референсы диалога
    [Header("Референсы Диалога")]
    [SerializeField] private AudioSource audioDialogue;
    [SerializeField] private string[] nameDialogue;
    [SerializeField] private Sprite[] spriteDialogue;
    [SerializeField] private AudioClip[] phrasesDialogue;
    [SerializeField] private Transform[] positionDialogue;
    [SerializeField] private SpriteRenderer[] personTalkingDialogue;
    int count;
    [TextArea(4,8)]
    [SerializeField] private string[] textDialogue;
    #endregion

    #region остальные референсы
    [Header("Остальные Референсы")]
    //[SerializeField] private SpriteRenderer characterDialogue;
    [SerializeField] private GameObject playerDialogue;
    [SerializeField] private GameObject cameraDialogue;
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
        }
        if (count != textDialogue.Length)
        {
            textHolder.text = "";
            StartCoroutine("PlayText");
            StartCoroutine("SmoothCamera");
            nameHolder.text = nameDialogue[count];
            imageHolder.sprite = spriteDialogue[count];
            personTalkingDialogue[count].sprite = spriteDialogue[count];
            audioDialogue.clip = phrasesDialogue[count];
            audioDialogue.Play();
            if (count > 0)
            {
                personTalkingDialogue[count].enabled = false;
                personTalkingDialogue[count - 1].enabled = true;
            }
            else
            {
                personTalkingDialogue[count].enabled = false;
            }
            count += 1;
        }
        else
        {
            HideDialogue();
            StartCoroutine("SmoothCameraEnd");
            personTalkingDialogue[count - 1].enabled = true;
            textHolder.text = "tochange";
            nameHolder.text = "tochange";
            imageHolder.sprite = null;
            audioDialogue.Stop();
            playerDialogue.GetComponent<Renderer>().enabled = true;
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
            yield return new WaitForSeconds(0.05f);
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
        Vector3 endPos = positionDialogue[count].position;
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
