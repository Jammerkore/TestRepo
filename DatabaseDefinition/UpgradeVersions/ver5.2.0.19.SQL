/*RELEASE 5.2.0.19 07-11-2013 -TT#739-MD -stodd -Delete Stores -store delete API */
IF .dbo.UDF_ColumnAlreadyExists('STR_LOAD_INFO','STORES_DELETED')=0
alter table STR_LOAD_INFO add "STORES_DELETED" int null
go
