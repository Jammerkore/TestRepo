alter table "METHOD_BLD_PACKS" 
	add constraint "METHOD_BLD_PACKS_FK1" foreign key ("METHOD_RID")
	 references "METHOD" ("METHOD_RID")
go
