alter table "SYSTEM_OPTIONS"
	add constraint "STORE_GROUP_SYSTEM_OPTIONS_FK3" foreign key (
		"DC_CARTON_ROUNDING_SG_RID")
	 references "STORE_GROUP" (
		"SG_RID")  

go
