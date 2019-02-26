using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controller used to manipulate a player instance in the game.
/// </summary>
public class PlayerController : MonoBehaviour
{
    /* PlayerController :
	 *
	 *	#####################
	 *	###### PURPOSE ######
	 *	#####################
	 *
	 *	Player controller of the game.
     *	    
     *	The character can perform the following actions :
     *	
     *	    [Movements] :
     *	
     *	        • Move along the X axis in both directions, and do not cross walls.
     *	        
     *	        • Jump when on ground, and move according to air restistance.
     *	        
     *	        • Jump when against a wall, pushing the character in the opposite direction.
     *	        
     *	        • Slide all along the walls.
     *	        
     *	    [Actions] :
     *	        
     *	        • Throw a grenade.
     *	    The player can load the throw by holding the button down.
     *	    An animation with particles around the hand indicates the throwing power.
     *	    When at maximum, the character keeps holding the grenade while not hit
     *	    or while the player keeps the button down.
     *	        When releasing the button, if the character is still holding it,
     *	    the grenade is thrown in the aiming direction with a force depending
     *	    on the throw load. At maximum, the grenade kicks some asses.
     *	        If the player just press and release immediatly the button,
     *	    the grenade is throw in the direction without effect.

     *	        • Strike with the bat.
     *	    The player can load the strike by holding the button down, just like the throw.
     *	    An animation with particles around the bat indicates the strike power.
     *	    When at maximum, the character keeps holding the bat in position while not hit
     *	    or while the player keeps the button down.
     *	        When releasing the button, if the character still prepares his stroke,
     *	    he knocks in the aiming direction with a force depending on the strike load.
     *	    At maximum, heads gonna fall.
     *	        If hitting a player, he will be projecting in the direction unless
     *	    he strikes in the mean time co counter the attack.
     *	        If hitting a grenade, it will pe propulled in the strike direction.
     *	    When hitting perfectly the grenade, an awesome effect can be invoked.
	 *
     *	#####################
	 *	####### TO DO #######
	 *	#####################
     * 
     *  [CONTROLLER]
     * 
     *      • Append a movement detection system to detect colliders and others,
     *  so that players do not cross walls and obstacles.
     *  
     *      • Implement a cool simple jump system, with the more you held the button down,
     *  the higher you jump.
     *  
     *      • Create a wall jump system, with player pushed away from support ; you can
     *  perform this action as much as you want with no restriction.
     *  
     *  [ACTIONS]
     *  
     *      • Implement the grenade with load, throw, explosions, death & everything that
     *  goes with it.
     *  
     *      • Implement the bat with load & repulse.
     * 
     * [OTHERS]
     * 
     *      • Link a Player object to the controller.
     * 
	 *	#####################
	 *	### MODIFICATIONS ###
	 *	#####################
	 *
     *	Date :			[26 / 02 / 2019]
	 *	Author :		[Guibert Lucas]
	 *
	 *	Changes :
     *	
     *	    • Reflexion, but minimum modifications ; set the base system for
     *	movement raycasts to avoid getting into obstacles.
	 *
	 *	-----------------------------------
     * 
     *	Date :			[08 / 02 / 2019]
	 *	Author :		[Guibert Lucas]
	 *
	 *	Changes :
     *	
     *	    • Minimum modifications, with the button to jump.
	 *
	 *	-----------------------------------
     * 
     *	Date :			[30 / 01 / 2019]
	 *	Author :		[Guibert Lucas]
	 *
	 *	Changes :
     *	
     *	    • Created the base version of the script, with movement system, main actions buttons, main components & others so that the player can move, and everything is prepared for implementing the core gameplay of the game.
	 *
	 *	-----------------------------------
     * 
	 *	Date :			[18 / 01 / 2019]
	 *	Author :		[Guibert Lucas]
	 *
	 *	Changes :
	 *
	 *	Creation of the PlayerController class.
     *	    
     *	    • Detailed script purpose with movements and actions like throw or strike.
	 *
	 *	-----------------------------------
	*/

    #region Events

    #endregion

    #region Fields / Properties

    #region Components & References
    /// <summary>
    /// Animator of the player, used to play all its animations, like running, dying, etc...
    /// </summary>
    [SerializeField] private Animator animator = null;

    /// <summary>
    /// Player box collider, used to detect collisions & physic detections
    /// </summary>
    [SerializeField] private new BoxCollider2D collider = null;

    /// <summary>
    /// Player rigidbody, used to give him velcity for jump, explosion recoil, etc...
    /// </summary>
    [SerializeField] private new Rigidbody2D rigidbody = null;
    #endregion

    #region Variables
    /// <summary>
    /// All grenades the player is carrying on.
    /// </summary>
    [SerializeField]
    private Dictionary<GrenadeType, int> grenades = new Dictionary<GrenadeType, int>()
    {
        { GrenadeType.Classic, 3 }, { GrenadeType.Bouncing, 0 }, { GrenadeType.Sticky, 0 }
    };

    /// <summary>Backing field for <see cref="SelectedGrenade"/>.</summary>
    [SerializeField] private GrenadeType selectedGrenade = GrenadeType.Classic;

    /// <summary>
    /// The actually grenade type selected by the player.
    /// </summary>
    public GrenadeType SelectedGrenade
    {
        get { return selectedGrenade; }
        private set
        {
            selectedGrenade = value;
        }
    }

    /// <summary>Backing field for <see cref="IsFacingRight"/>.</summary>
    [SerializeField] private bool isFacingRight = true;

    /// <summary>
    /// If true, the player is facing the right side of the screen ; otherwise, facing the left one.
    /// </summary>
    public bool IsFacingRight
    {
        get { return isFacingRight; }
        private set { isFacingRight = value; }
    }

