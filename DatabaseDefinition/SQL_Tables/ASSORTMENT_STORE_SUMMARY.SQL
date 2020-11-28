--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 10-09-2013 - TT#845-MD -stodd - add OnHand to Assrt Summary
--dv		  03-04-2014 - TT#952-MD -stodd - Group Allocation Matrix
--dv          11/17/2017 - TT#2103-MD - JSmith - Matrix Need % Calc is wrong.  Should be using (Apply To Need *100 )divided by the Apply to Sales.  Currently is using the Basis Sales and not Apply To Sales.
--dv =============================================   
create table [dbo].[ASSORTMENT_STORE_SUMMARY] ( 
	"HDR_RID" int not null,
	"ST_RID" int not null,
	"VARIABLE_NUMBER" int not null,
	"UNITS" int not null,
	"INTRANSIT" int null,
	"PCT_NEED" float null,
	"NEED" int null,
	"VARIABLE_TYPE" int not null default 0,
	"COMMITTED" int null,
	"ONHAND" int null,
	"VSW_ONHAND" int null,
	"PLAN_SALES_UNITS" int not null default 0,
	"PLAN_STOCK_UNITS" int not null default 0
	)  

go
