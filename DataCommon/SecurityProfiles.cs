//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
//Major rewrite.  Do not auto-merge.
//End Track #5091 - JScott - Secuirty Lights don't change when permission changes
using System;
using System.Collections;
using System.Globalization;

namespace MIDRetail.DataCommon
{
	[Serializable]
	public class InvalidSecurityAction : Exception
	{
		public InvalidSecurityAction(eSecurityActions aActionID)
			: base("Attempting to set invalid Security Action: " + aActionID.ToString())
		{
		}
	}

	/// <summary>
	/// Summary description for SecurityProfiles.
	/// </summary>
	/// <summary>
	/// The SecurityProfile class identifies an items security profile.
	/// </summary>

	[Serializable]
	abstract public class SecurityProfile : Profile
	{
		//=======
		// FIELDS
		//=======

		private Hashtable _actions;
		private bool _initFullControl;
		private eSecurityLevel _fullControl;
        // Begin Track #5858 - JSmith - Validating store security only
        private eDatabaseSecurityTypes _fullControlDatabaseSecurityType;
        // End Track #5858
		
		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of SecurityProfile using the given Id.
		/// </summary>
		/// <param name="aKey">
		/// The Id of this profile.
		/// </param>

		public SecurityProfile(int aKey)
			: base(aKey)
		{
			_actions = null;
			_initFullControl = true;
			_fullControl = eSecurityLevel.NotSpecified;
            // Begin Track #5858 - JSmith - Validating store security only
            _fullControlDatabaseSecurityType = eDatabaseSecurityTypes.NotSpecified;
            // End Track #5858
			// Begin Track #5234 - JSmith - In user error
			AddSecurityAction(eSecurityActions.View);
			AddSecurityAction(eSecurityActions.Maintain);
			// End Track #5234
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.Security;
			}
		}

		/// <summary>
		/// Returns the Hashtable of actions which contains the user's security of this profile.
		/// </summary>
		/// <remarks>
		/// This is a Hashtable of eSecurityActions whos value is the eSecurityLevel
		/// </remarks>

