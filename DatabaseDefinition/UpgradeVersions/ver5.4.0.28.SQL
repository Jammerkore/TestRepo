/*RELEASE 5.4.0.28 11-21-2014 -TT#1398-MD - STodd - clean up allocation action list */
exec SP_MID_UPDATE_TEXT 802111,'Need',30,10,null,null,20
exec SP_MID_UPDATE_TEXT 802112,'Size Proportional',30,10,null,null,35
exec SP_MID_UPDATE_TEXT 802113,'Balance Proportional',30,10,null,null,25
exec SP_MID_UPDATE_TEXT 802114,'Balance to Reserve',30,10,null,null,30
exec SP_MID_UPDATE_TEXT 802115,'Balance Size Proportional',30,10,null,null,45
exec SP_MID_UPDATE_TEXT 802116,'Balance Size to Reserve',30,10,null,null,50
exec SP_MID_UPDATE_TEXT 802117,'Charge Intransit',30,10,null,null,70
exec SP_MID_UPDATE_TEXT 802118,'Release',30,10,null,null,75
exec SP_MID_UPDATE_TEXT 802119,'Reset',30,10,null,null,80
exec SP_MID_UPDATE_TEXT 802120,'Cancel Size Intransit',30,10,null,null,85
exec SP_MID_UPDATE_TEXT 802121,'Cancel Intransit',30,10,null,null,90
exec SP_MID_UPDATE_TEXT 802122,'Cancel Size Allocation',30,10,null,null,95
exec SP_MID_UPDATE_TEXT 802123,'Cancel Allocation',30,10,null,null,100
exec SP_MID_UPDATE_TEXT 802124,'Delete Header',30,10,null,null,0
exec SP_MID_UPDATE_TEXT 802125,'Cancel Rules',30,10,null,null,0
exec SP_MID_UPDATE_TEXT 802126,'Cancel Detail Pack Allocation',30,10,null,null,120
exec SP_MID_UPDATE_TEXT 802127,'Charge Size Intransit',30,10,null,null,0
exec SP_MID_UPDATE_TEXT 802128,'Forecast Spread',30,10,null,null,0
exec SP_MID_UPDATE_TEXT 802129,'Copy Chain Forecast',30,10,null,null,0
exec SP_MID_UPDATE_TEXT 802130,'Copy Store Forecast',30,10,null,null,0
exec SP_MID_UPDATE_TEXT 802131,'General Assortment',30,10,null,null,0  -- assorment
exec SP_MID_UPDATE_TEXT 802132,'Modify Sales',30,10,null,null,0
exec SP_MID_UPDATE_TEXT 802133,'Rebuild Intransit',30,10,null,null,0 /* TT#1137 (MID Track 4351) Rebuild Intransit Utility */
exec SP_MID_UPDATE_TEXT 802134,'Remove API Workflow',30,10,null,null,105
exec SP_MID_UPDATE_TEXT 802135,'Apply API Workflow',30,10,null,null,110
exec SP_MID_UPDATE_TEXT 802136,'Export',30,10,null,null,0
exec SP_MID_UPDATE_TEXT 802137,'Global Unlock',30,10,null,null,0
exec SP_MID_UPDATE_TEXT 802138,'Global Lock',30,10,null,null,0
exec SP_MID_UPDATE_TEXT 802139,'Rollup',30,10,null,null,0
exec SP_MID_UPDATE_TEXT 802140,'Size Curve',30,10,null,null,0
exec SP_MID_UPDATE_TEXT 802141,'Build Packs',30,10,null,null,0 

exec SP_MID_UPDATE_TEXT 802142,'Reapply Total Allocation',50,19,null,null,115 /* TT#785 - Header Load Interfacing a transaction  trying to Modify a WUB header with a PO type */
exec SP_MID_UPDATE_TEXT 802143,'Balance Size With Contraints',30,10,null,null,55  /* TT#843 – new Size Balance by Constraints */
exec SP_MID_UPDATE_TEXT 802144,'Balance Size Bilaterally',30,10,null,null,60  /* TT#794 – Size Balance Action for Wet Seal */
exec SP_MID_UPDATE_TEXT 802145,'Size Proportional With Contraints',50,19,null,null,40  /* TT#1391 - Jellis - Balance Size Constraints other options*/
exec SP_MID_UPDATE_TEXT 802146,'Group Allocation',30,10,null,null,0	/* TT#708-MD - Stodd - Group Allocation Prototype. */
exec SP_MID_UPDATE_TEXT 802147,'Balance to VSW',30,10,null,null,65		/* TT#1334-MD - Stodd - Balance to VSW */
GO

/* assortment actions */
exec SP_MID_UPDATE_TEXT 802150,'Redo',30,62,null,null,50
exec SP_MID_UPDATE_TEXT 802153,'Cancel Assortment',30,62,null,null,60
exec SP_MID_UPDATE_TEXT 802154,'Spread Average',30,62,null,null,20
exec SP_MID_UPDATE_TEXT 802155,'Index to Average',30,62,null,null,30
exec SP_MID_UPDATE_TEXT 802156,'Create Placeholders',30,62,null,null,10
exec SP_MID_UPDATE_TEXT 802158,'Balance Assortment',30,62,null,null,40
Go
