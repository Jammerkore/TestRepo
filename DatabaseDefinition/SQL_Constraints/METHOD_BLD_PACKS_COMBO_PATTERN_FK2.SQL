alter table "METHOD_BLD_PACKS_COMBO_PATTERN" 
	add constraint "METHOD_BLD_PACKS_COMBO_PATTERN_FK2" foreign key ("COMBO_RID")
	 references "METHOD_BLD_PACKS_COMBO" ("COMBO_RID") 
go 
