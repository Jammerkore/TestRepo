alter table "METHOD"
	add constraint "OLL_MODEL_HDR_METHOD_FK1" foreign key (
		"CUSTOM_OLL_RID")
	 references "OVERRIDE_LL_MODEL_HEADER" (
		"OLL_RID")  
	 
go
