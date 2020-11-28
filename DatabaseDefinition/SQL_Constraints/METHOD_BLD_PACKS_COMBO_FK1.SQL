/* Add foreign key constraints to table "METHOD_BLD_PACKS_COMBO".                              */
alter table "METHOD_BLD_PACKS_COMBO" 
	add constraint "METHOD_BLD_PACKS_COMBO_FK1" foreign key ("METHOD_RID")
	 references "METHOD" ("METHOD_RID") 
go 
