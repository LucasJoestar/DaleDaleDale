using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controller used to manipulate a player instance in the game.
/// </summary>
[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
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
     *      • Increase speed when moving.
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
     *	Date :			[27 / 02 / 2019]
	 *	Author :		[Guibert Lucas]
	 *
	 *	Changes :
     *	
     *	    • Created the movement system with 3 raycasts, so that player do not cross
     *	walls or obstacles ! 100% operational, pretty funky baby.
	 *
	 *	-----------------------------------
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

    #region Parameters
    /// <summary>
    /// Determines what is an obstacle, and what is not.
    /// </summary>
    [SerializeField] private LayerMask whatIsObstacle = new LayerMask();

    /// <summary>
    /// All grenades the player is carrying on.
    /// </summary>
    [SerializeField] private Dictionary<GrenadeType, int> grenades = new Dictionary<GrenadeType, int>()
    {
        { GrenadeType.Classic, 3 }, { GrenadeType.Bouncing, 0 }, { GrenadeType.Sticky, 0 }
    };

    /// <summary>Backing field for <see cref="SelectedGrenade"/>.</summary>
    [SerializeField] private GrenadeType selectedGrenade = GrenadeType.Classic;

    /// <summary>
    /// The actual grenade type selected by the player.
    /// </summary>
    public GrenadeType SelectedGrenade
    {
        get { return selectedGrenade; }
        private set
        {
            selectedGrenade = value;
        }
    }


    /// <summary>
    /// The currently charging action of the player.
    /// </summary>
    [SerializeField] private ChargingAction chargingAction = ChargingAction.None;

    /// <summary>
    /// The state of the current charging action of the player.
    /// </summary>
    [SerializeField] private ChargingActionState chargingActionState = ChargingActionState.Basic;

    /// <summary>
    /// Indicates if the player is against a wall, and if so at which side of him it is.
    /// </summary>
    [SerializeField] private AgainstWall againstWall = AgainstWall.None;


    /// <summary>
    /// If true, the player is facing the right side of the screen ; otherwise, facing the left one.
    /// </summary>
    [SerializeField] private bool isFacingRight = true;

    /// <summary>
    /// Indicates if the player is currently on ground, or in the air.
    /// </summary>
    [SerializeField] private bool isOnGround = false;

    /// <summary>
    /// Indicates if the player is currently moving.
    /// </summary>
    [SerializeField] private bool isMoving = false;

    /// <summary>
    /// If in animation, the player must wait to exit it before starting another action.
    /// </summary>
    [SerializeField] private bool isInAnimation = false;


    /// <summary>
    /// Current speed of the player movements.
    /// </summary>
    [SerializeField] private float speed = 0;

    /// <summary>
    /// Maximum speed of the player movements.
    /// </summary>
    [SerializeField] private float maxSpeed = 2;

    /// <summary>
    /// Coefficient used to multiple player speed by.
    /// </summary>
    [SerializeField] private float speedCoef = 1;

    /// <summary>
    /// Initial speed of the player movements when starting moving.
    /// </summary>
    [SerializeField] private float initialSpeed = 1;

    /// <summary>
    /// Time it take for the player when starting moving to reach his maximum speed.
    /// </summary>
    [SerializeField] private float speedIncreaseDuration = .5f;


    /// <summary>
    /// Y velocity added to the player when performing a standard jumping.
    /// </summary>
    [SerializeField] private int jumpForce = 250;

    /// <summary>
    /// Maximum duration of a standard jump.
    /// </summary>
    [SerializeField] private float jumpMaxDuration = 1;

    /// <summary>
    /// Force added the player when performing a wall jump.
    /// </summary>
    [SerializeField] private Vector2 wallJumpForce = new Vector2(-2, 1);

    /// <summary>
    /// Maximum duration of a wall jump.
    /// </summary>
    [SerializeField] private float wallJumpMaxDuration = .5f;
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
    private Vector3 colliderCenter = Vector3.zero;

    /// <summary>
    /// Extents of the collider (in world space).
    /// </summary>
    private Vector2 colliderExtents = Vector2.one;
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
        else if (isMoving)
        {
            isMoving = false;
        }
    }
    #endregion

    #region Movements
    /// <summary>
    /// Move the player in a direction.
    /// </summary>
    /// <param name="_position">Position to move the player in direction (in world space).</param>
    public void Move(Vector2 _position)
    {
        // Get the new position of the character
        Vector2 _newPosition = Vector2.Lerp(transform.position, _position, Time.deltaTime * speed * speedCoef);

        /* Before moving, raycast between the actual position and the desired one ;
         * To do that, follow these 3 steps :
         *
         *  - First, raycast from the collider facing side bottom corner, where it is most likely
         *  to encounter an obstacle.
         *  
         *  - If nothing is hit, continue by raycasting from the facing side top one,
         *  to check if something is at the top level of the player.
         *  
         *  - Finally, if it's all good, raycast from the facing side center point
         *  of the collider, to get if something is in the middle.
         *  
         *      During these steps, if an obstacle is found, stop raycasting and set
         *  the player new position against the obstacle.
        */

        // Creates variables for raycast
        RaycastHit2D _hit;
        Vector2 _direction = isFacingRight ? Vector2.right : Vector2.left;
        float _distance = Mathf.Abs(_newPosition.x - transform.position.x);

        Vector2 _colliderRight = new Vector2(transform.position.x + ((colliderCenter.x + colliderExtents.x) * isFacingRight.Sign()), transform.position.y + colliderCenter.y);

        // Bottom raycast
        _hit = Physics2D.Raycast(new Vector2(_colliderRight.x, _colliderRight.y - colliderExtents.y), _direction, _distance, whatIsObstacle);

        if (_hit.collider != null)
        {
            Debug.Log($"Obstacle at Bottom ! => {_hit.collider.name}");

            // Moves the player on the raycast result
            MoveOnRaycast(_hit);
            return;
        }

        // Top raycast
        _hit = Physics2D.Raycast(new Vector2(_colliderRight.x, _colliderRight.y + colliderExtents.y), _direction, _distance, whatIsObstacle);

        if (_hit.collider != null)
        {
            Debug.Log($"Obstacle at Top ! => {_hit.collider.name}");

            // Moves the player on the raycast result
            MoveOnRaycast(_hit);
            return;
        }

        // Center raycast
        _hit = Physics2D.Raycast(_colliderRight, _direction, _distance, whatIsObstacle);

        if (_hit.collider != null)
        {
            Debug.Log($"Obstacle at Center ! => {_hit.collider.name}");

            // Moves the player on the raycast result
            MoveOnRaycast(_hit);
            return;
        }

        // Moves the player
        transform.position = _newPosition;
        if (!isMoving) isMoving = true;
    }

    /// <summary>
    /// Referencing on a raycast hit, move or not the player.
    /// </summary>
    /// <param name="_hit">Raycast hit used to move the player. Must have a non null collider.</param>
    private void MoveOnRaycast(RaycastHit2D _hit)
    {
        // Set the player position if needed
        if (_hit.distance > 0)
        {
            float _xColliderEdge = _hit.collider.bounds.center.x - (_hit.collider.bounds.extents.x * isFacingRight.Sign());

            transform.position = new Vector3(_xColliderEdge - ((colliderCenter.x + colliderExtents.x) * isFacingRight.Sign()), transform.position.y);

            if (!isMoving) isMoving = true;
        }
        else if (isMoving)
        {
            isMoving = false;
        }
    }

    /// <summary>
    /// Flips the character on the horizontal axis ; in other words, change the side he's looking.
    /// </summary>
    public void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(Vector3.up, 180);
    }

    /// <summary>
    /// Make the character jump in a straight vertical movement.
    /// </summary>
    /// <returns>IEnumerator, baby.</returns>
    public IEnumerator Jump()
    {
        yield break;
    }

    /// <summary>
    /// Get collider center, max and min points in local space, and update
    /// local variables values on them.
    /// </summary>
    public void UpdateColliderBounds()
    {
        colliderCenter = collider.bounds.center - transform.position;
        colliderExtents = collider.bounds.extents - (Vector3.one * .001f);
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
            collider = GetComponent<BoxCollider2D>();
            if (!collider) Debug.LogWarning($"Collider missing on \"{name}\" Pinata");
        }
        if (!rigidbody)
        {
            rigidbody = GetComponent<Rigidbody2D>();
            if (!rigidbody) Debug.LogWarning($"Rigidbody missing on \"{name}\" Pinata");
        }
        #endif
    }

    // Implement OnDrawGizmos if you want to draw gizmos that are also pickable and always drawn
    private void OnDrawGizmos()
    {
        // Draws points positions used for movement raycasts
        if (collider)
        {
            Gizmos.color = Color.cyan;

            if (!UnityEditor.EditorApplication.isPlaying)
            {
                Gizmos.DrawSphere(collider.bounds.max, .1f);
                Gizmos.DrawSphere(new Vector2(collider.bounds.max.x, collider.bounds.center.y), .1f);
                Gizmos.DrawSphere(new Vector2(collider.bounds.max.x, collider.bounds.min.y), .1f);

                Gizmos.DrawSphere(new Vector2(collider.bounds.min.x, collider.bounds.max.y), .1f);
                Gizmos.DrawSphere(new Vector2(collider.bounds.center.x, collider.bounds.max.y), .1f);

                Gizmos.DrawSphere(new Vector2(collider.bounds.min.x, collider.bounds.min.y), .1f);
                Gizmos.DrawSphere(new Vector2(collider.bounds.center.x, collider.bounds.min.y), .1f);
            }
            else
            {
                Gizmos.DrawSphere(new Vector2(transform.position.x + colliderCenter.x + colliderExtents.x, transform.position.y + colliderCenter.y + colliderExtents.y), .1f);
                Gizmos.DrawSphere(new Vector2(transform.position.x + colliderCenter.x + colliderExtents.x, transform.position.y + colliderCenter.y), .1f);
                Gizmos.DrawSphere(new Vector2(transform.position.x + colliderCenter.x + colliderExtents.x, transform.position.y + colliderCenter.y - colliderExtents.y), .1f);

                Gizmos.DrawSphere(new Vector2(transform.position.x + colliderCenter.x - colliderExtents.x, transform.position.y + colliderCenter.y + colliderExtents.y), .1f);
                Gizmos.DrawSphere(new Vector2(transform.position.x + colliderCenter.x, transform.position.y + colliderCenter.y + colliderExtents.y), .1f);

                Gizmos.DrawSphere(new Vector2(transform.position.x + colliderCenter.x - colliderExtents.x, transform.position.y + colliderCenter.y - colliderExtents.y), .1f);
                Gizmos.DrawSphere(new Vector2(transform.position.x + colliderCenter.x, transform.position.y + colliderCenter.y - colliderExtents.y), .1f);
            }
        }
    }

    // Use this for initialization
    private void Start()
    {
        // Get collider bounds at start
        UpdateColliderBounds();
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

/// <summary>
/// Used to know if a player is against a wall, and if so,
/// which side of him is it.
/// </summary>
public enum AgainstWall
{
    None,
    Left,
    Right
}

/// <summary>
/// Used to know if a player is currently charging an action, and if so,
/// which action is it.
/// </summary>
public enum ChargingAction
{
    None,
    Strike,
    Throw
}

/// <summary>
/// Contains all different charging states for players actions, like striking or throwing.
/// </summary>
public enum ChargingActionState
{
    Basic,
    Loaded,
    Concentrate,
    Ultra
}
