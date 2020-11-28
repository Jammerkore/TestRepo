/*RELEASE 5.4.0.27 11-21-2014 -TT#1395-MD -stodd -Adding VSW Headers to a Group produces incorrect messages */
exec SP_MID_UPDATE_TEXT 085004,'Only one {0} or Placeholder row may be selected when adding to an {0}', 250,0,5,null,0
exec SP_MID_UPDATE_TEXT 085115,'Invalid header for {0}. Header already belongs to another assortment or group.', 250,0,5,null,0
exec SP_MID_UPDATE_TEXT 085116,'Invalid header for {0}. Either the status is not Received in Balance or it is a Multi-header, a VSW header, or a WorkUp Total Buy.', 250,0,5,null,0	
go