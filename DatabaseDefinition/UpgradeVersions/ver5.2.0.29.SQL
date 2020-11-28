/*RELEASE 5.2.0.29 08-10-2013 -TT#739-MD -stodd -Delete Stores - store delete in progress */
IF .dbo.UDF_ColumnAlreadyExists('STORE_REMOVAL_ANALYSIS', 'COMPLETED')=0
BEGIN
	alter table STORE_REMOVAL_ANALYSIS add "COMPLETED" [bit] default 0 NULL
END
go
