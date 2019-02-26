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
    [SerializeField] protected Animator animator = null;

    /// <summary>
    /// 2D Rigidbody of the grenade, used to infuse velocity in it.
    /// </summary>
    [SerializeField] protected new Rigidbody2D rigidbody2D = null;

    /// <summary>
    /// Radius of the circle from the center of the explosion, where it should deal absolutly incredible explosive damages.
    /// </summary>
    [SerializeField] protected float coreExplosionRadius = .1f;

    /// <summary>
    /// Radius of the circle from the center of the explosion, where the explosion should just destroy things.
    /// </summary>
    [SerializeField] protected float mainExplosionRadius = 2.5f;
    #endregion

    #region Methods

    #region Original Methods
    /// <summary>
    /// Makes the grenade explode. KA-BOUM.
    /// </summary>
    private void Explode()
    {
        animator.SetTrigger("Explosion");
    }

    /// <summary>
    /// Throws the grenade in the environment, and activate it.
    /// </summary>
    /// <param name="_velocity">Velocity to give to the grenade movement.</param>
    public void Throw(Vector2 _velocity)
    {
        rigidbody2D.velocity = _velocity;
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
    Bouncing,
    Classic,
    Sticky
}
