/* Add foreign key constraints to table "METHOD_BLD_PACKS_COMBO_PATTERN".                              */
alter table "METHOD_BLD_PACKS_COMBO_PATTERN" 
	add constraint "METHOD_BLD_PACKS_COMBO_PATTERN_FK1" foreign key ("METHOD_RID")
	 references "METHOD" ("METHOD_RID") 
go
