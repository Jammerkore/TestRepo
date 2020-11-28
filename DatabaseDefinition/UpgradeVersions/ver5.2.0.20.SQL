/*RELEASE 5.2.0.20 07-23-2013 -TT#739-MD -stodd -Delete Stores -store delete API */

IF .dbo.UDF_ColumnAlreadyExists('STR_LOAD_INFO','STORES_RECOVERED')=0
alter table STR_LOAD_INFO add "STORES_RECOVERED" int null
go
