using System;
using System.Collections;

namespace MIDRetail.DataCommon
{
	/// <summary>
	/// Abstract class that defines the general traits of a Profile.
	/// </summary>
	/// <remarks>
	/// A Profile is a description of an entity that can define an index on a dimension on a Cube.  The base level Profile must have an Id, which
	/// is a external numeric identifier for the Profile.
	/// </remarks>

	[Serializable]
	abstract public class Profile
	{
		//=======
		// FIELDS
		//=======

		protected int _key;

        private long _instanceID; // TT#488 - MD - Jellis - Group Allocation
		
		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instace of Profile using the given Id.
		/// </summary>
		/// <param name="aKey">
		/// The external numeric identifier for this Profile.
		/// </param>

		public Profile(int aKey)
		{
			_key = aKey;
            _instanceID = DateTime.Now.Ticks;  // TT#488 - MD - Jellis - Group Allocation
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets or sets the Id of the Profile.
		/// </summary>

		public int Key
		{
			get
			{
				return _key;
			}
			set
			{
				_key = value;
			}
		}
        // begin TT#488 - MD - Jellis - Group Allocation
        public long InstanceID
        {
            get { return _instanceID; }
        }
		/// <summary>
		/// Abstract.  Gets the ProfileType of the Profile.
		/// </summary>

		abstract public eProfileType ProfileType
		{
			get;
		}

        public bool isFound
        {
            get { return (_key != -1); }
        }

		//========
		// METHODS
		//========

		override public bool Equals(object obj)
		{
            try
            {
                if (obj is Profile && ((Profile)obj).Key == _key)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (InvalidCastException)
            {
                return false;
            }
		}

		override public int GetHashCode()
		{
			return this.Key;
		}
	}
	
	/// <summary>
	/// Defines a list of Profiles.
	/// </summary>
	/// <remarks>
	/// A ProfileList is a description of an entity that can define a dimension on a Cube.  A ProfileList also contains functionality to support filtering.
	/// </remarks>

	[Serializable]
	public class ProfileList : ICloneable
	{
		//=======
		// FIELDS
		//=======

		private eProfileType _componentType;
		private ArrayList arraylist;
		private Hashtable hashtable;
		// begin MID Track 4708 Size Performance Slow
        private Profile _lastProfile;
		// end MID Track 4708 Size Performance Slow
        private ProfileListChangeEvent _profileListChangeEvent; // TT#935 - MD - Jellis - Group ALlocation Infrastructure built wrong
		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of ProfileList with an empty array of Profiles.
		/// </summary>
		/// <param name="aProfileType">
		/// The eProfileType that defines the component type of this ProfileList.
		/// </param>

		public ProfileList(eProfileType aProfileType)
		{
			try
			{
				_componentType = aProfileType;
				arraylist = new ArrayList();
				hashtable = new Hashtable();
				_lastProfile = null; // MID Track 4708 Size Performance Slow
                _profileListChangeEvent = new ProfileListChangeEvent(); // TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Creates a new instance of ProfileList using the given eProfileType and array of Profiles.
		/// </summary>
		/// <param name="aProfileType">
		/// The eProfileType that defines the component type of this ProfileList.
		/// </param>
		/// <param name="aProfileArray">
		/// An array of Profiles that are included in this ProfileList.
		/// </param>

		public ProfileList(eProfileType aProfileType, System.Collections.ArrayList aProfileArray)
		{
			try
			{
				_componentType = aProfileType;
				arraylist = new ArrayList(aProfileArray);
				hashtable = new Hashtable();
                _profileListChangeEvent = new ProfileListChangeEvent(); // TT#990 - MD - stodd - null ref in OTS forecast

				foreach(Profile profile in aProfileArray)
				{
					hashtable.Add(profile.Key, profile);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
            // begin TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
            finally
            {
                _profileListChangeEvent.ChangeProfileList(this, ProfileType, eChangeType.add);
            }
            // end TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group

		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Indexer to access the Profiles in this ProfileList
		/// </summary>

		public Profile this[int aIndex]
		{
			get
			{
				try
				{
					return (Profile)arraylist[aIndex];
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets the eProfileType of this ProfileList.
		/// </summary>

		public eProfileType ProfileType
		{
			get
			{
				return _componentType;
			}
		}

		/// <summary>
		/// Gets the arraylist in the ProfileList.
		/// </summary>

		public ArrayList ArrayList
		{
			get
			{
				return arraylist;
			}
		}

		/// <summary>
		/// Gets the count of the items in the ProfileList.
		/// </summary>

		public int Count
		{
			get
			{
				return arraylist.Count;
			}
		}

		/// <summary>
		/// Gets the minimum key value of the items in the ProfileList.
		/// </summary>

		public int MinValue
		{
			get
			{
				int minValue;

				try
				{
					minValue = 0;

					if (arraylist.Count > 0)
					{
						minValue = int.MaxValue;

						foreach (Profile profile in arraylist)
						{
							if (profile.Key < minValue)
							{
								minValue = profile.Key;
							}
						}
					}

					return minValue;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets the maximum key value of the items in the ProfileList.
		/// </summary>

		public int MaxValue
		{
			get
			{
				int maxValue;

				try
				{
					maxValue = 0;

					if (arraylist.Count > 0)
					{
						maxValue = int.MinValue;

						foreach (Profile profile in arraylist)
						{
							if (profile.Key > maxValue)
							{
								maxValue = profile.Key;
							}
						}
					}

					return maxValue;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

        public ProfileListChangeEvent ProfileListChangeEvent
        {
            get { return _profileListChangeEvent; }
        }
		//========
		// METHODS
		//========

		/// <summary>
		/// Returns a clone of this object.
		/// </summary>
		/// <returns>
		/// A Clone of the object.
		/// </returns>

		public object Clone()
		{
			ProfileList newList;

			try
			{
				newList = new ProfileList(_componentType);
				newList.arraylist = (System.Collections.ArrayList)arraylist.Clone();
				newList.hashtable = (System.Collections.Hashtable)hashtable.Clone();

				return newList;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Adds a profile to the ProfileList.
		/// </summary>

		public void Add(Profile profile)
		{
			try
			{
                // Begin TT#2361 - JSmith - Error when saving changes to show details of a header
                //arraylist.Add(profile);
                //hashtable.Add(profile.Key, profile);
                if (!hashtable.ContainsKey(profile.Key))
                {
                    arraylist.Add(profile);
                    hashtable.Add(profile.Key, profile);
                }
                // End TT#2361
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
            // begin TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong
            finally
            {
                _profileListChangeEvent.ChangeProfileList(this, ProfileType, eChangeType.add);
            }
            // end TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong

		}

		// BEGIN Issue 4858 stodd 10.30.2007 forecast methods security
		/// <summary>
		/// Inserts a profile to the ProfileList.ArrayList at specified index.
		/// </summary>

		public void Insert(int idx, Profile profile)
		{
			try
			{
				arraylist.Insert(idx, profile);
				hashtable.Add(profile.Key, profile);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
            // begin TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong
            finally
            {
                _profileListChangeEvent.ChangeProfileList(this, ProfileType, eChangeType.add);
            }
            // end TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong

		}
		// End Issue 4858 stodd 10.30.2007 forecast methods security

		/// <summary>
		/// Rebuilds the hash table (use if keys have changed)
		/// </summary>

		public void HashRebuild()
		{
			try
			{
				hashtable.Clear();

				foreach (Profile profile in arraylist)
				{
					hashtable.Add(profile.Key, profile);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Adds a range of profiles to the ProfileList.
		/// </summary>

		public void AddRange(ProfileList aProfileList)
		{
            try
            {
                //arraylist.AddRange(aProfileList.arraylist);

                foreach (Profile profile in aProfileList.arraylist)
                {
                    arraylist.Add(profile);
                    hashtable.Add(profile.Key, profile);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
            // begin TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong
            finally
            {
                _profileListChangeEvent.ChangeProfileList(this, ProfileType, eChangeType.add);
            }
            // end TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong
		}

		/// <summary>
		/// Adds a range of profiles to the ProfileList.
		/// </summary>

		public void AddRange(ArrayList aArrayList)
		{
			try
			{
				//arraylist.AddRange(aArrayList);

				foreach (Profile profile in aArrayList)
				{
					arraylist.Add(profile);
					hashtable.Add(profile.Key, profile);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
            // begin TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong
            finally
            {
                _profileListChangeEvent.ChangeProfileList(this, ProfileType, eChangeType.add);
            }
            // end TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong

		}

		/// <summary>
		/// Removes a profile from the ProfileList.
		/// </summary>

		public void Remove(Profile aProfile)
		{
			try
			{
				arraylist.Remove(aProfile);
				hashtable.Remove(aProfile.Key);
				_lastProfile = null; // MID Track 4708 Size Performance Slow
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
            // begin TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong
            finally
            {
                _profileListChangeEvent.ChangeProfileList(this, ProfileType, eChangeType.delete);
            }
            // end TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong

		}

		/// <summary>
		/// Removes a profile from the ProfileList at a specific index.
		/// </summary>

		public void RemoveAt(int aIndex)
		{
			Profile profile;

			try
			{
				profile = (Profile)arraylist[aIndex];
				hashtable.Remove(profile.Key);
				arraylist.RemoveAt(aIndex);
				_lastProfile = null; // MID Track 4708 Size Performance Slow
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
            // begin TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong
            finally
            {
                _profileListChangeEvent.ChangeProfileList(this, ProfileType, eChangeType.delete);
            }
            // end TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong

		}

		/// <summary>
		/// Clears the profiles from the ProfileList.
		/// </summary>

		public void Clear()
		{
			try
			{
				arraylist.Clear();
				hashtable.Clear();
				_lastProfile = null; // MID Track 4708 Size Performance Slow
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
            // begin TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong
            finally
            {
                _profileListChangeEvent.ChangeProfileList(this, ProfileType, eChangeType.delete);
            }
            // end TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong

		}

		/// <summary>
		/// Determines if the ProfileList contains a Key.
		/// </summary>	
		/// <param name="aKey">
		/// The external numeric identifier for this Profile.
		/// </param>

		public bool Contains(object aKey)
		{
			try
			{
				return arraylist.Contains(aKey);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Determines if the ProfileList contains a key.
		/// </summary>	
		/// <param name="aKey">
		/// The external numeric identifier for this Profile.
		/// </param>

		public bool Contains(int aKey)
		{
			try
			{
				return hashtable.Contains(aKey);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Locates and returns a profile in the ProfileList.
		/// </summary>
		/// <param name="aKey">
		/// The external numeric identifier for this Profile.
		/// </param>	
		
		public Profile FindKey(int aKey)
		{
			try
			{
				// begin MID Track 4708 Size Performance Slow
				//return (Profile)hashtable[aKey];
				if (_lastProfile == null
					|| _lastProfile.Key != aKey)
				{
					_lastProfile = (Profile)hashtable[aKey];
				}
				return _lastProfile;
				// end MID Track 4708 Size Performance Slow
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Updates a profile in the ProfileList.
		/// </summary>
		/// <param name="aProfile">
		/// The Profile to be updated to the ProfileList.
		/// </param>	
		
		public void Update(Profile aProfile)
		{
			Profile profile;
			int index;

			try
			{
				profile = (Profile)hashtable[aProfile.Key];

				if (profile != null)
				{
					index = arraylist.IndexOf(profile);
					arraylist.Remove(profile);
					arraylist.Insert(index, aProfile);
					hashtable[aProfile.Key] = aProfile;
					_lastProfile = aProfile; // MID Track 4708 Size Performance Slow;
				}
				else
				{
					throw new Exception("Attempting to update Profile in ProfileList that does not exist");
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
            // begin TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong
            finally
            {
                _profileListChangeEvent.ChangeProfileList(this, ProfileType, eChangeType.update);
            }
            // end TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong

		}

		public virtual IEnumerator GetEnumerator()
		{
			try
			{
				return new ProfileEnumerator(this);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private class ProfileEnumerator : IEnumerator
		{
			//=======
			// FIELDS
			//=======

			private int _position;
			private ProfileList _profList;
		
			//=============
			// CONSTRUCTORS
			//=============

			public ProfileEnumerator(ProfileList aProfileList)
			{
				_position = -1;
				_profList = aProfileList;
			}

			//===========
			// PROPERTIES
			//===========

			public object Current
			{
				get
				{
					return _profList.arraylist[_position];
				}
			}

			//========
			// METHODS
			//========

			public void Reset()
			{
				_position = -1;
			}

			public bool MoveNext()
			{
				if (_position < _profList.arraylist.Count - 1)
				{
					_position++;
					return true;
				}
				else
				{
					return false;
				}
			}
		}
	}

    // begin TT#935 - Group Allocation Infrastructure Built Wrong
    #region ProfileListChangeEvent
    [Serializable]
    public class ProfileListChangeEvent
    {
        public delegate void ProfileListChangeEventHandler(object source, ProfileListChangeEventArgs e);
        public event ProfileListChangeEventHandler OnProfileListChangeHandler;

        public void ChangeProfileList(object source, eProfileType aProfileType, eChangeType aChangeType)
        {
            ProfileListChangeEventArgs ea;
            // fire the event if handler is defined
            if (OnProfileListChangeHandler != null)
            {
                ea = new ProfileListChangeEventArgs(aProfileType, aChangeType);
                OnProfileListChangeHandler(source, ea);
            }
            return;
        }
    }

    [Serializable]
    public class ProfileListChangeEventArgs : EventArgs
    {
        private eProfileType _profileType;
        private eChangeType _changeType;

        public ProfileListChangeEventArgs(eProfileType aProfileType)
        {
            _profileType = aProfileType;
            _changeType = eChangeType.none;
        }

        public ProfileListChangeEventArgs(eProfileType aProfileType, eChangeType aChangeType)
        {
            _profileType = aProfileType;
            _changeType = aChangeType;
        }

        public eProfileType ProfileType
        {
            get { return _profileType; }
        }

        public eChangeType ChangeType
        {
            get { return _changeType; }
            set { _changeType = value; }
        }
    }

    #endregion 
    // end TT#935 - Group Allocation Infrastructure Built Wrong
}
