/* Add foreign key constraints to table "METHOD_SPREAD_FORECAST".                             */
alter table "METHOD_SPREAD_FORECAST"
	add constraint "MTHD_MTHD_SPRD_FOR_FK1" foreign key (
		"METHOD_RID")
	 references "METHOD" (
		"METHOD_RID")  

go
