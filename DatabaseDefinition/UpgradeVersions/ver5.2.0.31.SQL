/*RELEASE 5.2.0.31 08-13-2013 -TT#739-MD -stodd -Delete Stores - store delete in progress */
IF .dbo.UDF_ColumnAlreadyExists('SYSTEM_OPTIONS', 'CONSTRAINTS_DROPPED')=0
BEGIN
	alter table SYSTEM_OPTIONS add "CONSTRAINTS_DROPPED" char(1) default '0' null
END
go
