/*RELEASE 17.01.0.03 5-3-2017 TT#1966-MD - JSmith - DC Fulfillment */
exec SP_MID_UPDATE_TEXT 085125,'"{0}" is a Work Up Buy, VSW, Multi-header, or part of a Master Header and cannot be added to the Group Allocation.', 250,0,5,null,0		/* TT#4116 - stodd - prohibit dropping of VSW header */
exec SP_MID_UPDATE_TEXT 900319,'MasterID',50,1,2,0,0 

GO
