alter table "METHOD_OVERRIDE_STORE_GRADES_VALUES"
	add constraint "MT_OV_MT_OV_STR_GRDS_VAL_FK1" foreign key (
		"METHOD_RID")
	 references "METHOD_OVERRIDE" (
		"METHOD_RID")  

go
