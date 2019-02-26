/// <summary>
/// Static class referencing all this project extension methods.
/// </summary>
public static class Extensions 
{
    /* Extensions :
	 *
	 *	#####################
	 *	###### PURPOSE ######
	 *	#####################
	 *
	 *	References all extensions methods in this DaleDaleDale proejct.
	 *
     *	#####################
	 *	####### TO DO #######
	 *	#####################
     * 
     *  [Nothing to see around here...]
     * 
	 *	#####################
	 *	### MODIFICATIONS ###
	 *	#####################
	 *
	 *	Date :			[08 / 02 / 2019]
	 *	Author :		[Guibert Lucas]
	 *
	 *	Changes :
	 *
	 *	Creation of the Extensions class.
     *	
     *	    • Created a method to get a boolean value as a 1 or -1 value.
	 *
	 *	-----------------------------------
	*/

    #region Methods

    #region Boolean
    /// <summary>
    /// Get this boolean value as an int, so as 1 or -1.
    /// </summary>
    /// <param name="_value">Reference value.</param>
    /// <returns>Returns 1 of boolean is equals to true, -1 if false.</returns>
    public static sbyte Sign(this bool _value)
    {
        if (_value) return 1;
        return -1;
    } 
    #endregion

    #endregion
}
