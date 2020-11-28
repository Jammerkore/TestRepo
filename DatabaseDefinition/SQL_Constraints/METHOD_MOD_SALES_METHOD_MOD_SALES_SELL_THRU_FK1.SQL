/* Add foreign key constraints to table "METHOD_MOD_SALES_SELL_THRU".                         */
alter table "METHOD_MOD_SALES_SELL_THRU"
	add constraint "METHOD_MOD_SALES_METHOD_MOD_SALES_SELL_THRU_FK1" foreign key (
		"METHOD_RID")
	 references "METHOD_MOD_SALES" (
		"METHOD_RID")  

go
