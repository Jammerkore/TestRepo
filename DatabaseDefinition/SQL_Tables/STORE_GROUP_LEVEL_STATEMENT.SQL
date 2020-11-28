--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================   
create table [dbo].[STORE_GROUP_LEVEL_STATEMENT] ( 
	"SGL_RID" int not null,
	"SGLS_STATEMENT_SEQ" int not null,
	"SGLS_STATEMENT" varchar(250) not null,
	"SGLS_CHAR_IND" char(1) null,
	"SGLS_CHAR_ID" int null,
	"SGLS_SQL_OPERATOR" int null,
	"SGLS_ENG_SQL" varchar(250) null,
	"SGLS_DT" int null,
	"SGLS_VALUE" varchar(250) null,
	"SGLS_PREFIX" varchar(50) null,
	"SGLS_SUFFIX" varchar(50) null)  

go
