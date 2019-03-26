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
     *	        • Jump when on ground, and move according to air resistance.
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
     *      • Polish the thing !
     *  
     *  [ACTIONS]
     *  
     *      • Implement the grenade with load, throw, explosions, death & everything that
     *  goes with it.
     *  
     *      • Implement the bat with load & repulse.
     * 
     *  [OTHERS]
     * 
     *      • Link a Player object to the controller.
     *  
	 *	#####################
	 *	### MODIFICATIONS ###
	 *	#####################
	 *
     *	Date :			[26 / 03 / 2019]
	 *	Author :		[Guibert Lucas]
	 *
	 *	Changes :
     *	
     *	    • Implemented base animator movement related system.
     *	Still got to fix jump & stick on wall ones.
	 *
	 *	-----------------------------------
     * 
     *	Date :			[20 / 03 / 2019]
	 *	Author :		[Guibert Lucas]
	 *
	 *	Changes :
     *	
     *	    • Some tests on the player gravity and movements in air.
	 *
	 *	-----------------------------------
     * 
     *	Date :			[19 / 03 / 2019]
	 *	Author :		[Guibert Lucas]
	 *
	 *	Changes :
     *	
     *	    • Created a sticky behaviour when against a wall.
	 *
	 *	-----------------------------------
     * 
     *	Date :			[17 / 03 / 2019]
	 *	Author :		[Guibert Lucas]
	 *
	 *	Changes :
     *	
     *	    • Here it is : a cool movement system, using rigidbody velocity when in air.
     *	It works fine, pretty fine actually. I'm happy.
	 *
	 *	-----------------------------------
     * 
     *	Date :			[15 / 03 / 2019]
	 *	Author :		[Guibert Lucas]
	 *
	 *	Changes :
     *	
     *	    • Tests on an improved movement system using the rigidbody velocity.
     *	It doesn't work very well...
	 *
	 *	-----------------------------------
     * 
     *	Date :			[01 / 03 / 2019]
	 *	Author :		[Guibert Lucas]
	 *
	 *	Changes :
     *	
     *	    • Players now have an increasing speed when moving.
     *	    
     *	    • We can now jump, in a vertical way ; the more you held the jump button,
     *	the higher you jump.
     *	
     *	    • First wall jump system version implemented ; but... use custom movement
     *	system along with velocity does not match very well. Got to try all with
     *	rigidbody, and see how it work.
	 *
	 *	-----------------------------------
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

    #region Constants
    /// <summary>
    /// Value used to modify speed coefficient by when in air.
    /// When getting off ground, speed coefficient is multiplied by this.
    /// Conversely, when getting on ground it is divided by this.
    /// </summary>
    private const float SPEED_CONSTRAINT_IN_AIR = 1.125f;

    /// <summary>
    /// Value used to get a sticky behaviour when against a wall in air.
    /// Player moving in this situation will accumulate velocity in X, and only move
    /// when this velocity will exceed the present value.
    /// </summary>
    private const int STICKY_BEHAVIOUR_VELOCITY = 200;
    #endregion

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

    /// <summary>Backing field for <see cref="AgainstWall"/>.</summary>
    [SerializeField] private AgainstWall againstWall = AgainstWall.None;

    /// <summary>
    /// Indicates if the player is against a wall, and if so at which side of him it is.
    /// </summary>
    public AgainstWall AgainstWall
    {
        get { return againstWall; }
        private set
        {
            againstWall = value;
            chargedVelocity = 0;

            if (value != AgainstWall.None)
            {
                // Flip if needed
                if (((value == AgainstWall.Right) && isFacingRight) || ((value == AgainstWall.Left) && !isFacingRight)) Flip();

                // Update animator state
                SetAnim(AnimationState.StickOnWall);
            }
            else if (gravityState == GravityState.OnGround)
            {
                // Update animator state
                SetAnim(AnimationState.OnGround);
            }
        }
    }


    /// <summary>Backing field for <see cref="GravityState"/>.</summary>
    [SerializeField] private GravityState gravityState = GravityState.OnGround;

    /// <summary>
    /// Indicates the current gravity-related state of the player.
    /// </summary>
    public GravityState GravityState
    {
        get { return gravityState; }
        private set
        {
            if (value == GravityState.OnGround)
            {
                speedCoef /= SPEED_CONSTRAINT_IN_AIR;

                // Update animator state
                SetAnim(AnimationState.OnGround);
            }
            else
            {
                if (gravityState == GravityState.OnGround)
                {
                    speedCoef *= SPEED_CONSTRAINT_IN_AIR;
                }

                if (value == GravityState.Ascending)
                {
                    // Update animator state
                    SetAnim(AnimationState.Ascend);
                }
                else
                {
                    // Update animator state
                    SetAnim(AnimationState.Fall);
                }
            }

            gravityState = value;
        }
    }

    /// <summary>
    /// If true, the player is facing the right side of the screen ; otherwise, facing the left one.
    /// </summary>
    [SerializeField] private bool isFacingRight = true;

    /// <summary>
    /// Indicates if the player can move. Yep, that's it.
    /// </summary>
    public bool CanMove = true;

    /// <summary>Backing field for <see cref="IsRunning"/>.</summary>
    [SerializeField] private bool isRunning = false;

    /// <summary>
    /// Indicates if the player is currently running.
    /// </summary>
    public bool IsRunning
    {
        get { return isRunning; }
        private set
        {
            isRunning = value;
            if (value)
            {
                // Update animator state
                SetAnim(AnimationState.Run);
            }
            else
            {
                speed = 0;

                // Update animator state
                SetAnim(AnimationState.Idle);
            }
        }
    }

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
    [SerializeField] private float jumpForce = 10;

    /// <summary>
    /// Y velocity added the player after a standard jump
    /// while jumping & holding the jump button.
    /// </summary>
    [SerializeField] private float jumpForceInDuration = .25f;

    /// <summary>
    /// Maximum duration of a standard jump.
    /// </summary>
    [SerializeField] private float jumpMaxDuration = 1;

    /// <summary>
    /// Force added the player when performing a wall jump.
    /// </summary>
    [SerializeField] private Vector2 wallJumpForce = new Vector2(-2, 1);

    /// <summary>
    /// Y velocity added the player after a wall jump
    /// while jumping & holding the jump button.
    /// </summary>
    [SerializeField] private float wallJumpForceInDuration = .25f;

    /// <summary>
    /// Maximum duration of a wall jump.
    /// </summary>
    [SerializeField] private float wallJumpMaxDuration = .5f;

    /// <summary>
    /// Stacked velocity when against a wall before moving, to make the character sticky.
    /// </summary>
    [SerializeField] private float chargedVelocity = 0;
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

    #region Animations
    /// <summary>
    /// Set player's animations.
    /// This updates the player animator parameters.
    /// </summary>
    /// <param name="_state">New state of the player.</param>
    public void SetAnim(AnimationState _state)
    {
        switch (_state)
        {
            case AnimationState.Idle:
                animator.SetBool("IsRunning", false);
                break;

            case AnimationState.Run:
                animator.SetBool("IsRunning", true);
                break;

            case AnimationState.OnGround:
                animator.SetInteger("GroundState", 0);
                break;

            case AnimationState.Jump:
                animator.SetTrigger("Jump");
                break;

            case AnimationState.Ascend:
                animator.SetInteger("GroundState", 1);
                break;

            case AnimationState.Fall:
                animator.SetInteger("GroundState", -1);
                break;

            case AnimationState.StickOnWall:
                animator.SetInteger("GroundState", 2);
                break;

            case AnimationState.Die:
                animator.SetTrigger("Die");
                break;

            default:
                // Nothing to see here...
                break;
        }
    }
    #endregion

    #region Health
    /// <summary>
    /// Makes the character die. And explode.
    /// </summary>
    public void Die()
    {
        // Update animator state
        SetAnim(AnimationState.Die);
    }
    #endregion

    #region Controller
    /// <summary>
    /// Checks the player inputs, and executes associated actions.
    /// </summary>
    private void CheckInputs()
    {
        // If pressing jump button, let's jump
        if (Input.GetButtonDown(JumpButton)) Jump();
    }

    /// <summary>
    /// Checks the player movements.
    /// </summary>
    private void CheckMovement()
    {
        // If cannot move, do not move
        if (!CanMove) return;

        // Get the horizontal movement, and move if not null
        float _horizontal = Input.GetAxis(HorizontalAxis);

        if (_horizontal != 0)
        {
            // Flip the character if looking the opposite side of his movement
            // Do not move if against a wall on the movement direction
            if ((isFacingRight && (againstWall != AgainstWall.Left) && _horizontal < 0) || (!isFacingRight && (againstWall != AgainstWall.Right) && _horizontal > 0)) Flip();

            if (againstWall == AgainstWall.None || ((againstWall == AgainstWall.Left) && _horizontal > 0) || ((againstWall == AgainstWall.Right) && _horizontal < 0))
            {
                Move(_horizontal);
            }
        }
        else if (isRunning)
        {
            IsRunning = false;
            // Change Velocity => Old : x * .25f | New : the same
            if (rigidbody.velocity.x != 0) rigidbody.velocity = new Vector2(rigidbody.velocity.x, rigidbody.velocity.y);
        }
    }
    #endregion

    #region Movements
    /// <summary>
    /// Moves the player in the X axis.
    /// </summary>
    /// <param name="_xMovement">Movement in the X axis.</param>
    public void Move(float _xMovement)
    {
        // Increase player speed if not at max
        if (speed < maxSpeed)
        {
            if (!isRunning) speed = initialSpeed;
            else
            {
                speed += ((maxSpeed - initialSpeed) / speedIncreaseDuration) * Time.deltaTime;
            }
        }

        if (!isRunning) IsRunning = true;

        // Get the new position of the character
        Vector2 _newPosition = Vector2.Lerp(transform.position, new Vector2(transform.position.x + _xMovement, transform.position.y), Time.fixedDeltaTime * speed * speedCoef);

        // If in the air and moving in the opposite direction
        // of the rigidbody velocity, add opposite force
        if (gravityState != GravityState.OnGround)
        {
            float _xForce = (_newPosition.x - transform.position.x) / (Time.fixedDeltaTime / 1.75f);

            //if (Mathf.Sign(_xMovement) != Mathf.Sign(_xForce)) _xForce *= 1.5f;

            // If against a wall, create a sticky behaviour
            if (againstWall != AgainstWall.None)
            {
                if (Mathf.Abs(chargedVelocity + _xForce) < STICKY_BEHAVIOUR_VELOCITY)
                {
                    chargedVelocity += _xForce;
                    return;
                }
            }

            if ((_xMovement < 0))
            {
                if (rigidbody.velocity.x > _xForce)
                {
                    // Coefficient => Old : 2 | New : 2.5
                    rigidbody.AddForce(new Vector2((_xForce - rigidbody.velocity.x) * 2.5f, 0));
                }
            }
            else if (rigidbody.velocity.x < _xForce)
            {
                rigidbody.AddForce(new Vector2((_xForce - rigidbody.velocity.x) * 2.5f, 0));
            }

            return;
        }

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
            //Debug.Log($"Obstacle at Bottom ! => {_hit.collider.name}");

            // Moves the player on the raycast result
            MoveOnRaycast(_hit);
            return;
        }

        // Top raycast
        _hit = Physics2D.Raycast(new Vector2(_colliderRight.x, _colliderRight.y + colliderExtents.y), _direction, _distance, whatIsObstacle);

        if (_hit.collider != null)
        {
            //Debug.Log($"Obstacle at Top ! => {_hit.collider.name}");

            // Moves the player on the raycast result
            MoveOnRaycast(_hit);
            return;
        }

        // Center raycast
        _hit = Physics2D.Raycast(_colliderRight, _direction, _distance, whatIsObstacle);

        if (_hit.collider != null)
        {
            //Debug.Log($"Obstacle at Center ! => {_hit.collider.name}");

            // Moves the player on the raycast result
            MoveOnRaycast(_hit);
            return;
        }

        // Directly moves the player to the destination if nothing is on the road
        transform.position = _newPosition;
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

            if (!isRunning) IsRunning = true;
        }
        else if (isRunning)
        {
            IsRunning = false;
        }
    }

    /// <summary>
    /// Stop the player from moving for a certain time amount.
    /// </summary>
    /// <param name="_time">Duration to stop the movement.</param>
    /// <returns></returns>
    public IEnumerator StopMove(float _time)
    {
        CanMove = false;

        while(_time > 0)
        {
            yield return null;

            _time -= Time.deltaTime;
        }

        CanMove = true;
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
    /// Makes the player perform a "High" or "Wall" jump depending on their situation.
    /// </summary>
    public void Jump()
    {
        if (gravityState == GravityState.OnGround) StartCoroutine(HighJump());
        else if (againstWall != AgainstWall.None) StartCoroutine(WallJump());
    }

    /// <summary>
    /// Makes the player jump in a straight vertical movement.
    /// </summary>
    /// <returns>IEnumerator, baby.</returns>
    private IEnumerator HighJump()
    {
        // Creates timer for jump duration
        float _timer = 0;

        // Adds initial force to jump
        if (speed != 0)
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x + (isFacingRight.Sign() * 5), jumpForce);
        }
        else
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);
        }

        // Update animator state
        SetAnim(AnimationState.Jump);

        yield return null;

        // While holding the jump button and the jump has not reach is maximum duration,
        // add more force to the jump !
        while (Input.GetButton(JumpButton) && _timer < jumpMaxDuration)
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, rigidbody.velocity.y + jumpForceInDuration);

            yield return null;

            _timer += Time.deltaTime;
        }

        yield break;
    }

    /// <summary>
    /// Makes the player jump against a wall.
    /// </summary>
    /// <returns>IEnumerator, baby.</returns>
    private IEnumerator WallJump()
    {
        // Creates timer for jump duration
        float _timer = 0;

        // Adds initial force to jump
        rigidbody.velocity = new Vector2(rigidbody.velocity.x + (wallJumpForce.x * (againstWall == AgainstWall.Left ? -1 : 1)), (rigidbody.velocity.y * .25f) + wallJumpForce.y);

        //StartCoroutine(StopMove(.2f));

        // Update animator state
        SetAnim(AnimationState.Jump);

        yield return null;

        // While holding the jump button and the jump has not reach is maximum duration,
        // add more force to the jump !
        while (Input.GetButton(JumpButton) && _timer < wallJumpMaxDuration)
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, rigidbody.velocity.y + wallJumpForceInDuration);

            yield return null;

            _timer += Time.deltaTime;
        }

        yield break;
    }

    /// <summary>
    /// Get collider center, max and min points in local space, and update
    /// local variables values on them.
    /// </summary>
    public void UpdateColliderBounds()
    {
        colliderCenter = collider.bounds.center - transform.position;
        colliderExtents = (Vector2)collider.bounds.extents - new Vector2(.001f, .001f);
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

    #region Update
    /// <summary>
    /// Updates various states of the player.
    /// </summary>
    private void UpdateState()
    {
        /* Get if the player is on ground
         * To do that, perform 3 raycast down from the collider bottom :
         * one from center, one from left side and a last one from right side ;
         * 
         * If any of these touch something, then the player is on ground
        */

        // Get point at the bottom center of the collider
        Vector2 _colliderPoint = new Vector2(transform.position.x + colliderCenter.x, transform.position.y + colliderCenter.y - colliderExtents.y);

        // Raycast
        if (!Raycast(_colliderPoint, Vector2.down, .05f) &&
            !Raycast(new Vector2(_colliderPoint.x + colliderExtents.x - .001f, _colliderPoint.y), Vector2.down, .05f) &&
            !Raycast(new Vector2(_colliderPoint.x - colliderExtents.x + .001f, _colliderPoint.y), Vector2.down, .05f))
        {
            // If nothing is hit, player is not on ground
            if (rigidbody.velocity.y > 0)
            {
                if (gravityState != GravityState.Ascending) GravityState = GravityState.Ascending;
            }
            else if (gravityState != GravityState.Falling)
            {
                GravityState = GravityState.Falling;
            }
        }
        else if (gravityState != GravityState.OnGround)
        {
            GravityState = GravityState.OnGround;
            if (rigidbody.velocity.x != 0) rigidbody.velocity *= .1f;
        }

        /* Now, get if the player is against a wall
         * To do so, that's the same than the ground check, except that
         * we have to check for left & right sides instead of bottom.
         * 
         * If player is against a wall on left side, don't raycast for right
        */

        // If on ground, player is not against a wall
        if (gravityState == GravityState.OnGround)
        {
            if (againstWall != AgainstWall.None) AgainstWall = AgainstWall.None;
            return;
        }

        _colliderPoint = new Vector2(transform.position.x + colliderCenter.x - colliderExtents.x, transform.position.y + colliderCenter.y);

        // Raycast for left side
        if (!Raycast(_colliderPoint, Vector2.left, .025f) &&
            !Raycast(new Vector2(_colliderPoint.x, _colliderPoint.y + colliderExtents.y), Vector2.left, .025f) &&
            !Raycast(new Vector2(_colliderPoint.x, _colliderPoint.y - colliderExtents.y), Vector2.left, .025f))
        {
            // If not against a wall on left side,
            // raycast for right side
            _colliderPoint = new Vector2(_colliderPoint.x + (colliderExtents.x * 2), _colliderPoint.y);

            if (!Raycast(_colliderPoint, Vector2.right, .025f) &&
                !Raycast(new Vector2(_colliderPoint.x, _colliderPoint.y + colliderExtents.y), Vector2.right, .025f) &&
                !Raycast(new Vector2(_colliderPoint.x, _colliderPoint.y - colliderExtents.y), Vector2.right, .025f))
            {
                // Well, player is not against a wall then ; update variable if needed
                if (againstWall != AgainstWall.None)
                {
                    AgainstWall = AgainstWall.None;
                }
            }
            // If on against a wall on right side, update variable if needed
            else if (againstWall != AgainstWall.Right)
            {
                AgainstWall = AgainstWall.Right;
            }
        }
        // If on against a wall on left side, update variable if needed
        else if (againstWall != AgainstWall.Left)
        {
            AgainstWall = AgainstWall.Left;
        }
    }

    /// <summary>
    /// Raycast down a given point and get if something was touched.
    /// </summary>
    /// <param name="_point">Position from where to start raycast (in world space).</param>
    /// <returns>Return true if something was touched, false otherwise.</returns>
    private bool Raycast(Vector2 _point, Vector2 _direction, float _distance) { return Physics2D.Raycast(_point, _direction, _distance, whatIsObstacle).collider != null; }
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

    // Frame-rate independent MonoBehaviour.FixedUpdate message for physics calculations.
    private void FixedUpdate()
    {
        // Check player movement
        CheckMovement();
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
        // Set speed coef at start
        if (gravityState == GravityState.OnGround) speedCoef = 1;
        else speedCoef = SPEED_CONSTRAINT_IN_AIR;

        // Get collider bounds at start
        UpdateColliderBounds();
    }
	
	// Update is called once per frame
	private void Update()
    {
        // Update various states of the player
        UpdateState();

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
/// Used to set the player animation state,
/// using animator parameters.
/// </summary>
public enum AnimationState
{
    Idle,
    Run,
    OnGround,
    Jump,
    Ascend,
    Fall,
    StickOnWall,
    Die
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

/// <summary>
/// Picture the gravity-related state of an object.
/// </summary>
public enum GravityState
{
    Falling = -1,
    OnGround,
    Ascending
}
