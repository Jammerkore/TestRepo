alter table "METHOD_BLD_PACKS" 
	add constraint "METHOD_BLD_PACKS_FK4" foreign key ("SIZE_CURVE_GROUP_RID")
	 references "SIZE_CURVE_GROUP" ("SIZE_CURVE_GROUP_RID") 
go 
