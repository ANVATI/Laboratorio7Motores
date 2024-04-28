using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody myRBD;
    [SerializeField] private float velocity;
    [SerializeField] private AudioSource audiosource;
    private Vector2 _movement;
    private Animator _animation;
    private NPCcontroller nearbyNPC;
    private bool _canDance;
    private bool _dance = false;

    private void Start()
    {
        audiosource = GetComponent<AudioSource>();
        _animation = GetComponentInChildren<Animator>();
    }

    private void FixedUpdate()
    {
        myRBD.velocity = new Vector3(_movement.x, myRBD.velocity.y, _movement.y);
        AudioMovement();
        RotatePlayer();
    }

    private void Update()
    {
        AnimationWalk();
    }

    public void AudioMovement()
    {
        if (_movement.magnitude > 0)
        {
            if (!audiosource.isPlaying)
            {
                audiosource.Play();
            }
        }
        else
        {
            audiosource.Stop();
        }
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        _movement = context.ReadValue<Vector2>() * velocity;

        if (_movement == Vector2.zero)
        {
            _canDance = true;
        }
        else
        {
            _canDance = false;
            _animation.SetBool("Dance", false); 
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "portal")
        {
            SceneManager.LoadScene("Nivel2");
        }
        if (collision.gameObject.tag == "portal1")
        {
            SceneManager.LoadScene("Nivel1");
        }
    }

    private void AnimationWalk()
    {
        _animation.SetFloat("X", _movement.x);
        _animation.SetFloat("Y", _movement.y);
    }

    public void InteractNPC(InputAction.CallbackContext context)
    {
        if (nearbyNPC != null)
        {
            nearbyNPC.Interact();
        }
    }

    public void PlayerDance(InputAction.CallbackContext context)
    {
        if (_canDance == true)
        {
            _dance = true;
        }
        else
        {
            _dance = false;
        }

        _animation.SetBool("Dance", _dance);
    }

    private void RotatePlayer()
    {
        if (_movement.magnitude > 0)
        {
            float angle = Mathf.Atan2(_movement.x, _movement.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "NPC")
        {

            nearbyNPC = other.GetComponent<NPCcontroller>();

            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                InteractNPC(new InputAction.CallbackContext());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "NPC")
        {
            nearbyNPC = null;
        }
    }
}