using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinataController : MonoBehaviour
{
    /* PinataController :
	 *
	 *	#####################
	 *	###### PURPOSE ######
	 *	#####################
	 *
	 *	    Player controller of the game.
     *	    
     *	    Inventory :
     *	    
     *	The character can perform the following actions :
     *	
     *	    [Movements] :
     *	
     *	        - Move along the X axis in both directions, and do not cross walls.
     *	        
     *	        - Jump when on ground, and move according to air restistance.
     *	        
     *	        - Jump when against a wall, pushing the character in the opposite direction.
     *	        
     *	        - Slide all along the walls/
     *	        
     *	    [Actions] :
     *	        
     *	        - Throw a grenade.
     *	    The player can load the throw by holding the button down.
     *	    An animation with particles around the hand indicates the throwing power.
     *	    When at maximum, the character keeps holding the grenade while not hit
     *	    or while the player keeps the button down.
     *	        When releasing the button, if the character is still holding it,
     *	    the grenade is thrown in the aiming direction with a force depending
     *	    on the throw load. At maximum, the grenade kicks some asses.
     *	        If the player just press and release immediatly the button,
     *	    the grenade is throw in the direction without effect.

     *	        - Strike with the bat.
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
	 *	### MODIFICATIONS ###
	 *	#####################
	 *
     *	Date :			[30 / 01 / 2019]
	 *	Author :		[Guibert Lucas]
	 *
	 *	Changes :
     *	
     *	    - Added the animator, rigidbody, grenades, selectedGrenade, isFacingRight, isOnGround, speed & jumpForce fields.
     *	    - Added the Move, Jump, Strike, ThrowGrenade, Die & Flip methods.
	 *
	 *	-----------------------------------
     * 
	 *	Date :			[18 / 01 / 2019]
	 *	Author :		[Guibert Lucas]
	 *
	 *	Changes :
	 *
	 *	    Creation of the class.
     *	    
     *	Detailed script purpose with movements and actions like throw & strike.
	 *
	 *	-----------------------------------
	*/

    #region Events

    #endregion

    #region Fields / Properties
    /// <summary>
    /// Animator of the player, used to play all its animations, like running, dying, etc...
    /// </summary>
    [SerializeField] private Animator animator = null;

    /// <summary>
    /// Player rigidbody, used to give him velcity for jump, explosion recoil, etc...
    /// </summary>
    [SerializeField] private new Rigidbody2D rigidbody2D = null;

    /// <summary>
    /// All grenades the player is carrying on.
    /// </summary>
    [SerializeField]
    private Dictionary<GrenadeType, int> grenades = new Dictionary<GrenadeType, int>()
    {
        { GrenadeType.Classic, 3 }, { GrenadeType.Bouncing, 0 }, { GrenadeType.Sticky, 0 }
    };

    /// <summary>
    /// The actually grenade type selected by the player.
    /// </summary>
    [SerializeField] private GrenadeType selectedGrenade = GrenadeType.Classic;

    /// <summary>
    /// If true, the player is facing the right side of the screen ; otherwise, facing the left one.
    /// </summary>
    [SerializeField] private bool isFacingRight = true;

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

    #region Methods

    #region Original Methods

    #region Movements
    /// <summary>
    /// Move the player in a direction.
    /// </summary>
    /// <param name="_position">Position where the character is moving to.</param>
    public void Move(Vector2 _position)
    {

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

    #region Others
    /// <summary>
    /// Makes the character die. And explode.
    /// </summary>
    public void Die()
    {

    }

    /// <summary>
    /// Flips the character on the horizontal axis ; in other words, change the side he's looking.
    /// </summary>
    public void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(Vector3.forward, 180);
    }
    #endregion

    #endregion

    #region Unity Methods
    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        
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
