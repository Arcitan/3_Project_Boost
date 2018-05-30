using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource audioSource;
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float boosterThrust = 1000f;


    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}
	

	// Update is called once per frame
	void Update () {
        ProcessInput(); 
	}

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                // do nothing
                print("OK");
                break;
            default:
                print("dead");
                break;
        }
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
