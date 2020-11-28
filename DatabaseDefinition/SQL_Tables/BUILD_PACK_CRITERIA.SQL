--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv ============================================= 
create table [dbo].[BUILD_PACK_CRITERIA] (
    "BPC_RID"        int identity not null,
	"BPC_NAME"       varchar(50) not null,
	"BPC_COMP_MIN"   int not null,
	"BPC_SIZE_MULT"	 int not null)  

go
