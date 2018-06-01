using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource audioSource;
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float boosterThrust = 1000f;
    [SerializeField] float levelLoadDelay = 2f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip completeLevel;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem deathParticles;

    private enum State { Alive, Dying, Transcending };
    State state = State.Alive;


    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}
	

	// Update is called once per frame
	void Update () {
        if (state == State.Alive)
        {
            ProcessInput();
        }
	}


    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive)
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                // do nothing
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }


    private void StartDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(deathSound);
        deathParticles.Play();
        Invoke("LoadFirstScene", levelLoadDelay);
    }


    private void StartSuccessSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(completeLevel);
        successParticles.Play();
        Invoke("LoadNextScene", levelLoadDelay);
    }


    private void LoadFirstScene()
    {
        SceneManager.LoadScene(0);
    }


    private void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }


    private void ProcessInput()
    {
        RespondToThrustInput();
        RespondToRotateInput();
    }


    private void RespondToThrustInput()
    {
        // Thrust input
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }


    private void ApplyThrust()
    {
        float boosterThisFrame = boosterThrust * Time.deltaTime;
        rigidBody.AddRelativeForce(Vector3.up * boosterThisFrame);

        if (!audioSource.isPlaying) // so it doesn't layer audio 
        {
            audioSource.PlayOneShot(mainEngine);
        }
        mainEngineParticles.Play();
    }


    private void RespondToRotateInput()
    {
        rigidBody.freezeRotation = true; // take manual control of the rotation
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        rigidBody.freezeRotation = false; // resume rotation physics 
    }
}
