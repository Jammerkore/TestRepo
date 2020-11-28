/* Add foreign key constraints to table "METHOD_BLD_PACKS_COMBO_PATTERN_SZRUN".                              */
alter table "METHOD_BLD_PACKS_COMBO_PATTERN_SZRUN" 
	add constraint "METHOD_BLD_PACKS_COMBO_PATTERN_SZRUN_FK1" foreign key ("METHOD_RID")
	 references "METHOD" ("METHOD_RID") 
go
