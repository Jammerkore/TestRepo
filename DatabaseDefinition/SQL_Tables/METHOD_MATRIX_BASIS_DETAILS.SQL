--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================  
create table [dbo].[METHOD_MATRIX_BASIS_DETAILS] ( 
	"METHOD_RID" int not null,
	"SEQ_ID" int not null,
	"FV_RID" int not null,
	"CDR_RID" int not null,
	"WEIGHT" float not null,
	"IS_INCLUDED_IND" char(1) not null)  

go
