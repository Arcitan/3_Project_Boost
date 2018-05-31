using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource audioSource;
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float boosterThrust = 1000f;

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
                state = State.Transcending;
                Invoke("LoadNextScene", 1f);
                break;
            default:
                state = State.Dying;
                Invoke("LoadFirstScene", 1f);
                break;
        }
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
        Thrust();
        Rotate();
    }


    private void Thrust()
    {
        // Thrust input
        if (Input.GetKey(KeyCode.Space))
        {
            float boosterThisFrame = boosterThrust * Time.deltaTime; 
            rigidBody.AddRelativeForce(Vector3.up * boosterThisFrame);

            if (!audioSource.isPlaying) // so it doesn't layer audio 
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }


    private void Rotate()
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
