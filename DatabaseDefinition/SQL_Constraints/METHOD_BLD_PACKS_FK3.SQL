alter table "METHOD_BLD_PACKS" 
	add constraint "METHOD_BLD_PACKS_FK3" foreign key ("SIZE_GROUP_RID")
	 references "SIZE_GROUP" ("SIZE_GROUP_RID") 
go
