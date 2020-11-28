/*RELEASE 5.4.0.30 11-25-2014 -TT#1398-MD - STodd - clean up allocation action list */
exec SP_MID_UPDATE_TEXT 080774,'Balance to VSW for header [{0}] started',250,0,2,null,0
exec SP_MID_UPDATE_TEXT 080775,'it belongs to a Group Allocation or an Assortment',250,0,6,null,0
exec SP_MID_UPDATE_TEXT 080776,'Action [{0}] for component [{2}] on header [{1}] failed. (Source [{3}/{4}])',250,0,2,null,0           
exec SP_MID_UPDATE_TEXT 080777,'Action [{0}] for component [{2}] on header [{1}] was successful. (Source [{3}/{4}])',250,0,2,null,0   
exec SP_MID_UPDATE_TEXT 080778,'Balance to VSW for header [{0}] ended',250,0,2,null,0		
exec SP_MID_UPDATE_TEXT 080779,'Balance to VSW changed Store Qty on header [{0}] for component [{1}] because Store Qty Allocated was greater than receipts',250,0,2,null,0     
Go
