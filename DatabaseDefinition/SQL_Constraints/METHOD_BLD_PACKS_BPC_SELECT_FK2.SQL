alter table "METHOD_BLD_PACKS_BPC_SELECT" 
	add constraint "METHOD_BLD_PACKS_BPC_SELECT_FK2" foreign key ("BPC_COMBO_RID")
	 references "BUILD_PACK_COMBO" ("BPC_COMBO_RID") 
go		
