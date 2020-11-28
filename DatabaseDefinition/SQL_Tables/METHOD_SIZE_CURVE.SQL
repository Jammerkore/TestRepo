--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: TT#1076 -JScott -Size Curves by Set
--dv History: TT#155 -JScott -Add Size Curve info to Node Properties
--dv History: TT#2079 -DOConnell -Size - Minimum % Tolerance Calculation Issue
--dv =============================================  
create table [dbo].[METHOD_SIZE_CURVE] ( 
	"METHOD_RID" int not null,
	"SIZE_GROUP_RID" int null,
	"SIZE_CURVES_BY_TYPE" int not null,
	"SIZE_CURVES_BY_SG_RID" int null,
	"MERCH_BASIS_EQUAL_WEIGHT_IND" int not null,
	"CURVE_BASIS_EQUAL_WEIGHT_IND" int not null,
	"APPLY_LOST_SALES_IND" int not null,  
	"MINIMUM_AVERAGE" float null,
	"SALES_TOLERANCE" float null,
	"INDEX_UNITS_TYPE" int null,
	"MIN_TOLERANCE" float null,
	"MAX_TOLERANCE" float null,
	"APPLY_MIN_TO_ZERO_TOLERANCE_IND" char null) 

go
