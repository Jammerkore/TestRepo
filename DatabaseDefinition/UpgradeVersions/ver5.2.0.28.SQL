/*RELEASE 5.2.0.28 08-09-2013 -TT#739-MD -stodd -Delete Stores - store delete in progress */
IF .dbo.UDF_ColumnAlreadyExists('SYSTEM_OPTIONS', 'STORE_DELETE_IN_PROGRESS_IND')=0
BEGIN
	alter table SYSTEM_OPTIONS add "STORE_DELETE_IN_PROGRESS_IND" char(1) default '0' null
END
go
