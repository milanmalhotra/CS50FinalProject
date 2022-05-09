using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    public GameObject bladeGameObject;
    public float bladeExtendSpeed = 0.1f;
    public bool weaponActive = false;
    public AudioClip soundOn;
    public AudioClip soundOff;
    public AudioClip soundLoop;
    public AudioSource AudioSource;
    public AudioSource AudioSourceLoop;
    public AudioSource AudioSourceSwing;
    public AnimationController animationController;

    Vector3 lastSwingPosition = Vector3.zero;
    KeyCode TOGGLE_KEY_CODE = KeyCode.F;
    const string SHADER_PROPERTY_EMISSION_COLOR = "_EmissionColor";
    class Blade
    {
        public GameObject gameObject;
        public bool active = false;
        float scaleMin;
        float scaleMax;
        float scaleCurrent;
        float extendDelta;
        float localScaleX;
        float localScaleZ;

        public Blade(GameObject gameObject, float extendSpeed, bool active)
        {

            this.gameObject = gameObject;
            this.active = active;
            // remember initial scale values (non extending part of the blade)
            this.localScaleX = gameObject.transform.localScale.x;
            this.localScaleZ = gameObject.transform.localScale.z;
            // remember initial scale values (extending part of the blade)
            this.scaleMin = 0f;
            this.scaleMax = gameObject.transform.localScale.y;

            extendDelta = this.scaleMax / extendSpeed;

            if (active)
            {
                // set blade size to maximum
                scaleCurrent = scaleMax;
                extendDelta *= 1;
            }
            else
            {
                // set blade size to minimum
                scaleCurrent = scaleMin;
                extendDelta *= -1;
            }
        }

        public void SetActive(bool active)
        {
            extendDelta = active ? Mathf.Abs(extendDelta) : -Mathf.Abs(extendDelta);
        }

        public void updateSize()
        {
            // consider delta time with blade extension
            scaleCurrent += extendDelta * Time.deltaTime;
            // clamp blade size
            scaleCurrent = Mathf.Clamp(scaleCurrent, scaleMin, scaleMax);
            // scale in z direction
            gameObject.transform.localScale = new Vector3(this.localScaleX, scaleCurrent, this.localScaleZ);
            // whether the blade is active or not
            active = scaleCurrent > 0;
            // show / hide the gameobject depending on the blade active state
            if (active && !gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }
            else if(!active && gameObject.activeSelf)
            {
                gameObject.SetActive(false);
            }
        }
    }

    List<Blade> blades;

    // Use this for initialization
    void Awake () {
        // store initial attributes
        blades = new List<Blade>();
        blades.Add(new Blade(bladeGameObject, bladeExtendSpeed, weaponActive));
        
        // initialize audio depending on beam activitiy
        InitializeAudio();
        // initially update blade length, so that it isn't set to what we have in unity's visual editor
        UpdateBlades();
    }

    void InitializeAudio()
    {
        // initialize audio depending on beam activitiy
        if (weaponActive)
        {
            AudioSourceLoop.clip = soundLoop;
            AudioSourceLoop.Play();
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(TOGGLE_KEY_CODE) ||
            (!weaponActive && animationController.getBlockingState()) ||
            (!weaponActive && Input.GetMouseButton(0)))
        {
            ToggleWeaponOnOff();
        }
        UpdateBlades();
    }

    void ToggleWeaponOnOff()
    {
        if (weaponActive)
        {
            WeaponOff();
        }
        else
        {
            WeaponOn();
        }
    }

    void WeaponOn()
    {
        
        blades[0].SetActive(true);
        
        AudioSource.PlayOneShot(soundOn);
        AudioSourceLoop.clip = soundLoop;
        AudioSourceLoop.Play();

    }

    void WeaponOff()
    {
        blades[0].SetActive(false);
        AudioSource.PlayOneShot(soundOff);
        AudioSourceLoop.Stop();
    }

    void UpdateBlades()
    {
        blades[0].updateSize();
        bool active = false;
        
        if (blades[0].active)
        {
            active = true;
        }
        this.weaponActive = active;
    }
}
