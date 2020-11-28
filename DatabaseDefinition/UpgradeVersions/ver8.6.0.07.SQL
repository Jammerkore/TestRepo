/*RELEASE 8.6.0.07 2-2-2017 TT#5189 - JSmith – Audit Message - references stores with allocation units when not units are allocated to the stores */
exec SP_MID_UPDATE_TEXT 060098, 'Store has existing Allocation units or is the last store referencing the VSW ID. The VSW ID cannot be removed.',250,0,6,null,0  /* TT#4685 - JSmith – clear NODE_IMO table when VSW ID is removed from store */

GO
