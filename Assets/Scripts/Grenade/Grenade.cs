using System.Collections;
using UnityEngine;

/// <summary>
/// Abstract class to inherit from to create a new grenade object , with a different behaviour.
/// </summary>
public class Grenade : MonoBehaviour 
{
    /* Grenade :
	 *
	 *	#####################
	 *	###### PURPOSE ######
	 *	#####################
	 *
	 *	Class to inherit from to create a cool grenade for the game.
	 *
     *	#####################
	 *	####### TO DO #######
	 *	#####################
     * 
     *      • Set the owner of the grenade in initialization, implement the explosion system
     *  and test it with a prefab.
     *  
     *      • Set this class as abstract when functional.
     * 
	 *	#####################
	 *	### MODIFICATIONS ###
	 *	#####################
	 *
	 *	Date :			[30 / 01 / 2019]
	 *	Author :		[Guibert Lucas]
	 *
	 *	Changes :
	 *
	 *	Creation of the Grenade class, and the GrenadeType enum.
     *	
     *	    • Created the base version of the script with main components, parameters and methods to fill so that everything is ready to implement the core behaviour of the grenade.
	 *
	 *	-----------------------------------
	*/

    #region Events

    #endregion

    #region Fields / Properties
    /// <summary>
    /// Animator of the grenade, to play different animations like explosion, blink and others.
    /// </summary>
    [SerializeField] protected Animator                     animator                        = null;

    /// <summary>
    /// Circle collider of the grenade, detecting collision.
    /// </summary>
    [SerializeField] protected new CircleCollider2D         collider                        = null;

    /// <summary>
    /// 2D Rigidbody of the grenade, used to infuse velocity in it.
    /// </summary>
    [SerializeField] protected new Rigidbody2D              rigidbody2D                     = null;


    /// <summary>
    /// Current life time of this grenade, that is remaining time before the explosion.
    /// </summary>
    [SerializeField] protected float                          lifeTime                        = 0;

    /// <summary>
    /// Initial life time of this grenade, that is time before the explosion before launch.
    /// </summary>
    [SerializeField] protected float                          initialLifeTime                 = 3;


    /// <summary>
    /// Total duration of the grenade explosion, in seconds.
    /// </summary>
    [SerializeField] protected float                        explosionDuration              = .9f;

    /// <summary>
    /// Radius of the circle from the center of the explosion, where it should deal absolutly incredible explosive damages.
    /// </summary>
    [SerializeField] protected float                        coreExplosionRadius             = .1f;

    /// <summary>
    /// Radius of the circle from the center of the explosion, where the explosion should just destroy things.
    /// </summary>
    [SerializeField] protected float                        mainExplosionRadius             = 2.5f;

    /// <summary>
    /// Radius of the explosion at this instant (zero if not exploding).
    /// </summary>
    [SerializeField] protected float                        explosionRadius                 = 0;
    #endregion

    #region Methods

    #region Original Methods
    /// <summary>
    /// Throws the grenade in the environment, and activate it.
    /// </summary>
    /// <param name="_velocity">Velocity to give to the grenade movement.</param>
    public void Throw(Vector2 _velocity)
    {
        rigidbody2D.velocity = _velocity;

        lifeTime = initialLifeTime;
        StartCoroutine(CountDown());
    }

    /// <summary>
    /// Countdown mechanic. Decrease lifetime until reach zero, and then explode.
    /// </summary>
    /// <returns>IEnumerator, baby.</returns>
    private IEnumerator CountDown()
    {
        while (lifeTime > 0)
        {
            yield return null;

            lifeTime -= Time.deltaTime;
        }

        StartCoroutine(Explode());
    }

    /// <summary>
    /// Makes the grenade explode. KA-BOUM.
    /// </summary>
    /// <returns>IEnumerator, baby.</returns>
    private IEnumerator Explode()
    {
        // Disable object and physic
        collider.enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        rigidbody2D.simulated = false;

        // Active the explosion animation
        //animator.SetTrigger("Explosion");

        // Calculate explosion according to duration & radius
        float _timer = 0;

        while (_timer < explosionDuration)
        {
            yield return null;
            _timer += Time.deltaTime;

            explosionRadius = (mainExplosionRadius / explosionDuration) * _timer;
            transform.localScale = new Vector2(explosionRadius + 1, explosionRadius + 1);
        }

        // At the end of the explosion, destroy this game object.
        Destroy(gameObject);
    }
    #endregion

    #region Unity Methods
    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        #if UNITY_EDITOR
        // Get the components references if needed, and debug it if some are missing
        if (!animator)
        {
            animator = GetComponent<Animator>();
            if (!animator) Debug.LogWarning($"Animator missing on the \"{name}\" Grenade.");
        }
        if (!rigidbody2D)
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
            if (!rigidbody2D) Debug.LogWarning($"Rigidbody2D missing on the \"{name}\" Grenade.");
        }
        #endif
    }

    // Implement OnDrawGizmos if you want to draw gizmos that are also pickable and always drawn
    private void OnDrawGizmos()
    {
        // Draw a sphere to show the explosion
        Gizmos.color = explosionRadius > coreExplosionRadius ? Color.blue : Color.red;
        Gizmos.DrawSphere(transform.position, explosionRadius);
    }

    // Use this for initialization
    private void Start()
    {
		
    }
	
	// Update is called once per frame
	private void Update()
    {
        
	}
	#endregion

	#endregion
}

/// <summary>
/// All differents grenade types in the game.
/// </summary>
public enum GrenadeType
{
    Classic,
    Bouncing,
    Sticky
}
