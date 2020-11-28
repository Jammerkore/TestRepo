/*RELEASE 5.2.0.30 08-12-2013 -TT#739-MD -stodd -Delete Stores - store delete in progress */
IF .dbo.UDF_ColumnAlreadyExists('STORE_REMOVAL_ANALYSIS', 'IN_PROGRESS')=0
BEGIN
	alter table STORE_REMOVAL_ANALYSIS add "IN_PROGRESS" [bit]  default 0 NULL
END
go

IF .dbo.UDF_ColumnAlreadyExists('STORE_REMOVAL_ANALYSIS', 'ANALYSIS_DATE')=0
BEGIN
	alter table STORE_REMOVAL_ANALYSIS add [ANALYSIS_DATE] [datetime] NULL
END
go
