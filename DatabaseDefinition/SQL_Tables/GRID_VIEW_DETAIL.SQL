--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv ============================================= 
create table [dbo].[GRID_VIEW_DETAIL] ( 
	"VIEW_RID" int not null,
	"BAND_KEY" varchar(50) not null,
	"COLUMN_KEY" varchar(50) not null,
	"VISIBLE_POSITION" int not null,
	"IS_HIDDEN" char(1) not null,
	"IS_GROUPBY_COL" char(1) not null,
	"SORT_DIRECTION" int not null,
	"SORT_SEQUENCE" int null,
	"WIDTH" int null,
	"COLUMN_TYPE" VARCHAR(1) NOT NULL DEFAULT 'B',
	"HCG_RID" INT NULL)  

go