    /// <summary>
    /// Indicates if the player is currently on ground, or in the air.
    /// </summary>
    [SerializeField] private bool isOnGround = false;

    /// <summary>
    /// Speed of the player movements.
    /// </summary>
    [SerializeField] private float speed = 1;

    /// <summary>
    /// Value used to add vertical velocity to the player when jumping.
    /// </summary>
    [SerializeField] private float jumpForce = 250;
    #endregion

    #region Inputs
    /// <summary>
    /// Input name for the horizontal axis. Used to move the character on the X axis.
    /// </summary>
    public string HorizontalAxis = "Horizontal";

    /// <summary>
    /// Input name for the button used to perform a jump.
    /// </summary>
    public string JumpButton = "Jump";
    #endregion

    #region Help & Memory
    /// <summary>
    /// Center point of the collider (in local space).
    /// </summary>
    private Vector3 colliderCenter = Vector2.zero;

    /// <summary>
    /// Point at the top right of the collider, always equal to center + extents (in local space).
    /// </summary>
    private Vector3 colliderMax = Vector2.one;

    /// <summary>
    /// Point at the bottom left of the collider, always equal to center - extents (in local space).
    /// </summary>
    private Vector3 colliderMin = -Vector2.one;
    #endregion

    #endregion

    #region Methods

    #region Original Methods

    #region Inputs
    /// <summary>
    /// Checks the player inputs, and executes associated actions.
    /// </summary>
    private void CheckInputs()
    {
        // Get the horizontal movement, and move if not null
        float _horizontal = Input.GetAxis(HorizontalAxis);

        if (_horizontal != 0)
        {
            // Flip the character if looking the opposite side of his movement
            if ((isFacingRight && _horizontal < 0) || (!isFacingRight && _horizontal > 0)) Flip();

            Move(new Vector2(transform.position.x + _horizontal, transform.position.y));
        }
    }
    #endregion

    #region Movements
    /// <summary>
    /// Flips the character on the horizontal axis ; in other words, change the side he's looking.
    /// </summary>
    public void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(Vector3.up, 180);
    }

    /// <summary>
    /// Get collider center, max and min points in local space, and update
    /// local variables values on them.
    /// </summary>
    public void GetColliderBounds()
    {
        colliderCenter = collider.bounds.center - transform.position;
        colliderMax = colliderCenter + collider.bounds.extents;
        colliderMin = colliderCenter - collider.bounds.extents;
    }

    /// <summary>
    /// Move the player in a direction.
    /// </summary>
    /// <param name="_position">Position where the character is moving to.</param>
    public void Move(Vector2 _position)
    {
        // Get the new position of the character
        Vector2 _newPosition = Vector2.Lerp(transform.position, _position, Time.deltaTime * speed);



        transform.position = _newPosition;
    }

    /// <summary>
    /// Make the character jump in a straight vertical movement.
    /// </summary>
    /// <returns></returns>
    public IEnumerator Jump()
    {
        yield break;
    }
    #endregion

    #region Actions & Attacks
    /// <summary>
    /// Make the character prepare a strike with his bat. It's gonna rock.
    /// </summary>
    /// <returns>IEnumerator, baby.</returns>
    public IEnumerator Strike()
    {
        yield break;
    }

    /// <summary>
    /// Make the character prepare a grenade throw.
    /// </summary>
    /// <returns>IEnumerator, baby.</returns>
    public IEnumerator ThrowGrenade()
    {
        yield break;
    }
    #endregion

    #region Health
    /// <summary>
    /// Makes the character die. And explode.
    /// </summary>
    public void Die()
    {

    }
    #endregion

    #endregion

    #region Unity Methods
    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        #if UNITY_EDITOR
        // Try to get missing component(s), and debug it if cannot be found
        if (!animator)
        {
            animator = GetComponentInParent<Animator>();
            if (!animator) Debug.LogWarning($"Animator missing on \"{name}\" Pinata");
        }
        if (!collider)
        {
            collider = GetComponentInChildren<BoxCollider2D>();
            if (!collider) Debug.LogWarning($"Collider missing on \"{name}\" Pinata");
        }
        if (!rigidbody)
        {
            rigidbody = GetComponentInChildren<Rigidbody2D>();
            if (!rigidbody) Debug.LogWarning($"Rigidbody missing on \"{name}\" Pinata");
        }
        #endif
    }

    // Implement OnDrawGizmos if you want to draw gizmos that are also pickable and always drawn
    private void OnDrawGizmos()
    {
        // Draws points positions used for movement raycasts
        if (!collider) return;

        Gizmos.color = Color.cyan;

        Gizmos.DrawSphere(collider.bounds.max, .1f);
        Gizmos.DrawSphere(new Vector2(collider.bounds.max.x, collider.bounds.center.y), .1f);
        Gizmos.DrawSphere(new Vector2(collider.bounds.max.x, collider.bounds.min.y), .1f);

        Gizmos.DrawSphere(new Vector2(collider.bounds.min.x, collider.bounds.max.y), .1f);
        Gizmos.DrawSphere(new Vector2(collider.bounds.center.x, collider.bounds.max.y), .1f);

        Gizmos.DrawSphere(new Vector2(collider.bounds.min.x, collider.bounds.min.y), .1f);
        Gizmos.DrawSphere(new Vector2(collider.bounds.center.x, collider.bounds.min.y), .1f);

        Gizmos.color = Color.white;

        
    }

    // Use this for initialization
    private void Start()
    {
        GetColliderBounds();
    }
	
	// Update is called once per frame
	private void Update()
    {
        // Check player inputs
        CheckInputs();
    }
	#endregion

	#endregion
}
