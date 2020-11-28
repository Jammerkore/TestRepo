/* Add foreign key constraints to table "METHOD_DC_CARTON_ROUNDING" */
alter table "METHOD_DC_CARTON_ROUNDING"
	add constraint "METHOD_DC_CARTON_ROUNDING_FK1" foreign key (
		"METHOD_RID")
	 references "METHOD" (
		"METHOD_RID")  

go
