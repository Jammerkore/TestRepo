alter table "METHOD_BLD_PACKS_COMBO_PATTERN_SZRUN" 
	add constraint "METHOD_BLD_PACKS_COMBO_PATTERN_SZRUN_FK4" foreign key ("SIZE_CODE_RID")
	 references "SIZE_CODE" ("SIZE_CODE_RID") 	
go    