		private Hashtable Actions
		{
			get
			{
				try
				{
					if (_actions == null)
					{
						_actions = new Hashtable();
					}

					return _actions;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets a flag identifying if the user has read only security to this item.
		/// </summary>

		public bool IsReadOnly
		{
			get
			{
				try
				{
					return (AllowView && !AllowUpdate);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets a flag identifying if the user has full control this item.
		/// </summary>

		public bool AllowFullControl
		{
			get
			{
				try
				{
					return (InitFullControl() == eSecurityLevel.Allow);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets a flag identifying if the user is denied access to this item.
		/// </summary>

		public bool AccessDenied
		{
			get
			{
				try
				{
					return (InitFullControl() == eSecurityLevel.Deny || InitFullControl() == eSecurityLevel.NotSpecified);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets a flag identifying if the user can update this item.
		/// </summary>

		public bool AllowUpdate
		{
			get
			{
				try
				{
					return (InitFullControl() == eSecurityLevel.Allow || GetSecurityLevel(eSecurityActions.Maintain) == eSecurityLevel.Allow);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets a flag identifying if the user can view this item.
		/// </summary>

		public bool AllowView
		{
			get
			{
				try
				{
					return (InitFullControl() == eSecurityLevel.Allow || GetSecurityLevel(eSecurityActions.View) == eSecurityLevel.Allow);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets a flag identifying if the user can delete this item.
		/// </summary>

		public bool AllowDelete
		{
			get
			{
				try
				{
					return (InitFullControl() == eSecurityLevel.Allow || GetSecurityLevel(eSecurityActions.Delete) == eSecurityLevel.Allow);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets a flag identifying if the user can execute this item.
		/// </summary>

		public bool AllowExecute
		{
			get
			{
				try
				{
					return (InitFullControl() == eSecurityLevel.Allow || GetSecurityLevel(eSecurityActions.Execute) == eSecurityLevel.Allow);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets a flag identifying if the user can move this item.
		/// </summary>

		public bool AllowMove
		{
			get
			{
				try
				{
					return (InitFullControl() == eSecurityLevel.Allow || GetSecurityLevel(eSecurityActions.Move) == eSecurityLevel.Allow);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets a flag identifying if the user can inactivate this item.
		/// </summary>

		public bool AllowInactivate
		{
			get
			{
				try
				{
					return (InitFullControl() == eSecurityLevel.Allow || GetSecurityLevel(eSecurityActions.Inactivate) == eSecurityLevel.Allow);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets a flag identifying if the user can process interactive.
		/// </summary>

		public bool AllowInteractive
		{
			get
			{
				try
				{
					return (InitFullControl() == eSecurityLevel.Allow || GetSecurityLevel(eSecurityActions.Interactive) == eSecurityLevel.Allow);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

        //  BEGIN TT#2015 - gtaylor - apply changes to lower levels
        public bool AllowApplyChangesToLowerLevels
        {
            get
            {
                try
                {
                    return (InitFullControl() == eSecurityLevel.Allow || GetSecurityLevel(eSecurityActions.ApplyChangesToLowerLevels) == eSecurityLevel.Allow);
                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
            }
        }
        //  END TT#2015 - gtaylor - apply changes to lower levels

		// Begin Track #4961 - JSmith - Add security for apply to lower levels.  
		/// <summary>
		/// Gets a flag identifying if the user can apply a setting to lower levels.
		/// </summary>

		public bool AllowApplyToLowerLevels
		{
			get
			{
				try
				{
					return (InitFullControl() == eSecurityLevel.Allow || GetSecurityLevel(eSecurityActions.ApplyToLowerLevels) == eSecurityLevel.Allow);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets a flag identifying if the user can remove the current settings to inherit from a higher level.
		/// </summary>

		public bool AllowInheritFromHigherLevel
		{
			get
			{
				try
				{
					return (InitFullControl() == eSecurityLevel.Allow || GetSecurityLevel(eSecurityActions.InheritFromHigherLevel) == eSecurityLevel.Allow);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}
		// End Track #4961

		//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
		/// <summary>
		/// Gets a flag identifying if the user can assign a value.
		/// </summary>

		public bool AllowAssign
		{
			get
			{
				try
				{
					return (InitFullControl() == eSecurityLevel.Allow || GetSecurityLevel(eSecurityActions.Assign) == eSecurityLevel.Allow);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}
		//End Track #4815


		//========
		// METHODS
		//========

		/// <summary>
		/// Copies the internal data from the given SecurityProfile to this profile.
		/// </summary>
		/// <param name="aSecProf">
		/// The SecurityProfile to copy from.
		/// </param>

		virtual public void CopyFrom(SecurityProfile aSecProf)
		{
			eSecurityActions actionID;
			SecurityPermission clonedPermission;

			try
			{
				_actions = new Hashtable();

				foreach (DictionaryEntry entry in aSecProf.Actions)
				{
					actionID = (eSecurityActions)entry.Key;
					clonedPermission = (SecurityPermission)((SecurityPermission)entry.Value).Clone();
					Actions.Add(actionID, clonedPermission);
				}

				_initFullControl = aSecProf._initFullControl;
				_fullControl = aSecProf._fullControl;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Clears all security levels for the item
		/// </summary>

		public void Clear()
		{
			try
			{
				Actions.Clear();
				_initFullControl = true;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets the inheritance flag for the provided action
		/// </summary>
		/// <param name="aActionID">
		/// The eSecurityActions of the action
		/// </param>
		/// <returns>
		/// A flag identifying if the security level for the action was inherited
		/// </returns>

		public bool IsInherited(eSecurityActions aActionID)
		{
			try
			{
				if (Actions.Contains(aActionID))
				{
					return ((SecurityPermission)Actions[aActionID]).IsInherited;
				}
				else
				{
					return false;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets the security level for the provided action
		/// </summary>
		/// <param name="aActionID">
		/// The eSecurityActions of the action
		/// </param>
		/// <returns>
		/// An eSecurityLevel containing the security level for the action
		/// </returns>

		public SecurityPermission GetSecurityPermission(eSecurityActions aActionID)
		{
			SecurityPermission permission;

			try
			{
				if (Actions.Contains(aActionID))
				{
					permission = (SecurityPermission)Actions[aActionID];

					if (aActionID == eSecurityActions.FullControl)
					{
						permission.SecurityLevel = InitFullControl();
					}
				}
				else
				{
					permission = new SecurityPermission(eSecurityLevel.NotSpecified);
				}

				return permission;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        // Begin TT#335 - JSmith - Menu should say Remove and not Delete
        /// <summary>
        /// Adds all possible security actions to the profile
        /// </summary>
        public void AddAllSecurityActions()
        {
            eSecurityActions action;
            foreach (int actionValue in Enum.GetValues(typeof(eSecurityActions)))
            {
                action = (eSecurityActions)actionValue;
                if (action != eSecurityActions.NotSpecified &&
                    action != eSecurityActions.FullControl)
                {
                    AddSecurityAction(action);
                }
            }
        }
        // End TT#335

		/// <summary>
		/// Adds the given action to this profile as an available action.
		/// </summary>
		/// <param name="aActionID">
		/// The eSecurityActions ID of the action to add.
		/// </param>

		public void AddSecurityAction(eSecurityActions aActionID)
		{
			SecurityPermission permission;

			try
			{
				// Begin Track #5234 - JSmith - In user error
//				permission = new SecurityPermission(eSecurityLevel.NotSpecified);
//				Actions.Add(aActionID, permission);
//				_initFullControl = true;
				if (!Actions.Contains(aActionID))
				{
					permission = new SecurityPermission(eSecurityLevel.NotSpecified);
					Actions.Add(aActionID, permission);
					_initFullControl = true;
				}
				// End Track #5234
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the security level for the provided action
		/// </summary>
		/// <param name="aActionID">
		/// The eSecurityActions of the action
		/// </param>
		/// <param name="aSecurityLevel">
		/// The eSecurityLevel of the action
		/// </param>

        public void SetSecurity(eSecurityActions aActionID, eSecurityLevel aSecurityLevel)
		{
			try
			{
                // Begin Track #5858 - JSmith - Validating store security only
                //SetSecurity(aActionID, aSecurityLevel, eSecurityOwnerType.NotSpecified, 0, eSecurityInheritanceTypes.NotSpecified, -1, 0, false, false);
                SetSecurity(aActionID, aSecurityLevel, eSecurityOwnerType.NotSpecified, 0, eSecurityInheritanceTypes.NotSpecified, -1, 0, false, false, eDatabaseSecurityTypes.NotSpecified);
                // End Track #5858
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        // Begin Track #5858 - JSmith - Validating store security only
        /// <summary>
        /// Sets the security level for the provided action
        /// </summary>
        /// <param name="aActionID">
        /// The eSecurityActions of the action
        /// </param>
        /// <param name="aSecurityLevel">
        /// The eSecurityLevel of the action
        /// </param>
        /// <param name="aDatabaseSecurityTypes">
        /// The eDatabaseSecurityTypes of the action.
        /// </param>

        public void SetSecurity(eSecurityActions aActionID, eSecurityLevel aSecurityLevel, eDatabaseSecurityTypes aDatabaseSecurityTypes)
        {
            try
            {
                SetSecurity(aActionID, aSecurityLevel, eSecurityOwnerType.NotSpecified, 0, eSecurityInheritanceTypes.NotSpecified, -1, 0, false, false, aDatabaseSecurityTypes);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        // End Track #5858

		/// <summary>
		/// Sets the security level for the provided action
		/// </summary>
		/// <param name="aActionID">
		/// The eSecurityActions of the action
		/// </param>
		/// <param name="aSecurityLevel">
		/// The eSecurityLevel of the action
		/// </param>
		/// <param name="aOwnerType">
		/// The eSecurityOwnerType of the item where the security was inherited
		/// </param>
		/// <param name="aOwnerKey">
		/// The key of the aOwnerType where the security was inherited
		/// </param>
		/// <param name="aSecurityInheritanceType">
		/// The eSecurityInheritanceTypes of the action
		/// </param>
		/// <param name="aInheritedFrom">
		/// The key of the item where the permission what inherited
		/// </param>
		/// <param name="aInheritanceOffset">
		/// The offset from the primary security where the setting was found
		/// </param>
		/// <param name="aSetInheritancePath">
		/// The flag identifies if the inheritance path is to be set
		/// </param>
		/// <param name="aInheritedFromGroup">
		/// The flag identifies if inheritance comes from a group and should always be set
		/// </param>
        /// <param name="aDatabaseSecurityTypes">
        /// The eDatabaseSecurityTypes of the action.
        /// </param>

		public void SetSecurity(
			eSecurityActions aActionID,
			eSecurityLevel aSecurityLevel,
			eSecurityOwnerType aOwnerType, 
			int aOwnerKey,
			eSecurityInheritanceTypes aSecurityInheritanceType,
			int aInheritedFrom,
			int aInheritanceOffset, 
			bool aSetInheritancePath,
            // Begin Track #5858 - JSmith - Validating store security only
            //bool aInheritedFromGroup)
            bool aInheritedFromGroup,
            eDatabaseSecurityTypes aDatabaseSecurityTypes)
            // End Track #5858
		{
			bool isSelected;
			eSecurityActions action;
			SecurityPermission permission;

			try
			{
				if (aActionID == eSecurityActions.FullControl)
				{
					foreach (DictionaryEntry entry in Actions)
					{
						action = (eSecurityActions)entry.Key;

						if (action != eSecurityActions.FullControl)
						{
							SetSecurity(
								action,
								aSecurityLevel,
								aOwnerType,
								aOwnerKey,
								aSecurityInheritanceType,
								aInheritedFrom,
								aInheritanceOffset,
								aSetInheritancePath,
                                // Begin Track #5858 - JSmith - Validating store security only
                                //aInheritedFromGroup);
                                aInheritedFromGroup,
                                aDatabaseSecurityTypes);
                                // End Track #5858
						}
					}

					// Begin Track #5858 - JSmith - Validating store security only
                    //_fullControl = aSecurityLevel;
                    if (_fullControlDatabaseSecurityType == eDatabaseSecurityTypes.NotSpecified ||
                        _fullControlDatabaseSecurityType == aDatabaseSecurityTypes)
                    {
                        _fullControl = aSecurityLevel;
                    }
                    // Allow overrides Deny across multiple security types
                    else if (_fullControl == eSecurityLevel.Deny &&
                        aSecurityLevel == eSecurityLevel.Allow)
                    {
                        _fullControl = aSecurityLevel;
                    }
                    // Begin Track #5892 - JSmith - Security set to view for Merchandise and Hierarchies, but no hierarchies show up.
                    //_fullControlDatabaseSecurityType = aDatabaseSecurityTypes;
                    // End Track #5858
                    _initFullControl = false;
                    foreach (SecurityPermission loggedPermission in Actions.Values)
                    {
						// Begin Track #5852 (2) stodd - security not getting blended correctly
                        if (_fullControlDatabaseSecurityType != eDatabaseSecurityTypes.NotSpecified ||
                            (loggedPermission.DatabaseSecurityType != eDatabaseSecurityTypes.NotSpecified &&
                            loggedPermission.DatabaseSecurityType != _fullControlDatabaseSecurityType))
						// End Track #5852 (2) stodd
                        {
                            _initFullControl = true;
                        }
                    }
                    _fullControlDatabaseSecurityType = aDatabaseSecurityTypes;
                    // End Track #5892
				}
				else if (Actions.Contains(aActionID))
				{
					isSelected = false;
					permission = (SecurityPermission)Actions[aActionID];

					if (aSecurityLevel == eSecurityLevel.NotSpecified ||
						permission.SecurityLevel == eSecurityLevel.NotSpecified ||
						permission.SecurityLevel == eSecurityLevel.Allow ||
						(permission.SecurityLevel == eSecurityLevel.Deny &&
						(aOwnerType == eSecurityOwnerType.User ||
                        // Begin Track #5892 - JSmith - Security set to view for Merchandise and Hierarchies, but no hierarchies show up.
                        //(aOwnerType == eSecurityOwnerType.Group && permission.OwnerKey == aOwnerKey))))
                        (aOwnerType == eSecurityOwnerType.Group && permission.OwnerKey == aOwnerKey))) ||
                        permission.DatabaseSecurityType != aDatabaseSecurityTypes)
                        // End Track #5892
					{
                        // Begin Track #5858 - JSmith - Validating store security only
                        //permission.SecurityLevel = aSecurityLevel;
                        if (permission.DatabaseSecurityType == eDatabaseSecurityTypes.NotSpecified ||
                            permission.DatabaseSecurityType == aDatabaseSecurityTypes)
                        {
                            permission.SecurityLevel = aSecurityLevel;
							// Begin Track #5852 (2) stodd - security not getting blended correctly
							permission.DatabaseSecurityType = aDatabaseSecurityTypes;
							// End Track #5852 (2) stodd
                        }
                        // Allow overrides Deny across multiple security types
                        else if (permission.SecurityLevel == eSecurityLevel.Deny &&
                            aSecurityLevel == eSecurityLevel.Allow)
                        {
                            permission.SecurityLevel = aSecurityLevel;
							// Begin Track #5852 (2) stodd - security not getting blended correctly
							permission.DatabaseSecurityType = aDatabaseSecurityTypes;
							// End Track #5852 (2) stodd
                        }
						// Begin Track #5852 (2) stodd - security not getting blended correctly
                        //permission.DatabaseSecurityType = aDatabaseSecurityTypes;
						// End Track #5852 (2) stodd
                        // End Track #5858
						_initFullControl = true;
						isSelected = true;

						if (aSecurityInheritanceType != eSecurityInheritanceTypes.NotSpecified)
						{
							if (aSecurityLevel == eSecurityLevel.NotSpecified || (Key == aInheritedFrom && !aInheritedFromGroup))
							{
								ClearInheritance(aActionID);
							}
							else
							{
								SetInheritance(aActionID, aOwnerType, aOwnerKey, aSecurityInheritanceType, aInheritedFrom, aInheritanceOffset);
							}
						}
					}

					if (aSecurityInheritanceType != eSecurityInheritanceTypes.NotSpecified)
					{
						if (aSetInheritancePath)
						{
							SetInheritancePath(aActionID, aOwnerType, aOwnerKey, aSecurityInheritanceType, aInheritedFrom, aSecurityLevel, isSelected);
						}
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the security of the item to full control
		/// </summary>

		public void SetFullControl()
		{
			Hashtable tempActions;

			try
			{
				tempActions = (Hashtable)Actions.Clone();
				Actions.Clear();

				foreach (DictionaryEntry entry in tempActions)
				{
					AddSecurityAction((eSecurityActions)entry.Key);
				}

				SetSecurity(eSecurityActions.FullControl, eSecurityLevel.Allow);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the security of the item to updateable
		/// </summary>

		public void SetAllowUpdate()
		{
			try
			{
				if (Actions.Contains(eSecurityActions.Maintain))
				{
					Actions.Remove(eSecurityActions.Maintain);
				}

				AddSecurityAction(eSecurityActions.Maintain);
				SetSecurity(eSecurityActions.Maintain, eSecurityLevel.Allow);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the security of the item to viewable
		/// </summary>

		public void SetAllowView()
		{
			try
			{
				if (Actions.Contains(eSecurityActions.View))
				{
					Actions.Remove(eSecurityActions.View);
				}

				AddSecurityAction(eSecurityActions.View);
				SetSecurity(eSecurityActions.View, eSecurityLevel.Allow);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the security of the item to deletable
		/// </summary>

		public void SetAllowDelete()
		{
			try
			{
				if (Actions.Contains(eSecurityActions.Delete))
				{
					Actions.Remove(eSecurityActions.Delete);
				}

				AddSecurityAction(eSecurityActions.Delete);
				SetSecurity(eSecurityActions.Delete, eSecurityLevel.Allow);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the security of the item to executable
		/// </summary>

		public void SetAllowExecute()
		{
			try
			{
				if (Actions.Contains(eSecurityActions.Execute))
				{
					Actions.Remove(eSecurityActions.Execute);
				}

				AddSecurityAction(eSecurityActions.Execute);
				SetSecurity(eSecurityActions.Execute, eSecurityLevel.Allow);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the security of the item to movable
		/// </summary>

		public void SetAllowMove()
		{
			try
			{
				if (Actions.Contains(eSecurityActions.Move))
				{
					Actions.Remove(eSecurityActions.Move);
				}

				AddSecurityAction(eSecurityActions.Move);
				SetSecurity(eSecurityActions.Move, eSecurityLevel.Allow);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the security of the item to inactivate
		/// </summary>

		public void SetAllowInactivate()
		{
			try
			{
				if (Actions.Contains(eSecurityActions.Inactivate))
				{
					Actions.Remove(eSecurityActions.Inactivate);
				}

				AddSecurityAction(eSecurityActions.Inactivate);
				SetSecurity(eSecurityActions.Inactivate, eSecurityLevel.Allow);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the security of the item to interactive
		/// </summary>

		public void SetAllowInteractive()
		{
			try
			{
				if (Actions.Contains(eSecurityActions.Interactive))
				{
					Actions.Remove(eSecurityActions.Interactive);
				}

				AddSecurityAction(eSecurityActions.Interactive);
				SetSecurity(eSecurityActions.Interactive, eSecurityLevel.Allow);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the security of the item to not updatable
		/// </summary>

		public void SetDenyUpdate()
		{
			try
			{
				if (Actions.Contains(eSecurityActions.Maintain))
				{
					Actions.Remove(eSecurityActions.Maintain);
					AddSecurityAction(eSecurityActions.Maintain);
					SetSecurity(eSecurityActions.Maintain, eSecurityLevel.Deny);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the security of the item to not viewable
		/// </summary>

		public void SetDenyView()
		{
			try
			{
				if (Actions.Contains(eSecurityActions.View))
				{
					Actions.Remove(eSecurityActions.View);
					AddSecurityAction(eSecurityActions.View);
					SetSecurity(eSecurityActions.View, eSecurityLevel.Deny);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the security of the item to not deletable
		/// </summary>

		public void SetDenyDelete()
		{
			try
			{
				if (Actions.Contains(eSecurityActions.Delete))
				{
					Actions.Remove(eSecurityActions.Delete);
					AddSecurityAction(eSecurityActions.Delete);
					SetSecurity(eSecurityActions.Delete, eSecurityLevel.Deny);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the security of the item to not Executable
		/// </summary>

		public void SetDenyExecute()
		{
			try
			{
				if (Actions.Contains(eSecurityActions.Execute))
				{
					Actions.Remove(eSecurityActions.Execute);
					AddSecurityAction(eSecurityActions.Execute);
					SetSecurity(eSecurityActions.Execute, eSecurityLevel.Deny);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the security of the item to not movable
		/// </summary>

		public void SetDenyMove()
		{
			try
			{
				if (Actions.Contains(eSecurityActions.Move))
				{
					Actions.Remove(eSecurityActions.Move);
					AddSecurityAction(eSecurityActions.Move);
					SetSecurity(eSecurityActions.Move, eSecurityLevel.Deny);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        //  BEGIN TT#2015 - gtaylor - apply changes to lower levels
        public void SetDeny(eSecurityActions action)
        {
            try
            {
                if (Actions.Contains(action))
                {
                    Actions.Remove(action);
                    AddSecurityAction(action);
                    SetSecurity(action, eSecurityLevel.Deny);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        //  END TT#2015 - gtaylor - apply changes to lower levels


		/// <summary>
		/// Sets the security of the item to not inactivate
		/// </summary>

		public void SetDenyInactivate()
		{
			try
			{
				if (Actions.Contains(eSecurityActions.Inactivate))
				{
					Actions.Remove(eSecurityActions.Inactivate);
					AddSecurityAction(eSecurityActions.Inactivate);
					SetSecurity(eSecurityActions.Inactivate, eSecurityLevel.Deny);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the security of the item to not interactive
		/// </summary>

		public void SetDenyInteractive()
		{
			try
			{
				if (Actions.Contains(eSecurityActions.Interactive))
				{
					Actions.Remove(eSecurityActions.Interactive);
					AddSecurityAction(eSecurityActions.Interactive);
					SetSecurity(eSecurityActions.Interactive, eSecurityLevel.Deny);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the security of the item to read only
		/// </summary>

		public void SetReadOnly()
		{
			try
			{
				SetAccessDenied();
				SetAllowView();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the security to deny access to this item
		/// </summary>

		public void SetAccessDenied()
		{
			try
			{
				SetSecurity(eSecurityActions.FullControl, eSecurityLevel.Deny);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the properties associated with an inherited permission path
		/// </summary>
		/// <param name="aActionID">
		/// The eSecurityActions of the action
		/// </param>
		/// <param name="aOwnerType">
		/// The eSecurityOwnerType of the item where the security was inherited
		/// </param>
		/// <param name="aOwnerKey">
		/// The key of the aOwnerType where the security was inherited
		/// </param>
		/// <param name="aSecurityInheritanceType">
		/// The eSecurityInheritanceTypes of the action
		/// </param>
		/// <param name="aInheritanceItem">
		/// The key of the item where the permission what checked
		/// </param>
		/// <param name="aSecurityLevel">
		/// The eSecurityLevel of the item
		/// </param>
		/// <param name="aIsSelected">
		/// The flag that identifies if this path item was the selected item
		/// </param>

		public void SetInheritancePath(eSecurityActions aActionID, eSecurityOwnerType aOwnerType, int aOwnerKey, eSecurityInheritanceTypes aSecurityInheritanceType, int aInheritanceItem, eSecurityLevel aSecurityLevel, bool aIsSelected)
		{
			SecurityPermission permission;
			int iheritanceIndex;

			try
			{
				if (Actions.Contains(aActionID))
				{
					permission = (SecurityPermission)Actions[aActionID];

					iheritanceIndex = permission.SetInheritancePathItem(aOwnerType, aOwnerKey, aSecurityInheritanceType, aInheritanceItem, aSecurityLevel);

					if (aIsSelected)
					{
						permission.SecurityInheritanceIndex = iheritanceIndex;
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Initializes FullControl based upon the individual actions for this profile
		/// </summary>
		/// <returns>
		/// An eSecurityLevel of FullControl initialized
		/// </returns>

		private eSecurityLevel InitFullControl()
		{
			bool allAllow;
			bool anyAllow;
			bool allDeny;

			try
			{
				if (_initFullControl)
				{
					_initFullControl = false;
					allAllow = true;
					anyAllow = false;
					allDeny = true;

					foreach (eSecurityActions action in Actions.Keys)
					{
						if (action != eSecurityActions.FullControl)
						{
							if (GetSecurityLevel(action) == eSecurityLevel.Allow)
							{
								anyAllow = true;
							}
							else
							{
								allAllow = false;
							}

							if (GetSecurityLevel(action) != eSecurityLevel.Deny)
							{
								allDeny = false;
							}
						}
					}

					if (allAllow && !allDeny)
					{
						_fullControl = eSecurityLevel.Allow;
					}
					else if (allDeny && !allAllow)
					{
						_fullControl = eSecurityLevel.Deny;
					}
					else if (anyAllow)
					{
						_fullControl = eSecurityLevel.PartialAllow;
					}
					else
					{
						_fullControl = eSecurityLevel.NotSpecified;
					}
				}

				return _fullControl;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets the security level for the provided action
		/// </summary>
		/// <param name="aActionID">The eSecurityActions of the action
		/// </param>
		/// <returns>
		/// An eSecurityLevel containing the security level for the action
		/// </returns>

		public eSecurityLevel GetSecurityLevel(eSecurityActions aActionID)
		{
			try
			{
				if (Actions.Contains(aActionID))
				{
					return ((SecurityPermission)Actions[aActionID]).SecurityLevel;
				}
				else
				{
					return eSecurityLevel.NotSpecified;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the properties associated with an inherited permission
		/// </summary>
		/// <param name="aActionID">
		/// The eSecurityActions of the action
		/// </param>

		private void ClearInheritance(eSecurityActions aActionID)
		{
			SecurityPermission permission;

			try
			{
				if (Actions.Contains(aActionID))
				{
					permission = (SecurityPermission)Actions[aActionID];
					permission.ClearInheritance();
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the properties associated with an inherited permission
		/// </summary>
		/// <param name="aActionID">
		/// The eSecurityActions of the action
		/// </param>
		/// <param name="aOwnerType">
		/// The eSecurityOwnerType of the item where the security was inherited
		/// </param>
		/// <param name="aOwnerKey">
		/// The key of the aOwnerType where the security was inherited
		/// </param>
		/// <param name="aSecurityInheritanceType">
		/// The eSecurityInheritanceTypes of the action
		/// </param>
		/// <param name="aInheritedFrom">
		/// The key of the item where the permission what inherited
		/// </param>
		/// <param name="aInheritanceOffset">
		/// The offset from the primary security where the setting was found
		/// </param>

		private void SetInheritance(
			eSecurityActions aActionID,
			eSecurityOwnerType aOwnerType,
			int aOwnerKey,
			eSecurityInheritanceTypes aSecurityInheritanceType,
			int aInheritedFrom,
			int aInheritanceOffset)
		{
			SecurityPermission permission;

			try
			{
				if (Actions.Contains(aActionID))
				{
					permission = (SecurityPermission)Actions[aActionID];
					permission.IsInherited = true;
					permission.OwnerType = aOwnerType;
					permission.OwnerKey = aOwnerKey;
					permission.SecurityInheritanceType = aSecurityInheritanceType;
					permission.SecurityInheritedFrom = aInheritedFrom;
					permission.SecurityInheritanceOffset = aInheritanceOffset;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// The SecurityPermission class contains properties and methods relating to the permission for an action.
	/// </summary>

	[Serializable]
	public class SecurityPermission : ICloneable
	{
		//=======
		// FIELDS
		//=======

		private eSecurityLevel _securityLevel;
		private bool _isInherited;
		private eSecurityOwnerType _ownerType; 
		private int _ownerKey;
		private eSecurityInheritanceTypes _securityInheritanceType;
		private int _securityInheritedFrom;
		private int _securityInheritanceOffset;
		private int _securityInheritanceIndex;	// the index in the _inheritancePath that contains the selected item
		private ArrayList _inheritancePath;
        // Begin Track #5858 - JSmith - Validating store security only
        private eDatabaseSecurityTypes _databaseSecurityType;
        // End Track #5858

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of SecurityPermission class.
		/// </summary>
		/// <param name="aSecurityLevel">
		/// The eSecurityLevel of this action.
		/// </param>

		public SecurityPermission(eSecurityLevel aSecurityLevel)
		{
			_securityLevel = aSecurityLevel;
			_securityInheritanceIndex = -1;
            // Begin Track #5858 - JSmith - Validating store security only
            _databaseSecurityType = eDatabaseSecurityTypes.NotSpecified;
            // End Track #5858

			ClearInheritance();
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets or sets the security level of the action
		/// </summary>

		public eSecurityLevel SecurityLevel
		{
			get
			{
				return _securityLevel;
			}
			set
			{
				_securityLevel = value;
			}
		}

		/// <summary>
		/// Gets or sets a flag identifying if the permission was inherited for the action.
		/// </summary>

		public bool IsInherited
		{
			get
			{
				return _isInherited;
			}
			set
			{
				_isInherited = value;
			}
		}

		/// <summary>
		/// Gets or sets the eSecurityOwnerType identifying where the permission was inherited form.
		/// </summary>
		/// <remarks>
		/// This is group or user
		/// </remarks>

		public eSecurityOwnerType OwnerType
		{
			get
			{
				return _ownerType;
			}
			set
			{
				_ownerType = value;
			}
		}

		/// <summary>
		/// Gets or sets the key of the eSecurityOwnerType identifying where the permission was inherited form.
		/// </summary>
		
		public int OwnerKey
		{
			get
			{
				return _ownerKey;
			}
			set
			{
				_ownerKey = value;
			}
		}

		/// <summary>
		/// Gets or sets the eSecurityInheritanceTypes identifying the type of object where the permission was inherited for the action.
		/// </summary>

		public eSecurityInheritanceTypes SecurityInheritanceType
		{
			get
			{
				return _securityInheritanceType;
			}
			set
			{
				_securityInheritanceType = value;
			}
		}

		/// <summary>
		/// Gets or sets item from where the permission was inherited for the action.
		/// </summary>

		public int SecurityInheritedFrom
		{
			get
			{
				return _securityInheritedFrom;
			}
			set
			{
				_securityInheritedFrom = value;
			}
		}

		/// <summary>
		/// Gets or sets offset from where the permission was inherited for the action.
		/// </summary>

		public int SecurityInheritanceOffset
		{
			get
			{
				return _securityInheritanceOffset;
			}
			set
			{
				_securityInheritanceOffset = value;
			}
		}

		/// <summary>
		/// Gets or sets index in the InheritancePath from where the permission was inherited for the action.
		/// </summary>

		public int SecurityInheritanceIndex
		{
			get
			{
				return _securityInheritanceIndex;
			}
			set
			{
				_securityInheritanceIndex = value;
			}
		}

		/// <summary>
		/// Gets or sets the ArrayList that contains InheritancePathItem objects that contain information
		/// about how the permission was determined.
		/// </summary>

		public ArrayList InheritancePath
		{
			get
			{
				try
				{
					if (_inheritancePath == null)
					{
						_inheritancePath = new ArrayList();
					}
					return _inheritancePath;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
			set
			{
				_inheritancePath = value;
			}
		}

        // Begin Track #5858 - JSmith - Validating store security only
        /// <summary>
		/// Gets or sets type of security from where the permission was inherited for the action.
		/// </summary>

		public eDatabaseSecurityTypes DatabaseSecurityType
		{
			get
			{
				return _databaseSecurityType;
			}
			set
			{
				_databaseSecurityType = value;
			}
		}
        // End Track #5858

		//========
		// METHODS
		//========

		public void ClearInheritance()
		{
			_isInherited = false;
			_ownerType = eSecurityOwnerType.NotSpecified;
			_ownerKey = -1;
			_securityInheritanceType = eSecurityInheritanceTypes.NotSpecified;
			_securityInheritedFrom = -1;
			_securityInheritanceOffset = 0;
		}

		public object Clone()
		{
			SecurityPermission permission;

			try
			{
				permission = (SecurityPermission)this.MemberwiseClone();
				permission.InheritancePath = new ArrayList();

				foreach (InheritancePathItem ipi in this.InheritancePath) // copy inheritance path
				{
					InheritancePathItem clonedInheritancePathItem = (InheritancePathItem)ipi.Clone();
					permission.InheritancePath.Add(clonedInheritancePathItem);
				}
				return permission;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public int SetInheritancePathItem(eSecurityOwnerType aOwnerType, int aOwnerKey, eSecurityInheritanceTypes aSecurityInheritanceType, int aInheritanceItem, eSecurityLevel aSecurityLevel)
		{
			try
			{
				InheritancePathItem ipi = null;
				bool addItem = false;
				int itemIndex = 0;
				// find path entry
				foreach (InheritancePathItem item in InheritancePath)
				{
					if (item.OwnerType == aOwnerType &&
						item.OwnerKey == aOwnerKey &&
						item.InheritanceType == aSecurityInheritanceType &&
						item.InheritanceItem == aInheritanceItem)
					{
						ipi = item;
						break;
					}
					++itemIndex;
				}
				if (ipi == null)
				{
					ipi = new InheritancePathItem();
					addItem = true;
				}
				ipi.OwnerType = aOwnerType;
				ipi.OwnerKey = aOwnerKey;
				ipi.InheritanceType = aSecurityInheritanceType;
				ipi.InheritanceItem = aInheritanceItem;
				ipi.SecurityLevel = aSecurityLevel;
				if (addItem)
				{
					InheritancePath.Add(ipi);
					itemIndex = InheritancePath.Count;
				}

				return itemIndex;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// The InheritancePathItem class identifies the items referenced to determine a permission.
	/// </summary>

	[Serializable]
	public class InheritancePathItem : ICloneable
	{
		//=======
		// FIELDS
		//=======
		private eSecurityOwnerType _ownerType; 
		private int _ownerKey;
		eSecurityInheritanceTypes _inheritanceType;
		int _inheritanceItem;
		eSecurityLevel _securityLevel;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of InheritancePathItem.
		/// </summary>

		public InheritancePathItem()
		{
			 _inheritanceType = eSecurityInheritanceTypes.NotSpecified;
			 _inheritanceItem = Include.NoRID;
			_securityLevel = eSecurityLevel.NotSpecified;
		}


		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets or sets the eSecurityOwnerType identifying where the permission was inherited form.
		/// </summary>
		/// <remarks>
		/// This is group or user
		/// </remarks>

		public eSecurityOwnerType OwnerType
		{
			get
			{
				return _ownerType;
			}
			set
			{
				_ownerType = value;
			}
		}

		/// <summary>
		/// Gets or sets the key of the eSecurityOwnerType identifying where the permission was inherited form.
		/// </summary>
		
		public int OwnerKey
		{
			get
			{
				return _ownerKey;
			}
			set
			{
				_ownerKey = value;
			}
		}

		/// <summary>
		/// Gets or sets the eSecurityInheritanceTypes that identifies they type of item.
		/// </summary>

		public eSecurityInheritanceTypes InheritanceType
		{
			get
			{
				return _inheritanceType;
			}
			set
			{
				_inheritanceType = value;
			}
		}

		/// <summary>
		/// Gets or sets the key of the item.
		/// </summary>

		public int InheritanceItem
		{
			get
			{
				return _inheritanceItem;
			}
			set
			{
				_inheritanceItem = value;
			}
		}

		/// <summary>
		/// Gets or sets the eSecurityLevel of the item.
		/// </summary>

		public eSecurityLevel SecurityLevel
		{
			get
			{
				return _securityLevel;
			}
			set
			{
				_securityLevel = value;
			}
		}
		#region ICloneable Members

		public object Clone()
		{
			InheritancePathItem inheritancePathItem;

			try
			{
				inheritancePathItem = (InheritancePathItem)this.MemberwiseClone();
				return inheritancePathItem;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		#endregion
	}

	/// <summary>
	/// The WorkflowMethodSecurityProfile class identifies the workflow or methods' security profile.
	/// </summary>

	[Serializable]
	public class WorkflowMethodSecurityProfile : SecurityProfile, ICloneable
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of HierarchyNodeSecurityProfile using the given Id.
		/// </summary>
		/// <param name="aKey">
		/// The Id of this profile.
		/// </param>

		public WorkflowMethodSecurityProfile(int aKey)
			: base(aKey)
		{
			
		}


		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.SecurityWorkflowMethod;
			}
		}

		
		//===========
		// METHODS
		//===========
		public object Clone()
		{
			WorkflowMethodSecurityProfile wmsp;

			try
			{
				wmsp = (WorkflowMethodSecurityProfile)this.MemberwiseClone();
				wmsp.CopyFrom(this);
				return wmsp;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

	}

	/// <summary>
	/// The HierarchyNodeSecurityProfile class identifies the hierarchy nodes' security profile.
	/// </summary>

	[Serializable]
	public class HierarchyNodeSecurityProfile : SecurityProfile, ICloneable
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of HierarchyNodeSecurityProfile using the given Id.
		/// </summary>
		/// <param name="aKey">
		/// The Id of this profile.
		/// </param>

		public HierarchyNodeSecurityProfile(int aKey)
			: base(aKey)
		{
			
		}


		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.SecurityHierarchyNode;
			}
		}

		//===========
		// METHODS
		//===========
		public object Clone()
		{
			HierarchyNodeSecurityProfile hnsp;

			try
			{
				hnsp = (HierarchyNodeSecurityProfile)this.MemberwiseClone();
				hnsp.CopyFrom(this);
				return hnsp;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

	}

	/// <summary>
	/// Used to retrieve and update a list of hierarchy node security profiles
	/// </summary>
	[Serializable()]
	public class HierarchyNodeSecurityProfileList : ProfileList
	{
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public HierarchyNodeSecurityProfileList(eProfileType aProfileType)
			: base(aProfileType)
		{
			
		}
	}

	/// <summary>
	/// The VersionSecurityProfile class identifies the versions' security profile.
	/// </summary>

	[Serializable]
	public class VersionSecurityProfile : SecurityProfile, ICloneable
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of VersionSecurityProfile using the given Id.
		/// </summary>
		/// <param name="aKey">
		/// The Id of this profile.
		/// </param>

		public VersionSecurityProfile(int aKey)
			: base(aKey)
		{
			
		}


		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.SecurityVersion;
			}
		}

		//===========
		// METHODS
		//===========
		public object Clone()
		{
			VersionSecurityProfile vsp;

			try
			{
				vsp = (VersionSecurityProfile)this.MemberwiseClone();
				vsp.CopyFrom(this);
				return vsp;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

	}

	/// <summary>
	/// Used to retrieve and update a list of version security profiles
	/// </summary>
	[Serializable()]
	public class VersionSecurityProfileList : ProfileList
	{
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public VersionSecurityProfileList(eProfileType aProfileType)
			: base(aProfileType)
		{
			
		}
	}

	/// <summary>
	/// The FunctionSecurityProfile class identifies the functions' security profile.
	/// </summary>

	[Serializable]
	public class FunctionSecurityProfile : SecurityProfile, ICloneable
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of FunctionSecurityProfile using the given Id.
		/// </summary>
		/// <param name="aKey">
		/// The Id of this profile.
		/// </param>

		public FunctionSecurityProfile(int aKey)
			: base(aKey)
		{

		}


		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.SecurityFunction;
			}
		}

		//===========
		// METHODS
		//===========
		public object Clone()
		{
			FunctionSecurityProfile fsp;

			try
			{
				fsp = (FunctionSecurityProfile)this.MemberwiseClone();
				fsp.CopyFrom(this);
				return fsp;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

	}

    //Begin TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions -unused class
    ///// <summary>
    ///// Used to retrieve and update a list of hierarchy node security profiles
    ///// </summary>
    //[Serializable()]
    //public class FunctionSecurityProfileList : ProfileList
    //{
    //    /// <summary>
    //    /// Used to construct an instance of the class.
    //    /// </summary>
    //    public FunctionSecurityProfileList(eProfileType aProfileType)
    //        : base(aProfileType)
    //    {
			
    //    }
    //}
    //End TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions -unused class
}
