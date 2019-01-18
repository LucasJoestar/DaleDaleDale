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

	#endregion

	#region Methods

	#region Original Methods

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
