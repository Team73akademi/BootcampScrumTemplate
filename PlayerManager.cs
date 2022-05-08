using UnityEngine;
using System.Collections;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public int PreviousTransformationLevel = 0;
    public int CurrentTransformationLevel = 0;
    public new Rigidbody rigidbody;
    public TextMeshProUGUI levelText;
    public Animator animator;
    public int level;
    public float forwardSpeed = 14f;
    private bool heldDown = false;
    private Vector2 touchedPosition = Vector2.zero;
    private float roadWidth; // half width
    public int CurrentEndGamePlatformNumber = 0;
    [SerializeField] private Material[] GlowMaterials;

    public float MinClampValue;
    public float MaxClampValue;
    #region Unity Methods
    private void Awake()
    {
        instance = this;
        animator = transform.GetChild(0).GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        roadWidth = Constants.PlatformHalfWidth - Constants.PlatformLineWidth - Constants.PlayerWidth - Constants.ExtraRoadWidthReduction;
        MinClampValue = -1 * roadWidth;
        MaxClampValue = roadWidth;
    }
    private void Start()
    {
        level = Constants.GetPlayerLevel();
        levelText.text = level.ToString();
        CurrentTransformationLevel = Mathf.Min((level / 500), 8);
        PreviousTransformationLevel = CurrentTransformationLevel;
        transform.GetChild(CurrentTransformationLevel).gameObject.SetActive(true);
        animator = transform.GetChild(CurrentTransformationLevel).GetComponent<Animator>();

    }
    void Update()
    {

        if (Input.GetMouseButtonDown(0) && UIManager.instance.DidLevelStart && !UIManager.instance.DidLevelEnd)
        {

            if (!heldDown)
            {
                heldDown = true;
                touchedPosition = Input.mousePosition;
                //Debug.Log("held down");
                StartCoroutine(EvaluateUserInput());
            }

        }

        if (Input.GetMouseButtonUp(0))
        {
            heldDown = false;
        }
    }


    public void Stop()
    {
        animator.SetBool("isRunning", false);
        rigidbody.velocity = Vector3.zero;
    }
    public void ResetPlayer()
    {
        transform.position = Constants.PlayerStartingPosition;
        level = Constants.GetPlayerLevel();
        levelText.text = level.ToString();
    }
    public void Move()
    {
        rigidbody.velocity = Vector3.forward * forwardSpeed;
    }

    private IEnumerator EvaluateUserInput()
    {
        while (true)
        {
            if (!UIManager.instance.DidLevelEnd)
            {
                transform.position += new Vector3((Input.mousePosition.x - touchedPosition.x) / 500f * roadWidth, 0, 0);

                float positionX = Mathf.Clamp(transform.position.x, MinClampValue, MaxClampValue);

                transform.position = new Vector3(positionX, transform.position.y, transform.position.z);
                touchedPosition = Input.mousePosition;
            }
            if (!heldDown)
            {
                //Debug.Log("held down false");
                yield break;
            }
            yield return new WaitForFixedUpdate();


        }
    }

    private void LostGame()
    {
        AudioManager.instance.Play("FinishFail");
        UIManager.instance.DidLevelEnd = true;
        Stop();
        UIManager.instance.EndGamePanel.gameObject.SetActive(true);
        UIManager.instance.EndGamePanel.GetChild(1).gameObject.SetActive(true);

        // TODO: animation switch to die
    }
}