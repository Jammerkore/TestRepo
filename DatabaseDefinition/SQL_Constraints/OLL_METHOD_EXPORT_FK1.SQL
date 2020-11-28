alter table "METHOD_EXPORT"
	add constraint "OLL_METHOD_EXPORT_FK1" foreign key ("OLL_RID")
	 references "OVERRIDE_LL_MODEL_HEADER" ("OLL_RID") on update no action on delete no action  	 
GO
