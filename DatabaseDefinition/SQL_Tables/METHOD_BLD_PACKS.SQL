--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	This table is a sub-table of METHOD.  It describes the Build Packs Method parameters used to generate packs for a Work Up Buy.
--dv Associated tables:  METHOD, METHOD_BLD_PACKS_COMBO, METHOD_BLD_PACKS_COMBO_PATTERN, METHOD_BLD_PACKS_COMBO_PATTERN_SZRUN, METHOD_BLD_PACK_BPC_SELECT
--dv Fields: BPC_RID:  this is an optional field that identifies standard BUILD_PACK_CRITERIA associated with this method instance.
--dv Fields: PATTERN_COMP_MIN:  mutually exclusive with BPC_RID.  When present, indicates the minimum order for any pack or total bulk in a Work Up Buy.
--dv Fields: PATTERN_SIZE_MULTIPLE: mutually exclusive with BPC_RID.  When present, indicates the multiple in which bulk sizes must be ordered.
--dv Fields: SIZE_GROUP_RID: mutually exclusive with SIZE_CURVE_GROUP_RID.  When present, identifies the valid sizes for the Work Up Buy.
--dv Fields: SIZE_CURVE_GROUP_RID: mutually exclusive with SIZE_GROUP_RID.  When present, identifies the valid sizes for the Work Up Buy.
--dv Fields: RESERVE_TOTAL: identifies the number of units to hold in reserve on the Work Up Buy.
--dv Fields: RESERVE_TOTAL_PERCENT_IND: "Y" indicates RESERVE_TOTAL is a percent; "N" or null indicates RESERVE_TOTAL is units
--dv Fields: RESERVE_BULK: identifies the portion of RESERVE_TOTAL to hold in reserve in BULK on the Work Up Buy
--dv Fields: RESERVE_BULK_PERCENT_IND: "Y" indicates RESERVE_BULK is a percentage of RESERVE_TOTAL. "N" or null indicates RESERVE_BULK is units.
--dv Fields: RESERVE_PACKS: identifies the portion of RESERVE_TOTAL to hold in reserve in PACKs as UNITSon the Work Up Buy
--dv Fields: RESERVE_PACKS_PERCENT_IND: "Y" indicates RESERVE_PACKS is a percentage of RESERVE_TOTAL. "N" or null indicates RESERVE_PACKS is units.
--dv Fields: AVG_PACK_DEV_TOLERANCE: the average error a given pack may introduce to a store's need.
--dv Fields: MAX_PACK_NEED_TOLERANCE: the maximum error that may be introduced to a given store's need.
--dv History: TT#669 Build Packs Variance Enhancement
--dv History: TT#744 -JEllis -Use Orig Pack Fit Logic; Option No Bulk
--dv =============================================  
create table [dbo].[METHOD_BLD_PACKS] ( 
	"METHOD_RID" int not null,
	"BPC_RID" int null,
	"PATTERN_COMP_MIN" int null,
	"PATTERN_SIZE_MULTIPLE" int null,
	"SIZE_GROUP_RID" int null,
	"SIZE_CURVE_GROUP_RID" int null,
	"RESERVE_TOTAL" float null,
	"RESERVE_TOTAL_PERCENT_IND" char(1) null,
    "RESERVE_BULK" float null,
	"RESERVE_BULK_PERCENT_IND" char(1) null,
    "RESERVE_PACKS" float null,
    "RESERVE_PACKS_PERCENT_IND" char(1) null,
	"AVG_PACK_DEV_TOLERANCE" float null,
	"MAX_PACK_NEED_TOLERANCE" float null,
    "DEPLETE_RESERVE_SELECTED" char(1) null,
    "INCREASE_BUY_SELECTED" char(1) null,
    "INCREASE_BUY_PCT"  float null, 
    "REMOVE_BULK_IND" char(1) null) 
go
