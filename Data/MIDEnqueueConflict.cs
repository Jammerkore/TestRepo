using System;
using System.Globalization;
using System.Data;
using System.Text;

using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
    /// <summary>
    /// Identifies an enqueue conflict
    /// </summary>
    [Serializable]
    public struct MIDEnqueueConflict
    {
        private eLockType _lockType;
        //private int _ridInConflict;  // TT#467 Enq Conflict Except During Size Hist Load
        string[] _keyLabel;            // TT#467 Enq Conflict Except During Size Hist Load
        int[] _keyInConflict;          // TT#467 Enq Conflict Except During Size Hist Load
        private int _ownerUserID;
        private int _ownerThreadID;
        /// <summary>
        /// Creates an instance of the EnqueueConflict structure
        /// </summary>
        /// <param name="aLockType">Lock Type with the conflict</param>
        /// <param name="aKeyLabel">Label for each of the key components</param>
        /// <param name="aKey_InConflict">Key with the conflict</param>
        /// <param name="aOwnedByUserID">Owner of the enqueued resource</param>
        /// <param name="aOwnedInThreadID">Thread owning the enqueue</param>
        public MIDEnqueueConflict(int aLockType, string[] aKeyLabel, int[] aKey_InConflict, int aOwnedByUserID, int aOwnedInThreadID) // TT#467 Enq Conflict Except During Size Hist Load
        {
            _lockType = (eLockType)aLockType;
            _keyLabel = aKeyLabel; // TT#467 Enq Conflict Except During Size Hist Load
            _keyInConflict = aKey_InConflict; // TT#467 Enq Conflict Except During Size Hist Load
            _ownerUserID = aOwnedByUserID;
            _ownerThreadID = aOwnedInThreadID;
        }
        /// <summary>
        /// Gets the lock type of the conflict
        /// </summary>
        public eLockType LockType
        {
            get { return _lockType; }
        }
        // begin TT#467 Enq Conflict Except During Size Hist Load
        /// <summary>
        /// Gets the Key labels for keys in conflict
        /// </summary>
        public string[] KeyLabel
        {
            get 
            {
                string unknownLabel = "unknown key";
                string[] keyLabel = new string[_keyInConflict.Length];
                if (_keyLabel == null)
                {
                    _keyLabel = new string[1];
                    _keyLabel[0] = "unknown key";
                }
                
                for (int i = 0; i < _keyInConflict.Length; i++)
                {
                    if (i < _keyLabel.Length)
                    {
                        keyLabel[i] = _keyLabel[i];
                    }
                    else
                    {
                        keyLabel[i] = unknownLabel;
                    }
                }
                return keyLabel; 
            }
        }
        /// <summary>
        /// Gets the RID in conflict
        /// </summary>
        public int[] Key_InConflict
        {
            get { return _keyInConflict; }
        }
        // end TT#467 Enq Conflict Except During Size Hist Load
        /// <summary>
        /// Gets the User ID of the owner of the enqueued resource
        /// </summary>
        public int OwnedByUserID
        {
            get { return _ownerUserID; }
        }
        /// <summary>
        /// Gets the Thread owning the enqueue (ie the thread that initiated the enqueue)
        /// </summary>
        public int OwnerThreadID
        {
            get { return _ownerThreadID; }
        }
    }
}