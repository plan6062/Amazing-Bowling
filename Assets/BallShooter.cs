using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallShooter : MonoBehaviour
{
    public Rigidbody ball;
    public Transform firePos;
    public Slider powerSlider;
    public AudioSource ShootingAudio;
    public AudioClip fireClip;
    public AudioClip charginClip;
    public float minForce = 15f;
    public float maxForce = 30f;
    public float chargingTime = 0.75f;

    private float currentForce;
    private float chargeSpeed;
    private bool fired;
    private void OnEnable(){
        currentForce = minForce;
        powerSlider.value = minForce;
        fired = false;

    }

    private void Start(){
        chargeSpeed = (maxForce - minForce)/chargingTime;
    }

    private void Update(){

        if(fired == true){
            return;
        }
        powerSlider.value = minForce;

        if(currentForce >= maxForce && !fired){
            currentForce = maxForce;
            Fire();
        }
        else if(Input.GetButtonDown("Fire1")){
            fired = false;
            currentForce = minForce;
            ShootingAudio.clip = charginClip;
            ShootingAudio.Play();

        }
        else if(Input.GetButton("Fire1") && !fired){
            currentForce = currentForce + chargeSpeed * Time.deltaTime;
            powerSlider.value = currentForce;

        }
        else if(Input.GetButtonUp("Fire1") && !fired){
            Fire();//발사처리
        }
    }

    private void Fire(){
        fired = true;
        Rigidbody ballInstance = Instantiate(ball, firePos.position, firePos.rotation);
        ballInstance.velocity = currentForce * firePos.forward;
        ShootingAudio.clip = fireClip;
        ShootingAudio.Play();

        currentForce = minForce;
    }


}
