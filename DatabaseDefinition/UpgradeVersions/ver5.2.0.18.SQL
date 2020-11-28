/*RELEASE 5.2.0.18 07-02-2013 -TT#739-MD -stodd -Delete Stores -store delete API */
IF .dbo.UDF_ColumnAlreadyExists('STORES','STORE_DELETE_IND')=0
alter table STORES add "STORE_DELETE_IND" char(1) default '0' not null
go
