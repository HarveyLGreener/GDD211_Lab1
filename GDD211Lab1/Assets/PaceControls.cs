using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateCheck { START, SETUP, PLAY, DEAD }

public class PaceControls : MonoBehaviour
{
    public StateCheck State;
    private Vector3 positionOffset;
    public Transform Target;
    private float angle;
    [SerializeField] private Animator anim;
    [SerializeField] private bool isGrounded = true;
    [SerializeField] private Collider ground;
    public Rigidbody rb;
    [SerializeField] private GameObject player;
    public float radius;
    [SerializeField] private Transform startingPoint;
    public int interpolationFramesCount = 30;
    int elapsedFrames = 0;
    [SerializeField] private GameObject initialCam;
    [SerializeField] private GameObject lookAt;
    [SerializeField] private GameObject creditScreen;
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject controlsScreen;
    [SerializeField] private GameObject respawn;
    // Start is called before the first frame update
    void Start()
    {
        State = StateCheck.START;
    }

    // Update is called once per frame
    void Update()
    {
        if (State == StateCheck.START)
        {
            player.transform.Rotate(new Vector3(0, -57.25f*Time.deltaTime, 0));
            float x = Target.position.x + Mathf.Cos(angle) * radius;
            float z = Target.position.z + Mathf.Sin(angle) * radius;
            player.transform.position = new Vector3(x, player.transform.position.y, z);
            angle += 1f * Time.deltaTime;
        }
        else if (State == StateCheck.SETUP)
        {
            var lookPos = lookAt.transform.position - player.transform.position;
            lookPos.y =0;
            var rotation = Quaternion.LookRotation(lookPos);
            player.transform.rotation = Quaternion.Slerp(Quaternion.Euler(player.transform.rotation.eulerAngles.x, player.transform.rotation.eulerAngles.y, player.transform.eulerAngles.z), rotation, 1);
            initialCam.SetActive(false);
            float interpolationRatio = (float)elapsedFrames / interpolationFramesCount;
            player.transform.position = Vector3.Lerp(player.transform.position, startingPoint.position, interpolationRatio);
            elapsedFrames += 1;
            if (elapsedFrames >= interpolationFramesCount)
            {
                State = StateCheck.PLAY;
            }
        }
        else if (State == StateCheck.PLAY)
        {
            if (transform.position.y <= -1)
            {
                State = StateCheck.DEAD;
            }
            anim.SetBool("started", true);
            if (Input.GetAxis("Jump")>0)
            {
                StartCoroutine(Jump());
            }
        }
        else if (State==StateCheck.DEAD)
        {
            respawn.SetActive(true);
            anim.SetTrigger("dead");
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y - 5, rb.velocity.z);
            player.GetComponent<Collider>().enabled = false;
            if (Input.GetKeyDown("r"))
            {
                Application.LoadLevel(Application.loadedLevel);
            }
        }

    }
    IEnumerator Jump()
    {
        isGrounded = false;
        anim.SetTrigger("jump");
        yield return new WaitForSeconds(0.3f);
        player.GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(0.85f);
        player.GetComponent<Collider>().enabled = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Dead")
        {
            State = StateCheck.DEAD;
        }
        else
        {
            StartCoroutine(Grounded());
        }
    }
    IEnumerator Grounded()
    {
        yield return new WaitForSeconds(0.5f);
        isGrounded = true;
    }
    public void OnStartClick()
    {
        startScreen.SetActive(false);
        State = StateCheck.SETUP;
    }
    public void OnCreditClick()
    {
        creditScreen.SetActive(true);
        startScreen.SetActive(false);
    }
    public void OnCreditClose()
    {
        creditScreen.SetActive(false);
        startScreen.SetActive(true);
    }
    public void OnControlsClick()
    {
        controlsScreen.SetActive(true);
        startScreen.SetActive(false);
    }
    public void OnControlsClose()
    {
        controlsScreen.SetActive(false);
        startScreen.SetActive(true);
    }
}

