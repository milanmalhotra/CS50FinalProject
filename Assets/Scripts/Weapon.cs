using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    public GameObject bladeGameObject;

    public float bladeExtendSpeed = 0.1f;

    public bool weaponActive = false;

    public Color bladeColor;
	
	public float bladeColorIntensity = 600f; 

	public float lightIntensity = 1000f;
	
    public AudioClip soundOn;
    public AudioClip soundOff;
    public AudioClip soundLoop;
    public AudioClip soundSwing;

    public AudioSource AudioSource;
    public AudioSource AudioSourceLoop;
    public AudioSource AudioSourceSwing;
    
    private float swingSpeed = 0;
    private Vector3 lastSwingPosition = Vector3.zero;
    private KeyCode TOGGLE_KEY_CODE = KeyCode.F;
    private const string SHADER_PROPERTY_EMISSION_COLOR = "_EmissionColor";
    private class Blade
    {
        public GameObject gameObject;
        public Light light;

        private float scaleMin;
        private float scaleMax;
        private float scaleCurrent;

        public bool active = false;

        private float extendDelta;

        private float localScaleX;
        private float localScaleZ;

        public Blade( GameObject gameObject, float extendSpeed, bool active)
        {

            this.gameObject = gameObject;
            this.light = gameObject.GetComponentInChildren<Light>();
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

        public void SetActive( bool active)
        {
            extendDelta = active ? Mathf.Abs(extendDelta) : -Mathf.Abs(extendDelta);
        }

        public void SetColor( Color color, float intensity)
        {
            if (light != null)
            {
                light.color = color;
            }			
			Color bladeColor = color * intensity;
			// set the color in the shader. _EmissionColor is a reference which is defined in the property of the graph
            gameObject.GetComponentInChildren<MeshRenderer>().materials[0].SetColor( SHADER_PROPERTY_EMISSION_COLOR, bladeColor);
        }

        public void updateLight( float lightIntensity)
        {
            if (this.light == null)
                return;
            // light intensity depending on blade size
            this.light.intensity = this.scaleCurrent * lightIntensity;
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

    private List<Blade> blades;

    // Use this for initialization
    void Awake () {
        // store initial attributes
        blades = new List<Blade>();
        blades.Add(new Blade(bladeGameObject, bladeExtendSpeed, weaponActive));
        
        // initialize audio depending on beam activitiy
        InitializeAudio();
        // light and blade color
        InitializeBladeColor();
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

    // set the color of the light and the blade
    void InitializeBladeColor()
    {
        // update blade color, light color and glow color
        blades[0].SetColor(bladeColor, bladeColorIntensity);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(TOGGLE_KEY_CODE))
        {
            ToggleWeaponOnOff();
        }
        UpdateBlades();
        //updateSwingHandler();
    }

    // calculate swing speed
    // come back to this later, not working well at all
    // private void updateSwingHandler()
    // {
    //     swingSpeed = (((transform.position - lastSwingPosition).magnitude) / Time.deltaTime);
    //     lastSwingPosition = transform.position;

    //     if (weaponActive)
    //     {
    //         if (swingSpeed > 200) 
    //         {
    //             if (!AudioSourceSwing.isPlaying)
    //             {
    //                 AudioSourceSwing.volume = 1f;
    //                 AudioSourceSwing.PlayOneShot(soundSwing);
    //             }
    //         }
    //         else
    //         {
    //             // fade out volume
    //             if(AudioSourceSwing.isPlaying && AudioSourceSwing.volume > 0)
    //             {
    //                 AudioSourceSwing.volume *= 0.9f; // just random swing values; should be more generic
    //             }
    //             else
    //             {
    //                 AudioSourceSwing.volume = 0;
    //                 AudioSourceSwing.Stop();
    //             }
    //         }
    //     }
    // }

    private void ToggleWeaponOnOff()
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

    private void WeaponOn()
    {
        
        blades[0].SetActive(true);
        
        AudioSource.PlayOneShot(soundOn);
        AudioSourceLoop.clip = soundLoop;
        AudioSourceLoop.Play();

    }

    private void WeaponOff()
    {
        
        blades[0].SetActive(false);
        
        AudioSource.PlayOneShot(soundOff);
        AudioSourceLoop.Stop();
    }

    private void UpdateBlades()
    {

        blades[0].updateLight( lightIntensity);
        blades[0].updateSize();


        bool active = false;
        
        if(blades[0].active)
        {
            active = true;
        }
        
        this.weaponActive = active;
    }
}
