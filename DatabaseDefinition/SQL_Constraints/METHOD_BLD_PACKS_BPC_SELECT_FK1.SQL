/* Add foreign key constraints to table "METHOD_BLD_PACKS_BPC_SELECT".                              */
alter table "METHOD_BLD_PACKS_BPC_SELECT" 
	add constraint "METHOD_BLD_PACKS_BPC_SELECT_FK1" foreign key ("METHOD_RID")
	 references "METHOD" ("METHOD_RID") 
go
