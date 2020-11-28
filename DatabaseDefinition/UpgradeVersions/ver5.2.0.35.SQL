/*RELEASE 5.2.0.35 08-28-2013 -TT#739-MD -stodd -Delete Stores - purge store sets */
IF .dbo.UDF_ColumnAlreadyExists('PURGE_INFO', 'EMPTY_ATTRIBUTE_SETS')=0
BEGIN
	alter table PURGE_INFO add "EMPTY_ATTRIBUTE_SETS" int  null
END
go
