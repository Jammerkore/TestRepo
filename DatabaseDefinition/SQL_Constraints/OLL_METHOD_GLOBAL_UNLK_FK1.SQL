alter table "METHOD_GLOBAL_UNLOCK"
	add constraint "OLL_METHOD_GLOBAL_UNLK_FK1" foreign key ("OLL_RID")
	 references "OVERRIDE_LL_MODEL_HEADER" ("OLL_RID") on update no action on delete no action  	 
GO
