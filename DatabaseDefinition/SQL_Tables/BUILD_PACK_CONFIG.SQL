--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv ============================================= 
create table [dbo].[BUILD_PACK_CONFIG] (
    "BPC_PACK_RID"         int identity not null,
	"BPC_COMBO_RID"        int not null,
	"BPC_PACK_MULT"        int not null,
	"BPC_COMBO_MAX_PACKS"  int not null)  

go
