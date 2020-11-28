/*RELEASE 5.3.0.39 10-22-2014 - TT#4116 -stodd - Prohibit VSW headers from being used in Group Allocation/Assortment */
exec SP_MID_UPDATE_TEXT 085125,'"{0}" is a Work Up Buy, VSW, or Multi-header and cannot be added to the Group Allocation.', 250,0,5,null,0	
exec SP_MID_UPDATE_TEXT 085116,'Invalid header for assortment. Either the status is not Recieved in Balance or it is a Multi-header, a VSW header, or a WorkUp Total Buy.', 250,0,5,null,0
GO