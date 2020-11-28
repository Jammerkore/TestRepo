alter table "METHOD_BLD_PACKS" 
	add constraint "METHOD_BLD_PACKS_FK2" foreign key ("BPC_RID")
	 references "BUILD_PACK_CRITERIA" ("BPC_RID") 
go
